using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tuan3.Migrations
{
    /// <inheritdoc />
    public partial class ThemDuLieuMau : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Grades",
                columns: ["GradeId", "GradeName"],
                values: [1, "21DTHE5"] 
            );
            migrationBuilder.InsertData(
                table: "Grades",
                columns: ["GradeId", "GradeName"],
                values: [2, "21DTHE7"]
            );
            migrationBuilder.InsertData(
                table: "Grades",
                columns: ["GradeId", "GradeName"],
                values: [3, "21DTHE3"]
            );
            migrationBuilder.InsertData(
                table: "Students",
                columns: ["StudentId", "FirstName", "LastName", "GradeId"],
                values: [1,"Hiếu", "Phạm",1]);
            migrationBuilder.InsertData(
                table: "Students",
                columns: ["StudentId", "FirstName", "LastName", "GradeId"],
                values: [2, "An", "Phạm", 1]);
            migrationBuilder.InsertData(
                table: "Students",
                columns: ["StudentId", "FirstName", "LastName", "GradeId"],
                values: [3, "Thanh", "Phạm", 3]);
            migrationBuilder.InsertData(
                table: "Students",
                columns: ["StudentId", "FirstName", "LastName", "GradeId"],
                values: [4, "Quan", "Phạm", 2]);
            migrationBuilder.InsertData(
                table: "Students",
                columns: ["StudentId", "FirstName", "LastName", "GradeId"],
                values: [5, "Lan", "Phạm", 2]);
            migrationBuilder.InsertData(
                table: "Students",
                columns: ["StudentId", "FirstName", "LastName", "GradeId"],
                values: [6, "Hanh", "Phạm", 2]);
            migrationBuilder.InsertData(
                table: "Students",
                columns: ["StudentId", "FirstName", "LastName", "GradeId"],
                values: [7, "Thang", "Phạm", 1]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
