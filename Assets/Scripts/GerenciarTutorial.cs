using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GerenciarTutorial : MonoBehaviour
{
    private int currentPopupIndex = 0;
    public GameObject[] popUps;

    public TMP_Text olaUsuario; // Reference to your UI Text component

    public Button[] botoes;

    public void ProximoPopUp()
    {
        olaUsuario.text = string.Format("Olá {0}{1}", PlayerPrefs.GetString("Username"), "!");

        if (currentPopupIndex < popUps.Length)
        {
            popUps[currentPopupIndex].SetActive(false);
            currentPopupIndex++;
            if (currentPopupIndex < popUps.Length)
            {
                popUps[currentPopupIndex].SetActive(true);
            }

        }
        if (currentPopupIndex == 4)
        {
            foreach (Button b in botoes)
            {
                b.interactable = true;
            }
        }
    }
}

