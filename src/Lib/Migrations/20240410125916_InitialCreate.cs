using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lib.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    PostCode = table.Column<string>(type: "TEXT", nullable: false),
                    HouseNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PizzaBases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzaBases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Toppings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toppings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeliveryCharge = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderPizzas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    BaseId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPizzas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderPizzas_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderPizzas_PizzaBases_BaseId",
                        column: x => x.BaseId,
                        principalTable: "PizzaBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderPizzaTopping",
                columns: table => new
                {
                    OrderPizzaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ToppingsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPizzaTopping", x => new { x.OrderPizzaId, x.ToppingsId });
                    table.ForeignKey(
                        name: "FK_OrderPizzaTopping_OrderPizzas_OrderPizzaId",
                        column: x => x.OrderPizzaId,
                        principalTable: "OrderPizzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderPizzaTopping_Toppings_ToppingsId",
                        column: x => x.ToppingsId,
                        principalTable: "Toppings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "FirstName", "HouseNumber", "LastName", "PostCode" },
                values: new object[,]
                {
                    { 1, "John", "1", "Doe", "LL572EG" },
                    { 2, "Alan", "2", "Wake", "B161AN" },
                    { 3, "Ian", "3", "Mile", "EH64DA" },
                    { 4, "Stephen", "4", "Bonnell", "BR75PN" },
                    { 5, "Kate", "5", "Braithwaite", "FY76SU" }
                });

            migrationBuilder.InsertData(
                table: "PizzaBases",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Deep Pan", 6.00m },
                    { 2, "Thin Crust", 5.00m }
                });

            migrationBuilder.InsertData(
                table: "Toppings",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Ham", 3.00m },
                    { 2, "Mushrooms", 2.00m },
                    { 3, "Mediterranean vegetables", 3.00m },
                    { 4, "Extra cheese", 1.50m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderPizzas_BaseId",
                table: "OrderPizzas",
                column: "BaseId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPizzas_OrderId",
                table: "OrderPizzas",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPizzaTopping_ToppingsId",
                table: "OrderPizzaTopping",
                column: "ToppingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderPizzaTopping");

            migrationBuilder.DropTable(
                name: "OrderPizzas");

            migrationBuilder.DropTable(
                name: "Toppings");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PizzaBases");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
