using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocos : MonoBehaviour
{
    // Delegado e evento para notificar quando o arraste é concluído
    public delegate void DelegadoArrastarTerminado(Blocos objetoArrastavel);
    public event DelegadoArrastarTerminado eventoArrastarTerminado;


    public delegate void BlocosConectados();
    public event BlocosConectados BlocoConectado;

    private const int MaxRecursionCount = 100;

    private bool estaArrastando = false;
    private bool ehMenu;

    private Vector3 mousePosicaoInicialArraste;
    private Vector3 spritePosicaoInicialArraste;
    private float tempoInicio;

    public bool estaEncaixado;
    private double tempoPressionado = 0.3;

    public List<Transform> pontosEncaixe;
    public List<GameObject> todosOsBlocos;

    public float alcanceEncaixe = 0.5f;

    private Transform pontoEncaixeMaisProximo = null;

    public List<GameObject> filhosBloco = new List<GameObject>();
    public List<Transform> snapFilhos;
    public List<float> alturaFilhos;

    private Transform containerSprites;
    private Transform objetoMeio;
    private Transform objetoInferior;
    private Transform objetoSuperior;

    private Vector3 escalaOriginal;
    private Vector3 posicaoMeioOriginal;
    private Vector3 posicaoInferiorOriginal;


    private void Start()
    {
        // Encontra e armazena as referências dos objetos "Sprites"
        containerSprites = gameObject.transform.Find("Sprites");

        objetoSuperior = containerSprites.Find("Top");
        objetoMeio = containerSprites.Find("Mid");
        objetoInferior = containerSprites.Find("Bot");

        // Armazena a escala original do objeto "Meio"
        escalaOriginal = objetoMeio.transform.localScale;
        // Armazena a posição original do objeto "Meio"
        posicaoMeioOriginal = objetoMeio.transform.localPosition;
        // Armazena a posição original do objeto "Inferior"
        posicaoInferiorOriginal = objetoInferior.transform.localPosition;

        // Atualiza os pontos de encaixe disponíveis
        AtualizarPontosEncaixe();
        // Adiciona os filhos do bloco atual
        AdicionarFilhosBloco();
    }

    private void Update()
    {
        // Atualiza os filhos do bloco atual
        AdicionarFilhosBloco();
    }

    private void OnMouseDown()
    {
        // Verifica se o objeto é um menu
        ehMenu = gameObject.CompareTag("Menu");

        // Inicia o arraste
        estaArrastando = true;
        // Salva a posição inicial do arraste do mouse
        mousePosicaoInicialArraste = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Salva a posição inicial do arraste do sprite
        spritePosicaoInicialArraste = transform.localPosition;
        // Salva o tempo de início do arraste
        tempoInicio = Time.time;

        // Atualiza os pontos de encaixe disponíveis
        AtualizarPontosEncaixe();
        Reposicionar(0);
    }

    private void OnMouseDrag()
    {
        // Verifica se o objeto está sendo arrastado e não é um menu
        if (estaArrastando && !ehMenu)
        {
            // Atualiza a posição do objeto durante o arraste
            transform.localPosition = spritePosicaoInicialArraste + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mousePosicaoInicialArraste);
        }

    }

    private void OnMouseUp()
    {
        

        // O arraste foi concluído
        estaArrastando = false;

        // Calcula o tempo decorrido desde o início do arraste
        float tempoDecorrido = Time.time - tempoInicio;

        // Verifica se o objeto foi pressionado por tempo suficiente e é um menu
        if (tempoDecorrido >= tempoPressionado && ehMenu)
        {
            // Cria uma cópia do objeto
            CriarCopia();
        }

        // Invoca o evento de arraste concluído
        eventoArrastarTerminado?.Invoke(this);

        // Realiza as ações após o término do arraste
        OnArrasteTerminado();
        // Atualiza os pontos de encaixe disponíveis
        AtualizarPontosEncaixe();

    }

    private void OnArrasteTerminado()
    {
        

        float distanciaMaisProxima = float.MaxValue;
        Transform novoPai = null;

        // Verifica qual é o ponto de encaixe mais próximo
        foreach (Transform pontoEncaixe in pontosEncaixe)
        {
            float distancia = Vector3.Distance(transform.position, pontoEncaixe.position);

            if (distancia < distanciaMaisProxima && distancia <= alcanceEncaixe)
            {
                pontoEncaixeMaisProximo = pontoEncaixe;
                distanciaMaisProxima = distancia;
                novoPai = pontoEncaixe;
            }
        }

        // Verifica se há um ponto de encaixe próximo e um novo pai válido
        if (pontoEncaixeMaisProximo != null && novoPai != null)
        {
            if (pontoEncaixeMaisProximo.CompareTag("delete"))
            {
                Destroy(this.gameObject);
            }
            // Agora, vamos buscar a referência ao componente SnapPoint
            SnapPoint snapPointComponent = pontoEncaixeMaisProximo.GetComponent<SnapPoint>();


            // Verifica se o Snap Point não está sendo usado
            if (snapPointComponent != null && !snapPointComponent.usado)
            {

                // Define o novo pai do objeto
                transform.SetParent(novoPai);

                // Salve a coordenada Z atual do objeto
                float currentZ = transform.position.z;

                // Defina a posição do objeto como a posição do ponto de encaixe mais próximo, mantendo a coordenada Z
                Vector3 newPosition = new Vector3(pontoEncaixeMaisProximo.position.x, pontoEncaixeMaisProximo.position.y, currentZ);
                transform.position = newPosition;
                // O objeto está agora encaixado
                estaEncaixado = true;

                // Ajusta a altura do bloco
                Blocos pai = this.gameObject.GetComponent<Blocos>();
                pai.AjustarAltura();

                snapPointComponent.DefinirUsado(true);
                


                /*---------------------
                   Cria novo SnapPoint 
                 ----------------------*/
                Transform newSnapPoint = Instantiate(pontoEncaixeMaisProximo, pontoEncaixeMaisProximo.position, pontoEncaixeMaisProximo.rotation, this.transform.parent.parent);

                foreach (Transform filho in newSnapPoint.GetChild(0))
                {
                    Destroy(newSnapPoint.GetChild(0).gameObject);
                }

                float altura = ObterAlturaTotalIrmao();
                newSnapPoint.transform.localPosition = pontoEncaixeMaisProximo.localPosition + new Vector3(0f, -altura, 0f);

                SnapPoint newSnapPointComponent = newSnapPoint.GetComponent<SnapPoint>();
                newSnapPointComponent.DefinirUsado(false);

                if (BlocoConectado != null)
                {
                    BlocoConectado();
                }
            }
        }

        else if (!ehMenu)
        {
            // Verifica se o pontoEncaixeMaisProximo não é nulo antes de buscar o componente SnapPoint
            if (pontoEncaixeMaisProximo != null)
            {
                // Agora, vamos buscar a referência ao componente SnapPoint
                SnapPoint snapPointComponent = pontoEncaixeMaisProximo.GetComponent<SnapPoint>();

                // Verifica se o Snap Point está sendo usado
                if (snapPointComponent != null && snapPointComponent.usado)
                {
                    // Define o Snap Point como não usado
                    snapPointComponent.DefinirUsado(false);
                }
            }

            estaEncaixado = false;
            transform.SetParent(null);
        }

        BlockTypes componente = gameObject.GetComponent<BlockTypes>();
        componente.OnValidate();

        CallReposicionarOnParents(transform.parent);

    }

    private void CallReposicionarOnParents(Transform currentTransform)
    {
        if (currentTransform == null) return;

        Blocos blocosComponent = currentTransform.GetComponent<Blocos>();
        if (blocosComponent != null)
        {
            blocosComponent.Reposicionar(0);
        }

        // Chama recursivamente a função nos pais
        CallReposicionarOnParents(currentTransform.parent);
    }



    private void CriarCopia()
    {
        GameObject original = gameObject;

        // Define a nova posição da cópia
        float randomZ = UnityEngine.Random.Range(-0.2f, 0f); // Gera um valor aleatório de Z entre 0 e -0.2
        Vector3 novaPosicao = original.transform.position - new Vector3(6.3f, 0f, randomZ);

        // Cria a cópia do objeto
        GameObject novaCopia = Instantiate(original, novaPosicao, original.transform.rotation);

        // Define a tag da cópia como "Blocos"
        novaCopia.tag = "Blocks";
        // Redefine a escala da cópia para o valor padrão
        novaCopia.transform.localScale = Vector3.one;
    }


    public void AtualizarPontosEncaixe()
    {
        // Limpa a lista de pontos de encaixe
        pontosEncaixe.Clear();

        // Obtém todos os objetos do jogo
        GameObject[] todosObjetos = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in todosObjetos)
        {
            if (obj.CompareTag("SnapPoint") || obj.CompareTag("delete"))
            {
                // Adiciona o ponto de encaixe à lista
                Transform pontoEncaixe = obj.transform;
                pontosEncaixe.Add(pontoEncaixe);
            }
        }


    }

    public void AdicionarFilhosBloco()
    {
        // Limpa a lista de filhos do bloco
        filhosBloco.Clear();
        snapFilhos.Clear();
        Transform pai = this.gameObject.transform;

        // Se o objeto for encontrado, busca seu filho com a tag "Blocks"
        foreach (Transform snapPoint in pai)
        {
            foreach (Transform filho in snapPoint)
            {
                if (filho.CompareTag("Blocks"))
                {
                    filhosBloco.Add(filho.gameObject);
                }

            }

            if (snapPoint.CompareTag("SnapPoint"))
            {
                snapFilhos.Add(snapPoint);
            }
        }

        // Chama o método de ajustar a altura se a lista de filhos do bloco for atualizada
        AjustarAltura();
    }

    private float ObterAlturaAtual(GameObject objeto)
    {
        float alturaMaxima = float.MinValue;
        float alturaMinima = float.MaxValue;

        Transform containerSprites = objeto.transform.Find("Sprites");

        if (containerSprites != null)
        {
            foreach (Transform filho in containerSprites)
            {
                Renderer renderer = filho.GetComponent<Renderer>();

                if (renderer != null)
                {
                    float posicaoSuperiorY = filho.position.y + renderer.bounds.extents.y;
                    float posicaoInferiorY = filho.position.y - renderer.bounds.extents.y;

                    alturaMaxima = Mathf.Max(alturaMaxima, posicaoSuperiorY);
                    alturaMinima = Mathf.Min(alturaMinima, posicaoInferiorY);
                }
            }
        }

        return alturaMaxima - alturaMinima;
    }

    private float ObterAlturaTotalFilho()
    {
        float alturaTotal = 0f;
        Transform snapPoint = transform.Find("SnapPoint");

        if (snapPoint != null)
        {
            foreach (GameObject obj in filhosBloco)
            {
                float altura = ObterAlturaAtual(obj);
                alturaTotal += altura;
            }
        }

        return alturaTotal;
    }

    private float ObterAlturaTotalIrmao()
    {
        float alturaTotal = 0f;

        foreach (Transform irmao in transform.parent)
        {
            if (irmao.CompareTag("Blocks"))
            {
                float altura = ObterAlturaAtual(irmao.gameObject);
                alturaTotal += altura;
            }
        }

        return alturaTotal;
    }

    public void AjustarAltura()
    {
        // Obtém o objeto "Sprites" atual do jogo
        Transform containerSprites = gameObject.transform.Find("Sprites");



        if (containerSprites != null)
        {
            // Calcula a altura total
            float altura = ObterAlturaTotalFilho();

            if (objetoMeio != null && altura != 0)
            {
                // Ajusta a escala do objeto "Meio"
                Vector3 novaEscala = objetoMeio.transform.localScale;
                novaEscala.y = altura;
                objetoMeio.transform.localScale = novaEscala;

                // Move o objeto "Inferior"
                if (objetoInferior != null)
                {
                    Vector3 novaPosicaoInferior = objetoInferior.transform.localPosition;
                    novaPosicaoInferior.y = -altura - 1;
                    objetoInferior.transform.localPosition = novaPosicaoInferior;
                }

                // Posiciona o objeto "Meio" no meio entre "Superior" e "Inferior"
                if (objetoSuperior != null && objetoInferior != null)
                {
                    Vector3 novaPosicaoMeio = objetoMeio.transform.localPosition;
                    novaPosicaoMeio.y = (objetoSuperior.transform.localPosition.y + objetoInferior.transform.localPosition.y) / 2f;
                    objetoMeio.transform.localPosition = novaPosicaoMeio;
                }
            }

            if (altura == 0)
            {
                // Ajusta a escala do objeto "Meio" para a escala original
                objetoMeio.transform.localScale = escalaOriginal;
                // Ajusta a posição do objeto "Meio" para a posição original
                objetoMeio.transform.localPosition = posicaoMeioOriginal;
                // Ajusta a posição do objeto "Inferior" para a posição original
                objetoInferior.transform.localPosition = posicaoInferiorOriginal;
            }
        }


    }

    public void Reposicionar(int recursionCount)
    {
        if (recursionCount < MaxRecursionCount)
        {
            alturaFilhos.Clear();

            float alturaTotal = 0f;  // Variable to track accumulated height

            foreach (GameObject bloco in filhosBloco)
            {
                Blocos filho = bloco.GetComponent<Blocos>();

                float altura = ObterAlturaAtual(bloco);
                alturaTotal += altura;  // Add current height to accumulated total height

                filho.Reposicionar(recursionCount + 1); // Pass incremented recursion count

                Debug.Log("Chamou Reposicionar() do objeto: " + bloco);
                Debug.Log("altura: " + altura + " " + bloco);
                alturaFilhos.Add(altura);

                // Check if it's not the first object in the list
                if (filhosBloco.IndexOf(bloco) > 0)
                {
                    // Get accumulated total height of previous objects
                    float alturaMovida = alturaTotal - altura;

                    // Move the object down based on accumulated height
                    bloco.transform.parent.localPosition = new Vector3(1f, -alturaMovida - 1, 0f);
                }
            }

            // After repositioning the blocks, update the position of empty SnapPoints
            AtualizarPosicaoSnapPointsVazios();
        }
        else
        {
            Debug.Log("Max recursion count reached. Terminating recursion.");
        }
    }


    private void AtualizarPosicaoSnapPointsVazios()
    {
        // Verifica se o transform.parent não é nulo
        if (transform.parent != null && transform.parent.parent != null)
        {
            // Percorre todos os SnapPoints vazios no nível acima
            foreach (SnapPoint snapPointVazio in transform.parent.parent.GetComponentsInChildren<SnapPoint>())
            {
                // Verifica se o SnapPoint não está sendo usado e se a posição dele não é nula
                if (snapPointVazio != null && !snapPointVazio.usado && snapPointVazio.transform.position != null)
                {
                    // Cria um novo GameObject vazio
                    GameObject novoSnapPoint = new GameObject("SnapPoint");

                    // Atribui a posição do SnapPoint vazio ao novo GameObject
                    novoSnapPoint.transform.position = snapPointVazio.transform.position;

                    // Atribui a rotação do SnapPoint vazio ao novo GameObject (se necessário)
                    novoSnapPoint.transform.rotation = snapPointVazio.transform.rotation;

                    // Define o pai do novo GameObject como o mesmo pai do SnapPoint vazio
                    novoSnapPoint.transform.SetParent(snapPointVazio.transform.parent);
                }
            }
        }
    }
}
