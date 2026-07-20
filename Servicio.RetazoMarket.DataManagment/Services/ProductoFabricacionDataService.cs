using System.Data;
using Servicio.RetazoMarket.DataManagment.Interfaces;
using Servicio.RetazoMarket.DataManagment.Mappers;
using Servicio.RetazoMarket.DataManagment.Models;

namespace Servicio.RetazoMarket.DataManagment.Services;

public class ProductoFabricacionDataService : IProductoFabricacionDataService
{
    private readonly IUnitOfWork _unitOfWork;
    public ProductoFabricacionDataService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<ProductoDataModel> RegistrarAsync(RegistrarProductoFabricadoDataModel model,CancellationToken cancellationToken=default)
    {
        await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable,cancellationToken);
        try
        {
            var producto = model.id_producto.HasValue
                ? await _unitOfWork.ProductoRepository.ObtenerParaActualizarAsync(model.id_producto.Value,cancellationToken)
                : model.ProductoNuevo is null ? null : ProductoDataMapper.ToEntity(model.ProductoNuevo);
            if(producto is null) throw new InvalidOperationException("El producto no existe.");

            if(!model.id_producto.HasValue)
            {
                producto.stock_actual=0;
                await _unitOfWork.ProductoRepository.AgregarAsync(producto,cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                foreach(var item in model.RecetaNueva)
                {
                    item.id_producto=producto.id_producto;
                    await _unitOfWork.ProductoMaterialRepository.AgregarAsync(ProductoMaterialDataMapper.ToEntity(item),cancellationToken);
                }
            }

            foreach(var consumo in model.Materiales)
            {
                var material=await _unitOfWork.MaterialRepository.ObtenerParaActualizarAsync(consumo.id_material,cancellationToken)
                    ?? throw new InvalidOperationException($"El material {consumo.id_material} no existe.");
                if(material.stock_actual<consumo.cantidad_consumir)
                    throw new InvalidOperationException($"Stock insuficiente para el material {material.mat_nombre}.");
                material.stock_actual-=consumo.cantidad_consumir;
                _unitOfWork.MaterialRepository.Actualizar(material);
                await _unitOfWork.MovimientoMaterialRepository.AgregarAsync(MovimientoMaterialDataMapper.ToEntity(new MovimientoMaterialDataModel{id_material=material.id_material,tipo_movimiento="EGR",cantidad=consumo.cantidad_consumir,fecha_mov=DateTime.UtcNow,motivo_mov=model.motivo}),cancellationToken);
            }

            producto.stock_actual=checked(producto.stock_actual+model.cantidad_fabricada);
            _unitOfWork.ProductoRepository.Actualizar(producto);
            await _unitOfWork.MovimientoProductoRepository.AgregarAsync(MovimientoProductoDataMapper.ToEntity(new MovimientoProductoDataModel{id_producto=producto.id_producto,tipo_movimiento="ING",cantidad=model.cantidad_fabricada,fecha_mov=DateTime.UtcNow,motivo_mov=model.motivo}),cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ProductoDataMapper.ToDataModel(producto);
        }
        catch
        {
            try { await _unitOfWork.RollbackTransactionAsync(CancellationToken.None); }
            finally { _unitOfWork.DescartarCambios(); }
            throw;
        }
    }
}
