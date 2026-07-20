namespace Servicio.RetazoMarket.Business.DTOs.Material;
public class MaterialFiltroRequest { public string? nombre {get;set;} public int? id_categoria {get;set;} public string? estado {get;set;} public decimal? stock_minimo {get;set;} public decimal? stock_maximo {get;set;} public int page_number {get;set;}=1; public int page_size {get;set;}=10; }
