using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonDataPopulater : MonoBehaviour
{
    [SerializeField]
    private Pokedex pokedex;
    [SerializeField]
    private float cardVisibleTime = 1f;
    private SaveData saveData;

    public void Initialize(SaveData saveData,ref List<Card> cards)
    {
        this.saveData = saveData;
        if(saveData.generatedPokemonDatas != null && saveData.generatedPokemonDatas.Count > 0)
        {
            LoadPokemonCardData(ref cards);
        }else
        {
            SelectRandomPokemonDataAndSet(ref cards);
        }
    }

    /// <summary>
    /// Select random Pokemon data from the Pokedex and set it to the cards.
    /// </summary>
    public void SelectRandomPokemonDataAndSet(ref List<Card> cards)
    {
        List<PokemonData> randomPokemonData = pokedex.GetRandomPokemonData(cards.Count / 2);
        List<Card> originalOrder = new List<Card>(cards);
        cards.Shuffle();
        int pokeIndex = 0;
        int cardIndex = 0;
        while (pokeIndex < randomPokemonData.Count)
        {
            cards[cardIndex].SetPokemonData(randomPokemonData[pokeIndex], cardVisibleTime);
            cards[cardIndex + 1].SetPokemonData(randomPokemonData[pokeIndex], cardVisibleTime);
            cardIndex = cardIndex + 2;
            pokeIndex++;
        }
        SaveGeneratedPokemonCardData(originalOrder);
    }

    private void SaveGeneratedPokemonCardData(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            saveData.generatedPokemonDatas.Add(cards[i].PokemonData);
        }
    }

    public void LoadPokemonCardData(ref List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetPokemonData(saveData.generatedPokemonDatas[i],0);
        }
    }
}
