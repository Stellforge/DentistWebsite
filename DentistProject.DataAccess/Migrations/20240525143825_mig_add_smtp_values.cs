using DentistProject.DataAccess.EntityFramework;
using DentistProject.Entities.Enum;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentistProject.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig_add_smtp_values : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var context =new  DatabaseContext();

            foreach (var item in Enum.GetValues<ESettingKey>())
            {
                context.SystemSettings.Add(new Entities.SystemSettingEntity
                {
                    CreateTime = DateTime.Now,
                    Description = "",
                    Key = item,
                    Name = item.ToString(),
                    Value = "",
                    IsDeleted = false,

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
