namespace Servicio.RetazoMarket.Business.DTOs.Proveedor;
public class ProveedorFiltroRequest { public string? nombre {get;set;} public string? correo {get;set;} public string? estado {get;set;} public int? id_material {get;set;} public int page_number {get;set;}=1; public int page_size {get;set;}=10; }
