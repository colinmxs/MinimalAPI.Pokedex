namespace MinimalAPI.Pokedex.Features;
using MinalAPI.Pokedex.Models;
using MinimalAPI.Pokedex.Services;
public class ListAllPokemon
{
    public static class RouteMapper
    {
        public static void Map(IEndpointRouteBuilder builder) =>
            builder.MapGet("/pokedex/all", async (IMediator mediator) => await mediator.Send(new Query()))
            .Produces<PokedexResponse>(StatusCodes.Status200OK);
    }

    public record Query() : IRequest<PokedexResponse>;

    public class QueryHandler : IRequestHandler<Query, PokedexResponse>
    {
        private readonly IPokedexRepository _repo;
        public QueryHandler(IPokedexRepository repo)
        {
            _repo = repo;
        }

        public async Task<PokedexResponse> Handle(Query request, CancellationToken cancellationToken) 
        {
            var pokemons = await _repo.LoadPokemons();
            return new PokedexResponse(pokemons.Select(p => new PokemonListItemEntity(p.Num, p.Name, p.Variations[0].Image)).ToList());
        } 
    }
}
