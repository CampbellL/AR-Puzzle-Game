﻿using System.Collections;
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
        GameManager.Instance.CreateGridInfoArray(rows, columns, gridParent);
        ApplyRandomDeskTemplate();
    }

    public void SpawnOnRandomTile(GridObject obj, int value = 0)
    {
        while (true)
        {
            int rand = Random.Range(0, gridParent.childCount);
            
            Transform randTile = gridParent.GetChild(rand);
            if (randTile.GetComponent<GridTile>().isOccupied) continue;
            
            SetRandomRotation(obj);
            
            var spawnPos = randTile.position;
            spawnPos.y = obj.transform.position.y;

            GridObject spawnObj = Instantiate(obj, transform);
            spawnObj.transform.position = spawnPos;
            spawnObj.SetValue(value);
            
            var tile = randTile.GetComponent<GridTile>();
            tile.isOccupied = true;
            spawnObj.occupiedTile = tile;

            break;
        }
    }

    private void ApplyRandomDeskTemplate()
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