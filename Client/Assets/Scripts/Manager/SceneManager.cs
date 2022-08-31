using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Protocol;

public class SceneManager : TS_Singleton<SceneManager>
{
    public UnityAction<float> onProgress = null;
    [HideInInspector]
    public bool isActiva;

    public void LoadScene(string name)
    {
        //GameStart.Instance.UILoad.gameObject.SetActive(true);
        //if (GameStart.Instance.selfPlayer != null) {
        //    GameStart.Instance.selfPlayer.cc.enabled = false;
        //    GameStart.Instance.selfPlayer.gameObject.SetActive(false);
        //}

        isActiva = false;
        GameStart.Instance.GS_StartCoroutine(LoadLevel(name));
    }

    IEnumerator LoadLevel(string name)
    {
        Debug.LogFormat("LoadLevel: {0}", name);
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = false;
        async.completed += LevelLoadCompleted;
        while (!async.isDone)
        {
            if (async.progress>=0.9f)
            {
                //GameStart.Instance.LoadOver.gameObject.SetActive(true);
            }
            if (isActiva)
            {
                //async.allowSceneActivation = isActiva;
                //GameStart.Instance.LoadOver.gameObject.SetActive(false);
                //if (GameStart.Instance.selfPlayer != null) {
                //    GameStart.Instance.selfPlayer.cc.enabled = true;
                //    GameStart.Instance.selfPlayer.gameObject.SetActive(true);
                //    GameStart.Instance.selfmapID = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
                //}
            }

            if (onProgress != null)
                onProgress(async.progress);
            yield return null;
        }
    }

    private void LevelLoadCompleted(AsyncOperation obj)
    {
        if (onProgress != null)
            onProgress(1f);

        Debug.Log("LevelLoadCompleted:" + obj.progress);
    }
}