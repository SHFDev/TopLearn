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
            return _context.wallets.Where(w => w.UserId == userId && w.IsPay).Select(x=> new WalletViewModel
            {
                Amount = x.Amount,
                DateTime=x.CreateDate,
                Description = x.Description,
                Type=x.TypeId
            }).ToList();
        }

        public void chargeWallet(string username, int Amount, string Description, bool IsPay = false)
        {
            var userid= GetUserIdByUserName(username);
            Wallet wallet = new Wallet()
            {
                Amount=Amount,
                CreateDate= DateTime.Now,
                Description = Description,
                IsPay = IsPay,
                TypeId=1,
                UserId = userid,
            };

            AddWallet(wallet);
        }

        public void AddWallet(Wallet wallet)
        {
            _context.wallets.Add(wallet);
            _context.SaveChanges();
        }
    }
}
