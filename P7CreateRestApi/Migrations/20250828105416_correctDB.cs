using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P7CreateRestApi.Migrations
{
    public partial class correctDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurveId",
                table: "CurvePoints");

            migrationBuilder.DropColumn(
                name: "Ask",
                table: "BidLists");

            migrationBuilder.DropColumn(
                name: "Bid",
                table: "BidLists");

            migrationBuilder.RenameColumn(
                name: "TradeId",
                table: "Trades",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "BidListDate",
                table: "BidLists",
                newName: "BidDate");

            migrationBuilder.RenameColumn(
                name: "BidListId",
                table: "BidLists",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Trades",
                newName: "TradeId");

            migrationBuilder.RenameColumn(
                name: "BidDate",
                table: "BidLists",
                newName: "BidListDate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BidLists",
                newName: "BidListId");

            migrationBuilder.AddColumn<byte>(
                name: "CurveId",
                table: "CurvePoints",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Ask",
                table: "BidLists",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Bid",
                table: "BidLists",
                type: "float",
                nullable: true);
        }
    }
}
