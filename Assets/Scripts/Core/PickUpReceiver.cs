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
    public void ConsumeItem(PickUpItemType item)
    {
        switch (item)
        {
            case PickUpItemType.Food:
                EatFood();
                break;
            default:
                break;
        }
    }

    private void EatFood()
    {
        Debug.Log("I got some food");
    }
}
