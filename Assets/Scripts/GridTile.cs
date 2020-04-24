using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public bool isOccupied;
    public SpriteRenderer tileMarker;

    public float markerFreeSize;
    public float markerOccupiedSize;

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
