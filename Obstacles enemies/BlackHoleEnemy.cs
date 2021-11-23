using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleEnemy : MonoBehaviour
{
    [SerializeField] LayerMask detectLayerMask;
    [SerializeField] float checkCircleRad;
    [SerializeField] float forceAmount;
    [SerializeField] ForceMode2D forceMode;
    [SerializeField] Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(player.position, transform.position) < checkCircleRad)
        {
            if (Physics2D.OverlapCircle(transform.position, checkCircleRad, detectLayerMask))
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkCircleRad, detectLayerMask);
                foreach (Collider2D element in colliders)
                {
                    if (element.gameObject.GetComponent<Rigidbody2D>())
                    {
                        Vector2 direction = (transform.position - element.transform.position).normalized;
                        //   float distance = Vector2.Distance(element.transform.position, transform.position);

                        //   element.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * (distanceForce / distance), forceMode);
                        element.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * forceAmount, forceMode);
                    }
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, checkCircleRad);
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //10 layer is moving enemies
        if (collision.gameObject.layer == 10||collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject, 0.1f);


        }
    }
    */
}
