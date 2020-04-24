using UnityEngine;
using Random = UnityEngine.Random;

public class MovementGrid : MonoBehaviour
{
    public static MovementGrid Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public GameObject[] deskTemplates;
    public GridObject chairPrefab;
    public LayerMask snappableLayer;

    private Transform[,] _grid;
    
    public float scale = 1f;
    
    private int _rows;
    private int _columns;
    

    // Start is called before the first frame update
    private void Start()
    {
        /*for (int i = 0; i < 5; i++)
        {
            SpawnOnRandomTile(desk);
            SpawnOnRandomTile(chair);
        }*/
    }

    private void SpawnOnRandomTile(GridObject obj)
    {
        while (true)
        {
            int randomRow = Random.Range(0, _rows);
            int randomColumn = Random.Range(0, _columns);
            Transform randObj = _grid[randomColumn, randomRow];
            if (randObj.GetComponent<GridTile>().isOccupied) continue;
            
            SetRandomRotation(obj);
            
            var spawnPos = randObj.transform.localPosition;
            spawnPos.y = obj.transform.position.y;

            if (obj.xSize > 1)
            {
                if (randomRow + 1 < _rows && !_grid[randomColumn, randomRow + 1].GetComponent<GridTile>().isOccupied)
                {
                    spawnPos.x += scale / 2;
                    _grid[randomColumn, randomRow + 1].GetComponent<GridTile>().isOccupied = true;
                }
                else
                {
                    continue;
                }
            }

            GridObject spawnObj = Instantiate(obj, transform);
            spawnObj.transform.localPosition = spawnPos;
            var tile = _grid[randomColumn, randomRow].GetComponent<GridTile>();
            tile.isOccupied = true;
            spawnObj.occupiedTile = tile;

            break;
        }
    }
    
    private void SetRandomRotation(GridObject obj)
    {
        int rand = Random.Range(0, 5);
        float yRotation = 0;

        switch (rand)
        {
            case 2:
                yRotation = 90;
                break;
            case 3:
                yRotation = 180;
                break;
            case 4:
                yRotation = -90;
                break;
        }
        
        obj.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public Transform GetClosestTileToPosition(Vector3 pos)
    {
        Collider closest = null;

        Collider[] colliders = Physics.OverlapSphere(pos, 3f, snappableLayer);
        
        UiManager.Instance.debugText.text = colliders.Length.ToString();                    //debug

        if (colliders.Length > 0)
        {
            closest = colliders[0];
            var closestDistance = Vector3.Distance(pos, colliders[0].transform.position);

            foreach (var collider1 in colliders)
            {
                float currentDistance = Vector3.Distance(pos, collider1.transform.position);
                
                if (currentDistance < closestDistance)
                {
                    closest = collider1;
                    closestDistance = currentDistance;
                }
            }
        }

        return (closest != null) ? closest.transform : null;
    }

    public void ShowGrid()
    {
        for (int i = 0; i < _columns; i++)
        {
            for (int j = 0; j < _rows; j++)
            {
                var tile = _grid[i, j].GetComponent<GridTile>();
                if(!tile.isOccupied) tile.ShowTile();
            }
        }
    }

    public void HideGrid()
    {
        for (int i = 0; i < _columns; i++)
        {
            for (int j = 0; j < _rows; j++)
            {
                _grid[i, j].GetComponent<GridTile>().HideTile();
            }
        }
    }
}