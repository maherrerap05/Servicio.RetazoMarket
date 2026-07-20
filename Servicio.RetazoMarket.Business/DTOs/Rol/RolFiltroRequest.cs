namespace Servicio.RetazoMarket.Business.DTOs.Rol
{
    public class RolFiltroRequest
    {
        public string? nombre_rol { get; set; }
        public int page_number { get; set; } = 1;
        public int page_size { get; set; } = 10;
    }
}
