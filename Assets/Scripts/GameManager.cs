using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public bool isGameOver;
    public int[,] gridInfo;
    public Dictionary<int, GridDesk> desks;
    public List<GridObject> gridObjects;
    
    public GridObject chairPrefab;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        desks = new Dictionary<int, GridDesk>();
        gridObjects = new List<GridObject>();
    }

    public void StartNewGame()
    {
        ClearGridInfo();
        MovementGrid.Instance.InitializeNewGridValues();
        StartCoroutine(Delay());
        isGameOver = false;
    }

    public void CheckWinCondition()
    {
        print(gridObjects.Count);
        if (gridObjects.Count == 0)
            EndGame();
    }

    private void EndGame()
    {
        UiManager.Instance.DisplayWinningScreen();
        isGameOver = true;
    }

    private void ClearGridInfo()
    {
        foreach (GridObject obj in FindObjectsOfType<GridObject>())
        {
            Destroy(obj.gameObject);
        }
        
        gridObjects = new List<GridObject>();
        desks = new Dictionary<int, GridDesk>();
    }

    public void CreateGridInfoArray(int rows, int columns, Transform gridParent)
    {
        gridInfo = new int[rows, columns];
        
        int row = 0;
        int column = 0;

        foreach (Transform child in gridParent)
        {
            if (column == columns)
            {
                row++;
                column = 0;
            }

            var tile = child.GetComponent<GridTile>();
            tile.isOccupied = false;
            tile.tileRow = row;
            tile.tileColumn = column;
            
            gridInfo[row, column++] = 0;
        }
    }

    public void ApplyDeskValues(Dictionary<int, int> deskValuePairs)
    {
        foreach (var deskValuePair in deskValuePairs)
        {
            desks[deskValuePair.Key].SetValue(deskValuePair.Value);
        }
    }

    public void SpawnChair(int value, GameObject symbol)
    {
        var obj = (GridChair)MovementGrid.Instance.SpawnOnRandomTile(chairPrefab);
        obj.SetValue(value);
        Instantiate(symbol, obj.symbolParent);
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(.1f);
        GetComponent<PuzzleGenerator>().SetupFixedPuzzle();
    }
}
