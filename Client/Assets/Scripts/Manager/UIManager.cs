using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class UIManager : TS_Singleton<UIManager>
{
    class UIELement
    {
        public string Resources;
        public bool Cache;
        public GameObject Instance;

    }
    private Dictionary<Type, UIELement> UIResources = new Dictionary<Type, UIELement>();

    public UIManager()
    {
        UIResources.Add(typeof(UIMessageBox), new UIELement() { Resources = "UI/UIMessageBox", Cache = true });
        UIResources.Add(typeof(UILogin), new UIELement() { Resources = "UI/UILogin", Cache = true });
        //UIResources.Add(typeof(UIInteraction), new UIELement() { Resources = "UI/UIInteraction", Cache = false });
        //UIResources.Add(typeof(UISelf), new UIELement() { Resources = "UI/UISelf", Cache = false });
        //UIResources.Add(typeof(UISelectedFrame), new UIELement() { Resources = "UI/UISelectedFrame", Cache = false });
    }

    public T Show<T>()
    {
        Type type = typeof(T);
        if (UIResources.ContainsKey(type))
        {
            UIELement info = UIResources[type];
            if (info.Instance != null)
            {
                info.Instance.SetActive(true);
            }
            else
            {
                UnityEngine.Object prefab = Resources.Load(info.Resources);
                if (prefab == null)
                {
                    return default(T);
                }
                info.Instance = (GameObject)GameObject.Instantiate(prefab, GameStart.Instance.MainUITransf.transform);
            }
            return info.Instance.GetComponent<T>();
        }
        return default(T);
    }

    public void Close(Type type)
    {
        if (UIResources.ContainsKey(type))
        {
            UIELement info = UIResources[type];
            if (info.Cache)
            {
                info.Instance.SetActive(false);
            }
            else
            {
                GameObject.Destroy(info.Instance);
                info.Instance = null;
            }
        }
    }
    public void Close<T>()
    {
        Close(typeof(T));
    }
}
