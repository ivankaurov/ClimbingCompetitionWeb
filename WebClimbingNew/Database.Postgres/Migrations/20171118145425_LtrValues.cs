using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Database.Postgres.Migrations
{
    public partial class LtrValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewValue",
                table: "ltr_object_properties");

            migrationBuilder.DropColumn(
                name: "OldValue",
                table: "ltr_object_properties");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "ltr_object_properties",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "ltr_object_properties");

            migrationBuilder.AddColumn<string>(
                name: "NewValue",
                table: "ltr_object_properties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldValue",
                table: "ltr_object_properties",
                nullable: true);
        }
    }
}
