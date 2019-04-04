using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class DeckTextMousePlay : MonoBehaviour
{
    public RectTransform MouseBox;
    public float MaxRotation, MaxDegreeRotationPerSecond, LagWeight, CenterZoom, ClickMovement;
    
    Vector2 RotationAtTopRight => new Vector2(-MaxRotation, MaxRotation);

    void Update ()
    {
        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(MouseBox, Input.mousePosition, CameraCache.Main, out point);

        point = Rect.PointToNormalized(MouseBox.rect, point);
        point -= new Vector2(.5f, .5f);
        point *= 2;

        // point is now (-1, -1) if mouse is at bottom left corner and (1, 1) at top right

        // cross multiply because we want the point's y axis to change the eventual rotation's x axis
        var targetRotation = Quaternion.Euler(new Vector2
        (
            point.y * RotationAtTopRight.x,
            point.x * RotationAtTopRight.y
        ));

        var step = MaxDegreeRotationPerSecond * Time.deltaTime + Quaternion.Angle(transform.rotation, targetRotation) * LagWeight;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

        float angleFromCenter = Quaternion.Angle(Quaternion.identity, transform.rotation) / MaxRotation;

        transform.localPosition = new Vector3
        (
            transform.localPosition.x,
            transform.localPosition.y,
            Mathf.Lerp(-CenterZoom, 0, angleFromCenter) + (Input.GetMouseButton(0) ? ClickMovement : 0)
        );
    }
}
