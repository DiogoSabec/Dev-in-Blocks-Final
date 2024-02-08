using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class ExecutarPrograma : MonoBehaviour
{
    private BlockTypes[] todosOsBlocos; // Array para armazenar todos os blocos na cena

    [TextArea]
    public string resultadoEsperado; // O resultado esperado que você deseja comparar
    [TextArea]
    public string resultadoEncontrado;

    public RectTransform respostaCerta;
    public RectTransform respostaErrada;

    public List <TMP_InputField> ip_esperado;
    public List <TMP_InputField> ip_encontrado;


    public void RodarCodigo(string nomeNivel)
    {
        VerificarPontuação verificar = this.gameObject.GetComponent<VerificarPontuação>();

        todosOsBlocos = FindObjectsOfType<BlockTypes>(); // Encontrar todos os blocos na cena
        int countBlocosSemPai = 0; 
        float tempoPassado = 0f;

        foreach (BlockTypes bloco in todosOsBlocos)
        {
            bloco.OnValidate();
            if (bloco.transform.parent == null) // Verificar se o bloco não possui pai
            {
                countBlocosSemPai++; // Incrementar o contador se um bloco sem pai for encontrado

                string resultadoCompleto = bloco.st_top + bloco.st_cond + bloco.st_mid + bloco.st_bot; // Juntar as strings
                Debug.Log("Resultado Completo do Bloco " + bloco.name + ": " + resultadoCompleto);
                resultadoEncontrado = resultadoCompleto; // Atribuir o resultado completo à variável resultadoEncontrado

                // Comparar os resultados
                if(SceneManager.GetActiveScene().name == "Tutorial" && resultadoEncontrado == resultadoEsperado)
                {
                    //Tutorial
                    Debug.Log("Resultado esperado encontrado. Voltando ao menu inicial.");

                    ip_encontrado[0].text = resultadoEncontrado;
                    ip_esperado[0].text = resultadoEsperado;

                    respostaCerta.gameObject.SetActive(true);
                }
                else if (resultadoEncontrado == resultadoEsperado)
                {
                        
                        Debug.Log("Resultado esperado encontrado.");


                        // Salvar o tempo que o jogador passou na fase nos PlayerPrefs
                        tempoPassado = verificar.ObterTempoPassado(); 
                        Debug.Log("Tempo Passado: " + tempoPassado);

                        // Salvar o tempo passado nos PlayerPrefs
                        PlayerPrefs.SetFloat(nomeNivel, tempoPassado);
                        PlayerPrefs.Save();

                        ip_encontrado[0].text = resultadoEncontrado;
                        ip_esperado[0].text = resultadoEsperado;

                        respostaCerta.gameObject.SetActive(true);



                    }
                else
                {
                    
                    Debug.Log("Resultado esperado não encontrado. Mostrando mensagem de erro.");
                    ip_encontrado[1].text = resultadoEncontrado;
                    ip_esperado[1].text = resultadoEsperado;

                    respostaErrada.gameObject.SetActive(true);

                }
            }
        }

        if (countBlocosSemPai > 1) 
        {
            Debug.Log("Erro: Mais de um bloco sem pai encontrado. Deve haver apenas um bloco sem pai.");
            
        }
    }
}

