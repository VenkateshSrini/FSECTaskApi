using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace TaskAPI.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class addedColumnParentTaskDes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "parent_task_details",
                table: "parent_tasks",
                type: "varchar(40)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "parent_task_details",
                table: "parent_tasks");
        }
    }
}
