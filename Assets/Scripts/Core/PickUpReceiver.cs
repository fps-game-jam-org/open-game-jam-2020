using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Attach this component to any object that should be able to receive
/// items, e.g. nests for building materials or chicks for food.  Add
/// cases to the `switch` statement in `OnCollisionEnter2D` to have
/// behaviour for more item types.
/// </summary>
public class PickUpReceiver : MonoBehaviour
{
    public void ConsumeItem(PickUpItem item)
    {
        switch (item.type)
        {
            case PickUpItemType.Food:
                EatFood(item);
                break;
            case PickUpItemType.Construction:
                AddConstruction(item);
                break;
            default:
                break;
        }
    }

    private void EatFood(PickUpItem item)
    {
        Debug.Log("I got some food");
        Destroy(item.gameObject);
        item.GetComponentInParent<PickUpCarrier>().DropItem();
    }

    private void AddConstruction(PickUpItem item)
    {
        HatchlingController hatchlingController =
            GetComponent<HatchlingController>();
        if (hatchlingController != null)
        {
            if (hatchlingController.ConstructionMaterialsConsumed < 3)
            {
                item.GetComponentInParent<PickUpCarrier>().DropItem();
                Destroy(item.gameObject);
            }
            hatchlingController.AddConstructionItem();
        }
    }
}
