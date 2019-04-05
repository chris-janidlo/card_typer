using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class MouseParticles : MonoBehaviour
{
    public RectTransform Canvas;

    void Update ()
    {
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(Canvas, Input.mousePosition, CameraCache.Main, out pos);
        transform.position = pos;   
    }
}
