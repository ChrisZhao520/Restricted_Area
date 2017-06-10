using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class click : MonoBehaviour
{
    public int FirstgunNum;
    public int SecondgunNum;
    public int ThirdgunNum;
    public int FourthgunNum;
    public GridUI GridUI_clickItem;
    public Transform[] GridsFour;
    public Transform[] Grids;
    public TooltipUI TooltipUI;
	// Use this for initialization
    void Awake()
    {
        GridUI.zbdaoju += daoju;
        GridFourUI.Tdaoju += gridpanelUI_Tdaoju;
        GridUI.clickItem += Click_clickItem;
    }

    
    
	
	// Update is called once per frame
	void Update () {
        
	}
    public void Click_clickItem(Transform left)
    {
        Image img = left.GetComponent<Image>();
        Sprite sps = Resources.Load("_Images/grids", typeof(Sprite)) as Sprite;
        Sprite sp = Resources.Load("_Images/grid", typeof(Sprite)) as Sprite;
        if (left.childCount == 0)
        {
            return;
        }
        else
        {
            foreach (Transform t in Grids)
            {
                Image imgs = t.GetComponent<Image>();
                if (imgs.sprite == sps)
                {
                    if (t.name == left.name)
                    {
                    }
                    else
                    {
                        imgs.sprite = sp;
                    }
                    
                     
                }
            }
            if (img.sprite == sps)
            {
                img.sprite = sp;
            }
            else
            {
                img.sprite = sps;
            }
            if (name == left.name)
            {
                img.sprite = sps;
            }
            else
            {

            }
            
        }
        
    }

    //右键点击道具栏把道具放回背包
    private void gridpanelUI_Tdaoju(Transform obj)
    {
        if (obj.childCount == 0)
        {
            return;
        }
        else
        {
            
            item item = Itemmodel.GetItemFour(obj.name);
            backpack_manger.Instancce.StoreItem(item.Id);
            Destroy(obj.GetChild(0).gameObject);
        }
    }
    //右键把道具放到道具栏
    public void daoju(Transform gum)
    {
        if (gum.childCount == 0)
        {
            return;
        }
        else
        {
            item item = Itemmodel.GetItem(gum.name);
            if (item.Id >= 0 && item.Id <= FirstgunNum)
            {
                Itemmodel.DeleteItem(gum.name);
                Destroy(gum.GetChild(0).gameObject);
                Image image = gum.GetComponent<Image>();
                Sprite right = Resources.Load("_Images/grid", typeof(Sprite)) as Sprite;
                image.sprite = right;
                if (GridsFour[0].childCount == 0)
                {
                    CreatNewItem(item,GridsFour[0]);
                }
                else
                {
                    item it = Itemmodel.GetItemFour(GridsFour[0].name);
                    Destroy(GridsFour[0].GetChild(0).gameObject);
                    backpack_manger.Instancce.StoreItem(it.Id);
                    CreatNewItem(item, GridsFour[0]);
                }
                
            }
            else if (item.Id > FirstgunNum && item.Id <= SecondgunNum)
            {
                Itemmodel.DeleteItem(gum.name);
                Destroy(gum.GetChild(0).gameObject);
                Image image = gum.GetComponent<Image>();
                Sprite right = Resources.Load("_Images/grid", typeof(Sprite)) as Sprite;
                image.sprite = right;
                if (GridsFour[1].childCount == 0)
                {
                    CreatNewItem(item, GridsFour[1]);

                }
                else
                {
                    item it = Itemmodel.GetItemFour(GridsFour[1].name);
                    Destroy(GridsFour[1].GetChild(0).gameObject);
                    backpack_manger.Instancce.StoreItem(it.Id);
                    CreatNewItem(item, GridsFour[1]);
                }

            }
            else if (item.Id > SecondgunNum && item.Id <= ThirdgunNum)
            {
                Itemmodel.DeleteItem(gum.name);
                Destroy(gum.GetChild(0).gameObject);
                Image image = gum.GetComponent<Image>();
                Sprite right = Resources.Load("_Images/grid", typeof(Sprite)) as Sprite;
                image.sprite = right;
                if (GridsFour[2].childCount == 0)
                {
                    CreatNewItem(item, GridsFour[2]);

                }
                else
                {
                    item it = Itemmodel.GetItemFour(GridsFour[2].name);
                    Destroy(GridsFour[2].GetChild(0).gameObject);
                    backpack_manger.Instancce.StoreItem(it.Id);
                    CreatNewItem(item, GridsFour[2]);
                }

            }
            else if (item.Id > ThirdgunNum && item.Id <= FourthgunNum)
            {
                Itemmodel.DeleteItem(gum.name);
                Destroy(gum.GetChild(0).gameObject);
                Image image = gum.GetComponent<Image>();
                Sprite right = Resources.Load("_Images/grid", typeof(Sprite)) as Sprite;
                image.sprite = right;
                if (GridsFour[3].childCount == 0)
                {
                    CreatNewItem(item, GridsFour[3]);

                }
                else
                {
                    item it = Itemmodel.GetItemFour(GridsFour[3].name);
                    Destroy(GridsFour[3].GetChild(0).gameObject);
                    backpack_manger.Instancce.StoreItem(it.Id);
                    CreatNewItem(item, GridsFour[3]);
                }

            }
        }
    }
    private void CreatNewItem(item item, Transform parent)
    {
        GameObject Itemprefabs = Resources.Load<GameObject>("_Prefabs/Item");
        Itemprefabs.GetComponent<ItemUI>().updateItem(item.Id);
        GameObject itemGo = GameObject.Instantiate(Itemprefabs);
        itemGo.transform.SetParent(parent);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        //存储数据
        Itemmodel.StoreItemFour(parent.name, item);
    }
}
