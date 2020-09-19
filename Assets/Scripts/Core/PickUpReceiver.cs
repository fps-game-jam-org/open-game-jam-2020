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
    void OnCollisionEnter2D(Collision2D other)
    {
        PickUpItem itemReceived =
            other.gameObject.transform.GetComponentInChildren<PickUpItem>();
        if (itemReceived != null)
        {
            switch (itemReceived.itemType)
            {
                case "food":
                    EatFood();
                    break;
                default:
                    Debug.Log("default");
                    break;
            }
        }
    }

    private void EatFood()
    {
        Debug.Log("I got some food");
    }
}
