using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Environments/Sun")]
public class Sun : MonoBehaviour {

    public float _minLightBrightness;
    public float _maxLightBrightness;

    public float _minFlareBrightness;
    public float _maxFlareBrightness;

    public bool giveLight = false;

    void Start()
    {
        if (GetComponent<Light>() != null)
        {
            //Debug.Log("123");
            giveLight = true;
        }
    }
}
