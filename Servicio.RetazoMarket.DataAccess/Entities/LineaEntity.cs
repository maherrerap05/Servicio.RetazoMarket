namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class LineaEntity
    {
        // =========================
        // CLAVE PRIMARIA
        // =========================
        public int id_linea { get; set; }

        // =========================
        // CAMPOS PRINCIPALES
        // =========================
        public string? lin_nombre { get; set; }

        // =========================
        // ESTADO / CICLO DE VIDA
        // =========================
        public string? lin_estado { get; set; }

        public ICollection<ProductoEntity> Productos { get; set; } = new List<ProductoEntity>();
    }
}
