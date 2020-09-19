using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Attach this script to an item you wish to be picked up.
/// GameObjects with the PickUpCarrier component will child GameObjects
/// with this component on collision enter.
/// The itemType attribute can be edited in the editor, and with
/// modification to PickUpReceiver, you can have many kinds of item
/// pickups, e.g. food, nest construction material, etc.
/// </summary>
public class PickUpItem : MonoBehaviour
{
    [ToolTip("The category of item as understood by the receiver")]
    public string itemType = "";

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PickUpReceiver>() != null
            && transform.parent.GetComponent<PickUpCarrier>() != null)
        {
            transform.parent.GetComponent<PickUpCarrier>()
                            .ConsumeItem(gameObject);
        }
    }
}
