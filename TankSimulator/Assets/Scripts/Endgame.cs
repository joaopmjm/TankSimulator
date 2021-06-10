using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Endgame : MonoBehaviour
{
    public TMP_Text pontos, veiculosDestruidos, textoPrincipal;

    void Start()
    {
        pontos.text = $"{GameData.Pontos}";
        veiculosDestruidos.text = $"{GameData.ArmoredDestroyed}";
        if(GameData.Won)
        {
            textoPrincipal.text = "You've made Stalin Proud Comrade";
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
