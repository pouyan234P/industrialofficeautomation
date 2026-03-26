using authentication.domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.presistence
{
    public class authDb: IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public authDb(DbContextOptions<authDb> options):base(options)
        {
            
        }
        public DbSet<Department> departments { get; set; }
        public DbSet<Position> positions { get; set; }
        public DbSet<signitureimage> signitureimages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserRole>(userrole =>
            {
                userrole.HasKey(ur => new { ur.UserId, ur.RoleId });
                userrole.HasOne(ur => ur.Role).WithMany(ur => ur.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();
            });
        }
    }
}
