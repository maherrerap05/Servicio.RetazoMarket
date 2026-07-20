namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class MaterialFiltroDataModel
    {
        public string? nombre { get; set; }
        public int? id_categoria { get; set; }
        public string? estado { get; set; }
        public decimal? stockMinimo { get; set; }
        public decimal? stockMaximo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
