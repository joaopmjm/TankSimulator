using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fase2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] ObligatoryPlaces;
    public GameObject[] NonObligatoryPlaces;
    public Text textoDoComandante;
    int targetsReached = 0, numOfTargets;
    void Start()
    {
        numOfTargets = ObligatoryPlaces.Length;
        textoDoComandante.text = $"{targetsReached}/{numOfTargets}";
        GameData.NextPhase = "Fase3";
    }

    private bool AllObligatoryPlacesReached()
    {
        foreach(GameObject target in ObligatoryPlaces)
        {
            if(!target.GetComponent<Waypoint>().arrived) return false;
        }
        return true;
    }

    private void UpdateScore()
    {
        foreach(GameObject target in ObligatoryPlaces)
        {
            GameData.Pontos += 3;
        }
        foreach(GameObject target in NonObligatoryPlaces)
        {
            GameData.Pontos++;
        }
    }

    private void UpdateText()
    {
        int r = 0;
        foreach(GameObject target in ObligatoryPlaces)
        {
            if(target.GetComponent<Waypoint>().arrived) 
            {
                r++;
            }
        }
        if(r == targetsReached) return;
        targetsReached = r;
        Debug.Log($"{targetsReached}/{numOfTargets}");
        textoDoComandante.text = $"{targetsReached}/{numOfTargets}";
    }
    private bool Won()
    {
        if(AllObligatoryPlacesReached())
        {
            UpdateScore();
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
        if(Won())
        {
            SceneManager.LoadScene("Fase3LS");
        }
    }
}
