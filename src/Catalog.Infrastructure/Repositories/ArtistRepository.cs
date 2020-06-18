using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly CatalogContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public ArtistRepository(CatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Artist Add(Artist artist) => _context.Artists.Add(artist).Entity;

        public async Task<IEnumerable<Artist>> GetAsync()
        {
            return await _context.Artists
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Artist> GetAsync(Guid id)
        {
            var artist = await _context.Artists.FindAsync(id);

            if (artist == default) return default;

            _context.Entry(artist).State = EntityState.Detached;
            return artist;
        }
    }
}
