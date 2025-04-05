using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalityUI : MonoBehaviour
{
    Vitality playerVitality;

    [SerializeField]
    Image healthBar;

    [SerializeField]
    Image foodBar;

    [SerializeField]
    Image drinkBar;


    private void Start()
    {
        TimeManager.OnTimeInterval += UpdateBars;
        playerVitality = GameObject.Find("Player").GetComponent<Vitality>();
    }

    void UpdateBars()
    {
        healthBar.fillAmount = playerVitality.health;
        foodBar.fillAmount = playerVitality.food;
        drinkBar.fillAmount = playerVitality.drink;
    }
}
