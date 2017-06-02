using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class backpack_manger : MonoBehaviour {

	private static backpack_manger _instancce;
	public static backpack_manger Instancce{get{ return _instancce;}}
	public gridpanelUI GridpanelUI;
    public Animator GridPanel;
	public TooltipUI TooltipUI;
    public DragItemUI DragItemUI;
    private bool isDrag = false;
    private bool isShow = false;
    public float doubeClickDelay = 0.4f;
    public bool firstClick = false;
    public float firstClickTime;
    public int targetDisplay;
    public GameObject camera;
    public string src;
    public Dictionary<int,item>ItemList=new Dictionary<int,item>();
	void Awake(){
		//单例
		_instancce = this;
        
		//数据
		load ();
		//事件
		GridUI.OnEnter += GridUI_OnEnter;
		GridUI.OnExit += GridUI_OnExit;
        GridUI.OnLeftBeginDrag += GridUI_OnLeftBeginDrag;
        GridUI.OnLeftEndDrag += GridUI_OnLeftEndDrag;
        //GridUI.OnDoubleClick += GridUI_OnDoubleClick;
	}
    void Update() {
        
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gameObject.transform as RectTransform, Input.mousePosition, null, out position);
        if (isDrag) {
            DragItemUI.show();
            DragItemUI.SetLocalPosition(position);
        }
        else if(isShow) {
            TooltipUI.show();
            TooltipUI.SetLocalPosition(position);
        }
       
    }

    //存储物体
    public void StoreItem(int itemId) {
        if (!ItemList.ContainsKey(itemId))
            return;
        Transform emptyGrid = GridpanelUI.GetEmptyGrid();
        if (emptyGrid == null)
        {
            return;
        }
        item temp = ItemList[itemId];
        this.CreatNewItem(temp, emptyGrid);
    }
    //模拟数据库取物体
	private void load(){
        ItemList = new Dictionary<int, item>();
        Weapon w1 = new Weapon(0,"枪","伤害力强大","");
        Weapon w2 = new Weapon(1, "匕首", "伤害力一般","");
        Weapon w3 = new Weapon(2, "木棒", "伤害力普通","");
        Weapon w4 = new Weapon(3, "板砖", "伤害力弱","");
        consumable w5 = new consumable(4, "血药", "加血", " ", 20 ,0);
        consumable w6 = new consumable(5, "血药", "加蓝", " ", 0, 20);
        Weapon w7 = new Weapon(6, "M92F手枪", "火力：150 装填速度：1.70 sec 弹夹容量：10", "");
        Weapon w8 = new Weapon(7, "M92F手枪弹夹", "弹夹容量：10", "");
        ItemList.Add(w1.Id, w1);
        ItemList.Add(w2.Id, w2);
        ItemList.Add(w3.Id, w3);
        ItemList.Add(w4.Id, w4);
        ItemList.Add(w5.Id, w5);
        ItemList.Add(w6.Id, w6);
        ItemList.Add(w7.Id, w7);
        ItemList.Add(w8.Id, w8);
	}
    private void GridUI_OnEnter(Transform gridtransform)
    { 
		item item = Itemmodel.GetItem (gridtransform.name);
        if (item == null)
        {
            
            return;
        }
        isShow = true;
        TooltipUI.UpdateTooltip(item.Name);
        
	}
    private void GridUI_OnExit()
    {
        isShow = false;
        TooltipUI.hidden();
	}
    private void GridUI_OnLeftBeginDrag(Transform gridTransform)
    {
        if (gridTransform.childCount == 0){
            return;
        }
        else{
            item item = Itemmodel.GetItem(gridTransform.name);
            
            DragItemUI.updateItem(item.Name);
            Destroy(gridTransform.GetChild(0).gameObject);
            
            isDrag = true;
        }
    }
    private void GridUI_OnLeftEndDrag(Transform prevtransform, Transform eventtransform)
    {

        isDrag = false;
        DragItemUI.hidden();
        if (eventtransform == null)//扔东西
        {
            item item = Itemmodel.GetItem(prevtransform.name);
            
            this.CreatNewItemPlane(item);
            Itemmodel.DeleteItem(prevtransform.name);
            

        }
        else if (eventtransform.tag == "Grid") {//拖到另一个格子或者当前格子
                 if (eventtransform.childCount == 0)//直接扔进去
                 {
                     item item = Itemmodel.GetItem(prevtransform.name);
                     this.CreatNewItem(item, eventtransform);
                     Itemmodel.DeleteItem(prevtransform.name);
                     
                 }
                 else //交换
                 {
                     Destroy(eventtransform.GetChild(0).gameObject);
                     item prevGridItem = Itemmodel.GetItem(prevtransform.name);
                     item enterGridItem = Itemmodel.GetItem(eventtransform.name);
                     this.CreatNewItem(prevGridItem, eventtransform);
                     this.CreatNewItem(enterGridItem, prevtransform);
                 }
        
        
        }
        else//拖到UI其他地方
        {
            item item = Itemmodel.GetItem(prevtransform.name);
            this.CreatNewItem(item, prevtransform);
        }
        

    }

    private void CreatNewItemPlane(item item)
    {
        
        string src = "_Prefabs" + "/" + item.Id;
        GameObject Itemprefabs = Resources.Load<GameObject>(src);
        GameObject itemGo = GameObject.Instantiate(Itemprefabs);
        GameObject Parent = GameObject.FindGameObjectWithTag("boot");
        itemGo.transform.parent = Parent.transform;
        itemGo.transform.position = camera.transform.TransformPoint(0,0,1);
        itemGo.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        
    }

    private void CreatNewItem(item item,Transform parent)
    {
        GameObject Itemprefabs = Resources.Load<GameObject>("_Prefabs/Item");
        Itemprefabs.GetComponent<ItemUI>().updateItem(item.Name);
        GameObject itemGo = GameObject.Instantiate(Itemprefabs);
        itemGo.transform.SetParent(parent);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        //存储数据
        Itemmodel.StoreItem(parent.name, item);
    }
    
}
