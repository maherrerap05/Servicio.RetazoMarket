namespace Servicio.RetazoMarket.Business.DTOs.Usuario;
public class UsuarioResponse
{
    public int id_usuario { get; set; }
    public int? id_cliente { get; set; }
    public int id_rol { get; set; }
    public string nombre { get; set; } = null!;
    public string correo { get; set; } = null!;
    public string usr_estado { get; set; } = null!;
    public DateTime ultimo_acceso { get; set; }
    public string? nombre_rol { get; set; }
}
