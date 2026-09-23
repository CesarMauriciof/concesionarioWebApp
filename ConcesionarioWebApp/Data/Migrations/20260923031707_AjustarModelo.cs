using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AjustarModelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdCliente",
                table: "Clients",
                newName: "Id_Cliente");

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id_Cliente", "Cedula", "Correo", "Direccion", "Nombre", "Telefono" },
                values: new object[] { 1, "001", "Yeison@gmail.com", "carrera 5 #24-10", "Yeison Anaya", "310546891" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id_Cliente",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "Id_Cliente",
                table: "Clients",
                newName: "IdCliente");
        }
    }
}
