using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToObject : MonoBehaviour
{
    [SerializeField] Transform lineToObject;
    [SerializeField] Vector3 offset0;
    [SerializeField] Vector3 offset1;

    private void Start()
    {
        LineRenderer lineRenderer = transform.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0,transform.position + offset0);

        lineRenderer.SetPosition(1, lineToObject.position + offset1);


    }
    private void OnValidate()
    {
        if (lineToObject != null)
        {
            LineRenderer lineRenderer = transform.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, transform.position + offset0);

            lineRenderer.SetPosition(1, lineToObject.position + offset1);
        }
    }
}
