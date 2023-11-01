using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdicionarBlocos : MonoBehaviour
{
    public GameObject objetoOriginal; // Objeto original a ser duplicado

    public int itemID;


    // Método para criar uma cópia do objeto com base no ID fornecido
    public void CriarCopiaDoBloco()
    {
        GameObject novaCopia = Instantiate(objetoOriginal, transform.position, transform.rotation);
        novaCopia.tag = "Blocks";
        novaCopia.SetActive(true);
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("Item" + itemID))
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
