using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;
    private Camera mainCam;
    private Transform toFollow;
    [SerializeField]private Vector2[] minMaxCameraDistances;
    [SerializeField]private Slider cameraDistanceSlider;
    [SerializeField]private Slider xOffsetSlider;
    [SerializeField,Range(30,100)]private float fieldOfView = 60;
    private float xOffset = 0;
    private float distanceOffset = 100;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }else
        {
            Instance = this;
        }
    }

    void Start()
    {
        mainCam = Camera.main;
        cameraDistanceSlider.maxValue = minMaxCameraDistances[0].y;
        cameraDistanceSlider.minValue = minMaxCameraDistances[0].x;
        cameraDistanceSlider.value = distanceOffset;
        xOffsetSlider.maxValue = 1000;
        xOffsetSlider.minValue = -1000;
        xOffsetSlider.value = 0;
    }

    void LateUpdate()
    {
        if(toFollow != null){transform.position = toFollow.position + new Vector3(xOffset, distanceOffset, -distanceOffset);}
        if(mainCam.fieldOfView != fieldOfView){ mainCam.fieldOfView = fieldOfView;}
    }

    public void SetCameraFollow(Transform obj,int index)
    {
        toFollow = obj;
        cameraDistanceSlider.maxValue = minMaxCameraDistances[index].y;
        cameraDistanceSlider.minValue = minMaxCameraDistances[index].x;
        cameraDistanceSlider.value = minMaxCameraDistances[index].x + 50;
    }

    public void ChangeCameraDistance()
    {
        distanceOffset = cameraDistanceSlider.value;
    }

    public void ChangeXOffset()
    {
        xOffset = xOffsetSlider.value;
    }
}
