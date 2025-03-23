using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloggie.Web.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e57531ab-259a-4d31-a235-f67ac00dfff4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5b6339e7-d32f-49d2-94a0-692c8a4be2d3", "AQAAAAIAAYagAAAAECeehLUoEDOadPG1P63vnCoNWPWQsKnYYmrs94yE5TABWLl4qcX8PVruc9frXOoA5Q==", "383dfdd9-c133-46f8-bf23-6d8baf793e56" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e57531ab-259a-4d31-a235-f67ac00dfff4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a404104e-52fe-4e72-b59c-03e9e727319e", "AQAAAAIAAYagAAAAEC9zOkuoQc2uITNYumRmci8xZKklMUP1xMijL8aBeKKsnphlKEmhtxV605/w7xR7cQ==", "3202249b-6cba-4378-9496-72394547e3ed" });
        }
    }
}
