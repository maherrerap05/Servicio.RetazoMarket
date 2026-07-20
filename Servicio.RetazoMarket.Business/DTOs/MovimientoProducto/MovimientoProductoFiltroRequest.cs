namespace Servicio.RetazoMarket.Business.DTOs.MovimientoProducto;
public class MovimientoProductoFiltroRequest { public int? id_producto {get;set;} public string? tipo_movimiento {get;set;} public string? motivo {get;set;} public DateTime? fecha_desde_utc {get;set;} public DateTime? fecha_hasta_utc {get;set;} public int page_number {get;set;}=1; public int page_size {get;set;}=10; }
