using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockTypes : MonoBehaviour
{
    private Blocos blocos;

    public BlockType blockType;
    public int itemID;

    public string st_top;
    public string st_cond;
    public string st_mid;
    public string st_bot;

    public List<TMP_InputField> inputs;
    public List<TMP_Dropdown> dropdowns;


    public enum BlockType
    {
        IF,
        PRINT,
        USER_INPUT,
        WHILE,
        CONDITION,

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

    // Example code execution function
    public void OnValidate()
    {
        blocos = GetComponent<Blocos>();
        List<GameObject> blocosConectados = blocos.filhosBloco;

        switch (blockType)
        {
            case BlockType.IF:
                ExecuteIfBlock();
                break;
            case BlockType.PRINT:
                ExecutePrintBlock();
                break;
            case BlockType.USER_INPUT:
                ExecuteUserInputBlock();
                break;
            case BlockType.WHILE:
                ExecuteWhile();
                break;
            case BlockType.CONDITION:
                ExecuteCondition();
                break;
                // Add more cases for other block types
        }
    }

    private void ExecuteCondition()
    {
        this.st_top = this.inputs[0].text + " " + this.dropdowns[0].options[dropdowns[0].value].text + " " + this.inputs[1].text + " " + this.dropdowns[1].options[dropdowns[1].value].text + " ";
        this.st_mid = BuscarCondicao();
        this.st_bot = "";
        this.st_cond = "";
    }

    private void ExecuteWhile()
    {

        this.st_top = "while(";
        this.st_cond = BuscarCondicao() + ") {";
        this.st_mid = BuscarMeio();
        this.st_bot = "}";
    }

    private void ExecuteIfBlock()
    {

        this.st_top = "if(";
        this.st_cond = BuscarCondicao() + ") {";
        this.st_mid = BuscarMeio();
        this.st_bot = "}";
    }



    private void ExecutePrintBlock()
    {
        this.st_top = "print(";
        this.st_bot = ");";
        this.st_mid = BuscarMeio();
        this.st_cond = "";
    }

    private void ExecuteUserInputBlock()
    {
        this.st_bot = "";
        this.st_cond = "";
        this.st_top = "";
        // Ajuste o código para acessar os itens da lista de inputs
        if (inputs != null && inputs.Count > 0)
        {
            
            this.st_mid = '"' + inputs[0].text + '"';
            
        }
    }

    public string BuscarMeio()
    {
        // Busca todas os filhos com tag SnapBlocks
        List<Transform> snapPoints = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("SnapPoint"))
            {
                snapPoints.Add(child);
            }
        }
        string resultado = "";

        // For each found SnapPoint
        foreach (Transform snapPoint in snapPoints)
        {
            // Busca todos os filhos desse SnapBlock com tag Blocks
            List<Transform> blocos = new List<Transform>();
            foreach (Transform child in snapPoint)
            {
                if (child.CompareTag("Blocks"))
                {
                    blocos.Add(child);
                }
            }

            // Pra cada bloco encontrado
            foreach (Transform bloco in blocos)
            {
                // Acessa as strings dos blocos filhos
                BlockTypes componente = bloco.GetComponent<BlockTypes>();

                // Check if the component was found
                if (componente != null)
                {
                    resultado += componente.st_top + componente.st_cond + componente.st_mid + componente.st_bot;
                }
                else
                {
                    resultado += "Component not found in: " + bloco.name + "\n";
                }
            }
        }

        return resultado;
    }

    public string BuscarCondicao()
    {
        // Busca todos os filhos com tag SnapCondition
        List<Transform> snapConditions = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("SnapCondition"))
            {
                snapConditions.Add(child);
            }
        }

        string resultado = "";

        // Para cada SnapCondition encontrado
        foreach (Transform snapCondition in snapConditions)
        {
            // Busca todos os filhos desse SnapCondition com tag Blocks
            List<Transform> blocos = new List<Transform>();
            foreach (Transform child in snapCondition)
            {
                if (child.CompareTag("Blocks"))
                {
                    blocos.Add(child);
                }
            }

            // Para cada bloco encontrado
            foreach (Transform bloco in blocos)
            {
                // Acessa as strings dos blocos filhos
                BlockTypes componente = bloco.GetComponent<BlockTypes>();

                // Verifica se o componente foi encontrado
                if (componente != null)
                {
                    resultado += componente.st_top + componente.st_mid + componente.st_bot;
                }
                else
                {
                    resultado += "Component not found in: " + bloco.name + "\n";
                }
            }
        }

        return resultado;
    }



}
