namespace Servicio.RetazoMarket.Business.DTOs.CategoriaMaterial
{
    public class CrearCategoriaMaterialRequest
    {
        public string cat_nombre { get; set; } = null!;
        public string cat_estado { get; set; } = "ACT";
    }
}
