using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Calendar : MonoBehaviour
{
    TimeManager timeManager;
    Image prevImage;
    Transform daysTransform;

    [SerializeField]
    Color currentDayColor;
    Color originDayColor;

    [SerializeField]
    TextMeshProUGUI monthText;
    [SerializeField]
    TextMeshProUGUI yearText;
    

    private void Start()
    {
        daysTransform = transform.Find("Days");
        timeManager = GameObject.Find("GameManager").GetComponent<TimeManager>();
        originDayColor = daysTransform.Find("1").gameObject.GetComponent<Image>().color;
    }

    void Update()
    {
        if(gameObject.activeSelf)
            RefreshCalendar();
    }

    void RefreshCalendar()
    {
        DateTime dateTime = timeManager.GetDateTime();

        int counter;

        int daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

        monthText.text = dateTime.ToString("MMMM");
        yearText.text = dateTime.ToString("yyyy");

        counter = 1;
        while (counter <= 31)
        {
            if (counter >= daysInMonth + 1)
            {
                daysTransform.Find(counter.ToString()).gameObject.SetActive(false);
            }
            else
            {
                daysTransform.Find(counter.ToString()).gameObject.SetActive(true);
            }

            counter++;
        }



        if (prevImage != daysTransform.Find(dateTime.Day.ToString()).gameObject.GetComponent<Image>())
        {
            if (prevImage != null)
                prevImage.color = originDayColor;

            daysTransform.Find(dateTime.Day.ToString()).gameObject.GetComponent<Image>().color = currentDayColor;
            prevImage = daysTransform.Find(dateTime.Day.ToString()).gameObject.GetComponent<Image>();
        }
    }
}
