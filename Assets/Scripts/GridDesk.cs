using UnityEngine;

public class GridDesk : GridObject
{
    public int id = 1;
    public TextMesh valueText;
    
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.desks.Add(id, this);
    }

    public override void SetValue(int value)
    {
        base.SetValue(value);
        valueText.text = value.ToString();
    }

    public override void Connect(GridObject other)
    {
        SetValue(Value - other.Value);

        if (Value == 0)
        {
            base.Connect(other);
            GameManager.Instance.gridObjects.Remove(this);
            GameManager.Instance.CheckWinCondition();
        }
    }

    public override void Disconnect(GridObject other)
    {
        SetValue(Value + other.Value);

        if (Value != 0)
        {
            IsConnected = false;
            mesh.material.SetColor(ColorProperty, Color.white);

            if (!GameManager.Instance.gridObjects.Contains(this))
            {
                GameManager.Instance.gridObjects.Add(this);
            }
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
