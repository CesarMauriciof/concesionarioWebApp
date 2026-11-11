using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class EsquemaInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cedula = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    correo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    direccion = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.id_cliente);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "sede",
                columns: table => new
                {
                    id_sede = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre_sede = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ciudad = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    direccion = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sede", x => x.id_sede);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "empleado",
                columns: table => new
                {
                    id_empleado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cedula = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cargo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    salario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    id_sede = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empleado", x => x.id_empleado);
                    table.ForeignKey(
                        name: "fk_empleado_sede",
                        column: x => x.id_sede,
                        principalTable: "sede",
                        principalColumn: "id_sede",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "vehiculo",
                columns: table => new
                {
                    id_vehiculo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    vin = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    marca = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    modelo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    precio = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    id_sede = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehiculo", x => x.id_vehiculo);
                    table.ForeignKey(
                        name: "fk_vehiculo_sede",
                        column: x => x.id_sede,
                        principalTable: "sede",
                        principalColumn: "id_sede",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "venta",
                columns: table => new
                {
                    id_venta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fecha = table.Column<DateTime>(type: "date", nullable: false),
                    total_pagado = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    id_cliente = table.Column<int>(type: "int", nullable: true),
                    id_empleado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venta", x => x.id_venta);
                    table.ForeignKey(
                        name: "fk_venta_cliente",
                        column: x => x.id_cliente,
                        principalTable: "cliente",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_venta_empleado",
                        column: x => x.id_empleado,
                        principalTable: "empleado",
                        principalColumn: "id_empleado",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "detalle_venta",
                columns: table => new
                {
                    id_detalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_venta = table.Column<int>(type: "int", nullable: false),
                    id_vehiculo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalle_venta", x => x.id_detalle);
                    table.ForeignKey(
                        name: "fk_detalle_venta_vehiculo",
                        column: x => x.id_vehiculo,
                        principalTable: "vehiculo",
                        principalColumn: "id_vehiculo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_detalle_venta_venta",
                        column: x => x.id_venta,
                        principalTable: "venta",
                        principalColumn: "id_venta",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "pago",
                columns: table => new
                {
                    id_pago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_venta = table.Column<int>(type: "int", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    fecha_pago = table.Column<DateTime>(type: "datetime", nullable: false),
                    metodo_pago = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pago", x => x.id_pago);
                    table.ForeignKey(
                        name: "fk_pago_venta",
                        column: x => x.id_venta,
                        principalTable: "venta",
                        principalColumn: "id_venta",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "garantia",
                columns: table => new
                {
                    id_garantia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_detalle = table.Column<int>(type: "int", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "date", nullable: false),
                    condiciones = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_garantia", x => x.id_garantia);
                    table.ForeignKey(
                        name: "fk_garantia_detalle_venta",
                        column: x => x.id_detalle,
                        principalTable: "detalle_venta",
                        principalColumn: "id_detalle",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "credito",
                columns: table => new
                {
                    id_credito = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_pago = table.Column<int>(type: "int", nullable: false),
                    entidad_bancaria = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    monto_aprobado = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    numero_cuotas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credito", x => x.id_credito);
                    table.ForeignKey(
                        name: "fk_credito_pago",
                        column: x => x.id_pago,
                        principalTable: "pago",
                        principalColumn: "id_pago",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "mantenimiento",
                columns: table => new
                {
                    id_mantenimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_garantia = table.Column<int>(type: "int", nullable: false),
                    kilometraje = table.Column<int>(type: "int", nullable: false),
                    anio_mantenimiento = table.Column<int>(type: "int", nullable: false),
                    servicios_realizados = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_programada = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mantenimiento", x => x.id_mantenimiento);
                    table.ForeignKey(
                        name: "fk_mantenimiento_garantia",
                        column: x => x.id_garantia,
                        principalTable: "garantia",
                        principalColumn: "id_garantia",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "uq_cliente_cedula",
                table: "cliente",
                column: "cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_credito_id_pago",
                table: "credito",
                column: "id_pago");

            migrationBuilder.CreateIndex(
                name: "ix_detalle_venta_id_vehiculo",
                table: "detalle_venta",
                column: "id_vehiculo");

            migrationBuilder.CreateIndex(
                name: "ix_detalle_venta_id_venta",
                table: "detalle_venta",
                column: "id_venta");

            migrationBuilder.CreateIndex(
                name: "ix_empleado_id_sede",
                table: "empleado",
                column: "id_sede");

            migrationBuilder.CreateIndex(
                name: "uq_empleado_cedula",
                table: "empleado",
                column: "cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_garantia_id_detalle",
                table: "garantia",
                column: "id_detalle");

            migrationBuilder.CreateIndex(
                name: "ix_mantenimiento_id_garantia",
                table: "mantenimiento",
                column: "id_garantia");

            migrationBuilder.CreateIndex(
                name: "ix_pago_id_venta",
                table: "pago",
                column: "id_venta");

            migrationBuilder.CreateIndex(
                name: "ix_vehiculo_id_sede",
                table: "vehiculo",
                column: "id_sede");

            migrationBuilder.CreateIndex(
                name: "uq_vehiculo_vin",
                table: "vehiculo",
                column: "vin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_venta_id_cliente",
                table: "venta",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "ix_venta_id_empleado",
                table: "venta",
                column: "id_empleado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "credito");

            migrationBuilder.DropTable(
                name: "mantenimiento");

            migrationBuilder.DropTable(
                name: "pago");

            migrationBuilder.DropTable(
                name: "garantia");

            migrationBuilder.DropTable(
                name: "detalle_venta");

            migrationBuilder.DropTable(
                name: "vehiculo");

            migrationBuilder.DropTable(
                name: "venta");

            migrationBuilder.DropTable(
                name: "cliente");

            migrationBuilder.DropTable(
                name: "empleado");

            migrationBuilder.DropTable(
                name: "sede");
        }
    }
}
