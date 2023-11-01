using UnityEngine;

public class PanAndZoom : MonoBehaviour
{
    public float minZoom = 1f;
    public float maxZoom = 10f;
    public float sensitivity = 2f;
    Vector3 posicaoCamera;
    Vector3 posicaoMouseNaTelaAnterior;
    Vector3 posicaoMouseNaTelaAtual;
    Vector3 posicaoCameraAnterior;
    Vector3 posicaoCameraProxima;
    Vector3 mouseNoMundo;
    Vector3 inicioArrastoCamera;
    Vector3 proximoArrastoCamera;

    // Start is called before the first frame update
    void Start()
    {
        posicaoCamera = Camera.main.transform.position;
        posicaoMouseNaTelaAnterior = new Vector3();
        posicaoMouseNaTelaAtual = new Vector3();
        posicaoCameraAnterior = new Vector3();
        mouseNoMundo = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            posicaoMouseNaTelaAnterior = posicaoMouseNaTelaAtual;
            posicaoMouseNaTelaAtual = Input.mousePosition;
            if (Vector3.Distance(posicaoMouseNaTelaAnterior, posicaoMouseNaTelaAtual) == 0)
            {
                float fov = Camera.main.orthographicSize;
                fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
                fov = Mathf.Clamp(fov, minZoom, maxZoom);
                Camera.main.orthographicSize = fov;
                Vector3 mouseNoMundoAtual = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 diferencaPosicao = mouseNoMundo - mouseNoMundoAtual;
                Vector3 posicaoCameraAtual = Camera.main.transform.position;
                Camera.main.transform.position = new Vector3(posicaoCameraAtual.x + diferencaPosicao.x, posicaoCameraAtual.y + diferencaPosicao.y, posicaoCameraAtual.z);
            }
            else
            {
                mouseNoMundo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        // Início do Pan ou movimentação de objetos na posição do mouse

        if (Input.GetMouseButtonDown(2))
        {
            inicioArrastoCamera = Input.mousePosition;
            posicaoCameraAnterior = Camera.main.transform.position;
        }

        // Pan ou movimentação de objetos na posição do mouse

        if (Input.GetMouseButton(2))
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                proximoArrastoCamera = Input.mousePosition;
                Vector3 deltaTela = inicioArrastoCamera - proximoArrastoCamera;
                Vector2 tamanhoTela = EscalaTamanhoTelaParaMundo(Camera.main.aspect, Camera.main.orthographicSize, Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight, deltaTela.x, deltaTela.y);

                Vector3 posicaoCameraMovimento = new Vector3(posicaoCameraAnterior.x + tamanhoTela.x, posicaoCameraAnterior.y + tamanhoTela.y, posicaoCameraAnterior.z);
                Camera.main.transform.position = posicaoCameraMovimento;
            }
        }
    }

    // Converte coordenada da tela para coordenada do mundo
    Vector2 EscalaTamanhoTelaParaMundo(float aspectoCamera, float tamanhoCamera, float larguraPixelTelaCamera, float alturaPixelTelaCamera, float larguraTela, float alturaTela)
    {
        float larguraCamera = aspectoCamera * tamanhoCamera * 2f;
        float alturaCamera = tamanhoCamera * 2f;
        float larguraMundoTela = larguraTela * (larguraCamera / larguraPixelTelaCamera);
        float alturaMundoTela = alturaTela * (alturaCamera / alturaPixelTelaCamera);

        return new Vector2(larguraMundoTela, alturaMundoTela);
    }
}
