using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fase3 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] ObligatoryPlaces;
    public GameObject[] NonObligatoryPlaces;
    public GameObject[] ObligatoryTargets;
    public Text textoDoComandante;
    int targetsReached = 0, numOfTargets;
    void Start()
    {
        GameData.NextPhase = "Endgame";
        textoDoComandante.text = "";
    }

    private bool AllObligatoryPlacesReached()
    {
        foreach(GameObject target in ObligatoryPlaces)
        {
            if(!target.GetComponent<Waypoint>().arrived) return false;
        }
        return true;
    }
        private bool AllObligatoryTargetsDestroyed()
    {
        foreach(GameObject target in ObligatoryTargets)
        {
            if(!target.GetComponent<EnemyBahaviour>().destroyed) return false;
        }
        return true;
    }

    private void UpdateScore()
    {
        foreach(GameObject target in ObligatoryPlaces)
        {
            GameData.Pontos += 3;
        }
        foreach(GameObject target in ObligatoryTargets)
        {
            GameData.ArmoredDestroyed++;
            GameData.Pontos += 3;
        }
        foreach(GameObject target in NonObligatoryPlaces)
        {
            GameData.ArmoredDestroyed++;
            GameData.Pontos++;
        }
    }
    private bool Won()
    {
        if(AllObligatoryPlacesReached() && AllObligatoryTargetsDestroyed())
        {
            UpdateScore();
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Won())
        {
            GameData.Won = true;
            SceneManager.LoadScene("Endgame");
        }
    }
}
