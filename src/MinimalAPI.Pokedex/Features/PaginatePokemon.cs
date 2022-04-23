namespace MinimalAPI.Pokedex.Features;
using MinalAPI.Pokedex.Models;
using MinimalAPI.Pokedex.Services;
public class PaginatePokemon
{
    public static class RouteMapper
    {
        public static void Map(IEndpointRouteBuilder builder) =>
            builder.MapGet("/pokedex", async (int? page, int? pageSize, IMediator mediator) => await mediator.Send(new Query(page ?? 1, pageSize ?? 10)))
            .Produces<PokedexPagedResponse>(StatusCodes.Status200OK);
    }

    public record Query(int Page, int PageSize) : IRequest<PokedexPagedResponse>;

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
            var pokemonCount = pokemons.Count();
            var totalaPages = Convert.ToInt32(Math.Ceiling((double)pokemonCount / request.PageSize));
            int skipCount = (request.Page - 1) * request.PageSize;
            return new PokedexPagedResponse(request.Page, totalaPages, pokemonCount, pokemons.Skip(skipCount).Take(request.PageSize).Select(p => new PokemonListItemEntity(p.Num, p.Name, p.Variations[0].Image)).ToList());
        }
    }
}
