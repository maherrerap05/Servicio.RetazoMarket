namespace Servicio.RetazoMarket.Business.DTOs.Usuario;
public class UsuarioFiltroRequest
{
    public string? nombre { get; set; }
    public string? correo { get; set; }
    public string? estado { get; set; }
    public int? id_rol { get; set; }
    public int? id_cliente { get; set; }
    public int page_number { get; set; } = 1;
    public int page_size { get; set; } = 10;
}
