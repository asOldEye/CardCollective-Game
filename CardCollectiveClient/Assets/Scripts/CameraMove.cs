using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraMove : RotateAround
{
    [SerializeField] PostProcessingBehaviour postProcessing;

    protected override void Update()
    {
        //transform.LookAt(Vector3.Lerp(transform.forward, PivotPoint.position, 0.1f));
    }

    void Awake()
    {
        if (postProcessing == null)
            postProcessing = GetComponent<PostProcessingBehaviour>();
    }

    public void PostProcessingDistanceRefresh(float distance)
    {
        //var f = new DepthOfFieldModel.Settings();
        //f.focusDistance = distance;
        //f.aperture = postProcessing.profile.depthOfField.settings.aperture;
        //f.useCameraFov = true;
        //f.kernelSize = postProcessing.profile.depthOfField.settings.kernelSize;
        //postProcessing.profile.depthOfField.settings = f;

        var n = postProcessing.profile.depthOfField.settings;
        n.focusDistance = distance;
        postProcessing.profile.depthOfField.settings = n;
    }

    public void Rotate(Vector2 vector)
    {
        transform.Rotate(new Vector2(vector.y * RotationSpeed * Time.deltaTime, vector.x * RotationSpeed * Time.deltaTime));
        transform.Rotate(new Vector2(vector.y * RotationSpeed * Time.deltaTime, 0));
    }

    public void RotateAround()
    {

    }
}
