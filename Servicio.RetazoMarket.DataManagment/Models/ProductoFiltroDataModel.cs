namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class ProductoFiltroDataModel
    {
        public string? nombre { get; set; }
        public int? id_linea { get; set; }
        public string? estado { get; set; }
        public string? es_personalizable { get; set; }
        public bool? conStock { get; set; }
        public decimal? precioMinimo { get; set; }
        public decimal? precioMaximo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
