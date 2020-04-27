using UnityEngine;

public class GridObject : MonoBehaviour
{
    public bool isSelectable = true;
    public bool isMovable = true;
    public MeshRenderer mesh;
    public GridTile occupiedTile;

    public int Value { get; protected set; }

    protected bool IsConnected = false;

    public void SnapToClosestFromTarget(Vector3 target)
    {
        if (!isMovable) return;
        
        //visualize tile your standing on
        occupiedTile.ShowTile();
        
        //move to next tile
        Transform closestTile = MovementGrid.Instance.GetClosestTileToPosition(target);
        var ctLocalPosition = closestTile.localPosition;
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
        
        mesh.material.SetColor("_Color", Color.red);
    }

    public virtual void Deselect()
    {
        MovementGrid.Instance.HideGrid();
        
        if(IsConnected)
            mesh.material.SetColor("_Color", Color.green);
        else
            mesh.material.SetColor("_Color", Color.white);
    }

    public virtual void Connect(GridObject other)
    {
        IsConnected = true;
        mesh.material.SetColor("_Color", Color.green);
    }

    public virtual void Disconnect(GridObject other)
    {
        IsConnected = false;
        mesh.material.SetColor("_Color", Color.red);
    }
    
    public virtual void SetValue(int value)
    {
        Value = value;
    }
    
    public void RotateLeft()
    {
        transform.Rotate(0, 90, 0);
    }

    public void RotateRight()
    {
        transform.Rotate(0, -90, 0);
    }
}
