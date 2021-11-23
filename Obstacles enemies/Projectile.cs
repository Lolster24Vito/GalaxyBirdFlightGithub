using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _projectileSpeed = 20f;
    Rigidbody2D rb;
    [SerializeField] GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + (transform.up * _projectileSpeed * Time.fixedDeltaTime));

    }/*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("bullet has coliideded with on trigger with " + collision.gameObject.name);
        if (particle != null) { 
        Instantiate(particle, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }*/
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (particle != null)
        {
            Instantiate(particle, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }
}
