using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    readonly float G = 100f;
    public Rigidbody[] stars;
    private int currentTarget = 0;
    public TextMeshProUGUI currentTargetDisplay;

    void Start()
    {
        InitialVelocity();
        currentTargetDisplay.text = stars[currentTarget].name.ToString();
        CameraFollow.Instance.SetCameraFollow(stars[currentTarget].transform,currentTarget);
    }

    void FixedUpdate()
    {
        Gravity();
    }

    void Gravity()
    {
        foreach(Rigidbody a in stars)
        {
            foreach(Rigidbody b in stars)
            {
                if(!a.Equals(b))
                {
                    float m1 = a.mass;
                    float m2 = b.mass;
                    float r = Vector3.Distance(a.transform.position,b.transform.position);

                    a.AddForce((b.transform.position - a.transform.position).normalized * (G *(m1 * m2) / (r * r)));
                }
            }
        }
    }

    void InitialVelocity()
    {
        foreach(Rigidbody a in stars)
        {
            foreach(Rigidbody b in stars)
            {
                if(!a.Equals(b))
                {
                    float m2 = b.mass;
                    float r = Vector3.Distance(a.gameObject.transform.position,b.gameObject.transform.position);
                    a.transform.LookAt(b.transform);
                    a.velocity += a.transform.right * Mathf.Sqrt((G * m2) / r);
                }
            }
        }
    }

    public void ChangeCameraTarget(int index)
    {
        currentTarget += index;
        if(currentTarget > stars.Length -1){currentTarget = 0;}
        if(currentTarget < 0){currentTarget = stars.Length -1;}
        currentTargetDisplay.text = stars[currentTarget].name.ToString();
        CameraFollow.Instance.SetCameraFollow(stars[currentTarget].transform,currentTarget);
    }
}