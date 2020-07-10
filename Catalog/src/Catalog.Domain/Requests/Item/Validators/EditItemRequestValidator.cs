using Catalog.Domain.Requests.Artist;
using Catalog.Domain.Requests.Genre;
using Catalog.Domain.Services;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Domain.Requests.Item.Validators
{
    public class EditItemRequestValidator : AbstractValidator<EditItemRequest>
    {
        private readonly IArtistService _artistService;
        private readonly IGenreService _genreService;

        public EditItemRequestValidator(IArtistService artistService, IGenreService genreService)
        {
            _artistService = artistService;
            _genreService = genreService;

            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.GenreId)
                .NotEmpty()
                .MustAsync(GenreExistsAsync).WithMessage("Genre must exists");
            RuleFor(x => x.ArtistId)
                .NotEmpty()
                .MustAsync(ArtistExistsAsync).WithMessage("Artist must exists");
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.Price).Must(x => x?.Amount > 0);
            RuleFor(x => x.ReleaseDate).NotEmpty();            
        }

        private async Task<bool> ArtistExistsAsync(Guid artistId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(artistId.ToString()))
                return false;

            var artist = await _artistService.GetArtistAsync(new GetArtistRequest { Id = artistId });

            return artist != default;
        }

        private async Task<bool> GenreExistsAsync(Guid genreId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(genreId.ToString()))
                return false;

            var genre = await _genreService.GetGenreAsync(new GetGenreRequest { Id = genreId });

            return genre != default;
        }
    }
}
