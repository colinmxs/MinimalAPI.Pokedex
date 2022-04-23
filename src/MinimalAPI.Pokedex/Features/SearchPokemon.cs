namespace MinimalAPI.Pokedex.Features;
using MinalAPI.Pokedex.Models;
using MinimalAPI.Pokedex.Services;
public class SearchPokemon
{
    public static class RouteMapper
    {
        public static void Map(IEndpointRouteBuilder builder) =>
            builder.MapGet("/pokedex/search", async (string query, int? page, int? pageSize, IMediator mediator) => 
            await mediator.Send(new Query(query, page ?? 1, pageSize ?? 10)))            
            .Produces<PokedexPagedResponse>(StatusCodes.Status200OK);
    }

    public record Query(string SearchTerm, int Page, int PageSize) : IRequest<PokedexPagedResponse>;

    public class QueryHandler : IRequestHandler<Query, PokedexPagedResponse>
    {
        private readonly IPokedexRepository _repo;
        public QueryHandler(IPokedexRepository repo)
        {
            _repo = repo;
        }

        public async Task<PokedexPagedResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var pokemons = await _repo.LoadPokemons();
            var filteredPokemons = pokemons.Where(p =>
            {
                var variation = p.Variations[0];
                return variation.Name.ToLowerInvariant().Contains(request.SearchTerm.ToLowerInvariant()) ||
                       variation.Description.ToLowerInvariant().Contains(request.SearchTerm.ToLowerInvariant()) ||
                       variation.Image.ToLowerInvariant().Contains(request.SearchTerm.ToLowerInvariant()) ||
                       variation.Specie.ToLowerInvariant().Contains(request.SearchTerm.ToLowerInvariant());
            });

            var pokemonCount = filteredPokemons.Count();
            var totalaPages = Convert.ToInt32(Math.Ceiling((double)pokemonCount / request.PageSize));
            int skipCount = (request.Page - 1) * request.PageSize;
            return new PokedexPagedResponse(request.Page, totalaPages, pokemonCount, filteredPokemons.Skip(skipCount).Take(request.PageSize).Select(p => new PokemonListItemEntity(p.Num, p.Name, p.Variations[0].Image)).ToList());
        }
    }
}
