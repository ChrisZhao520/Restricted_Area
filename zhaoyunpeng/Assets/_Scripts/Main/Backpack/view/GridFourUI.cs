using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class GridFourUI : MonoBehaviour ,IPointerClickHandler
{
    public static Action<Transform> Tdaoju;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Tdaoju != null)
            {
                Tdaoju(transform);
            }
        }
    }
}
