using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnGameObjectCircleCheck : MonoBehaviour
{
    [SerializeField] float radiousOffset = 0f;
    [SerializeField] float _radius = 0f;
    [SerializeField] float minRadius = 4f;
    [SerializeField] float spawnOffset = 1.5f;
    [SerializeField] GameObject spawnObject;
    [SerializeField] bool twosided=false;


    [SerializeField] LayerMask wallLayer;
    // Start is called before the first frame update
    void Start()
    {
        for (float i = 0; i < 60; i = i + 0.5f)
        {
            if (Physics2D.OverlapCircle(transform.position, i, wallLayer))
            {
                _radius = i;
                break;
            }

        }
        StartCoroutine(waitAndCheckRadiusAgain());
      

    }

    IEnumerator waitAndCheckRadiusAgain()
    {
        yield return new WaitForSeconds(0.1f);
        for (float i = 0; i < 60; i = i + 0.5f)
        {
            if (Physics2D.OverlapCircle(transform.position, i, wallLayer))
            {
                _radius = i;
                break;
            }

        }

        //make a objects in line in a line
        _radius -= radiousOffset;
        for (float i = spawnOffset; i < _radius; i = i + spawnOffset)
        {
            Instantiate(spawnObject, Vector3.up * i + transform.position, Quaternion.identity, transform);

        }
        if (twosided)
        {
                    for (float i = spawnOffset; i < _radius; i = i + spawnOffset)
        {
            Instantiate(spawnObject, Vector3.down * i + transform.position, Quaternion.identity, transform);

        }
        }
        if (minRadius > _radius)
        {
            _radius = minRadius;
        }

        makeCheckCollider();

    }
    void makeCheckCollider()
    {
        GameObject newGameObj = new GameObject("CheckCollider");
        newGameObj.transform.parent = transform;
        newGameObj.transform.localPosition = Vector3.zero;
        //11 is the checkLayer...A layer specifically for checking so that two obstacles don't go into each other
        newGameObj.layer = 11;
        CircleCollider2D checkCircle = newGameObj.AddComponent<CircleCollider2D>();
        checkCircle.radius = _radius;
    }
}
