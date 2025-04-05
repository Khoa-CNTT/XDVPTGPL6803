using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI dateTimeText;
    TimeManager timeManager;

    private void Start()
    {
        timeManager = GameObject.Find("GameManager").GetComponent<TimeManager>();
        TimeManager.OnTimeInterval += OnInterval;
        
    }

    void OnInterval()
    {
        DateTime dateTime = timeManager.GetDateTime();
        if (dateTimeText != null)
            dateTimeText.text = dateTime.ToString("hh:mm tt / dd.MM.yyyy", timeManager.cultureInfo);
    }
}
