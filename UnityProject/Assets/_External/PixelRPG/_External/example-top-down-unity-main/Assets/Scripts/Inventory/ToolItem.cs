using UnityEngine;

[CreateAssetMenu(fileName = "ToolItem", menuName = "Items/Tool", order = 2)]
public class ToolItem : Item
{


    public ToolType toolType;
    float durability = 1f;

    SpriteRenderer playerHandSpriteRenderer;

    public override void OnSelect()
    {
        playerHandSpriteRenderer = GameObject.Find("Player").transform.Find("Hand").gameObject.GetComponent<SpriteRenderer>();
        if (isSelected)
        {
            playerHandSpriteRenderer.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else
        {
            playerHandSpriteRenderer.GetComponent<SpriteRenderer>().sprite = null;
        }

    }
}