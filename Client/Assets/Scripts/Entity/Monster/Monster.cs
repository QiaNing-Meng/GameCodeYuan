using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Monster : MonoBehaviour
{
    public Animator animator;
    public CharacterHpInfo hpInfo;
    public float hp;
    public float maxHp;

    public void Damage(int harm,int harmState = 0) {
        hp -= harm;
        hpInfo.infoHpImg.fillAmount = Mathf.Max(hp / maxHp, 0);
        hpInfo.infoHp.text = string.Format("{0}/{1}", hp, maxHp);
        GameStart.Instance.GetFightingText().ShowTxt(
            $" ‹µΩ…À∫¶{harm}", 
            Color.red, 
            this.transform.position + new Vector3(0, 0.8f, 0));
         
        Hit(harmState);
    }
    public void Hit(int harmState) {
        animator.Play("hit");

        //animator.SetTrigger("Hit");
    }
    void Start()
    {
        maxHp = 1000;
        hp = maxHp;

        hpInfo.infoName.text = "««»À";
        hpInfo.infoHp.text = maxHp.ToString();
        hpInfo.infoHpImg.fillAmount = 1f;
    }

    void Update()
    {
        hpInfo.transform.rotation = GameStart.Instance.camera.transform.rotation;
        //hpInfo.transform.rotation = GameStart.Instance.playerQin.transform.rotation;
    }
}
