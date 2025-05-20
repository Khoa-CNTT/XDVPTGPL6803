using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Enitity")]
	[SerializeField] EntityData entityData;
    [SerializeField] bool isDestroyed;

    public EntityData EntityData { get => entityData; set => entityData = value; }
    public bool IsDestroyed { get => isDestroyed; set => isDestroyed = value; }

    public virtual void DestroyEntity()
    {
        isDestroyed = true;
        gameObject.SetActive(false);
    }

    // This method resets the entity by setting isDestroyed to false and activating the game object
    public virtual void ResetEntity()
    {
        isDestroyed = false;
        gameObject.SetActive(true);
    }
}

