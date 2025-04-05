using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vitality : MonoBehaviour
{   
    Player player;

    [Header("Vitality")]
    [Range(0f, 1f)]
    [SerializeField]
    public float health = 1f;

    [Range(0f, 1f)]
    [SerializeField]
    public float food = 1f;

    [Range(0f, 1f)]
    [SerializeField]
    public float drink = 1f;

    [Header("Vitality Modifier Per Interval")]
    [Range(0f, 50f)]
    [SerializeField]
    float healthModifier = 30f;

    [Range(0f, 5f)]
    [SerializeField]
    float foodModifier = 0.4f;

    [Range(0f, 5f)]
    [SerializeField]
    float drinkModifier = 0.6f;

    private void Start()
    {
        TimeManager.OnTimeInterval += VitalityInterval;
        player = gameObject.GetComponent<Player>();
    }

    
    void VitalityInterval()
    {
        
        food -= foodModifier / 1000;
        drink -= drinkModifier / 1000;

        if(food <= 0f || drink <= 0f)
        {
            health -= healthModifier / 1000;
        }

        if(health <= 0f)
        {
            player.Die();
        }
    }

}
