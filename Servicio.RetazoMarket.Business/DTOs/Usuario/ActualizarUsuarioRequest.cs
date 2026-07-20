namespace Servicio.RetazoMarket.Business.DTOs.Usuario;
public class ActualizarUsuarioRequest
{
    public int id_usuario { get; set; }
    public int? id_cliente { get; set; }
    public int id_rol { get; set; }
    public string nombre { get; set; } = null!;
    public string correo { get; set; } = null!;
    public string usr_estado { get; set; } = null!;
}
