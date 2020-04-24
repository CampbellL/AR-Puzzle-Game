using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public bool generateGrid;
    public float z = 20f;
    public float x = 10f;
    public GameObject gridPrefab;
    public Vector3 startBottomLeft;

    public float zBound = 0.5f;
    public float xBound = 0.5f;

    private int _rows;
    private int _columns;
    
    private Transform[,] _grid;
    
    // Start is called before the first frame update
    void Start()
    {
        x -= 2 * xBound;
        z -= 2 * zBound;

        _rows = (int) (x / MovementGrid.Instance.scale) + 1;
        _columns = (int) (z / MovementGrid.Instance.scale) + 1;
        
        _grid = new Transform[_columns, _rows];
        
        if(generateGrid) GenerateGrid();
    }

    private void GenerateGrid()
    {
        var spawnPos = new Vector3(startBottomLeft.x + xBound, startBottomLeft.y, startBottomLeft.z + zBound);

        for (int i = 0; i < _columns; i++)
        {
            for (int j = 0; j < _rows; j++)
            {
                Transform obj = Instantiate(gridPrefab, transform).transform;
                _grid[i, j] = obj;
                obj.localPosition = spawnPos;
                spawnPos.x += MovementGrid.Instance.scale;
            }

            spawnPos.z += MovementGrid.Instance.scale;
            spawnPos.x = startBottomLeft.x + xBound;
        }
    }
}
