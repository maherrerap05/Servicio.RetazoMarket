namespace Servicio.RetazoMarket.Business.DTOs.Usuario;
public class CrearUsuarioRequest
{
    public int? id_cliente { get; set; }
    public int id_rol { get; set; }
    public string nombre { get; set; } = null!;
    public string correo { get; set; } = null!;
    public string contrasena { get; set; } = null!;
    public string usr_estado { get; set; } = "ACT";
}
