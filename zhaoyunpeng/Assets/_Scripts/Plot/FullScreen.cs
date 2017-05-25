using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreen : MonoBehaviour
{

    void Start()
    {
        //得到主Camera
        Camera cam = Camera.main;
        //设置为正交
        cam.orthographic = true;
        //设置大小为屏幕宽度的一半。乘以10是为了以后使用物体缩放时直接使用像素
        cam.orthographicSize = Screen.height / 2 * 10;
        //设置背景Plane的大小
        gameObject.transform.localScale = new Vector3(cam.pixelWidth, 1, cam.pixelHeight);
    }

    //以后Plane上的物体都使用此缩放
    static public Vector3 GetScale()
    {
        Camera cam = Camera.main;
        return new Vector3(cam.pixelHeight, 1, cam.pixelHeight);
    }
}
