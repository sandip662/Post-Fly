using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloggie.Web.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class InitialMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e57531ab-259a-4d31-a235-f67ac00dfff4",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dd660e73-a199-4297-a674-b11ce2eabb53", "sandipmondal9748@gmail.com", "SANDIPMONDAL9748@GMAIL.COM", "AQAAAAIAAYagAAAAEAc9zf2DqiJIQGF5bC7pgYDH39vrDYyZXnkwHxr6/a0gw6A34yVNoPa58NgYifRGkA==", "6a670317-23d1-4851-b389-cf669c94e08d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e57531ab-259a-4d31-a235-f67ac00dfff4",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d3f4d8fe-2e38-4113-8eb4-7f37bbf5c361", "superadmin@bloggie.com", "SUPERADMIN@BLOGGIE.COM", "AQAAAAIAAYagAAAAELvAmUIW+03DxfLwT85RKW6K2yilrkQLpV5Xjz9p2nQolyLaw/XjJ1AqEQ0EHcxdWA==", "27e54c44-abaf-47dc-8445-be8557388e7d" });
        }
    }
}
