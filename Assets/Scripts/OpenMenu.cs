using UnityEngine;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour
{
    public Button button; // Refer�ncia ao bot�o no Unity Inspector
    public GameObject objectToMove; // Refer�ncia ao objeto a ser movido no Unity Inspector

    private bool isMoved = false; // Flag para indicar se o objeto foi movido ou n�o
    private Vector3 posicaoOriginal; // Posi��o original do objeto a ser movido
    private Vector3 posicaoOriginalBotao; // Posi��o original do bot�o

    private void Start()
    {
        posicaoOriginal = objectToMove.transform.localPosition; // Armazena a posi��o original do objeto a ser movido
        posicaoOriginalBotao = button.transform.localPosition; // Armazena a posi��o original do bot�o

        button.onClick.AddListener(MoverObjeto); // Adiciona um ouvinte de clique ao bot�o para chamar o m�todo MoverObjeto
    }

    private void MoverObjeto()
    {
        if (!isMoved) // Se o objeto n�o foi movido
        {
            Vector3 novaPosicao = objectToMove.transform.localPosition;
            Vector3 novaPosicaoBotao = button.transform.localPosition;

            novaPosicao.x -= 875f; // Move o objeto na dire��o do eixo x em -8 unidades
            novaPosicaoBotao.x -= 600f; // Move o bot�o na dire��o do eixo x em -6 unidades
            objectToMove.transform.localPosition = novaPosicao; // Define a nova posi��o do objeto
            button.transform.localPosition = novaPosicaoBotao; // Define a nova posi��o do bot�o

            isMoved = true; // Define a flag como verdadeira, indicando que o objeto foi movido
        }
        else // Se o objeto foi movido
        {
            objectToMove.transform.localPosition = posicaoOriginal; // Retorna o objeto para a posi��o original
            button.transform.localPosition = posicaoOriginalBotao; // Retorna o bot�o para a posi��o original

            isMoved = false; // Define a flag como falsa, indicando que o objeto n�o est� mais movido
        }
    }

    // M�todo para exibir apenas os itens comprados no menu
    private void ExibirItensComprados()
    {
        // Iterar sobre todos os itens da loja
        foreach (Transform child in objectToMove.transform)
        {
            ItemLoja item = child.GetComponent<ItemLoja>(); // Obter o componente ItemLoja do filho

            if (item.comprado) // Verificar se o item foi comprado
            {
                child.gameObject.SetActive(true); // Mostrar o item se estiver comprado
            }
            else
            {
                child.gameObject.SetActive(false); // Ocultar o item se n�o estiver comprado
            }
        }
    }
}
