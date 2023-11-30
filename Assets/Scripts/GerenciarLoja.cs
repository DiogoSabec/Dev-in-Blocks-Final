using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class GerenciarLoja : MonoBehaviour
{
    public int[,] itensLoja = new int[6, 6];
    private int moedas;
    public TMP_Text txt_Moeda;

    private List<int> itensComprados = new List<int>();

    void Start()
    {
        moedas = PlayerPrefs.GetInt("Dinheiro");

        txt_Moeda.text = "Dinheiro: " + moedas.ToString();

        //ids
        itensLoja[1, 1] = 1;
        itensLoja[1, 2] = 2;
        itensLoja[1, 3] = 3;
        itensLoja[1, 4] = 4;
        itensLoja[1, 5] = 5;

        //preço
        itensLoja[2, 1] = 10;
        itensLoja[2, 2] = 20;
        itensLoja[2, 3] = 30;
        itensLoja[2, 4] = 40;
        itensLoja[2, 5] = 50;

        // Recuperar itens comprados do PlayerPrefs
        string itensCompradosString = PlayerPrefs.GetString("ItensComprados");
        if (!string.IsNullOrEmpty(itensCompradosString))
        {
            string[] itens = itensCompradosString.Split(',');
            foreach (string item in itens)
            {
                itensComprados.Add(int.Parse(item));
            }
        }
    }

    public void Comprar()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ItemLoja itemLoja = ButtonRef.GetComponent<ItemLoja>();

        if (moedas >= itensLoja[2, itemLoja.itemID] && !itemLoja.comprado)
        {
            moedas -= itensLoja[2, itemLoja.itemID];
            PlayerPrefs.SetInt("Dinheiro", moedas);
            txt_Moeda.text = "Dinheiro: " + moedas.ToString();

            itemLoja.comprado = true;

            // Salvar o item como comprado
            itensComprados.Add(itemLoja.itemID);
            PlayerPrefs.SetInt("Item" + itemLoja.itemID, 1); // Atualizar o PlayerPrefs para indicar que o item foi comprado

            
        }
        else
        {
            StartCoroutine(PiscarBotao(ButtonRef));
        }
    }


    IEnumerator PiscarBotao(GameObject ButtonRef)
    {
        var buttonImage = ButtonRef.GetComponent<Image>();
        if (buttonImage == null)
        {
            Debug.LogError("Componente de imagem não encontrado no botão.");
            yield break;
        }

        Color originalColor = buttonImage.color;
        Color redColor = Color.red;
        float duration = 0.1f; // ajuste a duração conforme necessário
        int numBlinks = 5; // ajuste o número de piscadas conforme necessário

        for (int i = 0; i < numBlinks; i++)
        {
            buttonImage.color = redColor;
            yield return new WaitForSeconds(duration);
            buttonImage.color = originalColor;
            yield return new WaitForSeconds(duration);
        }

        // Instrução de retorno adicionada
        yield break;
    }
}
