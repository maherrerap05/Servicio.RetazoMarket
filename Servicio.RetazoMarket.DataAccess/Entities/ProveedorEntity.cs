namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class ProveedorEntity
    {
        public int id_proveedor { get; set; }
        public string prov_nombre { get; set; } = null!;
        public string prov_telefono { get; set; } = null!;
        public string prov_correo { get; set; } = null!;
        public string prov_direccion { get; set; } = null!;
        public string prov_estado { get; set; } = null!;

        public ICollection<ProveedorMaterialEntity> MaterialesSuministrados { get; set; } = new List<ProveedorMaterialEntity>();
    }
}
