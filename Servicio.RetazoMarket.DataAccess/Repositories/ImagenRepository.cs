using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class ImagenRepository : IImagenRepository
    {
        private readonly RetazoMarketDbContext _context;
        public ImagenRepository(RetazoMarketDbContext context) => _context = context;
        public async Task<ImagenEntity?> ObtenerPorIdAsync(int id_imagen, CancellationToken cancellationToken = default) =>
            await _context.Imagenes.AsNoTracking().FirstOrDefaultAsync(i => i.id_imagen == id_imagen, cancellationToken);
        public async Task<ImagenEntity?> ObtenerParaActualizarAsync(int id_imagen, CancellationToken cancellationToken = default) =>
            await _context.Imagenes.FirstOrDefaultAsync(i => i.id_imagen == id_imagen, cancellationToken);
        public async Task<IReadOnlyList<ImagenEntity>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default) =>
            await _context.Imagenes.AsNoTracking().Where(i => i.id_producto == id_producto).OrderBy(i => i.orden).ThenBy(i => i.id_imagen).ToListAsync(cancellationToken);
        public async Task AgregarAsync(ImagenEntity imagen, CancellationToken cancellationToken = default) =>
            await _context.Imagenes.AddAsync(imagen, cancellationToken);
        public void Actualizar(ImagenEntity imagen) => _context.Imagenes.Update(imagen);
    }
}
