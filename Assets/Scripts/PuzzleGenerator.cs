using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Symbol
{
    public GameObject symbol;
    public int value;
    public int spawnAmount;
}

[System.Serializable]
public struct Desk
{
    public int id;
    public int value;
}

public class PuzzleGenerator : MonoBehaviour
{
    public Desk[] deskValues;
    public Symbol[] symbols;

    public void SetupFixedPuzzle()
    {
        Dictionary<int, int> fixedDeskValues = new Dictionary<int, int>();

        foreach (var desk in deskValues)
        {
            fixedDeskValues.Add(desk.id, desk.value);
        }

        GameManager.Instance.ApplyDeskValues(fixedDeskValues);

        foreach (var symbol in symbols)
        {
            for (int i = 0; i < symbol.spawnAmount; i++)
            {
                GameManager.Instance.SpawnChair(symbol.value, symbol.symbol);
            }
        }
    }
}
