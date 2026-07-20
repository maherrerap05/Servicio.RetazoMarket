namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class DescuentoFiltroDataModel
    {
        public int? id_producto { get; set; }
        public int? cantidad_minima { get; set; }
        public decimal? porcentaje { get; set; }
        public string? estado { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
