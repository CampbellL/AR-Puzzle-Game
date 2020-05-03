using System.Collections;
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
    public LayerMask snappableLayer;

    [Space][Header("Grid Settings")]
    public Transform gridParent;
    public int rows;
    public int columns;

    private Transform[,] _grid;
    
    public float scale = 1f;


    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.StartNewGame();
    }

    public void InitializeNewGridValues()
    {
        GameManager.Instance.CreateGridInfoArray(rows, columns, gridParent);
        ApplyRandomDeskTemplate();
    }

    public GridObject SpawnOnRandomTile(GridObject obj)
    {
        while (true)
        {
            int rand = Random.Range(0, gridParent.childCount);
            
            Transform randTile = gridParent.GetChild(rand);
            if (randTile.GetComponent<GridTile>().isOccupied) continue;
            
            SetRandomRotation(obj);
            
            var spawnPos = randTile.localPosition;
            spawnPos += randTile.parent.localPosition; //add the offset of the tiles
            spawnPos.y = obj.transform.localPosition.y;

            GridObject spawnObj = Instantiate(obj, transform);
            spawnObj.transform.localPosition = spawnPos;

            var tile = randTile.GetComponent<GridTile>();
            tile.isOccupied = true;
            spawnObj.occupiedTile = tile;

            return spawnObj;
        }
    }

    public void ApplyRandomDeskTemplate()
    {
        Instantiate(deskTemplates[Random.Range(0, deskTemplates.Length)], transform);
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

        if (colliders.Length > 0)
        {
            var closestDistance = 1000f;

            foreach (var collider1 in colliders)
            {
                float currentDistance = Vector3.Distance(pos, collider1.transform.position);
                
                if (currentDistance < closestDistance && !collider1.GetComponent<GridTile>().isOccupied)
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
        foreach (Transform tile in gridParent)
        {
            var tileScript = tile.GetComponent<GridTile>();
            if(!tileScript.isOccupied) tileScript.ShowTile();
        }
    }

    public void HideGrid()
    {
        foreach (Transform tile in gridParent)
        {
            tile.GetComponent<GridTile>().HideTile();
        }
    }
}