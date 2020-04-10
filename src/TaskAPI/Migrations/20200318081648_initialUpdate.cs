using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TaskAPI.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class initialUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "parent_tasks",
                columns: table => new
                {
                    parent_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    parent_task = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parent_tasks", x => x.parent_id);
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    task_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    task = table.Column<string>(type: "varchar(40)", nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    parent_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.task_id);
                    table.ForeignKey(
                        name: "FK_tasks_parent_tasks_parent_id",
                        column: x => x.parent_id,
                        principalTable: "parent_tasks",
                        principalColumn: "parent_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tasks_parent_id",
                table: "tasks",
                column: "parent_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "parent_tasks");
        }
    }
}
