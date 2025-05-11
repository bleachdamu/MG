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
    private PokemonDataPopulater pokemonDataPopulater;
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
        CheckAndLoadGameData();
    }

    /// <summary>
    /// Check if there is saved game data and load it. If not, initialize a new game.
    /// </summary>
    private void CheckAndLoadGameData()
    {
        gridGenerator.Initialize();

        saveData = saveAndLoadSaveDataHandler.LoadData();
        if (saveData != null)
        {
            choosenLayoutData = saveData.gridSaveData;
            InitializeGame();
        }
        else
        {
            saveData = new SaveData();
        }
    }

    /// <summary>
    /// Initialize the game by generating the grid and setting up the cards.
    /// </summary>
    private void InitializeGame()
    {
        layoutParent.SetActive(false);

        List<GridElement> gridElements = new List<GridElement>();
        gridGenerator.GenerateGrid(choosenLayoutData, out gridElements);
        saveData.gridSaveData = gridGenerator.GridData;

        cards = ConvertGridElementToCard(ref gridElements);
        matcheableCardsCount = cards.Count / 2 - saveData.matchedPositions.Count / 2;
        pokemonDataPopulater.Initialize(saveData, ref cards);
        cardComparisionHandler.Initialize(ref cards, OnCardMatching, saveData);

        saveAndLoadSaveDataHandler.SaveData(saveData);
        pointsHandler.Initialize(saveData);
    }

    /// <summary>
    /// Set the grid layout and start the game.Called from butto click.
    /// </summary>
    public void SetGridAndStartGame(int index)
    {
        choosenLayoutData = index < layoutDatas.Count ? layoutDatas[index] : layoutDatas[0];
        InitializeGame();
    }

    /// <summary>
    /// Convert the grid elements to cards.
    /// </summary>
    /// <returns></returns>
    private List<Card> ConvertGridElementToCard(ref List<GridElement> gridElements)
    {
        List<Card> cards = new List<Card>();
        for (int i = 0; i < gridElements.Count; i++)
        {
            cards.Add(gridElements[i] as Card);
        }
        return cards;
    }

    /// <summary>
    /// Handle the card matching event.
    /// </summary>
    private void OnCardMatching(int combo)
    {
        currentMatchCount++;
        pointsHandler.AddPoint(combo);
        saveAndLoadSaveDataHandler.SaveData(saveData);
        if (currentMatchCount == matcheableCardsCount)
        {
            GameEnded();
            Debug.Log("Game Ended");
        }
    }

    /// <summary>
    /// Exit the game and return to the main menu.
    /// </summary>
    public void ExitToMainMenu()
    {
        GameEnded();
        gridGenerator.ResetGrid();
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Handle the end of the game.
    /// </summary>
    private void GameEnded()
    {
        currentMatchCount = 0;
        matcheableCardsCount = 0;
        pointsHandler.GameEnded();
        saveData.Reset();
        saveAndLoadSaveDataHandler.DeleteSaveData();
        cardComparisionHandler.Reset();
    }
}
