using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExecutarPrograma : MonoBehaviour
{
    private BlockTypes[] todosOsBlocos; // Array para armazenar todos os blocos na cena
    public string resultadoEsperado; // O resultado esperado que voc� deseja comparar
    public string resultadoEncontrado;

    
    public void RodarCodigo(string nomeNivel)
    {
        VerificarPontua��o verificar = this.gameObject.GetComponent<VerificarPontua��o>();

        todosOsBlocos = FindObjectsOfType<BlockTypes>(); // Encontrar todos os blocos na cena
        int countBlocosSemPai = 0; // Contador para controlar o n�mero de blocos sem pai
        float tempoPassado = 0f; // Vari�vel para armazenar o tempo passado

        foreach (BlockTypes bloco in todosOsBlocos)
        {
            if (bloco.transform.parent == null) // Verificar se o bloco n�o possui pai
            {
                countBlocosSemPai++; // Incrementar o contador se um bloco sem pai for encontrado

                string resultadoCompleto = bloco.st_top + bloco.st_cond + bloco.st_mid + bloco.st_bot; // Juntar as strings
                Debug.Log("Resultado Completo do Bloco " + bloco.name + ": " + resultadoCompleto);
                resultadoEncontrado = resultadoCompleto; // Atribuir o resultado completo � vari�vel resultadoEncontrado

                // Comparar os resultados
                if(SceneManager.GetActiveScene().name == "Tutorial" && resultadoEncontrado == resultadoEsperado)
                {
                    // Fa�a algo aqui se o resultado for o esperado
                    Debug.Log("Resultado esperado encontrado. Voltando ao menu inicial.");
                    SceneManager.LoadScene(0);
                }
                else if (resultadoEncontrado == resultadoEsperado)
                {
                        // Fa�a algo aqui se o resultado for o esperado
                        Debug.Log("Resultado esperado encontrado. Voltando ao menu inicial.");


                        // Salvar o tempo que o jogador passou na fase nos PlayerPrefs
                        tempoPassado = verificar.ObterTempoPassado(); // Armazenar o tempo passado
                        Debug.Log("Tempo Passado: " + tempoPassado);

                        // Salvar o tempo passado nos PlayerPrefs
                        PlayerPrefs.SetFloat(nomeNivel, tempoPassado);
                        PlayerPrefs.Save();
                        SceneManager.LoadScene(0);
                    }
                else
                {
                    // Fa�a algo aqui se o resultado n�o for o esperado
                    Debug.Log("Resultado esperado n�o encontrado. Mostrando mensagem de erro.");
                    // Coloque aqui a l�gica para exibir uma mensagem de erro
                }
            }
        }

        if (countBlocosSemPai > 1) // Verificar se mais de um bloco sem pai foi encontrado
        {
            Debug.Log("Erro: Mais de um bloco sem pai encontrado. Deve haver apenas um bloco sem pai.");
            // Fa�a algo aqui para lidar com mais de um bloco sem pai
        }
    }
}

