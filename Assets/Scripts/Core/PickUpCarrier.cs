using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
