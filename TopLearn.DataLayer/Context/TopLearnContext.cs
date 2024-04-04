using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TopLearn.DataLayer.Entities.Course;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.DataLayer.Context
{
    public class TopLearnContext : DbContext
    {

        public TopLearnContext(DbContextOptions<TopLearnContext> options) : base(options)
        {

        }

        #region User

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        #endregion


        #region Wallet
        public DbSet<WalletType> walletTypes { get; set; }
        public DbSet<Wallet> wallets { get; set; }

        #endregion


        #region permission 
        public DbSet<Permission> permission { get; set; }
        public DbSet<RolePermission> rolepermission { get; set; }


        #endregion


        #region Course 

        public DbSet<CourseGroup> courseGroups { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseEpisode> CourseEpisodes { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<CourseStatus> CourseStatuses { get; set; }


        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDelete);
            modelBuilder.Entity<Role>().HasQueryFilter(x => !x.IsDelete);
            modelBuilder.Entity<CourseGroup>().HasQueryFilter(G => !G.IsDelete);
            modelBuilder.Entity<Course>().HasQueryFilter(G => !G.IsDeleted);
            base.OnModelCreating(modelBuilder);
        }


    }
}
