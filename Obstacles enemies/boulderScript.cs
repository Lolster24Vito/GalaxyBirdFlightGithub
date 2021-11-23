using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boulderScript : MonoBehaviour
{
    [SerializeField] float power = 10f;
    [SerializeField] Transform jumpDir;
    [SerializeField] float distanceToJump=90f;
    [SerializeField] bool fire_On_Player_Close=true;
    [SerializeField] bool fireOnEnable = false;
    [SerializeField] float gravityScale = 0.8f;

    Rigidbody2D rb;
    bool nearPlayer = false;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb=this.GetComponent<Rigidbody2D>();
        if(fire_On_Player_Close)
        rb.gravityScale = 0f;

        
    }
    private void OnEnable()
    {
        if (fireOnEnable)
        {
            if (rb == null) { rb = this.GetComponent<Rigidbody2D>();
            }
            Vector3 dir = (jumpDir.position - transform.position).normalized;
           // Debug.LogWarning("MAAAN1:" + rb.gravityScale);
            rb.gravityScale = gravityScale;
            rb.AddForce(dir * power, ForceMode2D.Impulse);
            rb.AddTorque(100f);
         //   Debug.LogWarning("MAAAN2:" + rb.gravityScale);
        }
    }
    
    // Update is called once per frame
    void Update()
    {

        if (fire_On_Player_Close)
        {
            if (!nearPlayer)
            {
                if (Vector3.Distance(transform.position, player.position) < distanceToJump)
                {
                    nearPlayer = true;
                    Vector3 dir = (jumpDir.position - transform.position).normalized;
                    rb.gravityScale = gravityScale;
                    rb.AddForce(dir * power, ForceMode2D.Impulse);
                    rb.AddTorque(100f);
                }
            }

        }
    }
}
