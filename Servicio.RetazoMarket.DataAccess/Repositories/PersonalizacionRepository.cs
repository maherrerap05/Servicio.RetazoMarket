using Microsoft.EntityFrameworkCore;
using Servicio.RetazoMarket.DataAccess.Context;
using Servicio.RetazoMarket.DataAccess.Entities;
using Servicio.RetazoMarket.DataAccess.Repositories.Interfaces;

namespace Servicio.RetazoMarket.DataAccess.Repositories
{
    public class PersonalizacionRepository : IPersonalizacionRepository
    {
        private readonly RetazoMarketDbContext _context;
        public PersonalizacionRepository(RetazoMarketDbContext context) => _context = context;
        public async Task<PersonalizacionEntity?> ObtenerPorIdAsync(int id_opcion, CancellationToken cancellationToken = default) =>
            await _context.Personalizaciones.AsNoTracking().FirstOrDefaultAsync(p => p.id_opcion == id_opcion, cancellationToken);
        public async Task<PersonalizacionEntity?> ObtenerParaActualizarAsync(int id_opcion, CancellationToken cancellationToken = default) =>
            await _context.Personalizaciones.FirstOrDefaultAsync(p => p.id_opcion == id_opcion, cancellationToken);
        public async Task<IReadOnlyList<PersonalizacionEntity>> ObtenerPorProductoAsync(int id_producto, CancellationToken cancellationToken = default) =>
            await _context.Personalizaciones.AsNoTracking().Where(p => p.id_producto == id_producto).OrderBy(p => p.id_opcion).ToListAsync(cancellationToken);
        public async Task AgregarAsync(PersonalizacionEntity personalizacion, CancellationToken cancellationToken = default) =>
            await _context.Personalizaciones.AddAsync(personalizacion, cancellationToken);
        public void Actualizar(PersonalizacionEntity personalizacion) => _context.Personalizaciones.Update(personalizacion);
    }
}
