using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridDesk : GridObject
{
    public int chairSpots;
    public TextMesh valueText;
    
    private int _value;

    public void SetValue(int value)
    {
        _value = value;
        valueText.text = value.ToString();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            other.gameObject.GetComponent<GridTile>().isOccupied = true;
        }
    }
}
