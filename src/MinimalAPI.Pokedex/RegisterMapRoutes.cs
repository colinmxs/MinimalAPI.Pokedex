namespace MinimalAPI.Pokedex;
using MinimalAPI.Pokedex.Features;
public static class RegisterMapRoutes
{
    public static IEndpointRouteBuilder RegisterRoutes(this IEndpointRouteBuilder builder)
    {
        GetPokemon.RouteMapper.Map(builder);
        ListAllPokemon.RouteMapper.Map(builder);
        PaginatePokemon.RouteMapper.Map(builder);
        SearchPokemon.RouteMapper.Map(builder);
        return builder;
    }
}