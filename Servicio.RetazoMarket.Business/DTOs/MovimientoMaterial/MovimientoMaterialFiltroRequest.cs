namespace Servicio.RetazoMarket.Business.DTOs.MovimientoMaterial;
public class MovimientoMaterialFiltroRequest { public int? id_material {get;set;} public string? tipo_movimiento {get;set;} public string? motivo {get;set;} public DateTime? fecha_desde_utc {get;set;} public DateTime? fecha_hasta_utc {get;set;} public int page_number {get;set;}=1; public int page_size {get;set;}=10; }
