using UnityEngine;

/// <summary>
/// <c>Interactable</c> represents the base class of all interactable objects in the scene
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    
    /// <summary>
    /// <c>InteractinType</c> is the type of an interaction. So the "PlayerInteractin.cs" can handle the interaction input.
    /// 
    /// <list type="bullet">
    /// <item>Click: A single click which triggers the Interact Method</item>
    /// <item>Hold: Holding a until time is reached an then triggers the Interact Method(Time resets if the Player stops holding)</item>
    /// <item>Harvest: Same as Hold but time does not reset. So the time progress gets saved. Used for class "Harvastable.cs"</item>
    /// </list> 
    /// </summary>
    public enum InteractionType {
        Click,
        Hold,
        Harvest
    }
    [Header("Interactable Properties")]
    public InteractionType interactionType;

    [Tooltip("The time the player has to hold until the Interact method triggers.")]
    [Range(0.1f, 99.9f)]
    [SerializeField]
    float holdDuration = 1f; // The time the player has to hold until the Interact method triggers
    float holdTime; // The time for how long the player has already pressed

    // Used to measure the distance between the player and the object
    Transform playerTransform;

    [Tooltip("The range in which the player can interact with an object")]
    [Range(1f, 50f)]
    [SerializeField]
    float radius = 3f;

    [SerializeField]
    Transform centerPoint;

    void Awake()
    {
        if(centerPoint == null)
        {
            centerPoint = gameObject.transform;
        }
    }
    #region GETTER
    /// <summary>
    /// <c>GetDescription</c> gets the description of an interactable object.
    /// <list type="bullet">
    /// <item>The description is usally used to display a help text for the player (e.g. "Turn Lights On")</item>
    /// </list>
    /// </summary>
    /// <returns><strong>A string containing the description of the interactable object</strong></returns>
    public abstract string GetDescription();

    /// <summary>
    /// <c>GetHoldTime</c> gets the time for how long the player has already pressed.
    /// </summary>
    /// <returns><strong>A float of the time for how long the player has already pressed</strong></returns>
    public float GetHoldTime() => holdTime;

    /// <summary>
    /// <c>GetHoldDuration</c> gets the time the player has to hold until the Interact method triggers.
    /// </summary>
    /// <returns><strong>A float of the time the player has to hold until the Interact method triggers</strong></returns>
    public float GetHoldDuration() => holdDuration;

    /// <summary>
    /// <c>GetHoldTimeLeft</c> gets the time for how long a player still has to hold.
    /// </summary>
    /// <returns>A float containig the result of <strong>holdDuration - holdTime</strong></returns>
    public float GetHoldTimeLeft() => holdDuration - holdTime;

    /// <summary>
    /// <c>GetRadius</c> gets the maximum distance of a player is allowed to have in order to interact with an object.
    /// </summary>
    /// <returns><strong>A float containing the interaction distance</strong></returns>
    public float GetRadius() => radius;
    #endregion

    /// <summary>
    /// <c>Interact</c> is the method which gets called when a player start the interaction with an object.
    /// 
    /// <list type="bullet">
    /// <item>If the player clicks on an object</item>
    /// <item>If the holdTime is greater or equals the holdDuration</item>
    /// <item>If the harvestTime is greater or equals the harvestDuration</item>
    /// </list>
    /// 
    /// It usually gets called in the "PlayerInteraction.cs"
    /// </summary>
    public abstract void Interact();

    /// <summary>
    /// <c>IncreaseHoldTime</c> increases the holdTime by Time.deltaTime.
    /// <code>holdTime += Time.deltaTime;</code>
    /// </summary>
    public void IncreaseHoldTime() => holdTime += Time.deltaTime;

    /// <summary>
    /// <c>ResetHoldTime</c><strong> resets the holdTime to 0f.</strong>
    /// </summary>
    public void ResetHoldTime() => holdTime = 0f;

    /// <summary>
    /// <c>isInRange</c> checks if the player is in the interaction range of an interactable object.
    /// 
    /// <list type="bullet">
    /// <item><strong>True</strong>: Player is in range</item>
    /// <item><strong>False</strong>: Player is NOT in range</item>
    /// </list>
    /// </summary>

    /// <returns>A bool which says if the player is in the interaction range of an interactable object</returns>
    public bool isInRange()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").gameObject.transform; // Maybe singleton later?

        float distance = Vector2.Distance(centerPoint.position, playerTransform.position);

        if(distance <= radius)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    

    // Show interactable range in editor but NOT IN-GAME
    private void OnDrawGizmosSelected()
    {
        // Same as in "Awake()", cause the object does not get "awakend" in inspector
        if (centerPoint == null)
        {
            centerPoint = gameObject.transform;
        }

        // Gizmos are only visible in the scene view -> NOT visible IN-GAME (DEBUG Reasons)
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(centerPoint.position, radius);
    }
}
