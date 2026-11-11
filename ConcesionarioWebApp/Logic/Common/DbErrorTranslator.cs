using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Logic.Common;

/// <summary>
/// Traduce los errores de MySQL / EF Core a mensajes entendibles para el usuario,
/// evitando mostrar en pantalla textos técnicos como "Cannot add or update a child row...".
/// </summary>
internal static partial class DbErrorTranslator
{
    // Nombre del índice único -> descripción del campo
    private static readonly Dictionary<string, string> UniqueFields = new(StringComparer.OrdinalIgnoreCase)
    {
        ["uq_cliente_cedula"] = "la cédula",
        ["uq_empleado_cedula"] = "la cédula",
        ["uq_vehiculo_vin"] = "el VIN",
        ["uq_usuario_nombre_usuario"] = "el nombre de usuario",
        ["uq_usuario_correo"] = "el correo",
        ["uq_usuario_id_empleado"] = "el empleado (ya tiene un usuario asignado)",
    };

    public static BusinessException Translate(Exception ex)
    {
        if (ex is DbUpdateConcurrencyException)
        {
            return new BusinessException("El registro ya no existe o fue modificado por otro usuario. Recargue la página.", ex);
        }

        var mySqlEx = FindMySqlException(ex);
        if (mySqlEx == null)
        {
            return new BusinessException("Ocurrió un error al guardar los datos.", ex);
        }

        var message = mySqlEx.ErrorCode switch
        {
            MySqlErrorCode.DuplicateKeyEntry => DuplicateMessage(mySqlEx.Message),
            MySqlErrorCode.RowIsReferenced2 or MySqlErrorCode.RowIsReferenced =>
                "No se puede eliminar porque tiene información asociada (ventas, pagos, garantías, etc.). " +
                "Elimine primero los registros que dependen de él.",
            MySqlErrorCode.NoReferencedRow2 or MySqlErrorCode.NoReferencedRow =>
                "El registro relacionado que seleccionó no existe.",
            MySqlErrorCode.DataTooLong => "Uno de los valores supera la longitud permitida.",
            MySqlErrorCode.ColumnCannotBeNull => "Hay un campo obligatorio sin diligenciar.",
            _ => "Ocurrió un error en la base de datos al procesar la solicitud."
        };

        return new BusinessException(message, ex);
    }

    private static MySqlException? FindMySqlException(Exception? ex)
    {
        while (ex != null)
        {
            if (ex is MySqlException mySqlEx)
            {
                return mySqlEx;
            }

            ex = ex.InnerException;
        }

        return null;
    }

    private static string DuplicateMessage(string rawMessage)
    {
        // Formato MySQL: Duplicate entry 'valor' for key 'tabla.indice'
        var match = DuplicateEntryRegex().Match(rawMessage);
        if (!match.Success)
        {
            return "Ya existe un registro con esos datos.";
        }

        var value = match.Groups["value"].Value;
        var key = match.Groups["key"].Value.Split('.').Last();
        var field = UniqueFields.GetValueOrDefault(key, "ese valor");

        return $"Ya existe un registro con {field} '{value}'.";
    }

    [GeneratedRegex("Duplicate entry '(?<value>.*)' for key '(?<key>[^']+)'")]
    private static partial Regex DuplicateEntryRegex();
}
