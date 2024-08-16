using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkiingStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserAddressAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "City",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "State",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "UserAddress");
        }
    }
}
