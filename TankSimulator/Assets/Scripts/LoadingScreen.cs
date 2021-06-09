using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Text mensagem;
    public Slider loadingLevelBar;
    private int dot = 1;
    private float timelastUp, deltaTimeInLoading = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        timelastUp = Time.time;
        StartCoroutine(LoadLevelAsync());   
    }

    private IEnumerator LoadLevelAsync()
    {
        string level = GameData.NextPhase;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);
        asyncLoad.allowSceneActivation = false;
        while(!asyncLoad.isDone)
        {
            loadingLevelBar.value = asyncLoad.progress;
            if(Time.time - timelastUp > deltaTimeInLoading)
            {
                string l = "Loading";
                for(int i=0;i<dot;i++)
                {
                    l += ".";
                }
                dot = dot < 3 ? dot++ : 1;
            }
            if(asyncLoad.progress >= .9f)
            {
                loadingLevelBar.value = 1f;
                mensagem.text = "Press any key to continue";
                if (Input.anyKeyDown)
                {
                    mensagem.text = "Entering...";
                    asyncLoad.allowSceneActivation = true;
                }
            }
            yield return false;
        }

    }
}
