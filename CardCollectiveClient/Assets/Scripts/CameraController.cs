using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class CameraController : MonoBehaviour
{
    CameraMove cameraMove;
    Vector3 oldMousePos;


    void Start()
    {
        cameraMove = GetComponent<CameraMove>();

        oldMousePos = Input.mousePosition;
    }

    void Update()
    {
        if (cameraMove != null)
        {
            if (Input.GetKey(KeyCode.Mouse2))
            {
                cameraMove.Rotate(new Vector2(Input.GetAxis("Mouse X") , Input.GetAxis("Mouse Y")));
            }
        }
    }
}

