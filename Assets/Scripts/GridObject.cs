using UnityEngine;

public class GridObject : MonoBehaviour
{
    public bool isSelectable = true;
    public bool isMovable = true;
    public float xSize = 1f;
    public MeshRenderer mesh;
    public GridTile occupiedTile;
    
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
        mesh.material.SetColor("_Color", Color.white);
    }
}
