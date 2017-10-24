﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Database.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ltr",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ltr", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ltr_objects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeType = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    LogObjectClass = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    LtrId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ltr_objects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ltr_objects_ltr_LtrId",
                        column: x => x.LtrId,
                        principalTable: "ltr",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ltr_object_properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LtrObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    PropertyType = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ltr_object_properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ltr_object_properties_ltr_objects_LtrObjectId",
                        column: x => x.LtrObjectId,
                        principalTable: "ltr_objects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ltr_object_properties_LtrObjectId",
                table: "ltr_object_properties",
                column: "LtrObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ltr_objects_LtrId",
                table: "ltr_objects",
                column: "LtrId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "ltr_object_properties");

            migrationBuilder.DropTable(
                name: "ltr_objects");

            migrationBuilder.DropTable(
                name: "ltr");
        }
    }
}
