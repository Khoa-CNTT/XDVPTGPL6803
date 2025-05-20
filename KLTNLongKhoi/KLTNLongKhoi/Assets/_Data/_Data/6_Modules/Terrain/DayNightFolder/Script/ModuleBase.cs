using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModuleBase : MonoBehaviour
{
    protected DayNight dayNightControl;

    private void OnEnable()
    {
        dayNightControl = this.GetComponent<DayNight>();
        if (dayNightControl != null )
            dayNightControl.AddModule(this);
    }

    private void OnDisable()
    {
        if ( dayNightControl != null )
            dayNightControl.RemoveModule(this);
    }

    public abstract void UpdateModule(float intensity);

}
