namespace Servicio.RetazoMarket.Business.DTOs.CategoriaMaterial
{
    public class CategoriaMaterialResponse
    {
        public int id_categoria { get; set; }
        public string cat_nombre { get; set; } = null!;
        public string cat_estado { get; set; } = null!;
    }
}
