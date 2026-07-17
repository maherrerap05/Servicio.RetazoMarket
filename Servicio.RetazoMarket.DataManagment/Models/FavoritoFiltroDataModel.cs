namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class FavoritoFiltroDataModel
    {
        public int id_cliente { get; set; }
        public string? nombre_producto { get; set; }
        public int? id_linea { get; set; }
        public string? estado_producto { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
