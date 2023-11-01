using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerificarPontuação : MonoBehaviour
{
    private float tempoInicial;
    private float tempoAtual;
    private float tempoPassado;

    private bool cronometroAtivo = false;

    void Start()
    {
        tempoInicial = Time.time;
        cronometroAtivo = true;
    }

    void PararCronometro()
    {
        cronometroAtivo = false;
    }

    void ReiniciarCronometro()
    {
        tempoInicial = Time.time;
        tempoPassado = 0f;
    }

    void Update()
    {
        if (cronometroAtivo)
        {
            tempoAtual = Time.time;
            tempoPassado = tempoAtual - tempoInicial;
        }
    }

    // Obter o tempo passado em segundos
    public float ObterTempoPassado()
    {
        return tempoPassado;
    }
}
