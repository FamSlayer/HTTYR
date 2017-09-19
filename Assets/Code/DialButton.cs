using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public DialTurner dt;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        dt.Activate();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dt.DeActivate();
    }
}
