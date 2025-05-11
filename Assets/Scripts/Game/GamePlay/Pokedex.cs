using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokemonData
{
    public string pokemonName;
    public int pokemonId;
    public Sprite pokemonSprite;
}

[CreateAssetMenu(fileName = "Pokedex", menuName = "ScriptableObjects/PokeDex", order = 1)]
public class Pokedex : ScriptableObject
{
    public List<PokemonData> pokemonDatas;

    /// <summary>
    /// Get random Pokemon data from the Pokedex.
    /// </summary>
    /// <returns></returns>
    internal List<PokemonData> GetRandomPokemonData(int randomCount)
    {
        this.pokemonDatas.Shuffle();
        List<PokemonData> pokemonDatas = new List<PokemonData>();
        for (int i = 0; i < randomCount; i++)
        {
            if (i < this.pokemonDatas.Count)
            {
                PokemonData randomPokemonData = this.pokemonDatas[i];
                pokemonDatas.Add(randomPokemonData);
            }
            else
            {
                PokemonData randomPokemonData = this.pokemonDatas[UnityEngine.Random.Range(0,this.pokemonDatas.Count - 1)];
                pokemonDatas.Add(randomPokemonData);
            }
        }
        return pokemonDatas;
    }
}
