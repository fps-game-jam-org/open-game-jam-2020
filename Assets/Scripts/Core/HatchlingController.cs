using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Attach this to the bird nest.  A Pick Up Carrier can can carry
/// Pick Up Items of type Construction to a Game Object with this
/// component.  If a user-defined number of Construction items are
/// consumed, then the nest will be flagged as fully constructed.
/// </summary>
public class HatchlingController : MonoBehaviour
{
    [Tooltip("The number of construction items needed for the nest to "
             + "be fully built")]
    public int constructionMaterialsNeeded = 3;

    public int ConstructionMaterialsConsumed
    {
        get { return _constructionMaterialsConsumed; }
    }

    public bool FullyConstructed {
        get { return _fullyConstructed; }
    }

    private int _constructionMaterialsConsumed;
    private bool _fullyConstructed;

    void Start()
    {
        _constructionMaterialsConsumed = 0;
        _fullyConstructed = false;
    }

    public void AddConstructionItem()
    {
        _constructionMaterialsConsumed =
            Mathf.Clamp(++_constructionMaterialsConsumed,
                        0,
                        constructionMaterialsNeeded);

        if (_constructionMaterialsConsumed == constructionMaterialsNeeded)
        {
            _fullyConstructed = true;
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
