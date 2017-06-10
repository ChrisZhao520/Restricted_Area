using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class GridUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    #region 鼠标移入移出
    public static Action<Transform> OnEnter;
	public static Action OnExit;
	public void OnPointerEnter (PointerEventData eventData )
	{
        if (eventData.pointerEnter.tag == "Grid")
        {
            if (OnEnter != null)
            {
                OnEnter(transform);
            }

        }
	}
    
    public void OnPointerExit (PointerEventData eventData )
	{
        if (eventData.pointerEnter.tag == "Grid")
        {
            if (OnExit != null)
            {
                OnExit();
            }
        }
    }
    #endregion

    #region  拖拽
    public static Action<Transform> OnLeftBeginDrag;
    public static Action<Transform, Transform> OnLeftEndDrag;
    //public static Action DoubleClick;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (OnLeftBeginDrag != null)
            {
                OnLeftBeginDrag(transform);
            }
        }
    }
   
    public void OnDrag(PointerEventData eventData)
    {
    
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (OnLeftEndDrag != null)
            {
                if (eventData.pointerEnter == null)
                {
                    OnLeftEndDrag(transform, null);
                }
                else
                {
                    OnLeftEndDrag(transform, eventData.pointerEnter.transform);
                }
               
            }
        }
    }
    #endregion

    #region    背包点击
    public static Action<Transform> clickItem;
    public static Action<Transform> zbdaoju;
    public static Action<Transform> xianshi;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (zbdaoju != null)
            {
                xianshi(transform);
                zbdaoju(transform);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (clickItem != null)
            {

                clickItem(transform);
            }
        }
    }
    #endregion



    
}
