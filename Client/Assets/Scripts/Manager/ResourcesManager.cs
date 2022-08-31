using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ResourcesManager : TS_Singleton<ResourcesManager>
{
    public T ResourcesLoadGameObject<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }

    public IEnumerator UnityWebRequestGetData(Image _imageComp, string _url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isHttpError || uwr.isNetworkError) Debug.Log(uwr.error);
            else
            {
                if (uwr.isDone)
                {
                    Texture2D texture2d = DownloadHandlerTexture.GetContent(uwr);
                    Sprite tempSprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
                    _imageComp.sprite = tempSprite;
                    Resources.UnloadUnusedAssets();
                }
            }
        }
    }


}
