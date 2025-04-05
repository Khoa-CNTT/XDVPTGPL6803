using UnityEngine;

/// <summary>
/// <c>Harvestable</c> represents the base class of all harvestable objects in the scene and inherits of "Interactable.cs"
/// </summary>
public abstract class Harvestable : Interactable
{
    [Header("Harvestable Properties")]
    [Tooltip("The time for how long the object needs to be harvested.")]
    [Range(0.1f, 99.9f)]
    [SerializeField]
    float harvestDuration = 3f;
    
    float harvestTime = 0f;// time for how long the player already "harvested" the object

    public ToolType toolType = ToolType.AXE;
    public bool isHarvested = false;

    /// <summary>
    /// <c>IncreaseHarvestTime</c> increases the harvestTime by Time.deltaTime.
    /// <code>harvestTime += Time.deltaTime;</code>
    /// </summary>
    public void IncreaseHarvestTime() => harvestTime += Time.deltaTime;

    /// <summary>
    /// <c>ResetHarvestTime</c><strong> resets the harvestTime to 0f.</strong>
    /// </summary>
    public void ResetHarvestTime() => harvestTime = 0f;

    /// <summary>
    /// <c>GetHarvestTime</c> gets the time for how long the player has already "harvested".
    /// </summary>
    /// <returns><strong>A float of the time for how long the player has already "harvested"</strong></returns>
    public float GetHarvestTime() => harvestTime;

    /// <summary>
    /// <c>GetHarvestTimeLeft</c> gets the time for how long a player still has to "harvest".
    /// </summary>
    /// <returns>A float containig the result of <strong>harvestDuration - harvestTime</strong></returns>
    public float GetHarvestTimeLeft() => harvestDuration - harvestTime;

    /// <summary>
    /// <c>GetHarvestDuration</c> gets the time for how long the object needs to be harvested until the interact method triggers.
    /// </summary>
    /// <returns><strong>A float of the time for how long the object needs to be harvested until the interact method triggers</strong></returns>
    public float GetHarvestDuration() => harvestDuration;
}
