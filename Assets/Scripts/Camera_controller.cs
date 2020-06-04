using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class Camera_controller : MonoBehaviour
{
    GameObject CarObject;
    GameObject Cam_helper;
    Camera Main_Camera;
    AimConstraint Cam_Aim;

    [SerializeField] float Helper_Range = 10f;  //car distance from cam_helper that zooming should start
    [SerializeField] float FOVMin = 15f;
    [SerializeField] float FOVMax = 30f;

    // Start is called before the first frame update
    void Start()
    {
        CarObject = GameObject.Find("Car");
        Cam_helper = GameObject.Find("Camera_helper");
        Main_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Cam_Aim = GameObject.Find("Main Camera").GetComponent<AimConstraint>();

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(CarObject.transform.position, Cam_helper.transform.position);
        float zoom = ((distance * 100) / Helper_Range)/100; //get percentage difference between current distance and helper_range
        
        if (distance < Helper_Range)
        {
            Main_Camera.fieldOfView = Mathf.Max(FOVMax * zoom, FOVMin);

        } else 
        {
            Main_Camera.fieldOfView = FOVMax;
        }
    }
}
