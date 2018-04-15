using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rotate : MonoBehaviour
{
    [SerializeField] Vector3 rotationAxis = new Vector3(0, 0, 0);
    [SerializeField] float rotationSpeed = 1f;

    private void Awake()
    {
        RotationAxis = rotationAxis;
    }

    public Vector3 RotationAxis
    {
        get { return rotationAxis; }
        set
        {
            if (value != null) rotationAxis = value.normalized;
            else rotationAxis = value;
        }
    }
    public float RotationSpeed
    {
        get { return rotationSpeed; }
        set { rotationSpeed = value; }
    }

    protected void RotateMe()
    {
        if (rotationAxis != null && RotationSpeed != 0)
            transform.Rotate(rotationAxis * Time.deltaTime * RotationSpeed);
    }

    protected virtual void Update()
    {
        RotateMe();
    }
}
