using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.Core.DTOs;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IUserService
    {
        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        User GetUserByEmail(string email);
        User GetUserByUserId(int userId);
        User GetUserByActiveCode(string activeCode);
        User GetUserByUserName(string userName);
        void UpdateUser(User user);
        bool ActiveAccount(string activeCode);
        int GetUserIdByUserName(string userName);
        void DeleteUser(int userid);
        

        #region UserPanel
        InformationUserViewModel GetUserInformation(string Username);
        InformationUserViewModel GetUserInformation(int UserId);
        SideBarViewModel GetSideBarUserPanelData(string userName);
        EditProfileViewModel GetDataForEditProfileUser(string Username);
        void EditProfile(string Userename, EditProfileViewModel editProfile);
        bool CompareOldPassword(string oldPassword, string username);
        void ChangeUserPassword(string username, string newPassword);
        #endregion
       
        
        #region AdminPanle
        UserForAdminViewModel GetUsers(int pageid=1,string filterEmail= "",string filterUserName= "");
        UserForAdminViewModel GetDeletedUsers(int pageid=1,string filterEmail= "",string filterUserName= "");
        int AddUserFromAdmin(CreateUserViewModel user);

        EditUserViewModel GetUserForShowInEditMode(int userId);

        void EditUserUserFromAdmin(EditUserViewModel editUser);


        #endregion

        #region Wallet
        int BalanceUserWallet(string userName);
        List<WalletViewModel> GetUserWallets(string userName);
        int chargeWallet(string username, int Amount,string Description, bool IsPay = false);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);
        void UpdateWallet(Wallet wallet);
        
        #endregion


    }
}
