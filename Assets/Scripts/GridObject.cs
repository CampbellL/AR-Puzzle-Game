using UnityEngine;

public class GridObject : MonoBehaviour
{
    public bool isSelectable = true;
    public bool isMovable = true;
    public MeshRenderer mesh;
    [HideInInspector] public GridTile occupiedTile;

    public int Value { get; protected set; }

    protected bool IsConnected;
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

    protected virtual void Start()
    {
        GameManager.Instance.gridObjects.Add(this);
    }

    public void SnapToClosestFromTarget(Vector3 target)
    {
        if (!isMovable) return;
        
        //visualize tile your standing on
        occupiedTile.ShowTile();
        occupiedTile.isOccupied = false;
        
        //move to next tile
        Transform closestTile = MovementGrid.Instance.GetClosestTileToPosition(target);
        var ctLocalPosition = closestTile.localPosition;
        ctLocalPosition += closestTile.parent.localPosition; //add the tile offset
        var transform1 = transform;
        transform1.localPosition = new Vector3(ctLocalPosition.x, transform1.localPosition.y, ctLocalPosition.z);
        
        //update occupied tile and hide it
        occupiedTile = closestTile.GetComponent<GridTile>();
        occupiedTile.HideTile();
    }
    
    public virtual void Select()
    {
        if (!isSelectable) return;
        
        if(isMovable) 
            MovementGrid.Instance.ShowGrid();
        
        mesh.material.SetColor(ColorProperty, Color.red);
    }

    public virtual void Deselect()
    {
        MovementGrid.Instance.HideGrid();

        mesh.material.SetColor(ColorProperty, IsConnected ? Color.green : Color.white);
    }

    public virtual void Connect(GridObject other)
    {
        IsConnected = true;
        mesh.material.SetColor(ColorProperty, Color.green);
    }

    public virtual void Disconnect(GridObject other)
    {
        IsConnected = false;
        mesh.material.SetColor(ColorProperty, Color.red);
    }
    
    public virtual void SetValue(int value)
    {
        Value = value;
    }
    
    public void RotateLeft()
    {
        var transform1 = transform;
        transform.RotateAround(transform1.position, transform1.up, 90f);
    }

    public void RotateRight()
    {
        var transform1 = transform;
        transform.RotateAround(transform1.position, transform1.up, -90f);
    }
}
