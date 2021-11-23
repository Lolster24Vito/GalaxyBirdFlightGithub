using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRayShoot : MonoBehaviour
{
    [SerializeField] float _projectileSpeed = 15f;
    [SerializeField] GameObject particleOnDeath;
    [SerializeField] LayerMask layer;
    [SerializeField] float test;

    Transform Player;
    bool _activated = false;
    AudioSource audio;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {


        //  if (Vector2.Angle(transform.up + transform.position, Player.position) <= 15f && Vector2.Distance(Player.position, transform.position) < 70f) 
        // {
        
       
        //}


        // If it hits something...

    }
    private void FixedUpdate()
    {

        if (_activated)
        {
            rb.MovePosition(transform.position + (transform.up * _projectileSpeed * Time.fixedDeltaTime));

        }

        else 

        if (Vector2.Distance(Player.position, transform.position) < 70f)
        {
          Vector2 playerDir = (transform.position - Player.position).normalized;
            if (Vector2.Dot(transform.up, playerDir) <-0.95f)
          {
        
            RaycastHit2D hit = Physics2D.CircleCast(transform.position + transform.up, 0.3f, transform.up, 100f, layer);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    transform.GetComponent<SpriteRenderer>().color = Color.red;
                    _activated = true;
                        audio.Play();
                }

            }
           }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_activated)
        {
            if (particleOnDeath != null)
            {
                Instantiate(particleOnDeath, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position+transform.up,transform.position+transform.up*10f);
    }
}
