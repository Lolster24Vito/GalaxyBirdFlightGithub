using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanObstacle : MonoBehaviour
{
    [SerializeField] Vector2 forceAmount;
    [SerializeField] float forceWhenEntering;
    [SerializeField] float offsetBoxCollider;
    [SerializeField] float distanceMax = 120f;
    [SerializeField] float enemyForceIncreaseMultiplier = 10f;
    [SerializeField] LayerMask checkLayer;
    [SerializeField] GameObject spikesOnEnd;
    bool playerInside = false;




    // Start is called before the first frame update
    void Start()
    {
        //set the trigger on boxcollider and lineRenderer 
        RaycastHit2D hit = Physics2D.CircleCast(transform.position,0.5f, transform.right,distanceMax,checkLayer);
        RaycastHit2D spike = Physics2D.CircleCast(transform.position, 0.5f, transform.right, distanceMax);
        if (spike.collider != null)
        {
            if (spike.collider.gameObject.layer == 8)
            {

                Instantiate(spikesOnEnd, spike.point, Quaternion.FromToRotation(Vector3.up, hit.normal), transform);
            }
        }
        if (hit.point == Vector2.zero)
        {
            StartCoroutine(checkAgain());
        }
        float distanceFromPoint = Mathf.Clamp(Vector2.Distance(transform.position, hit.point),0,distanceMax);
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(distanceFromPoint- offsetBoxCollider, boxCollider2D.size.y);
        boxCollider2D.offset = new Vector2(distanceFromPoint / 2, boxCollider2D.offset.y);
        transform.GetComponent<LineRenderer>().SetPosition(1, new Vector3(distanceFromPoint, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //might make bugs if two or more are activated at the same time so fix it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<ConstantForce2D>())
        {
            float enemyForceIncrease = 1f;
            if (collision.gameObject.CompareTag("Enemy"))
            {
                enemyForceIncrease = enemyForceIncreaseMultiplier;
            }
            if (collision.gameObject.CompareTag("Player"))
            {
                playerInside = true;
            }
            collision.transform.GetComponent<ConstantForce2D>().force += transform.right * forceAmount* enemyForceIncrease;
            if (collision.transform.GetComponent<Rigidbody2D>())
            {
                collision.GetComponent<Rigidbody2D>().AddForce(transform.right * forceWhenEntering);
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<ConstantForce2D>())
        {
            float enemyForceIncrease = 1f;
            if (collision.gameObject.CompareTag("Enemy"))
            {
                enemyForceIncrease = enemyForceIncreaseMultiplier;
            }
            collision.transform.GetComponent<ConstantForce2D>().force -= transform.right * forceAmount* enemyForceIncrease;
            if (collision.gameObject.CompareTag("Player"))
            {
                playerInside = false;
            }
        }
    }
   IEnumerator checkAgain()
    {
        int safetyCounter = 0;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, transform.right, distanceMax, checkLayer);
        while(hit.point == Vector2.zero|| safetyCounter<30)
        {
            safetyCounter += 1;
            hit = Physics2D.CircleCast(transform.position, 0.5f, transform.right, distanceMax, checkLayer);
            yield return new WaitForSeconds(0.4f);
        }

        float distanceFromPoint = Mathf.Clamp(Vector2.Distance(transform.position, hit.point), 0, distanceMax);
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(distanceFromPoint - offsetBoxCollider, boxCollider2D.size.y);
        boxCollider2D.offset = new Vector2(distanceFromPoint / 2, boxCollider2D.offset.y);
        transform.GetComponent<LineRenderer>().SetPosition(1, new Vector3(distanceFromPoint, 0));
    }
    private void OnDestroy()
    {
        if (playerInside)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<ConstantForce2D>().force -= transform.right * forceAmount;
        }
    }
    private void OnDisable()
    {
        if (playerInside)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<ConstantForce2D>().force -= transform.right * forceAmount;
        }
    }
}
