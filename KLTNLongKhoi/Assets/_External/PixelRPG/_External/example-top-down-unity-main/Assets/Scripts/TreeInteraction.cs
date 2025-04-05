
using UnityEngine;

public class TreeInteraction : Harvestable
{
    [SerializeField]
    Item dropItem;

    
    public override string GetDescription()
    {
        if (isInRange())
            return "Baum muss weg";
        else  
            return "Tree is not in range";
    }
    public override void Interact()
    {
        if (!isHarvested)
        {
            Debug.Log("Harvest BAUM");
            Inventory.PlayerInstance.Add(dropItem, Random.Range(1, 10));
            isHarvested = true;
        }
        
    }
}