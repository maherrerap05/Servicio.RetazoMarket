using System.Text.Json;

namespace Servicio.RetazoMarket.DataAccess.Entities
{
    public class PersonalizacionEntity
    {
        public int id_opcion { get; set; }
        public int id_producto { get; set; }
        public string nombre_atr { get; set; } = null!;
        public string tipo_valor { get; set; } = null!;
        public JsonDocument valores_json { get; set; } = null!;
        public decimal costo_adicional { get; set; }

        public ProductoEntity Producto { get; set; } = null!;
    }
}
