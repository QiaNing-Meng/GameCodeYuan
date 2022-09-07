using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FightingText : MonoBehaviour
{
    public TextMeshPro damageTxt;
    bool isShow;
    Vector3 overPos;

    public void ShowTxt(string strDamage , Color color , Vector3 startPos , float offset=2) {
        this.gameObject.SetActive(true);
        this.gameObject.transform.position = startPos;
        isShow = true;
        damageTxt.text = strDamage;
        damageTxt.color = color;
        overPos = startPos + 
            new Vector3( 
                Random.Range(-offset, offset), 
                Random.Range(-offset, offset), 
                Random.Range(-offset, offset));
    }

    private void Update()
    {
        if (isShow == false) return;
        
        if ((this.transform.position - overPos).magnitude < 0.2f)
        {
            isShow = false;
            this.gameObject.SetActive(false);
        }
        this.transform.position = Vector3.Lerp(this.transform.position,overPos,Time.deltaTime);

        this.transform.rotation = GameStart.Instance.camera.transform.rotation;
        //this.transform.rotation = GameStart.Instance.playerQin.transform.rotation;
    }
}
