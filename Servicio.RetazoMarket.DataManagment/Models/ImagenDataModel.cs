namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class ImagenDataModel
    {
        public int id_imagen { get; set; }
        public int id_producto { get; set; }
        public string url { get; set; } = null!;
        public string es_principal { get; set; } = null!;
        public int orden { get; set; }
    }
}
