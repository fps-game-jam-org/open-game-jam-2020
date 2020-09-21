using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimpleWinCondition : MonoBehaviour
{
    public TextMeshProUGUI winText;

    private HatchlingController _hatchlingController;

    void Start()
    {
        _hatchlingController = GetComponent<HatchlingController>();

        if (winText != null)
        {
            winText.text = "";
        }
    }

    void Update()
    {
        if (_hatchlingController.ConstructionMaterialsConsumed
            == _hatchlingController.constructionMaterialsNeeded)
        {
            winText.text = "You Win!";
        }
    }
}
