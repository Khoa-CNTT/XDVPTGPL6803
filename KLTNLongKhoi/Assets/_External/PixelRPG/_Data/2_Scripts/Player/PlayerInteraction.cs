using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject btnPressToTalk;
    public NPC npcInteract;

    public NPC GetNPCInteract()
    {
        return npcInteract;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC") && collision.GetComponent<NPC>())
        {
            btnPressToTalk.SetActive(true);
            npcInteract = collision.GetComponent<NPC>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            if (btnPressToTalk) btnPressToTalk.SetActive(false);
            npcInteract = null;
        }
    }
}