namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class RolEntity
    {
        // =========================
        // CLAVE PRIMARIA
        // =========================
        public int id_rol { get; set; }

        // =========================
        // IDENTIFICACIÓN
        // =========================
        public string nombre_rol { get; set; } = null!;

        // =========================
        // RELACIONES
        // =========================
        public ICollection<UsuarioEntity> Usuarios { get; set; } = new List<UsuarioEntity>();
    }
}
