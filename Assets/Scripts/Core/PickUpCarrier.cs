using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Attach this component to a GameObject that needs to pick up items,
/// e.g. a player character.  A maximum of 1 item can be carried at a
/// time.
/// </summary>
public class PickUpCarrier : MonoBehaviour
{
    private bool isCarryingItem = false;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PickUpItem>() != null
            && !isCarryingItem)
        {
            other.gameObject.transform.SetParent(transform);
            isCarryingItem = true;
        }
    }

    public void ConsumeItem(GameObject gameObject)
    {
        if (isCarryingItem)
        {
            Destroy(gameObject);
            isCarryingItem = false;
        }
    }
}
