using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenStick : /*MonoBehaviour*/  OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private string m_ControlPath;
    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }

    private Vector2 m_PointerDownPos;
    public float m_MovementRange = 50;
    public float movementRange
    {
        get => m_MovementRange;
        set => m_MovementRange = value;
    }


    public Vector2 delta;
    public Vector2 newPos;

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
        delta = position - m_PointerDownPos;

        delta = Vector2.ClampMagnitude(delta, movementRange);

        newPos = new Vector2(delta.x / movementRange, delta.y / movementRange);

        GameStart.Instance.TouchMoveInput(newPos);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameStart.Instance.TouchMoveInput(Vector2.zero);
    }
}
