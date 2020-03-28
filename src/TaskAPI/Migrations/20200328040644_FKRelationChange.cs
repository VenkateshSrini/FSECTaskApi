using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskAPI.Migrations
{
    public partial class FKRelationChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tasks_parent_tasks_parent_id",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_parent_id",
                table: "tasks");

            migrationBuilder.AddColumn<int>(
                name: "Parent_Task",
                table: "tasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tasks_Parent_Task",
                table: "tasks",
                column: "Parent_Task");

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_parent_tasks_Parent_Task",
                table: "tasks",
                column: "Parent_Task",
                principalTable: "parent_tasks",
                principalColumn: "parent_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tasks_parent_tasks_Parent_Task",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_Parent_Task",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "Parent_Task",
                table: "tasks");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_parent_id",
                table: "tasks",
                column: "parent_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tasks_parent_tasks_parent_id",
                table: "tasks",
                column: "parent_id",
                principalTable: "parent_tasks",
                principalColumn: "parent_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
