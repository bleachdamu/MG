using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string saveFilePath;

    private void Start()
    {
        if (SaveAndLoadFileHandler.FileExists(Application.persistentDataPath + saveFilePath))
        {
            LoadGame();
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
