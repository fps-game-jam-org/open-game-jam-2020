using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
