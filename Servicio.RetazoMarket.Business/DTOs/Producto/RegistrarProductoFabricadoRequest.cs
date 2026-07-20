using Servicio.RetazoMarket.Business.DTOs.ProductoMaterial;
namespace Servicio.RetazoMarket.Business.DTOs.Producto;
public class RegistrarProductoFabricadoRequest { public int? id_producto {get;set;} public CrearProductoRequest? producto_nuevo {get;set;} public IReadOnlyCollection<CrearProductoMaterialRequest> receta {get;set;}=Array.Empty<CrearProductoMaterialRequest>(); public int cantidad_fabricada {get;set;} public string motivo {get;set;}="FABRICACION"; }
