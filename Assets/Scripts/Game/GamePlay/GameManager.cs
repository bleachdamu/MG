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
        InitializeGame();
    }

    /// <summary>
    /// Initialize the game by generating the grid and setting up the cards.
    /// </summary>
    private void InitializeGame()
    {
        gridGenerator.Initialize();

        saveData = saveAndLoadSaveDataHandler.LoadData();
        if (saveData != null)
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

        cards = ConverGridElementToCard(ref gridElements);
        pokemonDataPopulater.Initialize(saveData, ref cards);
        cardComparisionHandler.Initialize(ref cards, OnCardMatching,saveData);

        saveAndLoadSaveDataHandler.SaveData(saveData);
        pointsHandler.Initialize(saveData);
    }

    private void LoadGame()
    {
        List<GridElement> gridElements = new List<GridElement>();

        gridGenerator.GenerateGrid(saveData.gridSaveData,out gridElements);

        cards = ConverGridElementToCard(ref gridElements);

        pokemonDataPopulater.Initialize(saveData, ref cards);
        cardComparisionHandler.Initialize(ref cards, OnCardMatching, saveData);
        pointsHandler.Initialize(saveData);
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
