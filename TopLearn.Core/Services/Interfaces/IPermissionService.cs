using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IPermissionService
    {
        #region Rolse
        List<Role> GetRoles();
        void AddRolesToUser(List<int> roleIds,int userId);
        #endregion
    }
}
