using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pharmacies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    ServicesDescription = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suppliers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyMedicines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PharmacyId = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicineId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyMedicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmacyMedicines_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PharmacyMedicines_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PharmacyId = table.Column<Guid>(type: "uuid", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierMedicines",
                columns: table => new
                {
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicineId = table.Column<Guid>(type: "uuid", nullable: false),
                    WholesalePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    MinimumOrderQuantity = table.Column<int>(type: "integer", nullable: false),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierMedicines", x => new { x.SupplierId, x.MedicineId });
                    table.ForeignKey(
                        name: "FK_SupplierMedicines_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierMedicines_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicineId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => new { x.OrderId, x.MedicineId });
                    table.ForeignKey(
                        name: "FK_OrderItems_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Medicines",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"), "Paracetamol 500mg for pain relief and fever.", "Panadol Advance" },
                    { new Guid("b2c3d4e5-f6a7-4b6c-9d0e-1f2a3b4c5d6e"), "Amoxicillin 500mg Broad-spectrum antibiotic.", "Amoxil" },
                    { new Guid("c3d4e5f6-a7b8-4c7d-0e1f-2a3b4c5d6e7f"), "Diclofenac Sodium 50mg Non-steroidal anti-inflammatory drug (NSAID).", "Voltaren" },
                    { new Guid("d4e5f6a7-b8c9-4d8e-1f2a-3b4c5d6e7f8a"), "Furosemide 40mg Diuretic for fluid retention.", "Lasix" },
                    { new Guid("e5f6a7b8-c9d0-4e9f-2a3b-4c5d6e7f8a9b"), "Co-amoxiclav 1g Antibiotic for bacterial infections.", "Augmentin" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { new Guid("f1a2b3c4-d5e6-4a5b-8c9d-0e1f2a3b4c5d"), "admin@iqpharma.com", "Iraqi Pharma Admin", "AQAAAAEAACcQAAAAE...", 2 },
                    { new Guid("f2b3c4d5-e6f7-4b6c-9d0e-1f2a3b4c5d6e"), "admin@pioneer.iq", "Pioneer Admin", "AQAAAAEAACcQAAAAE...", 2 },
                    { new Guid("f3c4d5e6-f7a8-4c7d-0e1f-2a3b4c5d6e7f"), "admin@mansour.com", "Mansour Pharma Admin", "AQAAAAEAACcQAAAAE...", 2 },
                    { new Guid("f4d5e6f7-a8b9-4d8e-1f2a-3b4c5d6e7f8a"), "owner@baghdadpharma.com", "Baghdad Pharmacy Owner", "AQAAAAEAACcQAAAAE...", 1 },
                    { new Guid("f5e6f7a8-b9c0-4e9f-2a3b-4c5d6e7f8a9b"), "owner@erbilpharma.com", "Erbil Pharmacy Owner", "AQAAAAEAACcQAAAAE...", 1 },
                    { new Guid("f6f7a8b9-c0d1-4f0a-3b4c-5d6e7f8a9b0c"), "owner@basrapharma.com", "Basra Pharmacy Owner", "AQAAAAEAACcQAAAAE...", 1 }
                });

            migrationBuilder.InsertData(
                table: "Pharmacies",
                columns: new[] { "Id", "Address", "Name", "PhoneNumber", "UserId" },
                values: new object[,]
                {
                    { new Guid("77777777-1111-2222-3333-444444444444"), "Baghdad, Palestine St", "Al-Amal Pharmacy", "+9647811122233", new Guid("f4d5e6f7-a8b9-4d8e-1f2a-3b4c5d6e7f8a") },
                    { new Guid("88888888-1111-2222-3333-444444444444"), "Erbil, 60 Meter Rd", "Zheen Pharmacy", "+9647501112223", new Guid("f5e6f7a8-b9c0-4e9f-2a3b-4c5d6e7f8a9b") },
                    { new Guid("99999999-1111-2222-3333-444444444444"), "Basra, Corniche", "Al-Fayhaa Pharmacy", "+9647711122233", new Guid("f6f7a8b9-c0d1-4f0a-3b4c-5d6e7f8a9b0c") }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Address", "Name", "PhoneNumber", "ServicesDescription", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-2222-3333-4444-555555555555"), "Baghdad, Al-Mansour District", "Iraqi Pharmaceutical Industry", "+9647801234567", "Local manufacturer and wholesale distributor.", new Guid("f1a2b3c4-d5e6-4a5b-8c9d-0e1f2a3b4c5d") },
                    { new Guid("22222222-3333-4444-5555-666666666666"), "Erbil, Industrial Zone", "Pioneer Pharmaceutical Industries", "+9647501234567", "Import/Export and large scale distribution.", new Guid("f2b3c4d5-e6f7-4b6c-9d0e-1f2a3b4c5d6e") },
                    { new Guid("33333333-4444-5555-6666-777777777777"), "Baghdad, Baghdad Al-Jadida", "Al-Mansour Pharmaceuticals", "+9647701234567", "Specialized in chronic disease medications.", new Guid("f3c4d5e6-f7a8-4c7d-0e1f-2a3b4c5d6e7f") }
                });

            migrationBuilder.InsertData(
                table: "SupplierMedicines",
                columns: new[] { "MedicineId", "SupplierId", "MinimumOrderQuantity", "StockQuantity", "WholesalePrice" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"), new Guid("11111111-2222-3333-4444-555555555555"), 10, 500, 1250m },
                    { new Guid("b2c3d4e5-f6a7-4b6c-9d0e-1f2a3b4c5d6e"), new Guid("11111111-2222-3333-4444-555555555555"), 5, 200, 3500m },
                    { new Guid("e5f6a7b8-c9d0-4e9f-2a3b-4c5d6e7f8a9b"), new Guid("11111111-2222-3333-4444-555555555555"), 3, 100, 8000m },
                    { new Guid("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"), new Guid("22222222-3333-4444-5555-666666666666"), 20, 1000, 1200m },
                    { new Guid("c3d4e5f6-a7b8-4c7d-0e1f-2a3b4c5d6e7f"), new Guid("22222222-3333-4444-5555-666666666666"), 10, 350, 2500m },
                    { new Guid("b2c3d4e5-f6a7-4b6c-9d0e-1f2a3b4c5d6e"), new Guid("33333333-4444-5555-6666-777777777777"), 5, 150, 3600m },
                    { new Guid("d4e5f6a7-b8c9-4d8e-1f2a-3b4c5d6e7f8a"), new Guid("33333333-4444-5555-6666-777777777777"), 10, 400, 1500m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MedicineId",
                table: "OrderItems",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PharmacyId",
                table: "Orders",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SupplierId",
                table: "Orders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_UserId",
                table: "Pharmacies",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyMedicines_MedicineId",
                table: "PharmacyMedicines",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyMedicines_PharmacyId",
                table: "PharmacyMedicines",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierMedicines_MedicineId",
                table: "SupplierMedicines",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_UserId",
                table: "Suppliers",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "PharmacyMedicines");

            migrationBuilder.DropTable(
                name: "SupplierMedicines");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Pharmacies");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
