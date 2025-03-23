using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloggie.Web.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class newlMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e57531ab-259a-4d31-a235-f67ac00dfff4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d3f4d8fe-2e38-4113-8eb4-7f37bbf5c361", "AQAAAAIAAYagAAAAELvAmUIW+03DxfLwT85RKW6K2yilrkQLpV5Xjz9p2nQolyLaw/XjJ1AqEQ0EHcxdWA==", "27e54c44-abaf-47dc-8445-be8557388e7d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e57531ab-259a-4d31-a235-f67ac00dfff4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5b6339e7-d32f-49d2-94a0-692c8a4be2d3", "AQAAAAIAAYagAAAAECeehLUoEDOadPG1P63vnCoNWPWQsKnYYmrs94yE5TABWLl4qcX8PVruc9frXOoA5Q==", "383dfdd9-c133-46f8-bf23-6d8baf793e56" });
        }
    }
}
