using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public virtual void Close() {
        GameStart.Instance._UIManager.Close(GetType());
    }

    public virtual void Init(params object[] obj) { 
    
    
    }
}
