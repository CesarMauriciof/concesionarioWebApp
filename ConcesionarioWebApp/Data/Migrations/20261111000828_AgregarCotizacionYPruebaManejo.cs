using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCotizacionYPruebaManejo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cotizacion",
                columns: table => new
                {
                    id_cotizacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fecha = table.Column<DateTime>(type: "date", nullable: false),
                    fecha_vencimiento = table.Column<DateTime>(type: "date", nullable: false),
                    precio_ofrecido = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    observaciones = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    id_vehiculo = table.Column<int>(type: "int", nullable: false),
                    id_empleado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cotizacion", x => x.id_cotizacion);
                    table.ForeignKey(
                        name: "fk_cotizacion_cliente",
                        column: x => x.id_cliente,
                        principalTable: "cliente",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_cotizacion_empleado",
                        column: x => x.id_empleado,
                        principalTable: "empleado",
                        principalColumn: "id_empleado",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_cotizacion_vehiculo",
                        column: x => x.id_vehiculo,
                        principalTable: "vehiculo",
                        principalColumn: "id_vehiculo",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "prueba_manejo",
                columns: table => new
                {
                    id_prueba = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fecha_hora = table.Column<DateTime>(type: "datetime", nullable: false),
                    estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    observaciones = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    id_vehiculo = table.Column<int>(type: "int", nullable: false),
                    id_empleado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prueba_manejo", x => x.id_prueba);
                    table.ForeignKey(
                        name: "fk_prueba_manejo_cliente",
                        column: x => x.id_cliente,
                        principalTable: "cliente",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_prueba_manejo_empleado",
                        column: x => x.id_empleado,
                        principalTable: "empleado",
                        principalColumn: "id_empleado",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_prueba_manejo_vehiculo",
                        column: x => x.id_vehiculo,
                        principalTable: "vehiculo",
                        principalColumn: "id_vehiculo",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "ix_cotizacion_id_cliente",
                table: "cotizacion",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "ix_cotizacion_id_empleado",
                table: "cotizacion",
                column: "id_empleado");

            migrationBuilder.CreateIndex(
                name: "ix_cotizacion_id_vehiculo",
                table: "cotizacion",
                column: "id_vehiculo");

            migrationBuilder.CreateIndex(
                name: "ix_prueba_manejo_id_cliente",
                table: "prueba_manejo",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "ix_prueba_manejo_id_empleado",
                table: "prueba_manejo",
                column: "id_empleado");

            migrationBuilder.CreateIndex(
                name: "ix_prueba_manejo_vehiculo_fecha",
                table: "prueba_manejo",
                columns: new[] { "id_vehiculo", "fecha_hora" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cotizacion");

            migrationBuilder.DropTable(
                name: "prueba_manejo");
        }
    }
}
