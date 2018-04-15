using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] Transform pivotPoint;

    public Transform PivotPoint
    {
        get { return pivotPoint; }
        set { pivotPoint = value; }
    }

    void Update()
    {
        if (pivotPoint != null)
            transform.LookAt(pivotPoint);
    }
}
