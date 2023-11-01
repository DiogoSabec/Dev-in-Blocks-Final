using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemLoja : MonoBehaviour
{
    public int itemID;
    public TMP_Text txt_Preco;

    public GameObject GerenteLoja;

    public bool comprado;

    void Start()
    {
        // Verificar se o item está comprado nos PlayerPrefs e atualizar o status de 'comprado' de acordo
        if (PlayerPrefs.HasKey("Item" + itemID))
        {
            int itemComprado = PlayerPrefs.GetInt("Item" + itemID);
            if (itemComprado == 1)
            {
                comprado = true;
            }
            else
            {
                comprado = false;
            }
        }
        else
        {
            comprado = false;
        }
    }

    void Update()
    {
        txt_Preco.text = "Preço: $" + GerenteLoja.GetComponent<GerenciarLoja>().itensLoja[2, itemID].ToString();

        Button b = this.GetComponent<Button>();
        if (comprado)
        {
            b.interactable = false;
        }
        else
        {
            b.interactable = true;
        }
    }
}