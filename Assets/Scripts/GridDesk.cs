using System;
using UnityEngine;

public class GridDesk : GridObject
{
    public int id = 1;
    public int chairSpots;
    public TextMesh valueText;

    private void Start()
    {
        GameManager.Instance.desks.Add(id, this);
    }

    public override void SetValue(int value)
    {
        base.SetValue(value);
        valueText.text = value.ToString();
    }

    public override void Connect(GridObject other)
    {
        Value -= other.Value;

        if (Value == 0)
        {
            base.Connect(other);
        }
    }

    public override void Disconnect(GridObject other)
    {
        Value += other.Value;

        if (Value != 0)
        {
            IsConnected = false;
            mesh.material.SetColor("_Color", Color.white);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chair"))
        {
            var chair = other.GetComponent<GridChair>();
            Connect(chair);
            chair.Connect(this);
        }
        else if (other.CompareTag("Tile"))
        {
            var tile = other.gameObject.GetComponent<GridTile>();
            tile.isOccupied = true;
            GameManager.Instance.gridInfo[tile.tileRow, tile.tileColumn] = id;
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Chair"))
        {
            var chair = other.GetComponent<GridChair>();
            Disconnect(chair);
            chair.Disconnect(this);
        }
    }
}
