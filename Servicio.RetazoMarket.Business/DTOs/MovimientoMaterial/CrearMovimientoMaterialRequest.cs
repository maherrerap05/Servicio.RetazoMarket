namespace Servicio.RetazoMarket.Business.DTOs.MovimientoMaterial;
public class CrearMovimientoMaterialRequest { public int id_material {get;set;} public string tipo_movimiento {get;set;}=null!; public decimal cantidad {get;set;} public string motivo_mov {get;set;}=null!; public string? metodo_pago {get;set;} }
