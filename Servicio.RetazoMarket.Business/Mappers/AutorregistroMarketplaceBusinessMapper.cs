using Servicio.RetazoMarket.Business.DTOs.Auth;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.Business.Mappers
{
    public static class AutorregistroMarketplaceBusinessMapper
    {
        public static AutorregistroMarketplaceDataModel ToDataModel(
            AutorregistroMarketplaceRequest request,
            int idRolCliente,
            string passwordHash)
        {
            var correo = request.correo.Trim().ToLowerInvariant();
            return new AutorregistroMarketplaceDataModel
            {
                Cliente = new ClienteDataModel
                {
                    nombre = request.nombre.Trim(),
                    apellidos = request.apellidos?.Trim(),
                    correo = correo,
                    telefono = request.telefono.Trim(),
                    direccion = request.direccion.Trim(),
                    origen = "MKT",
                    cli_estado = "ACT",
                    fecha_registro = DateTime.UtcNow
                },
                Usuario = new UsuarioDataModel
                {
                    id_rol = idRolCliente,
                    nombre = request.nombre.Trim(),
                    correo = correo,
                    contrasena_hash = passwordHash,
                    usr_estado = "ACT",
                    ultimo_acceso = DateTime.UtcNow
                }
            };
        }

        public static AutorregistroMarketplaceResponse ToResponse(
            AutorregistroMarketplaceResultDataModel result) => new()
        {
            Cliente = ClienteBusinessMapper.ToResponse(result.Cliente),
            Usuario = UsuarioBusinessMapper.ToResponse(result.Usuario)
        };
    }
}
