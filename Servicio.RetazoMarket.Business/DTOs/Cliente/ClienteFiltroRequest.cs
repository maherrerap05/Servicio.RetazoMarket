namespace Servicio.RetazoMarket.Business.DTOs.Cliente;
public class ClienteFiltroRequest
{
    public string? nombre { get; set; }
    public string? apellidos { get; set; }
    public string? correo { get; set; }
    public string? telefono { get; set; }
    public string? origen { get; set; }
    public string? estado { get; set; }
    public int page_number { get; set; } = 1;
    public int page_size { get; set; } = 10;
}
