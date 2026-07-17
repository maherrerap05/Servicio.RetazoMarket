namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class ProveedorFiltroDataModel
    {
        public string? nombre { get; set; }
        public string? correo { get; set; }
        public string? estado { get; set; }
        public int? id_material { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
