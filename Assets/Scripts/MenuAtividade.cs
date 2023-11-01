using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuAtividade : MonoBehaviour
{
    public Button[] buttons;
    public TMP_Text usuario;

    private void Awake()
    {

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i=0; i<buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void OpenLevel(int levelId)
    {
        string levelName = "Atividade " + levelId;
        SceneManager.LoadScene(levelName);
    }
}