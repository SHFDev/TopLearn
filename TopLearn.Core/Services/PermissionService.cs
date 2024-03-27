using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
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

        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach (var role in roleIds)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId=role,
                    UserId=userId
                });

            }
            _context.SaveChanges();
        }

        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
