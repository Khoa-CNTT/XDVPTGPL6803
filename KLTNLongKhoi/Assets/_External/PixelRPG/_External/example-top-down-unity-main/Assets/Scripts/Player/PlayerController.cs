using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the player input, such as interactions
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject calendar;

    void Update()
    {
        if (Input.GetButtonDown("Calendar") && calendar != null)
        {
            calendar.SetActive(!calendar.activeSelf);
        }
    }
}
