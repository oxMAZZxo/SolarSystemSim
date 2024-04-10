using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private static CameraFollow Instance;
    private Camera mainCam;
    public Transform toFollow;
    [Range(30,100)]public float fieldOfView = 60;
    [Range(-1000,1000)]public float xOffset = 0;
    [Range(0,15000)]public float distanceOffset = 100;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Instance = this;
        }else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if(toFollow != null){transform.position = toFollow.position + new Vector3(xOffset, distanceOffset, -distanceOffset);}
        if(mainCam.fieldOfView != fieldOfView){ mainCam.fieldOfView = fieldOfView;}
    }
}
