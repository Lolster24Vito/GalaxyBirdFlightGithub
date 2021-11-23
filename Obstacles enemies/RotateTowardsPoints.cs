using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsPoints : MonoBehaviour
{
    [SerializeField] Transform point1;
    [SerializeField] Transform point2;
    Vector2 point1Dir;
    Vector2 point2Dir;

    [SerializeField] float rotationSpeed;
    [SerializeField] int dirHelper=1;
    [SerializeField] float point1Helper;
    [SerializeField] float point2Helper;
    bool rotate;

    float targetAngle;
    Vector2 dirToPoint;
    // Start is called before the first frame update
    void Start()
    {
        rotate = true;
        point1Dir = (transform.position - point1.position).normalized;
        point2Dir = (transform.position - point2.position).normalized;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rotate)
        {
            transform.Rotate(Vector3.back * Time.fixedDeltaTime * rotationSpeed * dirHelper);
            if (Vector2.Dot(transform.up, point1Dir) > 0.98f)
            {
                dirHelper = 1;
            }
            if (Vector2.Dot(transform.up, point2Dir) > 0.98f)
            {
                dirHelper = -1;
            }

            point1Helper = Vector2.Dot(transform.up, point1Dir);
            point2Helper = Vector2.Dot(transform.up, point2Dir);
        }

    }
    public void StopRotating()
    {
        rotate = false;
    }
    public void ContinueRotating()
    {
        rotate = true;
    }
}
