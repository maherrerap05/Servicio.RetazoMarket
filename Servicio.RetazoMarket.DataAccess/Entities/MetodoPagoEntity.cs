namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class MetodoPagoEntity
    {
        // =========================
        // CLAVE PRIMARIA
        // =========================
        public int id_metodo { get; set; }

        // =========================
        // CAMPOS PRINCIPALES
        // =========================
        public string met_nombre { get; set; } = null!;
        public string codigo_sri { get; set; } = null!;

        public ICollection<PedidoEntity> Pedidos { get; set; } = new List<PedidoEntity>();
    }
}
