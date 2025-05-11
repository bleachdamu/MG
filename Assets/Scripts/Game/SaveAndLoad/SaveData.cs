using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<PokemonData> generatedPokemonDatas = new List<PokemonData>();
    public List<Vector2> matchedPositions = new List<Vector2>();
    public GridData gridSaveData;
    public int points = 0;
    public bool saveDataExists = false;

    public void Reset()
    {
        generatedPokemonDatas.Clear();
        matchedPositions.Clear();
        gridSaveData = null;
        saveDataExists = false;
        points = 0;
    }
}
