using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IPermissionService
    {
        #region Rolse
        List<Role> GetRoles();
        int AddRole(Role role);
        void UpdateRole(Role role);
        Role GetRoleById(int RoleId); 
        void AddRolesToUser(List<int> roleIds,int userId);
        void EditRolesUser(int userId,List<int> roleIds);
        void DeleteRole(Role role);

        #endregion

        #region permission
        List<Permission> GetAllPermission();
        void AddPermissionsToRole(int roleId,List<int> permissions);
        List<int> PermissionsRole(int rolid);
        void EditPermissionsRole(int roleId, List<int> permissions);

        bool CheckPermission(int permissionId, string userName);
        List<int> UserRoles(string Username);
        int GetUserIdByUsername(string Username);
        #endregion
    }
}
