namespace MinalAPI.Pokedex.Models;

public record PokemonEntity(int Num, string Name, Variation[] Variations, string Link);

public record Variation(string Name, string Description, string Image, string[] Types, string Specie, float Height, float Weight, string[] Abilities, Stats Stats, string[] Evolutions);

public record Stats (int Total, int Hp, int Attack, int Defense, int SpeedAttack, int SpeedDefense, int Speed);