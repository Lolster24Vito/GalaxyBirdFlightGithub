using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectRaycastUp : MonoBehaviour
{
    [SerializeField] LayerMask checkLayer;
    [SerializeField] GameObject spawnObject;
    [SerializeField] float maxDistance = 99f;
    [SerializeField] float minDistance = 4f;
    [SerializeField] float spawnOffset;
    [SerializeField] float divider = 2f;
    [SerializeField] float distance;
    [SerializeField] float radius = 1f;

    // Start is called before the first frame update
    IEnumerator Start()
    {

      distance= getDistance();
        for (float i = spawnOffset; i < distance; i = i + spawnOffset)
        {
            Instantiate(spawnObject, transform.up * i + transform.position, Quaternion.identity, transform);

        }
        yield return new WaitForSeconds(0.25f);

        if (distance != getDistance())
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (float i = spawnOffset; i < distance; i = i + spawnOffset)
            {
                Instantiate(spawnObject, transform.up * i + transform.position, Quaternion.identity, transform);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    float getDistance()
    {
          
        RaycastHit2D ray = Physics2D.CircleCast(transform.position, radius,transform.up, maxDistance,checkLayer);
        if (ray)
        {
           return Mathf.Clamp(Vector2.Distance(ray.point, transform.position) / divider, minDistance, maxDistance);
        }
        else
        {
            return  maxDistance/divider;

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
