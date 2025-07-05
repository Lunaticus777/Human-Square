using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Human_Evolution.Migrations
{
    /// <inheritdoc />
    public partial class AddSelectedDomainsToServiceRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedDomains",
                table: "ServiceRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedDomains",
                table: "ServiceRequests");
        }
    }
}
