using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyRandomMove : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float distanceToMove = 80f;
    [SerializeField] bool startRandomDir =true;
    Vector3 startingScale;

    Vector2 dir =Vector2.left;
    Rigidbody2D rb;
    GameObject player;
    static int _direction = -1;
    // Start is called before the first frame update
    void Start()
    {
        startingScale = transform.localScale;
        _direction *= -1;
        rb = GetComponent<Rigidbody2D>();
        player= GameObject.FindGameObjectWithTag("Player");
        if (startRandomDir)
        {
            dir = new Vector2(Random.Range(-1, 2), 1 * _direction);

        }
        else
        {
            dir = transform.up;
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);

        if (dir.x == 0)
        {
            dir.x = _direction;
        }
      
    }
    private void OnEnable()
    {
        if (startRandomDir)
        {
            dir = new Vector2(Random.Range(-1, 2), 1 * _direction);

        }
        else
        {
            dir = transform.up;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 ballLocation = transform.position;
        Vector2 inNormal = (ballLocation - collision.contacts[0].point).normalized;
        dir = Vector2.Reflect(dir, inNormal);
        rb.velocity=dir * speed;


    }
    private void FixedUpdate()
    {

        if (Vector2.Distance(player.transform.position, transform.position) < distanceToMove)
        {
            rb.velocity = dir * speed;
          //  rb.MovePosition(rb.position + (dir*speed*Time.fixedDeltaTime));

            //rotate enemy
            if (dir.x > 0)
            {
                transform.eulerAngles = Vector3.forward * Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.localScale = new Vector3(-startingScale.x, startingScale.y, 1);

            }
            if (dir.x < 0)
            {
                transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180);
                transform.localScale = new Vector3(startingScale.x, startingScale.y, 1);

            }
        }
        else
        {
            rb.velocity = dir * 0;

        }


        // if (_moveToPlayer)
        //{
        //}
        //else
        //{
        //    rb.velocity = Vector2.zero;
        //}
    }
    /* IEnumerator move()
      {
          while (true)
          {
              if (_counter == moveToPlayerEveryMoves)
              {
                   dir = (player.transform.position - transform.position).normalized;
                //  // rb.AddForce(dirToPlayer * speed, ForceMode2D.Impulse);
                  rb.velocity = dir * speed;

                  _counter = 0;

              }
              else {
                  rb.velocity = dir * speed;
                  _counter++;
              }
              yield return new WaitForSeconds(1.5f);
          }
      }
      */
    
}
