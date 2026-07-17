namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class CategoriaMaterialEntity
    {
        // =========================
        // CLAVE PRIMARIA
        // =========================
        public int id_categoria { get; set; }

        // =========================
        // CAMPOS PRINCIPALES
        // =========================
        public string cat_nombre { get; set; } = null!;

        // =========================
        // ESTADO / CICLO DE VIDA
        // =========================
        public string cat_estado { get; set; } = null!;

        // =========================
        // RELACIONES
        // =========================
        public ICollection<MaterialEntity> Materiales { get; set; } = new List<MaterialEntity>();
    }
}
