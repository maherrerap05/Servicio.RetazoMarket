namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class MaterialFiltroDataModel
    {
        public string? nombre { get; set; }
        public int? id_categoria { get; set; }
        public string? estado { get; set; }
        public int? stockMinimo { get; set; }
        public int? stockMaximo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
