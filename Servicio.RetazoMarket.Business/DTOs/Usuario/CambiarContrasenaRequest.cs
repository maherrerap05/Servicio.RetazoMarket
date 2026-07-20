namespace Servicio.RetazoMarket.Business.DTOs.Usuario;
public class CambiarContrasenaRequest
{
    public int id_usuario { get; set; }
    public string contrasena_actual { get; set; } = null!;
    public string contrasena_nueva { get; set; } = null!;
}
