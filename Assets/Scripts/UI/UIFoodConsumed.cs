using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIFoodConsumed : MonoBehaviour
{
    private HatchlingController _hatchlingController;
    private TMP_Text _text;

    void Start()
    {
        _hatchlingController = GetComponent<HatchlingController>();
        _text = GetComponentInChildren<TMP_Text>();        
    }

    void Update()
    {
        int constructionMaterialsConsumed =
            _hatchlingController.ConstructionMaterialsConsumed;
        int constructionMaterialsNeeded =
            _hatchlingController.constructionMaterialsNeeded;
        _text.text =
            $"{constructionMaterialsConsumed}/{constructionMaterialsNeeded}";
    }
}
