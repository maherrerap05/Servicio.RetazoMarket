namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class UsuarioEntity
    {
        // =========================
        // CLAVE PRIMARIA
        // =========================
        public int id_usuario { get; set; }

        // =========================
        // CLAVES FORÁNEAS
        // =========================
        public int? id_cliente { get; set; }
        public int id_rol { get; set; }

        // =========================
        // IDENTIFICACIÓN
        // =========================
        public string nombre { get; set; } = null!;
        public string correo { get; set; } = null!;

        // =========================
        // SEGURIDAD
        // =========================
        public string contrasena_hash { get; set; } = null!;

        // =========================
        // ESTADO Y ACCESO
        // =========================
        public string usr_estado { get; set; } = null!;
        public DateTime ultimo_acceso { get; set; }

        // =========================
        // RELACIONES
        // =========================
        public ClienteEntity? Cliente { get; set; }
        public RolEntity Rol { get; set; } = null!;
    }
}
