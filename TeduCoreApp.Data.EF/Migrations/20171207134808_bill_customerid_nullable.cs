using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TeduCoreApp.Data.EF.Migrations
{
    public partial class bill_customerid_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_AppUsers_CustomerId",
                table: "Bills");

            migrationBuilder.AlterColumn<string>(
                name: "FunctionId",
                table: "Permissions",
                type: "nvarchar(450)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "Bills",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "AnnouncementId",
                table: "AnnouncementUsers",
                type: "nvarchar(450)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "PageId",
                table: "AdvertistmentPositions",
                type: "nvarchar(450)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_AppUsers_CustomerId",
                table: "Bills",
                column: "CustomerId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_AppUsers_CustomerId",
                table: "Bills");

            migrationBuilder.AlterColumn<string>(
                name: "FunctionId",
                table: "Permissions",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "Bills",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AnnouncementId",
                table: "AnnouncementUsers",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "PageId",
                table: "AdvertistmentPositions",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_AppUsers_CustomerId",
                table: "Bills",
                column: "CustomerId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
