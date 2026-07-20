namespace Servicio.RetazoMarket.Business.DTOs.Linea
{
    public class LineaFiltroRequest
    {
        public string? lin_nombre { get; set; }
        public string? lin_estado { get; set; }
        public int page_number { get; set; } = 1;
        public int page_size { get; set; } = 10;
    }
}
