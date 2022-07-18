using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera game_camera;

    private void Awake()
    {
        game_camera = GetComponent<Camera>();
        SetUpResolution();
    }

    private void SetUpResolution()
    {
        float widthAspect = 16f;
        float heightAspect = 9f;

        game_camera.aspect = widthAspect / heightAspect;

        float widthRatio = (float)Screen.width / widthAspect;
        float heightRatio = (float)Screen.height / heightAspect;

        float widthOffset = ((widthRatio / (heightRatio / 100f)) - 100f) / 200f;
        float heightOffset = ((heightRatio / (widthRatio / 100f)) - 100f) / 200f;

        if (heightRatio > widthRatio)
        {
            widthOffset = 0f;
        }
        else
            heightOffset = 0f;

        game_camera.rect = new Rect(0f, 0f
            , game_camera.rect.width + (widthOffset * 2), game_camera.rect.height + (heightOffset * 2));
    }
}
