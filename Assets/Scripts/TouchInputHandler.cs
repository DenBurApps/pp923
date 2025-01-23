using System;
using System.Collections;
using System.Collections.Generic;
using BalloonTap;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInputHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    public event Action<Cloud> CloudClicked;
    public event Action<Balloon> BalloonClicked;

    private Coroutine _touchDetectingCoroutine;

    public void StartDetectingTouch()
    {
        if (_touchDetectingCoroutine == null)
        {
            _touchDetectingCoroutine = StartCoroutine(DetectTouch());
        }
    }

    public void StopDetectingTouch()
    {
        if (_touchDetectingCoroutine != null)
        {
            StopCoroutine(_touchDetectingCoroutine);
            _touchDetectingCoroutine = null;
        }
    }

    private IEnumerator DetectTouch()
    {
        while (true)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        yield return null;
                        continue;
                    }
                    
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    
                    RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity);

                    if (hit.collider != null)
                    {
                        if (hit.collider.TryGetComponent(out Balloon balloon))
                        {
                            BalloonClicked?.Invoke(balloon);
                        }
                        else if(hit.collider.TryGetComponent(out Cloud cloud))
                        {
                            CloudClicked?.Invoke(cloud);
                        }
                    }
                    else
                    {
                        /*Missed?.Invoke();
                        var gameObject = Instantiate(_incorrectTouchObject);
                        gameObject.transform.position = touchPosition;
                        Destroy(gameObject, 2);*/
                    }
                }
            }

            yield return null;
        }
    }
}