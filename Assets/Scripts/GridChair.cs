using UnityEngine;

public class GridChair : GridObject
{
    public Transform symbolParent;
    public GameObject rotators;
    
    private bool _rotatorsActive;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if(rotators) rotators.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_rotatorsActive)
        {
            var lookPos = Camera.main.transform.position - rotators.transform.position;
            lookPos.y = 0;
            var lookRotation = Quaternion.LookRotation(lookPos);

            rotators.transform.rotation = Quaternion.Slerp(rotators.transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public override void Disconnect(GridObject other)
    {
        base.Disconnect(other);
        if (!GameManager.Instance.gridObjects.Contains(this))
        {
            print("add chair");
            GameManager.Instance.gridObjects.Add(this);
        }
    }

    public override void Connect(GridObject other)
    {
        base.Connect(other);
        GameManager.Instance.gridObjects.Remove(this);
        print("remove chair");
        GameManager.Instance.CheckWinCondition();
    }

    public override void Select()
    {
        base.Select();
        
        rotators.SetActive(true);
        _rotatorsActive = true;
    }

    public override void Deselect()
    {
        base.Deselect();
        
        rotators.SetActive(false);
        _rotatorsActive = false;
    }
}
