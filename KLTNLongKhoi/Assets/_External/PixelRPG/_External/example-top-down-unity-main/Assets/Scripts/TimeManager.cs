using System;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static Action OnTimeInterval;
    public static TimeManager instance;

    public enum PartOfDay
    {
        MORNING,
        AFTERNOON,
        EVENING,
        NIGHT
    }

    public PartOfDay partOfDay;

    [SerializeField]
    float intervalTime = 1.0f; // 1.0f -> 1 real second is 1 ingame minute

    int minutesPerInterval = 1;

    public CultureInfo cultureInfo = new CultureInfo("en-us");
    DateTime dateTime = new DateTime(1, 1, 1, 0, 0, 0);
    float timer;

    public DateTime GetDateTime() => dateTime;
    
    public string GetTime() => dateTime.ToString("hh:mm tt", cultureInfo);
    public string GetDate() => dateTime.ToString("dd/mm/yyyy", cultureInfo);
    public float GetintervalTime() => intervalTime;
    

    void Start()
    {
        timer = intervalTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            dateTime = dateTime.AddMinutes(minutesPerInterval);
            CheckPartsOfDay();
            OnTimeInterval?.Invoke();
            timer = intervalTime;
        }
    }

    void CheckPartsOfDay()
    {
        if (dateTime.Hour >= 22)
            partOfDay = PartOfDay.NIGHT;
        else if(dateTime.Hour < 6)
            partOfDay = PartOfDay.NIGHT;
        else if (dateTime.Hour >= 6 && dateTime.Hour < 12)
            partOfDay = PartOfDay.MORNING;
        else if (dateTime.Hour >= 12 && dateTime.Hour < 17)
            partOfDay = PartOfDay.AFTERNOON;
        else if (dateTime.Hour >= 17 && dateTime.Hour < 22)
            partOfDay = PartOfDay.EVENING;
    }
}
