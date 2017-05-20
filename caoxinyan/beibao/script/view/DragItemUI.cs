using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragItemUI : ItemUI 
{
    public void show()
    {
        gameObject.SetActive(true);
    }
    public void hidden()
    {
        gameObject.SetActive(false);
    }
    public void SetLocalPosition(Vector2 position)
    {
        transform.localPosition = position;
    }

}
