using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsHandler : MonoBehaviour
{
    [SerializeField]
    private int pointsForWin = 1;
    [SerializeField]
    private int points = 0;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private SaveData saveData;

    public void Initialize(SaveData saveData)
    {
        this.saveData = saveData;
        this.points = saveData.points;
        scoreText.text = points.ToString();
    }

    public void AddPoint(int combo)
    {
        points += pointsForWin * combo;
        scoreText.text = points.ToString();
        saveData.points = points;
    }

    public void GameEnded()
    {

    }
}
