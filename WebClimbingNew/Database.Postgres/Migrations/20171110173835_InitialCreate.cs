using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Database.Postgres.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ltr",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    when_changed = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    when_created = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ltr", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "people",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp", nullable: false),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    surname = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    when_changed = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    when_created = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
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
                    code = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    parent_team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    when_changed = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    when_created = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ltr_objects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangeType = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    LogObjectClass = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    LtrId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    when_changed = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    when_created = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LtrObjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    PropertyName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    PropertyType = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    when_changed = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    when_created = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_people_surname_name_date_of_birth",
                table: "people",
                columns: new[] { "surname", "name", "date_of_birth" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_teams_code",
                table: "teams",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_teams_parent_team_id",
                table: "teams",
                column: "parent_team_id");

            migrationBuilder.CreateIndex(
                name: "IX_teams_name_parent_team_id",
                table: "teams",
                columns: new[] { "name", "parent_team_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ltr_object_properties");

            migrationBuilder.DropTable(
                name: "people");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "ltr_objects");

            migrationBuilder.DropTable(
                name: "ltr");
        }
    }
}
