using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    [Header("Time")]
    [Tooltip("Day length in Minutes")]
    [SerializeField]
    private float _targetDayLength = 0.5f;
    public float targetDayLength
    {
        get
        {
            return _targetDayLength;
        }
    }
    [SerializeField]
    [Range(0f, 1f)]
    private float _timeOfDay;
    public float timeOfDay
    {
        get
        {
            return _timeOfDay;
        }
    }
    [SerializeField]
    private int _dayNumber = 0;
    public int dayNumber
    {
        get
        {
            return _dayNumber;
        }
    }
    [SerializeField]
    private int _yearNumber = 0;
    public int yearNumber
    {
        get
        {
            return _yearNumber;
        }
    }
    private float _timeScale = 100f;
    [SerializeField]
    private int _yearLength = 100;
    public float yearLength
    {
        get
        {
            return _yearLength;
        }
    }
    public bool pause = false;
    [SerializeField]
    private AnimationCurve timeCurve;
    private float timeCurveNormalization;

    [Header("Sun Light")]
    [SerializeField]
    private Transform dailyRotation;
    [SerializeField]
    private Light sun;
    private float intensity;
    [SerializeField]
    private float sunBaseIntensity = 1f;
    [SerializeField]
    private float sunVariation = 1.5f;
    [SerializeField]
    private Gradient sunColor;

    [Header("Seasonal Variables")]
    [SerializeField]
    private Transform sunSeasonalRotation;
    [SerializeField]
    [Range(-45f, 45f)]
    private float maxSeasonalTilt;

    [Header("Modules")]
    private List<ModuleBase> moduleList = new List<ModuleBase> ();

    private void Start()
    {
        NormalTimeCurve();
    }

    private void Update()
    {
        if (!pause)
        {
            UpdateTimeScale();
            UpdateTime();
        }
        AdjustSunRotation();
        SunIntensity();
        AdjustSunColor();
        UpdateModule();
    }

    private void UpdateTimeScale()
    {
        _timeScale = 24 / (_targetDayLength / 60);
        _timeScale *= timeCurve.Evaluate(_timeOfDay);
        _timeScale /= timeCurveNormalization;
    }

    private void NormalTimeCurve()
    {
        float stepSize = 0.01f;
        int numberSteps = Mathf.FloorToInt(1f / stepSize);
        float curveTotal = 0;

        for (int i = 0; i < numberSteps; i++)
        {
            curveTotal += timeCurve.Evaluate(i * stepSize);
        }
        timeCurveNormalization = curveTotal / numberSteps;
    }

    private void UpdateTime()
    {
        _timeOfDay += Time.deltaTime * _timeScale / 86400;
        if (_timeOfDay > 1)
        {
            _dayNumber++;
            _timeOfDay -= 1;
            if (_dayNumber > _yearLength)
            {
                _yearNumber++;
                _dayNumber = 0;
            }
        }
    }
    private void AdjustSunRotation()
    {
        float sunAngel = timeOfDay * 360f;
        dailyRotation.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, sunAngel));

        float seasonalAngle = -maxSeasonalTilt * Mathf.Cos(dayNumber / _yearLength * 2f * Mathf.PI);
        sunSeasonalRotation.localRotation = Quaternion.Euler(new Vector3(seasonalAngle, 0f, 0f));
    }

    private void SunIntensity()
    {
        intensity = Vector3.Dot(sun.transform.forward, Vector3.down);
        intensity = Mathf.Clamp01(intensity);

        sun.intensity = intensity * sunVariation + sunBaseIntensity;
    }

    private void AdjustSunColor()
    {
        sun.color = sunColor.Evaluate(intensity);
    }

    public void AddModule(ModuleBase module)
    {
        moduleList.Add(module);
    }

    public void RemoveModule(ModuleBase module)
    {  
        moduleList.Remove(module);
    }

    private void UpdateModule()
    {
        foreach (ModuleBase module in moduleList)
        {
            module.UpdateModule(intensity);
        }
    }
}
