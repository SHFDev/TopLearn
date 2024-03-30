using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Services
{
    public class UserService : IUserService
    {
        private TopLearnContext _context;

        public UserService(TopLearnContext context)
        {
            _context = context;
        }


        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public User LoginUser(LoginViewModel login)
        {
            string hashPassword = PasswordHelper.EncodePasswordMd5(login.Password);
            string email = FixedText.FixEmail(login.Email);
            return _context.Users.SingleOrDefault(u => u.Email == email && u.Password == hashPassword);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetUserByActiveCode(string activeCode)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public bool ActiveAccount(string activeCode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
            if (user == null || user.IsActive)
                return false;

            user.IsActive = true;
            user.ActiveCode = NameGenerator.GenerateUniqCode();
            _context.SaveChanges();

            return true;
        }

        public InformationUserViewModel GetUserInformation(string Username)
        {
            var user = GetUserByUserName(Username);
            InformationUserViewModel information = new InformationUserViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                RegisterDate = user.RegisterDate,
                Wallet = BalanceUserWallet(user.UserName),
            };
            return information;
        }
        public InformationUserViewModel GetUserInformation(int UserId)
        {
            var user = GetUserByUserId(UserId);
            InformationUserViewModel information = new InformationUserViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                RegisterDate = user.RegisterDate,
                Wallet = BalanceUserWallet(user.UserName),
            };
            return information;
        }

        public User GetUserByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(x => x.UserName == userName);
        }

        public SideBarViewModel GetSideBarUserPanelData(string userName)
        {
            var user = GetUserByUserName(userName);
            SideBarViewModel sideBarView = new SideBarViewModel()
            {
                UserName = user.UserName,
                ImageName = user.UserAvatar,
                RegisterDate = user.RegisterDate,
            };
            return sideBarView;
        }

        public EditProfileViewModel GetDataForEditProfileUser(string Username)
        {
            return _context.Users.Where(x => x.UserName == Username).Select(U => new EditProfileViewModel
            {
                AvatarName = U.UserAvatar,
                UserName = U.UserName,
                Email = U.Email,
            }).SingleOrDefault();
        }

        public void EditProfile(string Userename, EditProfileViewModel editProfile)
        {
            if (editProfile.UserAvatar != null)
            {
                string imagepath = "";
                if (editProfile.AvatarName != "Defult.jpg")
                {
                    imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editProfile.AvatarName);
                    if (File.Exists(imagepath))
                    {
                        File.Delete(imagepath);
                    }
                }
                editProfile.AvatarName = NameGenerator.GenerateUniqCode() + Path.GetExtension(editProfile.UserAvatar.FileName);
                imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editProfile.AvatarName);
                using (var stream = new FileStream(imagepath, FileMode.Create))
                {
                    editProfile.UserAvatar.CopyTo(stream);
                }
            }
            var User = GetUserByUserName(Userename);
            User.UserName = editProfile.UserName;
            User.Email = editProfile.Email;
            User.UserAvatar = editProfile.AvatarName;
            UpdateUser(User);
        }

        public bool CompareOldPassword(string oldPassword, string username)
        {
            string HashOldPassword = PasswordHelper.EncodePasswordMd5(oldPassword);
            return _context.Users.Any(u => u.UserName == username && u.Password == HashOldPassword);
        }

        public void ChangeUserPassword(string username, string newPassword)
        {
            var User = GetUserByUserName(username);
            User.Password = PasswordHelper.EncodePasswordMd5(newPassword);
            UpdateUser(User);
        }

        public int BalanceUserWallet(string userName)
        {
            var Userid = GetUserIdByUserName(userName);
            var Deposit = _context.wallets.Where(w => w.UserId == Userid && w.TypeId == 1 && w.IsPay).Select(w => w.Amount).ToList();
            var withdraw = _context.wallets.Where(w => w.UserId == Userid && w.TypeId == 2 && w.IsPay).Select(w => w.Amount).ToList();

            return (Deposit.Sum() - withdraw.Sum());
        }

        public int GetUserIdByUserName(string userName)
        {
            return _context.Users.Single(x => x.UserName == userName).UserId;

        }

        public List<WalletViewModel> GetUserWallets(string userName)
        {
            int userId = GetUserIdByUserName(userName);
            return _context.wallets.Where(w => w.UserId == userId && w.IsPay).Select(x => new WalletViewModel
            {
                Amount = x.Amount,
                DateTime = x.CreateDate,
                Description = x.Description,
                Type = x.TypeId
            }).ToList();
        }

        public int chargeWallet(string username, int Amount, string Description, bool IsPay = false)
        {
            var userid = GetUserIdByUserName(username);
            Wallet wallet = new Wallet()
            {
                Amount = Amount,
                CreateDate = DateTime.Now,
                Description = Description,
                IsPay = IsPay,
                TypeId = 1,
                UserId = userid,
            };

            return AddWallet(wallet);
        }

        public int AddWallet(Wallet wallet)
        {
            _context.wallets.Add(wallet);
            _context.SaveChanges();
            return wallet.WalletId;
        }

        public Wallet GetWalletByWalletId(int walletId)
        {
            return _context.wallets.Find(walletId);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.wallets.Update(wallet);
        }

        public UserForAdminViewModel GetUsers(int pageid = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result = _context.Users;

            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }
            if (!string.IsNullOrEmpty(filterUserName))
            {
                result = result.Where(u => u.UserName.Contains(filterUserName));
            }
            //show item in page
            //int Take = 20;        
            int Take = 20;
            int skip = (pageid - 1) * Take;
            UserForAdminViewModel List = new UserForAdminViewModel();
            List.CurrentPage = pageid;
            List.PageCount = result.Count() / Take;

            List.Users = result.OrderBy(o => o.RegisterDate).Skip(skip).Take(Take).ToList();

            return List;

        }

        public int AddUserFromAdmin(CreateUserViewModel user)
        {
            var User = new User();
            User.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            User.Email = user.Email;
            User.UserName = user.UserName;
            User.ActiveCode = NameGenerator.GenerateUniqCode();
            User.IsActive = true;
            User.RegisterDate = DateTime.Now;
            #region save avatar
            if (user.UserAvatar != null)
            {
                string imagepath = "";
                User.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(user.UserAvatar.FileName);
                imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", User.UserAvatar);
                using (var stream = new FileStream(imagepath, FileMode.Create))
                {
                    user.UserAvatar.CopyTo(stream);
                }
            }
            #endregion
            return AddUser(User);
        }

        public EditUserViewModel GetUserForShowInEditMode(int userId)
        {

            return _context.Users.Where(u => u.UserId == userId)
                .Select(u => new EditUserViewModel
                {
                    UserId = u.UserId,
                    AvatarName = u.UserAvatar,
                    Email = u.Email,
                    UserName = u.UserName,
                    UserRoles = u.UserRoles.Select(r => r.RoleId).ToList(),

                }).Single();

        }

        public void EditUserUserFromAdmin(EditUserViewModel editUser)
        {
            var user = GetUserByUserId(editUser.UserId);
            user.Email = editUser.Email;
            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMd5(editUser.Password);
            }
            if (editUser.UserAvatar != null)
            {
                if (editUser.AvatarName != "Defult.jpg")
                {
                    var deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editUser.AvatarName);
                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }
                }
                #region save avatar
                if (user.UserAvatar != null)
                {
                    string imagepath = "";
                    user.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(editUser.UserAvatar.FileName);
                    imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.UserAvatar);
                    using (var stream = new FileStream(imagepath, FileMode.Create))
                    {
                        editUser.UserAvatar.CopyTo(stream);
                    }
                }
                #endregion
            }
            _context.Users.Update(user);
            _context.SaveChanges();

        }

        public User GetUserByUserId(int userId)
        {
            return _context.Users.SingleOrDefault(x => x.UserId == userId);
        }

        public UserForAdminViewModel GetDeletedUsers(int pageid = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(x=>x.IsDelete);

            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }
            if (!string.IsNullOrEmpty(filterUserName))
            {
                result = result.Where(u => u.UserName.Contains(filterUserName));
            }
            int Take = 20;
            int skip = (pageid - 1) * Take;
            UserForAdminViewModel List = new UserForAdminViewModel();
            List.CurrentPage = pageid;
            List.PageCount = result.Count() / Take;

            List.Users = result.OrderBy(o => o.RegisterDate).Skip(skip).Take(Take).ToList();

            return List;
        }

        public void DeleteUser(int userid)
        {
            var user = GetUserByUserId(userid);
            user.IsDelete = true;
            UpdateUser(user);
        }
    }
}
