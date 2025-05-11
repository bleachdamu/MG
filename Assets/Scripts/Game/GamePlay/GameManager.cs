using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GridGenerator gridGenerator;
    [SerializeField]
    private CardComparisionHandler cardComparisionHandler;
    [SerializeField]
    private PointsHandler pointsHandler;
    [SerializeField]
    private SaveAndLoadSaveDataHandler saveAndLoadSaveDataHandler;
    [SerializeField]
    private Pokedex pokedex;
    [SerializeField]
    private List<GridData> layoutDatas;
    [SerializeField]
    private GameObject layoutParent;

    private SaveData saveData;
    private List<Card> cards = new List<Card>();
    private int matcheableCardsCount = 0;
    private int currentMatchCount = 0;
    private GridData choosenLayoutData;


    private void Start()
    {
        InitializeGame();
    }

    /// <summary>
    /// Initialize the game by generating the grid and setting up the cards.
    /// </summary>
    private void InitializeGame()
    {
        gridGenerator.Initialize();

        saveData = saveAndLoadSaveDataHandler.LoadData();
        if (saveData != null && saveData.saveDataExists == true)
        {
            layoutParent.SetActive(false);
            LoadGame();
        }
    }

    public void SetGridAndStartGame(int index)
    {
        layoutParent.SetActive(false);
        choosenLayoutData = index < layoutDatas.Count ? layoutDatas[index] : layoutDatas[0];
        NewGame();
    }

    private void NewGame()
    {
        List<GridElement> gridElements = new List<GridElement>();

        gridGenerator.GenerateGrid(choosenLayoutData,out gridElements);
        saveData = new SaveData();
        //saving grid data
        saveData.gridSaveData = gridGenerator.GridData;
        saveData.saveDataExists = true;

        cards = ConverGridElementToCard(ref gridElements);
        SelectRandomPokemonDataAndSet(ref cards);
        cardComparisionHandler.Initialize(cards, OnCardMatching,saveData);

        saveAndLoadSaveDataHandler.SaveData(saveData);
        pointsHandler.Initialize(saveData);
    }

    private void LoadGame()
    {
        List<GridElement> gridElements = new List<GridElement>();

        gridGenerator.GenerateGrid(saveData.gridSaveData,out gridElements);

        cards = ConverGridElementToCard(ref gridElements);
        LoadPokemonCardData(ref cards);
        cardComparisionHandler.Initialize(cards, OnCardMatching, saveData);
        pointsHandler.Initialize(saveData);
    }

    /// <summary>
    /// Select random Pokemon data from the Pokedex and set it to the cards.
    /// </summary>
    private void SelectRandomPokemonDataAndSet(ref List<Card> cards)
    {
        matcheableCardsCount = cards.Count / 2;
        List<PokemonData> randomPokemonData = pokedex.GetRandomPokemonData(matcheableCardsCount);
        List<Card> originalOrder = new List<Card>(cards);
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
        SaveGeneratedPokemonCardData(originalOrder);
    }

    private void SaveGeneratedPokemonCardData(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            saveData.generatedPokemonDatas.Add(cards[i].PokemonData);
        }
    }

    private void LoadPokemonCardData(ref List<Card> cards)
    {
        matcheableCardsCount = cards.Count / 2;
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetPokemonData(saveData.generatedPokemonDatas[i]);
        }
    }

    private List<Card> ConverGridElementToCard(ref List<GridElement> gridElements)
    {
        List<Card> cards = new List<Card>();
        for (int i = 0; i < gridElements.Count; i++)
        {
            cards.Add(gridElements[i] as Card);
        }
        return cards;
    }    

    private void OnCardMatching(int combo)
    {
        currentMatchCount++;
        pointsHandler.AddPoint(combo);
        if (currentMatchCount == matcheableCardsCount)
        {
            GameEnded();
            Debug.Log("Game Ended");
        }

        saveAndLoadSaveDataHandler.SaveData(saveData);
    }

    public void ExitToMainMenu()
    {
        GameEnded();
        SceneManager.LoadScene("MainMenu");
    }

    private void GameEnded()
    {
        currentMatchCount = 0;
        gridGenerator.ResetGrid();
        pointsHandler.GameEnded();
        saveAndLoadSaveDataHandler.DeleteSaveData();
    }
}
