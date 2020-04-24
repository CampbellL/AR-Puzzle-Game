using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask selectableLayer;
    public LayerMask movableLayer;
    private GridObject _selectedObj;
    
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                if (_selectedObj != null) _selectedObj.Deselect();
                
                var obj = GetSelectableObject(touch);

                if (obj != null)
                {
                    obj.Select();
                }
                
                _selectedObj = obj;
            } 
            else if (touch.phase == TouchPhase.Moved)
            {
                if(_selectedObj) MoveSelectedObject(touch);
            }
        }
    }

    private GridObject GetSelectableObject(Touch touch)
    {
        var touchPosRay = Camera.main.ScreenPointToRay(touch.position);
        bool hasHit = Physics.Raycast(touchPosRay, out var hit, selectableLayer);

        if (hasHit)
        {
            var moveObj = hit.transform.parent.GetComponent<GridObject>();

            if (moveObj.isSelectable)
                return moveObj;
        }
        
        return null;
    }

    private void MoveSelectedObject(Touch touch)
    {
        var touchPosRay = Camera.main.ScreenPointToRay(touch.position);
        bool hasHit = Physics.Raycast(touchPosRay, out var hit, movableLayer);

        if (hasHit)
        {
            _selectedObj.SnapToClosestFromTarget(hit.point);
        }
    }
}
