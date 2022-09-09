using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTrigger : MonoBehaviour
{
    public void Darg(BaseEventData data)
    {
        if (data == null) return;

        var data1 = (PointerEventData)data;


        //Debug.Log("Darg" + data1.delta);

        var cim = GameStart.Instance._freeLook;

        if (cim != null)
        {
            cim.m_YAxis.Value += -(data1.delta.y) * Time.deltaTime * 0.05f;
            cim.m_XAxis.Value += data1.delta.x * Time.deltaTime * 3f;
        }
    }



    public void OnClickPoint(BaseEventData data)
    {
        if (data == null) return;

        var data1 = (PointerEventData)data;
        Vector2 pointPos = data1.position;
        //Debug.Log(pointPos);

        //if (frameItem == null)
        //{
        //    ray = Camera.main.ScreenPointToRay(pointPos);
        //    //if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        //    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        //    {
        //        var temp = hit.collider.GetComponent<FrameItem>();

        //        if (Vector3.Distance(temp.transform.position, GameStart.Instance.selfPlayer.transform.position) <= raySizeMax)
        //        {
        //            Debug.DrawLine(ray.origin, hit.point, Color.red);
        //            frameItem = temp;
        //            //frameItem.collider.isTrigger = true;
        //            var _UISelectedFrame = GameStart.Instance._UIManager.Show<UISelectedFrame>();
        //            _UISelectedFrame.Init(GameStart.Instance.context, 3);
        //            _UISelectedFrame.Set3DFrameItem(frameItem);
        //        }

        //    }
        //    else
        //    {
        //        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green);
        //    }
        //}

    }
}
