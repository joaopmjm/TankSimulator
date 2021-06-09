using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fase1 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] ObligatoryTargets;
    public GameObject[] NonObligatoryTargets;
    void Start()
    {
        GameData.NextPhase = "Fase2";
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
        foreach(GameObject target in ObligatoryTargets)
        {
            GameData.ArmoredDestroyed++;
            GameData.Pontos += 3;
        }
        foreach(GameObject target in NonObligatoryTargets)
        {
            GameData.ArmoredDestroyed++;
            GameData.Pontos++;
        }
    }
    private bool Won()
    {
        if(AllObligatoryTargetsDestroyed())
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
            SceneManager.LoadScene("Fase2LS");
        }
    }
}
