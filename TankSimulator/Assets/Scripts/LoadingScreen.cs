using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Text mensagem;
    public Slider loadingLevelBar;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadLevelAsync());   
    }

    private IEnumerator LoadLevelAsync()
    {
        string level = GameData.NextPhase;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);
        asyncLoad.allowSceneActivation = false;
        while(!asyncLoad.isDone)
        {
            loadingLevelBar.value = asyncLoad.progress + .1f;
            if(asyncLoad.progress >= .9f && !asyncLoad.allowSceneActivation)
            {
                mensagem.text = "Press any key to continue";
                if (Input.anyKeyDown)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }
            yield return null;
        }

    }
}
