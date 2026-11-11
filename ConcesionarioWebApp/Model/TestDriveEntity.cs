using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

/// <summary>Prueba de manejo: cita de un cliente para probar un vehículo acompañado por un asesor.</summary>
[Table("prueba_manejo")]
public class TestDriveEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_prueba")]
    public int IdPrueba { get; set; }

    [Column("fecha_hora", TypeName = "datetime")]
    public DateTime FechaHora { get; set; } = DateTime.Today.AddDays(1).AddHours(9);

    [Column("estado")]
    public TestDriveStatus Estado { get; set; } = TestDriveStatus.Programada;

    [StringLength(250)]
    [Column("observaciones")]
    public string? Observaciones { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente.")]
    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [ForeignKey(nameof(IdCliente))]
    public virtual ClientEntity? Cliente { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un vehículo.")]
    [Column("id_vehiculo")]
    public int IdVehiculo { get; set; }

    [ForeignKey(nameof(IdVehiculo))]
    public virtual VehicleEntity? Vehiculo { get; set; }

    // Asesor que acompaña la prueba (opcional: si el empleado se elimina, el registro se conserva)
    [Column("id_empleado")]
    public int? IdEmpleado { get; set; }

    [ForeignKey(nameof(IdEmpleado))]
    public virtual EmployeeEntity? Empleado { get; set; }
}
