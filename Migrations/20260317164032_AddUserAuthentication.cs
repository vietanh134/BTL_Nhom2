using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BTL_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAuthentication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenDangNhap = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    HoTen = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    SoDienThoai = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    VaiTro = table.Column<int>(type: "INTEGER", nullable: false),
                    HoatDong = table.Column<bool>(type: "INTEGER", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "HoTen", "HoatDong", "MatKhau", "NgayTao", "SoDienThoai", "TenDangNhap", "VaiTro" },
                values: new object[,]
                {
                    { 1, "admin@agricontract.vn", "Quản trị viên", true, "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0900000000", "admin", 0 },
                    { 2, "hoa@agricontract.vn", "Nguyễn Thị Hoa", true, "jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=", new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "0911111111", "nhanvien1", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenDangNhap",
                table: "Users",
                column: "TenDangNhap",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
