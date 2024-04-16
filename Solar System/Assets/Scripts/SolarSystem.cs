using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class SolarSystem : MonoBehaviour
{
    readonly float G = 100f;
    public Rigidbody[] stars;
    private int currentTarget = 0;
    public TextMeshProUGUI currentTargetDisplay;
    [SerializeField]private InputActionReference changePlanetInput;
    [SerializeField]private InputActionReference changeCameraDistanceInput;
    [SerializeField]private InputActionReference settingsInput;
    private bool settingsOpen;
    [SerializeField]private GameObject settingsPanel;

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

    private void OnChangePlanetInput(InputAction.CallbackContext input)
    {
        ChangeCameraTarget((int)input.ReadValue<float>());
    }

    private void OnCameraDistanceInput(InputAction.CallbackContext input)
    {
        if(input.control.device.name == "Mouse")
        {
            CameraFollow.Instance.ChangeCameraDistance(currentTarget, input.ReadValue<float>());
        }else
        {
            CameraFollow.Instance.ChangeCameraDistance(currentTarget, 10 * input.ReadValue<float>());
        }
    }

    private void OnSettingsInput(InputAction.CallbackContext input)
    {
        if(settingsOpen)
        {
            settingsPanel.SetActive(false);
            settingsOpen = false;
        }else
        {
            settingsPanel.SetActive(true);
            settingsOpen = true;
        }
    }

    void OnEnable()
    {
        changePlanetInput.action.Enable();
        changePlanetInput.action.performed += OnChangePlanetInput;
        settingsInput.action.Enable();
        settingsInput.action.performed += OnSettingsInput;
        changeCameraDistanceInput.action.Enable();
        changeCameraDistanceInput.action.performed += OnCameraDistanceInput;
    }

    void OnDisable()
    {
        changePlanetInput.action.Disable();
        changePlanetInput.action.performed -= OnChangePlanetInput;
        settingsInput.action.Disable();
        settingsInput.action.performed -= OnSettingsInput;
        changeCameraDistanceInput.action.Disable();
        changeCameraDistanceInput.action.performed -= OnCameraDistanceInput;
    }

    public void Exit()
    {
        Application.Quit();
    }

}