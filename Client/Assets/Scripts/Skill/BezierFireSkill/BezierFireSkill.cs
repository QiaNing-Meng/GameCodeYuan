using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class BezierFireSkill:SkillBase
{
    public BezierBullet bezierBullet;
    public Transform BezierBulletGo;

    public BezierFireSkill() {
        bezierBullet = GameStart.Instance._ResourcesManager.ResourcesLoad<BezierBullet>("Entity/Bullet/BezierBullet");
        BezierBulletGo = GameStart.Instance.BezierBulletEnqueueGo;

        for (int i = 0; i < 20; i++)
        {

            BezierBullet bullet = GameStart.Instance._ResourcesManager.ResourcesLoadInstantiate<BezierBullet>(
                    bezierBullet, BezierBulletGo.transform);
            bullet.gameObject.SetActive(false);
            GameStart.Instance.bullets.Enqueue(bullet);
        }

        bezierBullet = null;
        BezierBulletGo = null;
    }


    public override void startSkill()
    {
        base.startSkill();

        Debug.LogError($"Ðé¿ÕË÷µÐ!!");

        GameStart.Instance.GS_StartCoroutine(fire());

    }

    public Vector3 GetRandomPoint(float r)
    {
        return GameStart.Instance.BezierBulletFireGo.position + 
            new Vector3(Random.Range(-r, r), Random.Range(-r, r), Random.Range(-r, r));
    }

    IEnumerator fire()
    {
        for (int i = 0; i < 10; i++)
        {
            BezierBullet bullet =GameStart.Instance.GetBezierBullet();
            bullet.Fire(GameStart.Instance.BezierBulletFireGo.position, 
                GetRandomPoint(40), 
                GameStart.Instance.enemy.position + new Vector3(0,0.4f,0));
            yield return new WaitForSeconds(0.1f);
        }

            
    }

}
