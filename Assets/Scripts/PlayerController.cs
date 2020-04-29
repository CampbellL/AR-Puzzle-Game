using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float inputDelay = 0.01f;
    public LayerMask selectableLayer;
    public LayerMask movableLayer;
    
    private float _currentInputDelay;
    private GridObject _selectedObj;

    private void Start()
    {
        _currentInputDelay = inputDelay;
    }

    private void Update()
    {
        if (Input.touchSupported)
        {
            if (Input.touchCount > 0 && _currentInputDelay <= 0)
            {
                var touch = Input.GetTouch(0);

                UiManager.Instance.debugText.text = System.Enum.GetName(typeof(TouchPhase), touch.phase);

                if (touch.phase == TouchPhase.Began)
                {
                    var obj = GetSelectableObject(touch);

                    if (obj != null)
                    {
                        obj.Select();
                    }

                    _selectedObj = obj;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    if (_selectedObj) MoveSelectedObject(touch);
                }

                _currentInputDelay = inputDelay;
            }

            _currentInputDelay -= Time.deltaTime;
        }
    }

    private GridObject GetSelectableObject(Touch touch)
    {
        var touchPosRay = Camera.main.ScreenPointToRay(touch.position);
        bool hasHit = Physics.Raycast(touchPosRay, out var hit, selectableLayer);

        if (hasHit)
        {
            if (_selectedObj != null)
            {
                if (hit.transform.CompareTag("LeftRotator"))
                {
                    _selectedObj.RotateLeft();
                    return _selectedObj;
                } 
                
                if (hit.transform.CompareTag("RightRotator"))
                {
                    _selectedObj.RotateRight();
                    return _selectedObj;
                }

                _selectedObj.Deselect();
                _selectedObj = null;
            }
            
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
