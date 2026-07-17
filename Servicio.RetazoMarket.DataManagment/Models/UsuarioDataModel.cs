namespace Servicio.RetazoMarket.DataManagment.Models
{
    public class UsuarioDataModel
    {
        public int id_usuario { get; set; }
        public int? id_cliente { get; set; }
        public int id_rol { get; set; }
        public string nombre { get; set; } = null!;
        public string correo { get; set; } = null!;
        public string contrasena_hash { get; set; } = null!;
        public string usr_estado { get; set; } = null!;
        public DateTime ultimo_acceso { get; set; }
        public RolResumenDataModel? Rol { get; set; }
        public ClienteResumenDataModel? Cliente { get; set; }
    }

    public class RolResumenDataModel
    {
        public int id_rol { get; set; }
        public string nombre_rol { get; set; } = null!;
    }

    public class ClienteResumenDataModel
    {
        public int id_cliente { get; set; }
        public string nombre { get; set; } = null!;
        public string? apellidos { get; set; }
        public string correo { get; set; } = null!;
    }
}
