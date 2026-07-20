namespace Servicio.RetazoMarket.Business.DTOs.Cliente;
public class CrearClienteRequest
{
    public string nombre { get; set; } = null!;
    public string? apellidos { get; set; }
    public string correo { get; set; } = null!;
    public string telefono { get; set; } = null!;
    public string direccion { get; set; } = null!;
    public string origen { get; set; } = "FIS";
    public string cli_estado { get; set; } = "ACT";
}
