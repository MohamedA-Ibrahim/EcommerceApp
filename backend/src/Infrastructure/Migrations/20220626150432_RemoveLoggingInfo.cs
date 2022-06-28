using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class RemoveLoggingInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttributeTypes_AspNetUsers_CreatedBy",
                table: "AttributeTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_AttributeValues_AspNetUsers_CreatedBy",
                table: "AttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_CreatedBy",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_CreatedBy",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CreatedBy",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAddresses_AspNetUsers_CreatedBy",
                table: "UserAddresses");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CreatedBy",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_AttributeValues_CreatedBy",
                table: "AttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_AttributeTypes_CreatedBy",
                table: "AttributeTypes");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "UserAddresses");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "UserAddresses");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "UserAddresses");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "AttributeValues");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AttributeValues");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "AttributeValues");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "AttributeValues");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "AttributeTypes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AttributeTypes");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "AttributeTypes");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "AttributeTypes");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "UserAddresses",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddresses_CreatedBy",
                table: "UserAddresses",
                newName: "IX_UserAddresses_UserId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Orders",
                newName: "BuyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CreatedBy",
                table: "Orders",
                newName: "IX_Orders_BuyerId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Items",
                newName: "SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_CreatedBy",
                table: "Items",
                newName: "IX_Items_SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_SellerId",
                table: "Items",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_BuyerId",
                table: "Orders",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddresses_AspNetUsers_UserId",
                table: "UserAddresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_SellerId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_BuyerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAddresses_AspNetUsers_UserId",
                table: "UserAddresses");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserAddresses",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddresses_UserId",
                table: "UserAddresses",
                newName: "IX_UserAddresses_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "Orders",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_BuyerId",
                table: "Orders",
                newName: "IX_Orders_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Items",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Items_SellerId",
                table: "Items",
                newName: "IX_Items_CreatedBy");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "UserAddresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "UserAddresses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "UserAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Items",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "AttributeValues",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AttributeValues",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "AttributeValues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "AttributeValues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "AttributeTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AttributeTypes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "AttributeTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "AttributeTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatedBy",
                table: "Categories",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeValues_CreatedBy",
                table: "AttributeValues",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeTypes_CreatedBy",
                table: "AttributeTypes",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeTypes_AspNetUsers_CreatedBy",
                table: "AttributeTypes",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeValues_AspNetUsers_CreatedBy",
                table: "AttributeValues",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_CreatedBy",
                table: "Categories",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_CreatedBy",
                table: "Items",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CreatedBy",
                table: "Orders",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddresses_AspNetUsers_CreatedBy",
                table: "UserAddresses",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
