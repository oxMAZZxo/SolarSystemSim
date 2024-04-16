using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private Vector3 initialVelocity;

    void FixedUpdate()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.Rotate(initialVelocity * Time.fixedDeltaTime, Space.World);
    }

    public void SetInitalVelocity(Vector3 newValue)
    {
        initialVelocity = newValue;
        //initialVelocity.x = newValue.y;
        //initialVelocity.y = newValue.x;
    }
}
