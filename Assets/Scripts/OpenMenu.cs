using UnityEngine;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour
{
    public Button button; // Referência ao botão no Unity Inspector
    public GameObject objectToMove; // Referência ao objeto a ser movido no Unity Inspector

    private bool isMoved = false; // Flag para indicar se o objeto foi movido ou não
    private Vector3 posicaoOriginal; // Posição original do objeto a ser movido
    private Vector3 posicaoOriginalBotao; // Posição original do botão

    private void Start()
    {
        posicaoOriginal = objectToMove.transform.localPosition; // Armazena a posição original do objeto a ser movido
        posicaoOriginalBotao = button.transform.localPosition; // Armazena a posição original do botão

        button.onClick.AddListener(MoverObjeto); // Adiciona um ouvinte de clique ao botão para chamar o método MoverObjeto
    }

    private void MoverObjeto()
    {
        if (!isMoved) // Se o objeto não foi movido
        {
            Vector3 novaPosicao = objectToMove.transform.localPosition;
            Vector3 novaPosicaoBotao = button.transform.localPosition;

            novaPosicao.x -= 875f; // Move o objeto na direção do eixo x em -8 unidades
            novaPosicaoBotao.x -= 600f; // Move o botão na direção do eixo x em -6 unidades
            objectToMove.transform.localPosition = novaPosicao; // Define a nova posição do objeto
            button.transform.localPosition = novaPosicaoBotao; // Define a nova posição do botão

            isMoved = true; // Define a flag como verdadeira, indicando que o objeto foi movido
        }
        else // Se o objeto foi movido
        {
            objectToMove.transform.localPosition = posicaoOriginal; // Retorna o objeto para a posição original
            button.transform.localPosition = posicaoOriginalBotao; // Retorna o botão para a posição original

            isMoved = false; // Define a flag como falsa, indicando que o objeto não está mais movido
        }
    }

    // Método para exibir apenas os itens comprados no menu
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
                child.gameObject.SetActive(false); // Ocultar o item se não estiver comprado
            }
        }
    }
}
