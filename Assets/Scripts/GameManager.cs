using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public int[,] gridInfo;
    public Dictionary<int, GridDesk> desks;
    
    public GridObject chairPrefab;

    public GameObject testSymbolPrefab;
    
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
    }

    private void Start()
    {
        StartCoroutine(Delay());
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

    public void SpawnChairs(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            MovementGrid.Instance.SpawnOnRandomTile(chairPrefab);
        }
    }
    
    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(.1f);
        //SpawnChairs(16);

        for (int i = 0; i < 16; i++)
        {
            SpawnChair(10, testSymbolPrefab);
        }
    }
}
