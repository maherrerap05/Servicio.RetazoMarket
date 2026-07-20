namespace Servicio.RetazoMarket.Business.DTOs.ProveedorMaterial;
public class ActualizarProveedorMaterialRequest { public int id_material {get;set;} public int id_proveedor {get;set;} public string origen {get;set;}=null!; public decimal precio_compra {get;set;} public int cantidad_min {get;set;} public int dias_entrega {get;set;} }
