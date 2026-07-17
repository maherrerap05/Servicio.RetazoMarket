namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class ClienteEntity
    {
        // =========================
        // CLAVE PRIMARIA
        // =========================
        public int id_cliente { get; set; }

        // =========================
        // INFORMACIÓN PERSONAL
        // =========================
        public string nombre { get; set; } = null!;
        public string? apellidos { get; set; }

        // =========================
        // INFORMACIÓN DE CONTACTO
        // =========================
        public string correo { get; set; } = null!;
        public string telefono { get; set; } = null!;
        public string direccion { get; set; } = null!;

        // =========================
        // ORIGEN Y ESTADO
        // =========================
        public string origen { get; set; } = null!;
        public string cli_estado { get; set; } = null!;

        // =========================
        // REGISTRO
        // =========================
        public DateTime fecha_registro { get; set; }

        // =========================
        // RELACIONES
        // =========================
        public ICollection<UsuarioEntity> Usuarios { get; set; } = new List<UsuarioEntity>();
        public ICollection<FavoritoEntity> Favoritos { get; set; } = new List<FavoritoEntity>();
        public ICollection<PedidoEntity> Pedidos { get; set; } = new List<PedidoEntity>();
    }
}
