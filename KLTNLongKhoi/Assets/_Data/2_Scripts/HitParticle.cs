using UnityEngine;

public class HitParticle : MonoBehaviour
{
    [SerializeField] private float destroyTime = 0.5f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
