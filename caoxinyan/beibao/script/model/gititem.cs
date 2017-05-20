using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gititem : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            
        if (Input.GetKey(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo,1))
            {
                //划出射线，只有在scene视图中才能看到
                GameObject gameObj = hitInfo.collider.gameObject;
                if (gameObj.tag == "qiang")//当射线碰撞目标为qiang类型的物品 ，执行拾取操作
                {
                    backpack_manger.Instancce.StoreItem(0);
                    Destroy(gameObj);
                    return;
                }
                if (gameObj.tag == "mubang")
                {
                    //Debug.Log("pick up!");
                    backpack_manger.Instancce.StoreItem(2);
                    Destroy(gameObj);
                    return;
                }
                if (gameObj.tag == "bishou")
                {
                    
                    backpack_manger.Instancce.StoreItem(1);
                    Destroy(gameObj);
                    return;
                }
                if (gameObj.tag == "banzhuan")
                {
                    
                    backpack_manger.Instancce.StoreItem(3);
                    Destroy(gameObj);
                    return;
                }
            }
        }
    }
}
