using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.Core.DTOs;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IUserService
    {
        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        User GetUserByEmail(string email);
        User GetUserByActiveCode(string activeCode);
        User GetUserByUserName(string userName);
        void UpdateUser(User user);
        bool ActiveAccount(string activeCode);
        int GetUserIdByUserName(string userName);

        #region UserPanel
        InformationUserViewModel GetUserInformation(string Username);
        SideBarViewModel GetSideBarUserPanelData(string userName);
        EditProfileViewModel GetDataForEditProfileUser(string Username);
        void EditProfile(string Userename, EditProfileViewModel editProfile);
        bool CompareOldPassword(string oldPassword, string username);
        void ChangeUserPassword(string username, string newPassword);
        #endregion

        #region Wallet
        int BalanceUserWallet(string userName);
        
        
        #endregion


    }
}
