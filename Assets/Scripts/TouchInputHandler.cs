using System;
using System.Collections;
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
                
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, _layerMask);

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent(out Balloon balloon))
                    {
                        BalloonClicked?.Invoke(balloon);
                    }
                    else if (hit.collider.TryGetComponent(out Cloud cloud))
                    {
                        CloudClicked?.Invoke(cloud);
                    }
                }
                else
                {
                    Debug.Log("null");
                }
            }
            else
            {
                Debug.Log("no imput");
            }

            yield return null;
        }
    }
}