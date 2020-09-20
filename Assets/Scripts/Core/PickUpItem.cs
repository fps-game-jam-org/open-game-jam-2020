using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickUpItemType
{
    Food,
    None,
}

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
    [Tooltip("The sort of item it is as the receiver is concerned")]
    public PickUpItemType itemType = PickUpItemType.None;

    void OnCollisionEnter2D(Collision2D other)
    {
        PickUpReceiver receiver =
            other.gameObject.GetComponent<PickUpReceiver>();
        if (receiver != null
            && transform.parent.GetComponent<PickUpCarrier>() != null)
        {
            receiver.ConsumeItem(itemType);
            transform.parent.GetComponent<PickUpCarrier>()
                            .RemoveItem(gameObject);

        }
    }
}
