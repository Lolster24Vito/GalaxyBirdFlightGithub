using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveToParentLineRendererPoints : MonoBehaviour
{

    private Vector3[] Points;
    private int _nextPoint = 1;
    [SerializeField] float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        LineRenderer parentLineRenderer = transform.parent.GetComponent<LineRenderer>();
        Points = new Vector3[parentLineRenderer.positionCount];
        parentLineRenderer.GetPositions(Points);
        Transform parent = transform.parent;
       for(int i = 0; i < Points.Length; i++)
        {
            Points[i] += parent.position;


        }
    }

    private void FixedUpdate()
    {
        // transform.position=Vector3.SmoothDamp(transform.position, Points[nextPoint].position, ref smoothVelocityRef, smoothTime, maxSpeed);
        transform.position = Vector3.MoveTowards(transform.position, Points[_nextPoint], speed * Time.fixedDeltaTime);
        if (Vector3.Distance(transform.position, Points[_nextPoint]) < 0.1f)
        {
            _nextPoint++;
            if (_nextPoint > Points.Length - 1)
            {
                _nextPoint = 0;
            }
        }
    }
}
