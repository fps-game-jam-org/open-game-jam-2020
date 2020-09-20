using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchlingController : MonoBehaviour
{
    [Tooltip("The number of construction items needed for the nest to "
             + "be fully built")]
    public int constructionMaterialsNeeded = 3;
    public int ConstructionMaterialsConsumed
    {
        get { return _constructionMaterialsConsumed; }
    }

    private int _constructionMaterialsConsumed;
    private PickUpReceiver pickUpReceiver;

    void Start()
    {
        pickUpReceiver = GetComponent<PickUpReceiver>();
        _constructionMaterialsConsumed = 0;
    }

    public void AddConstructionItem()
    {
        _constructionMaterialsConsumed =
            Mathf.Clamp(++_constructionMaterialsConsumed,
                        0,
                        constructionMaterialsNeeded);

        if (_constructionMaterialsConsumed == constructionMaterialsNeeded)
        {
            Debug.Log("Nest fully constructed");
        }
        else
        {
            Debug.Log("Nest construction "
                      + $"{_constructionMaterialsConsumed}"
                      + $"/{constructionMaterialsNeeded}");
        }
    }
}
