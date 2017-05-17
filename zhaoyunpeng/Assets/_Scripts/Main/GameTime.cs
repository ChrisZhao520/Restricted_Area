using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour {
    public enum TimeOfDay
    {
        Idle,
        sunRise,
        sunSet
    }

    public Transform[] sun;                             // 一个包含所有太阳的数组
    public float dayCycleInMinutes = 1;

    public float sunRise;                               // 每天太阳开始升起的时间
    public float sunSet;                                // 每天太阳开始落山的时间
    public float skyboxBlendModifier;                   // 天空盒中纹理混合的速度

    public Color ambLightMin;
    public Color ambLightMax;

    public float morningLight;
    public float nightLight;

    private bool _isMorning = false;

    private Sun[] _sunScript;
    private float _degreeRotation;
    private float _timeOfDay;

    private float _dayCycleInSeconds;

    private const float SECOND = 1;
    private const float MINUTE = 60 * SECOND;
    private const float HOUR = 60 * MINUTE;
    private const float DAY = 24 * HOUR;

    private const float DEGREES_PER_SECOND = 360 / DAY;

    private TimeOfDay _tod;
    private float _noonTime;                            // 正午的时间
    private float _morningLength;
    private float _eveningLength;
    public int _survialday;                             // 存活的天数

	// Use this for initialization
	void Start () {
        _tod = TimeOfDay.Idle;
        _survialday = 0;

        _dayCycleInSeconds = dayCycleInMinutes * MINUTE;

        RenderSettings.skybox.SetFloat("_Blend", 0);

        _sunScript = new Sun[sun.Length];

        for (int cnt = 0; cnt < sun.Length; cnt++)
        {
            Sun temp = sun[cnt].GetComponent<Sun>();
            if (temp == null)
            {
                Debug.LogWarning("Sun script not found. Adding it.");
                sun[cnt].gameObject.AddComponent<Sun>();
                temp = sun[cnt].GetComponent<Sun>();
            }
            _sunScript[cnt] = temp;
        }

        _timeOfDay = 0;
        _degreeRotation = DEGREES_PER_SECOND * DAY / _dayCycleInSeconds;

        sunRise *= _dayCycleInSeconds;
        sunSet *= _dayCycleInSeconds;
        _noonTime = _dayCycleInSeconds / 2;
        _morningLength = _noonTime - sunRise;               // 以秒计算早上的长度
        _eveningLength = sunSet - _noonTime;                // 以秒计算晚上的长度

        morningLight *= _dayCycleInSeconds;
        nightLight *= _dayCycleInSeconds;

        // 最小光强
        SetupLighting();
	}
	
	// Update is called once per frame
	void Update () {
        
        _timeOfDay += Time.deltaTime;
        GameManager.Instance.SetMaxSurvialDay(_survialday);
        if (_timeOfDay > _dayCycleInSeconds)
        {  
            _survialday++;
            _timeOfDay -= _dayCycleInSeconds;
            GameManager.Instance.SetSurvialDay(_survialday);
            
            //Debug.Log(_survialday);
        }
        //Debug.Log(_timeOfDay);
        //Debug.Log(Time.realtimeSinceStartup);

        // 根据时间控制户外灯光的效果
        if (!_isMorning && _timeOfDay > morningLight && _timeOfDay < nightLight)
        {
            _isMorning = true;
            Messenger<bool>.Broadcast("Morning Light Time", true);
            //Debug.Log("Morning");
        }
        else if (_isMorning && _timeOfDay > nightLight)
        {
            _isMorning = false;
            Messenger<bool>.Broadcast("Morning Light Time", false);
            //Debug.Log("Night");
        }


        for (int cnt = 0; cnt < sun.Length; cnt++) { 
            sun[cnt].Rotate(new Vector3(_degreeRotation, 0, 0) * Time.deltaTime);
        }

        if (_timeOfDay > sunRise && _timeOfDay < _noonTime)
        {
            AdjustLighting(true);
        }
        else if (_timeOfDay > _noonTime && _timeOfDay < sunSet)
        {
            AdjustLighting(false);
        }

        if (_timeOfDay > sunRise && _timeOfDay < sunSet && RenderSettings.skybox.GetFloat("_Blend") < 1)
        {
            _tod = GameTime.TimeOfDay.sunRise;
            BlendSkybox();
        }
        else if (_timeOfDay > sunSet && RenderSettings.skybox.GetFloat("_Blend") > 0)
        {
            _tod = GameTime.TimeOfDay.sunSet;
            BlendSkybox();
        }
        else
        {
            _tod = GameTime.TimeOfDay.Idle;
        }
    }

    private void BlendSkybox()
    {
        float temp = 0;
        
        switch(_tod){
            case TimeOfDay.sunRise:
                temp = (_timeOfDay - sunRise) / _dayCycleInSeconds * skyboxBlendModifier;
                break;
            case TimeOfDay.sunSet:
                temp = (_timeOfDay - sunSet) / _dayCycleInSeconds * skyboxBlendModifier;
                temp = 1 - temp;
                break;
        }

        RenderSettings.skybox.SetFloat("_Blend", temp);

        //Debug.Log(temp);
    }

    private void SetupLighting()
    {
        RenderSettings.ambientLight = ambLightMin;

        for (int cnt = 0; cnt < _sunScript.Length; cnt++)
        {
            if (_sunScript[cnt].giveLight)
            {
                sun[cnt].GetComponent<Light>().intensity = _sunScript[cnt]._minLightBrightness;
            }
        }
    }

    private void AdjustLighting(bool brighten)
    {
        float pos = 0;
        if (brighten)
        {
            //Debug.Log(brighten); 
            pos = (_timeOfDay - sunRise) / _morningLength;        // 得到太阳在早晨天空中的位置
        }
        else
        {
            //Debug.Log(brighten);
            pos = (sunSet - _timeOfDay) / _eveningLength;        // 得到太阳在早晨天空中的位置
        }

        RenderSettings.ambientLight = new Color(ambLightMin.r + ambLightMax.r * pos, 
                                                ambLightMin.g + ambLightMax.g * pos, 
                                                ambLightMin.b + ambLightMax.b * pos);
        //Debug.Log(RenderSettings.ambientLight);

        for (int cnt = 0; cnt < _sunScript.Length; cnt++)
        {
            if (_sunScript[cnt].giveLight)
            {
                //Debug.Log(pos);
                _sunScript[cnt].GetComponent<Light>().intensity = _sunScript[cnt]._maxFlareBrightness * pos;
            }
        }
    }
}