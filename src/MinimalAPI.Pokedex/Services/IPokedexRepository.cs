namespace MinimalAPI.Pokedex.Services;
using MinalAPI.Pokedex.Models;
using System.Text.Json;

public interface IPokedexRepository
{
    public Task<List<PokemonEntity>> LoadPokemons();
}

public class JsonFilePokedexRepository : IPokedexRepository
{
    private List<PokemonEntity>? _pokemons;
    readonly IWebHostEnvironment environment;

    private readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public JsonFilePokedexRepository(IWebHostEnvironment _environment)
    {
        environment = _environment;
    }

    public async Task<List<PokemonEntity>> LoadPokemons()
    {
        if (_pokemons == null)
        {
            var data = await File.ReadAllTextAsync(Path.Combine(environment.ContentRootPath, "db", "pokemon.json"));
            _pokemons = new List<PokemonEntity>();
            using var document = JsonDocument.Parse(data);
            foreach (var item in document.RootElement.EnumerateArray())
            {
                var pokemonEntity = JsonSerializer.Deserialize<PokemonEntity>(item, jsonOptions);
                if(pokemonEntity != null) _pokemons.Add(pokemonEntity);
            }
        }

        return _pokemons;
    }
}
