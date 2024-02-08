using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Gerenciamento : MonoBehaviour
{
    private int dinheiro = 0;

    public TMP_Text txtDinheiro;

    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialCompleto") == 0)
        {
            SceneManager.LoadScene("Tutorial");
            PlayerPrefs.SetInt("TutorialCompleto", 1);
            PlayerPrefs.SetInt("Item1", 1);
            PlayerPrefs.SetInt("Item2", 1);

        }

        if (txtDinheiro != null)
        {
            txtDinheiro.text = string.Format("Dinheiro: {0}", PlayerPrefs.GetInt("Dinheiro"));
        }
    }

    public void Proximo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Sair()
    {
        Debug.Log("Saiu");
        Application.Quit();
    }

    public void Voltar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene(0);
    }

    //Carregar e desbloquear procima fase
    public void DesbloquearNovaAtv()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    public void AdicionarDinheiro(int adicionar)
    {
        dinheiro = PlayerPrefs.GetInt("Dinheiro");
        dinheiro += adicionar;
        PlayerPrefs.SetInt("Dinheiro", dinheiro);
    }
}


