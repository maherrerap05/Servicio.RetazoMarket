using Servicio.RetazoMarket.Business.DTOs.Cliente;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.Business.Mappers;
public static class ClienteBusinessMapper
{
    public static ClienteDataModel ToDataModel(CrearClienteRequest r) => new() { nombre=r.nombre.Trim(), apellidos=r.apellidos?.Trim(), correo=r.correo.Trim().ToLowerInvariant(), telefono=r.telefono.Trim(), direccion=r.direccion.Trim(), origen=r.origen.Trim().ToUpperInvariant(), cli_estado=r.cli_estado.Trim().ToUpperInvariant(), fecha_registro=DateTime.UtcNow };
    public static ClienteDataModel ToDataModel(ActualizarClienteRequest r) => new() { id_cliente=r.id_cliente, nombre=r.nombre.Trim(), apellidos=r.apellidos?.Trim(), correo=r.correo.Trim().ToLowerInvariant(), telefono=r.telefono.Trim(), direccion=r.direccion.Trim(), origen=r.origen.Trim().ToUpperInvariant(), cli_estado=r.cli_estado.Trim().ToUpperInvariant() };
    public static ClienteResponse ToResponse(ClienteDataModel m) => new() { id_cliente=m.id_cliente, nombre=m.nombre, apellidos=m.apellidos, correo=m.correo, telefono=m.telefono, direccion=m.direccion, origen=m.origen, cli_estado=m.cli_estado, fecha_registro=m.fecha_registro };
}
