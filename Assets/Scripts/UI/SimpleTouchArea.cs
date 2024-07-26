using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SimpleTouchArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public UnityEvent OnTouchBegin;
    public UnityEvent OnTouchEnd;

    private bool buttonDownInCurrentFrame;
    private bool pointerInsideObject = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouchBegin?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerInsideObject = true;
        if (buttonDownInCurrentFrame)
        {
            OnTouchBegin?.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerInsideObject = false;
        if (buttonDownInCurrentFrame)
        {
            OnTouchEnd?.Invoke();
        }
    }

    public void Update()
    {
        buttonDownInCurrentFrame = Input.GetMouseButton(0);

        if(pointerInsideObject && Input.GetMouseButtonUp(0))
        { OnTouchEnd?.Invoke(); }
    }
}
