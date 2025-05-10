using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GridGenerator gridGenerator;
    [SerializeField]
    private Pokedex pokedex;
    [SerializeField]
    private CardComparisionHandler cardComparisionHandler;

    private void Start()
    {
        InitializeGame();
    }

    /// <summary>
    /// Initialize the game by generating the grid and setting up the cards.
    /// </summary>
    private void InitializeGame()
    {
        List<GridElement> gridElements = new List<GridElement>();
        gridGenerator.GenerateGrid(out gridElements);

        List<Card> cards = new List<Card>();
        for(int i=0;i<gridElements.Count;i++)
        {
            cards.Add(gridElements[i] as Card);
        }

        cardComparisionHandler.Initialize(cards);
        SelectRandomPokemonDataAndSet(ref cards);
    }

    /// <summary>
    /// Select random Pokemon data from the Pokedex and set it to the cards.
    /// </summary>
    private void SelectRandomPokemonDataAndSet(ref List<Card> cards)
    {
        int randomCount = cards.Count / 2;
        List<PokemonData> randomPokemonData = pokedex.GetRandomPokemonData(randomCount);
        cards.Shuffle();
        int pokeIndex = 0;
        int cardIndex = 0;
        while(pokeIndex < randomPokemonData.Count)
        {
            cards[cardIndex].SetPokemonData(randomPokemonData[pokeIndex]);
            cards[cardIndex+1].SetPokemonData(randomPokemonData[pokeIndex]);
            cardIndex = cardIndex + 2;
            pokeIndex++;
        }
    }
}
