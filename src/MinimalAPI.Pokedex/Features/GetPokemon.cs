namespace MinimalAPI.Pokedex.Features;
using MinalAPI.Pokedex.Models;
using MinimalAPI.Pokedex.Services;
public class GetPokemon
{
    public static class RouteMapper
    {
        public static void Map(IEndpointRouteBuilder builder) =>
            builder.MapGet("/pokedex/{name}", async (string name, IMediator mediator) => 
            await mediator.Send(new Query(name))
            is PokemonEntity pokemon
            ? Results.Ok(pokemon)
            : Results.NotFound())
            .Produces<PokemonEntity>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    public record Query(string Name) : IRequest<PokemonEntity?>;

    public class QueryHandler : IRequestHandler<Query, PokemonEntity?>
    {
        private readonly IPokedexRepository _repo;
        public QueryHandler(IPokedexRepository repo)
        {
            _repo = repo;
        }

        public async Task<PokemonEntity?> Handle(Query request, CancellationToken cancellationToken)
        {
            var pokemons = await _repo.LoadPokemons();            
            return pokemons.FirstOrDefault(p => p.Name.ToLowerInvariant() == request.Name.ToLowerInvariant());
        }
    }
}
