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

    public TMP_InputField inputField;


    public enum BlockType
    {
        IF,
        PRINT,
        USER_INPUT,
        
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
                // Add more cases for other block types
        }
    }

    private void ExecuteIfBlock()
    {
        this.st_top = "if(";
        this.st_bot = "}";
        
        this.st_mid = "){ \n" + BuscarMeio();
    }

    private void ExecutePrintBlock()
    {
        this.st_top = "print(";
        this.st_bot = ");";
        this.st_mid = BuscarMeio();
    }

    private void ExecuteUserInputBlock()
    {


        this.st_mid = '"' + inputField.text + '"';
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