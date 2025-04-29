using DentistProject.Core.ExtensionMethods;
using DentistProject.DataAccess.EntityFramework;
using DentistProject.Entities;
using DentistProject.Entities.Enum;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentistProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class migAddvalues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var context = new DatabaseContext();
            var user=context.Users.Add(new Entities.UserEntity
            {
                Address = "",
                BirthDate = DateTime.Now,
                CreateTime = DateTime.Now,
                Email = "halilcinar1260@gmail.com",
                Gender = Entities.Enum.EGender.Male,
                Name = "Admin",
                Phone = "",
                Surname = "Admin",
                IsDeleted = false,

            });
            context.SaveChanges();
            var role = context.UserRoles.Add(new Entities.UserRoleEntity
            {
                IsDeleted = false,
                CreateTime = DateTime.Now,
                Role = Entities.Enum.ERoleType.Admin,
                UserId = user.Entity.Id,
            });
            
            var salt = ExtensionsMethods.GenerateRandomString(128);
            var entity = new IdentityEntity
            {
                CreateTime = DateTime.Now,
                UserId = user.Entity.Id,
                ExpiryDate = null,
                IsDeleted = false,
                PasswordSalt = salt,
                PasswordHash = ExtensionsMethods.CalculateMD5Hash(salt +"As12345" + salt),

            };
            var identity = context.Identitys.Add(entity);

            context.SaveChanges();


            var methods=Enum.GetValues<EMethod>();

            foreach (var method in methods)
            {
                context.RoleMethods.Add(new RoleMethodEntity
                {
                    CreateTime = DateTime.Now,
                    IsDeleted = false,
                    Method = method,
                    Role = ERoleType.Admin,

                });
            }
            context.SaveChanges();


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
