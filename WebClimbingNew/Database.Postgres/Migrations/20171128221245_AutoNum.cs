﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Database.Postgres.Migrations
{
    public partial class AutoNum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "autonum_descriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    format_string = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    include_parent_number = table.Column<bool>(type: "bool", nullable: false),
                    sequence_name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    split_sequence_by_parent = table.Column<bool>(type: "bool", nullable: false),
                    when_changed = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    when_created = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_autonum_descriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "autonum_values",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<long>(type: "int8", nullable: false),
                    autonum_description_id = table.Column<Guid>(type: "uuid", nullable: false),
                    split_by = table.Column<Guid>(type: "uuid", nullable: true),
                    when_changed = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    when_created = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_autonum_values", x => x.Id);
                    table.ForeignKey(
                        name: "FK_autonum_values_autonum_descriptions_autonum_description_id",
                        column: x => x.autonum_description_id,
                        principalTable: "autonum_descriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_autonum_descriptions_sequence_name",
                table: "autonum_descriptions",
                column: "sequence_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_autonum_values_autonum_description_id",
                table: "autonum_values",
                column: "autonum_description_id");

            migrationBuilder.CreateIndex(
                name: "IX_autonum_values_split_by_autonum_description_id",
                table: "autonum_values",
                columns: new[] { "split_by", "autonum_description_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "autonum_values");

            migrationBuilder.DropTable(
                name: "autonum_descriptions");
        }
    }
}
