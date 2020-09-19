using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject BackButtonObject = GameObject.Find("/Canvas/Back Button");
        Button BackButton = BackButtonObject.GetComponent<Button>();
        BackButton.onClick.AddListener(OnBackClicked);
    }

    void OnBackClicked() {
        //load title scene
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }
}
