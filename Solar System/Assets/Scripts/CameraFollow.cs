using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private TrailRenderer[] starTrails;
    [SerializeField]private InputActionReference trailInput;
    private bool showTrails = true;
    [SerializeField]private GameObject HUD;
    [SerializeField]private InputActionReference hudInput;
    private bool showHud = true;

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

        starTrails = FindObjectsByType<TrailRenderer>(FindObjectsSortMode.None);
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
        
        if(cameraDistanceSlider.value > minMaxCameraDistances[index].y)
        {
            distanceOffset = minMaxCameraDistances[index].y;
            cameraDistanceSlider.value = distanceOffset;
        }
        if(cameraDistanceSlider.value < minMaxCameraDistances[index].x)
        {
            distanceOffset = minMaxCameraDistances[index].x;
            cameraDistanceSlider.value = distanceOffset;
        }
    }

    public void ChangeCameraDistance()
    {
        distanceOffset = cameraDistanceSlider.value;
    }

    public void ChangeCameraDistance(int index, float amount)
    {
        // Debug.Log("Amount to move by is " + amount);
        // Debug.Log("Current distance from target is " + distanceOffset);
        // Debug.Log("Current amount + distance offset is " + (amount + distanceOffset).ToString());

        if(amount + distanceOffset > minMaxCameraDistances[index].y) 
        {
            //Debug.Log("Reached max distance from camera");
            distanceOffset = minMaxCameraDistances[index].y; 
            return;
        }
        if(amount + distanceOffset < minMaxCameraDistances[index].x) 
        {
            //Debug.Log("Reached min distance from camera");
            distanceOffset = minMaxCameraDistances[index].x; 
            return;
        }
        distanceOffset += amount;
        cameraDistanceSlider.value += amount;
    }

    public void ChangeXOffset()
    {
        xOffset = xOffsetSlider.value;
    }

    private void OnTrailInput(InputAction.CallbackContext input)
    {
        showTrails = !showTrails;
        foreach(TrailRenderer trail in starTrails)
        {
            trail.enabled = showTrails;
        }
    }

    private void OnHUDInput(InputAction.CallbackContext input)
    {
        showHud = !showHud;
        HUD.SetActive(showHud);
    }

    public void ResetXOffset()
    {
        xOffset = 0;
        xOffsetSlider.value = 0;
    }

    void OnEnable()
    {
        trailInput.action.Enable();
        hudInput.action.Enable();
        trailInput.action.performed += OnTrailInput;
        hudInput.action.performed += OnHUDInput;

    }

    void OnDisable()
    {
        trailInput.action.Disable();
        hudInput.action.Disable();
        trailInput.action.performed -= OnTrailInput;
        hudInput.action.performed -= OnHUDInput;
    }
}
