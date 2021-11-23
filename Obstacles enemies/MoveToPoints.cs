using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoints : MonoBehaviour
{
    [SerializeField] float distanceToPoint = 0.1f;
    [SerializeField] float speed;
    [SerializeField] float maxDistance;
    [SerializeField] float offset;
    [SerializeField] bool testing = false;

    [SerializeField] Transform[] Points;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] float  checkColliderWidth=3f;


    private int nextPoint=1;
    private Vector3 smoothVelocityRef;
    GameObject checkColliderObj;
    BoxCollider2D checkBoxCol;

    private void Start()
    {
        CheckAndSetPositions();
        SetCheckCollider();
        StartCoroutine(checkAgain());

    }
    private void FixedUpdate()
    {
        // transform.position=Vector3.SmoothDamp(transform.position, Points[nextPoint].position, ref smoothVelocityRef, smoothTime, maxSpeed);
        transform.position = Vector3.MoveTowards(transform.position, Points[nextPoint].position, speed*Time.fixedDeltaTime);
        if (Vector3.Distance(transform.position,Points[nextPoint].position)< distanceToPoint)
        {
            nextPoint++;
            if (nextPoint > Points.Length-1)
            {
                nextPoint = 0;
            }
        }
    }

    private void OnValidate()
    {
        if (testing)
        {

            Vector2 dir = (Points[0].position - Points[1].position).normalized;
            RaycastHit2D positionRaycast = Physics2D.CircleCast(Points[0].position, 0.5f, dir, 50);
            if (positionRaycast)
            {
                Points[0].position = positionRaycast.point - dir * offset;
            }
            else
            {
                Vector3 pointPosition = Points[0].position + (new Vector3(dir.x, dir.y, 0) * maxDistance);
                Points[0].position = pointPosition;

            }
            //  Instantiate(testDot, idk.point, Quaternion.identity);
            positionRaycast = Physics2D.CircleCast(Points[1].position, 0.1f, -dir, 50);
            if (positionRaycast)
            {
                Points[1].position = positionRaycast.point + dir * offset;
            }
            else
            {
                Vector3 pointPosition = Points[1].position - (new Vector3(dir.x, dir.y, 0) * maxDistance);
                Points[1].position = pointPosition;

            }


            transform.position = Points[0].position;


        }
    }
    void CheckAndSetPositions()
    {
        testing = false;
        Vector2 dir = (Points[0].position - Points[1].position).normalized;
        RaycastHit2D positionRaycast = Physics2D.CircleCast(Points[0].position, 0.5f, dir, 60, wallLayer);
        if (positionRaycast)
        {
            Points[0].position = positionRaycast.point - dir * offset;
        }
        else
        {
            Vector3 pointPosition = Points[0].position + (new Vector3(dir.x, dir.y, 0) * maxDistance);
            Points[0].position = pointPosition;

        }
        //  Instantiate(testDot, idk.point, Quaternion.identity);
        positionRaycast = Physics2D.CircleCast(Points[1].position, 0.5f, -dir, 60, wallLayer);
        if (positionRaycast)
        {
            Points[1].position = positionRaycast.point + dir * offset;
        }
        else
        {
            Vector3 pointPosition = Points[1].position - (new Vector3(dir.x, dir.y, 0) * maxDistance);
            Points[1].position = pointPosition;

        }


        transform.position = Points[0].position;

        setLineRendererPositions();

        //SetCheckCollider();

    }
    void SetCheckCollider()
    {
        Vector3 middleBetweenTwoPoints = Vector3.Lerp(Points[0].position, Points[1].position, 0.5f);
        if (checkColliderObj == null)
        {
            checkColliderObj = new GameObject("CheckCollider");
        }
        checkColliderObj.layer = 11;
        checkColliderObj.transform.position = middleBetweenTwoPoints;
        checkColliderObj.transform.rotation = transform.parent.rotation;
        checkColliderObj.transform.parent = transform.parent;
        if (checkBoxCol == null)
        {
            checkBoxCol = checkColliderObj.AddComponent<BoxCollider2D>();
        }
        checkBoxCol.size = new Vector2(checkColliderWidth, Vector3.Distance(Points[0].position, Points[1].position));
    }
    void setLineRendererPositions()
    {
        LineRenderer lineRenderer = Points[0].GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);

        lineRenderer.SetPosition(1, Points[1].position );
    }
    IEnumerator checkAgain()
    {
       
            yield return new WaitForSeconds(0.15f);
        SetCheckCollider();
            CheckAndSetPositions();
            SetCheckCollider();
        
    }
}
