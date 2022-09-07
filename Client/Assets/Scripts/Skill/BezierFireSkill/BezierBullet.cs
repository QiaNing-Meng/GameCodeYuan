using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;


public class BezierBullet : MonoBehaviour
{
    public float speed = 10;
    public bool isOver;
    public float progress;
    public Vector3 _start;
    public Vector3 _midPoint;
    public Vector3 _target;

    private void Start()
    {
        StartCoroutine(RecycleButtle());
    }


    private void Update()
    {
        if (isOver == false) return;



        progress += speed * Time.deltaTime;
        if (progress >= 1) progress = 1;

        Vector3 p1 = Vector3.Lerp(_start, _midPoint, progress);
        Vector3 p2 = Vector3.Lerp(_midPoint, _target, progress);
        Vector3 p = Vector3.Lerp(p1, p2, progress);

        Vector3 dir = p - transform.position;
        transform.up = dir;
        //transform.position = p;
        transform.position = Vector3.MoveTowards(transform.position, p, speed * Time.deltaTime);
    }

    IEnumerator RecycleButtle()
    {
        yield return new WaitForSeconds(15f);
        StopAllCoroutines();
        GameStart.Instance.BulletEnqueue(this);
        isOver = false;
        progress = 0;
        Debug.LogError("未命中，回收");
    }



    internal void Fire(Vector3 start, Vector3 midPoint, Vector3 target)
    {
        this.transform.position = start;
        this._start = start;
        this._midPoint = midPoint;
        this._target = target;
        isOver = true;
        progress = 0;
        //StartCoroutine(Move(start, midPoint, target));
    }

    IEnumerator Move(Vector3 start, Vector3 midPoint, Vector3 target)
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            Vector3 p1 = Vector3.Lerp(start, midPoint, i);
            Vector3 p2 = Vector3.Lerp(midPoint, target, i);
            Vector3 p = Vector3.Lerp(p1, p2, i);
            yield return StartCoroutine(MoveToPoint(p));
        }
        yield return StartCoroutine(MoveToPoint(target));
    }

    IEnumerator MoveToPoint(Vector3 p)
    {
        yield return null;
        while ((this.transform.position - p).magnitude > 0.2f)
        {
            Vector3 dir = p - transform.position;
            transform.up = dir;
            //this.transform.rotation = Quaternion.identity;
            transform.position = Vector3.MoveTowards(transform.position, p, speed * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator MoveToPoint(Transform target)
    {
        yield return null;
        
        while ((this.transform.position - target.position).magnitude > 0.2f)
        {
            Vector3 dir = target.position - transform.position;
            transform.up = dir;
            //this.transform.rotation = Quaternion.identity;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            StopAllCoroutines();
            GameStart.Instance.BulletEnqueue(this);
            isOver = false;
            progress = 0;
            Debug.LogError("击中目标");
            collision.collider.GetComponent<Monster>().Damage(10);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            StopAllCoroutines();
            GameStart.Instance.BulletEnqueue(this);
            isOver = false;
            progress = 0;
            Debug.LogError("击中目标");
            other.GetComponent<Monster>().Damage(10);
        }
    }

}
