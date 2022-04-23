namespace MinalAPI.Pokedex.Models;

public record PokedexPagedResponse(int Page, int TotalPages, int TotalResults, List<PokemonListItemEntity> Data) : PokedexResponse(Data);
