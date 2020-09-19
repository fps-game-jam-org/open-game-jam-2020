using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{

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
