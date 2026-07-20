namespace Servicio.RetazoMarket.Business.DTOs.Material;
public class MaterialResponse { public int id_material {get;set;} public int id_categoria {get;set;} public string mat_nombre {get;set;}=null!; public string unidad_medida {get;set;}=null!; public decimal stock_actual {get;set;} public string mat_estado {get;set;}=null!; public string? categoria_nombre {get;set;} }
