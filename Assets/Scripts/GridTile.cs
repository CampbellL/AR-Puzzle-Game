﻿using UnityEngine;

public class GridTile : MonoBehaviour
{
    public bool isOccupied;
    public SpriteRenderer tileMarker;

    public int tileRow;
    public int tileColumn;

    private void Start()
    {
        tileMarker.gameObject.SetActive(false);
    }

    public void ShowTile()
    {
        if (isOccupied)
        {
            tileMarker.gameObject.SetActive(false);
            return;
        }
        
        tileMarker.gameObject.SetActive(true);
    }

    public void HideTile()
    {
        tileMarker.gameObject.SetActive(false);
    }
}
