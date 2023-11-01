using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VerEstatistica : MonoBehaviour
{

    public int levelId;
    public TMP_Text tempo;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Nível " + levelId))
        {
            tempo.text = string.Format("Atividade concluida em {0} segundos", PlayerPrefs.GetFloat("Nível "+levelId));
        }
        else
        {
            tempo.text = "Atividade ainda não concluida.";
        }
    }
}
