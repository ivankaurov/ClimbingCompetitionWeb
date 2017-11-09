using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Database.Postgres.Migrations
{
    public partial class PeopleAndTeams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "WhenChanged",
                table: "ltr_objects",
                type: "timestamptz",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "WhenCreated",
                table: "ltr_objects",
                type: "timestamptz",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "WhenChanged",
                table: "ltr_object_properties",
                type: "timestamptz",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "WhenCreated",
                table: "ltr_object_properties",
                type: "timestamptz",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "WhenChanged",
                table: "ltr",
                type: "timestamptz",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "WhenCreated",
                table: "ltr",
                type: "timestamptz",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateTable(
                name: "people",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp", nullable: false),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    surname = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    WhenChanged = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    WhenCreated = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_people", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    local_code = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    parent_team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    WhenChanged = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    WhenCreated = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_teams_teams_parent_team_id",
                        column: x => x.parent_team_id,
                        principalTable: "teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_people_surname_name_date_of_birth",
                table: "people",
                columns: new[] { "surname", "name", "date_of_birth" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_teams_parent_team_id",
                table: "teams",
                column: "parent_team_id");

            migrationBuilder.CreateIndex(
                name: "IX_teams_local_code_parent_team_id",
                table: "teams",
                columns: new[] { "local_code", "parent_team_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_teams_name_parent_team_id",
                table: "teams",
                columns: new[] { "name", "parent_team_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "people");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropColumn(
                name: "WhenChanged",
                table: "ltr_objects");

            migrationBuilder.DropColumn(
                name: "WhenCreated",
                table: "ltr_objects");

            migrationBuilder.DropColumn(
                name: "WhenChanged",
                table: "ltr_object_properties");

            migrationBuilder.DropColumn(
                name: "WhenCreated",
                table: "ltr_object_properties");

            migrationBuilder.DropColumn(
                name: "WhenChanged",
                table: "ltr");

            migrationBuilder.DropColumn(
                name: "WhenCreated",
                table: "ltr");
        }
    }
}
