using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services
{
    public class PermissionService : IPermissionService
    {
        TopLearnContext _context;
        public PermissionService(TopLearnContext context)
        {
            _context = context;
        }

        public void AddPermissionsToRole(int roleId, List<int> permissions)
        {
            foreach (var item in permissions)
            {
                _context.rolepermission.Add(new RolePermission
                {
                    PermissionId = item,
                    RoleId = roleId,
                });
            }
            _context.SaveChanges();
        }

        public int AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role.RoleId;
        }

        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach (var role in roleIds)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId = role,
                    UserId = userId
                });

            }
            _context.SaveChanges();
        }

        public bool CheckPermission(int permissionId, string userName)
        {
            List<int> userRoles = UserRoles(userName);
            if (!userRoles.Any())
                return false;

            List<int> rolePermission = _context.rolepermission
                .Where(x => x.PermissionId == permissionId)
                .Select(x => x.Rp_Id).ToList(); ;

            var check = userRoles.Equals(permissionId);

            return rolePermission.Any(x=> userRoles.Contains(x));
        }

        public void DeleteRole(Role role)
        {
            role.IsDelete = true;
            UpdateRole(role);
        }

        public void EditPermissionsRole(int roleId, List<int> permissions)
        {
            var data = _context.rolepermission.Where(x => x.RoleId == roleId).ToList();
            foreach (var item in data)
            {
                _context.Remove(item);
            }

            AddPermissionsToRole(roleId, permissions);

        }

        public void EditRolesUser(int userId, List<int> roleIds)
        {
            //Delete All UserRole
            _context.UserRoles.Where(r => r.UserId == userId).ToList().ForEach(r => _context.UserRoles.Remove(r));

            AddRolesToUser(roleIds, userId);


        }

        public List<Permission> GetAllPermission()
        {
            return _context.permission.ToList();
        }

        public Role GetRoleById(int RoleId)
        {
            return _context.Roles.Find(RoleId);
        }

        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public int GetUserIdByUsername(string Username)
        {
            return _context.Users.Single(x => x.UserName == Username).UserId;
        }

        public List<int> PermissionsRole(int rolid)
        {
            return _context.rolepermission.Where(x => x.RoleId == rolid).Select(x => x.PermissionId).ToList();
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        public List<int> UserRoles(string Username)
        {
            int Userid = GetUserIdByUsername(Username);
            return _context.UserRoles.Where(x => x.UserId == Userid).Select(x => x.RoleId).ToList();
        }
    }  
}
