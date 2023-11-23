using Microsoft.EntityFrameworkCore.Migrations;

namespace FinAp_WEB_API_CRUD.Migrations
{
    public partial class TeacherClassroomRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherClassroom_Classrooms_ClassroomId",
                table: "TeacherClassroom");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherClassroom_Teacher_TeacherId",
                table: "TeacherClassroom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherClassroom",
                table: "TeacherClassroom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teacher",
                table: "Teacher");

            migrationBuilder.RenameTable(
                name: "TeacherClassroom",
                newName: "TeacherClassrooms");

            migrationBuilder.RenameTable(
                name: "Teacher",
                newName: "Teachers");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherClassroom_ClassroomId",
                table: "TeacherClassrooms",
                newName: "IX_TeacherClassrooms_ClassroomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherClassrooms",
                table: "TeacherClassrooms",
                columns: new[] { "TeacherId", "ClassroomId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherClassrooms_Classrooms_ClassroomId",
                table: "TeacherClassrooms",
                column: "ClassroomId",
                principalTable: "Classrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherClassrooms_Teachers_TeacherId",
                table: "TeacherClassrooms",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherClassrooms_Classrooms_ClassroomId",
                table: "TeacherClassrooms");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherClassrooms_Teachers_TeacherId",
                table: "TeacherClassrooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherClassrooms",
                table: "TeacherClassrooms");

            migrationBuilder.RenameTable(
                name: "Teachers",
                newName: "Teacher");

            migrationBuilder.RenameTable(
                name: "TeacherClassrooms",
                newName: "TeacherClassroom");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherClassrooms_ClassroomId",
                table: "TeacherClassroom",
                newName: "IX_TeacherClassroom_ClassroomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teacher",
                table: "Teacher",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherClassroom",
                table: "TeacherClassroom",
                columns: new[] { "TeacherId", "ClassroomId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherClassroom_Classrooms_ClassroomId",
                table: "TeacherClassroom",
                column: "ClassroomId",
                principalTable: "Classrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherClassroom_Teacher_TeacherId",
                table: "TeacherClassroom",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
