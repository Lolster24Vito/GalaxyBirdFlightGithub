using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLineRendererParent : MonoBehaviour
{
    public int segments;
    public float radiousOffset;
    public float minRadius;
    private float xradius;
    private float yradius;
private    float _radius;
    public LayerMask wallLayer;
    [SerializeField] bool testing=false;

    LineRenderer line;
    GameObject checkColliderObject;
    CircleCollider2D checkCollider;

    void Awake()
    {
        testing = false;
        if (transform.parent) { 
        transform.parent.rotation = Quaternion.Euler(Vector3.zero);
        }
        //fucking hell fix this and move to line renderer script too
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        CheckRadius();
        makeCheckCollider();

        line = gameObject.GetComponent<LineRenderer>();

        line.positionCount=segments + 1;
        line.useWorldSpace = false;
        //CreatePoints();
        StartCoroutine(waitAndCheckRadiusAgain());



    }

    void CheckRadius()
    {
        if (checkCollider != null)
        {
            checkCollider.gameObject.SetActive(false);
        }
        //Test the max radious of the circle
        for (float i = 0; i < 60; i = i + 0.5f)
        {
            if (Physics2D.OverlapCircle(transform.position, i, wallLayer))
            {
                _radius = i;
                break;
            }

        }
        //Debug.Log(_radius + "___ " + gameObject.name+ "   "+gameObject.transform.parent.gameObject.name);
        if (minRadius > _radius)
        {
            _radius = minRadius;
        }
        xradius = (_radius) - radiousOffset;
        yradius = (_radius) - radiousOffset;

        if (checkCollider != null)
        {
            checkCollider.gameObject.active = true;
        }
    }
    void CreatePoints()
    {
        float x;
        float y;
        float z = 0f;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
        }
    }
    private void OnValidate()
    {
        if (testing)
        {

           
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            //Test the max radious of the circle
            for (float i = 0; i < 40; i = i + 0.5f)
            {
                if (Physics2D.OverlapCircle(transform.position, i, wallLayer))
                {
                    _radius = i;
                    break;
                }

            }
            if (minRadius > _radius)
            {
                _radius = minRadius;
            }
            xradius = (_radius) - radiousOffset;
            yradius = (_radius) - radiousOffset;


            line = gameObject.GetComponent<LineRenderer>();

            line.SetVertexCount(segments + 1);
            line.useWorldSpace = false;
            CreatePoints();

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }


        }
    }
    IEnumerator waitAndCheckRadiusAgain()
    {
        yield return new WaitForSeconds(0.1f);
        CheckRadius();
        makeCheckCollider();
        CreatePoints();

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

    }
    void makeCheckCollider()
    {
        if (checkColliderObject == null)
        {
            checkColliderObject = new GameObject("CheckCollider");
        }
        checkColliderObject.transform.parent = transform;
        checkColliderObject.transform.localPosition = Vector3.zero;
        //11 is the checkLayer...A layer specifically for checking so that two obstacles don't go into each other
        checkColliderObject.layer = 11;

        if (checkCollider==null)
        {
            checkCollider = checkColliderObject.AddComponent<CircleCollider2D>();
        }
        checkCollider.radius = _radius;

    }
}
