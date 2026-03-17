using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BTL_backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Supplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenVatTu = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    LoaiVatTu = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    DonViTinh = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MoTa = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenThonXa = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    HuyenQuan = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TinhThanhPho = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    GhiChu = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Farmers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HoTen = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    SoDienThoai = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    SoCCCD = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    DiaChi = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    DienTichCanhTac = table.Column<decimal>(type: "TEXT", nullable: false),
                    ZoneId = table.Column<int>(type: "INTEGER", nullable: false),
                    GhiChu = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farmers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farmers_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaHopDong = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FarmerId = table.Column<int>(type: "INTEGER", nullable: false),
                    MuaVu = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SanPhamThuMua = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    GiaThuMua = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LaiSuat = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TrangThai = table.Column<int>(type: "INTEGER", nullable: false),
                    GhiChu = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Harvests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContractId = table.Column<int>(type: "INTEGER", nullable: false),
                    TenNongSan = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SoLuong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GiaThuMua = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayThuMua = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ChatLuong = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    GhiChu = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Harvests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Harvests_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Settlements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContractId = table.Column<int>(type: "INTEGER", nullable: false),
                    TongTienThuMua = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongNoVatTu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TienLai = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoTienThucNhan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayQuyetToan = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GhiChu = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settlements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Settlements_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplyDebts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContractId = table.Column<int>(type: "INTEGER", nullable: false),
                    SupplyId = table.Column<int>(type: "INTEGER", nullable: false),
                    SoLuong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayLay = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GhiChu = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyDebts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplyDebts_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplyDebts_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Supplies",
                columns: new[] { "Id", "DonGia", "DonViTinh", "LoaiVatTu", "MoTa", "TenVatTu" },
                values: new object[,]
                {
                    { 1, 15000m, "kg", "Giống", "Giống lúa năng suất cao", "Giống lúa IR50404" },
                    { 2, 22000m, "kg", "Giống", "Giống lúa thơm chất lượng", "Giống lúa Jasmine 85" },
                    { 3, 850000m, "bao (50kg)", "Phân bón", "Phân bón tổng hợp", "Phân NPK 20-20-15" },
                    { 4, 520000m, "bao (50kg)", "Phân bón", "Phân đạm", "Phân Ure Phú Mỹ" },
                    { 5, 45000m, "gói", "Thuốc BVTV", "Thuốc trừ sâu đục thân", "Thuốc trừ sâu Regent 800WG" }
                });

            migrationBuilder.InsertData(
                table: "Zones",
                columns: new[] { "Id", "GhiChu", "HuyenQuan", "TenThonXa", "TinhThanhPho" },
                values: new object[,]
                {
                    { 1, null, "Huyện Củ Chi", "Xã An Phú", "TP. Hồ Chí Minh" },
                    { 2, null, "Huyện Thủ Thừa", "Xã Tân Thạnh", "Long An" },
                    { 3, null, "Huyện Cao Lãnh", "Xã Mỹ Lộc", "Đồng Tháp" },
                    { 4, null, "Huyện Châu Thành", "Xã Long Hòa", "Tiền Giang" },
                    { 5, null, "Huyện Ô Môn", "Xã Phước Thới", "Cần Thơ" }
                });

            migrationBuilder.InsertData(
                table: "Farmers",
                columns: new[] { "Id", "DiaChi", "DienTichCanhTac", "GhiChu", "HoTen", "NgayTao", "SoCCCD", "SoDienThoai", "ZoneId" },
                values: new object[,]
                {
                    { 1, "Ấp 1, Xã An Phú", 2.5m, null, "Nguyễn Văn An", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "079201001234", "0901234567", 1 },
                    { 2, "Ấp 3, Xã An Phú", 1.8m, null, "Trần Thị Bé", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "079201005678", "0912345678", 1 },
                    { 3, "Ấp 2, Xã Tân Thạnh", 3.2m, null, "Lê Văn Cường", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "080201002345", "0923456789", 2 },
                    { 4, "Ấp 5, Xã Mỹ Lộc", 4.0m, null, "Phạm Thị Diệu", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "082201003456", "0934567890", 3 },
                    { 5, "Ấp 1, Xã Long Hòa", 2.0m, null, "Huỳnh Văn Em", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "086201004567", "0945678901", 4 }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "FarmerId", "GhiChu", "GiaThuMua", "LaiSuat", "MaHopDong", "MuaVu", "NgayBatDau", "NgayKetThuc", "NgayTao", "SanPhamThuMua", "TrangThai" },
                values: new object[,]
                {
                    { 1, 1, null, 7500m, 0.5m, "HD-2025-001", "Đông-Xuân 2025", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lúa IR50404", 0 },
                    { 2, 3, null, 8200m, 0m, "HD-2025-002", "Đông-Xuân 2025", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lúa Jasmine 85", 0 },
                    { 3, 4, null, 7200m, 0.5m, "HD-2025-003", "Hè-Thu 2025", new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lúa IR50404", 0 }
                });

            migrationBuilder.InsertData(
                table: "Harvests",
                columns: new[] { "Id", "ChatLuong", "ContractId", "GhiChu", "GiaThuMua", "NgayThuMua", "SoLuong", "TenNongSan", "ThanhTien" },
                values: new object[,]
                {
                    { 1, "Loại 1", 1, null, 7500m, new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 5000m, "Lúa IR50404", 37500000m },
                    { 2, "Loại 1", 2, null, 8200m, new DateTime(2025, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 7000m, "Lúa Jasmine 85", 57400000m }
                });

            migrationBuilder.InsertData(
                table: "SupplyDebts",
                columns: new[] { "Id", "ContractId", "DonGia", "GhiChu", "NgayLay", "SoLuong", "SupplyId", "ThanhTien" },
                values: new object[,]
                {
                    { 1, 1, 15000m, null, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 50m, 1, 750000m },
                    { 2, 1, 850000m, null, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 10m, 3, 8500000m },
                    { 3, 1, 520000m, null, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m, 4, 2600000m },
                    { 4, 2, 22000m, null, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 80m, 2, 1760000m },
                    { 5, 2, 850000m, null, new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 15m, 3, 12750000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_FarmerId",
                table: "Contracts",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_MaHopDong",
                table: "Contracts",
                column: "MaHopDong",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_ZoneId",
                table: "Farmers",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Harvests_ContractId",
                table: "Harvests",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Settlements_ContractId",
                table: "Settlements",
                column: "ContractId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplyDebts_ContractId",
                table: "SupplyDebts",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyDebts_SupplyId",
                table: "SupplyDebts",
                column: "SupplyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Harvests");

            migrationBuilder.DropTable(
                name: "Settlements");

            migrationBuilder.DropTable(
                name: "SupplyDebts");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Supplies");

            migrationBuilder.DropTable(
                name: "Farmers");

            migrationBuilder.DropTable(
                name: "Zones");
        }
    }
}
