using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemiesTrigger : MonoBehaviour
{
   public bool doKill = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (doKill)
        {
            if (collision.gameObject.layer == 10 || collision.gameObject.CompareTag("Enemy"))
            {
                if (PlayerStateManager.Instance.distanceFromPlayer(collision.gameObject.transform) < 72)
                {
                    //Destroy(collision.gameObject, 0.1f);
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (doKill)
        {
            if (collision.gameObject.layer == 10 || collision.gameObject.CompareTag("Enemy"))
            {
               // Debug.Log("testinjo3: " + PlayerStateManager.Instance.distanceFromPlayer(collision.gameObject.transform) + " " + collision.gameObject.name);
                if (PlayerStateManager.Instance.distanceFromPlayer(collision.gameObject.transform) < 72f)
                {
                 //   Debug.Log("testinjo4: " + PlayerStateManager.Instance.distanceFromPlayer(transform));
                    //Destroy(collision.gameObject, 0.1f);
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (doKill)
        {
            if (collision.gameObject.layer == 10 || collision.gameObject.CompareTag("Enemy"))
            {
                //Debug.Log("testinjo3: " + PlayerStateManager.Instance.distanceFromPlayer(collision.gameObject.transform) + " " + collision.gameObject.name);
                if (PlayerStateManager.Instance.distanceFromPlayer(collision.gameObject.transform) < 72f)
                {
                  //  Debug.Log("testinjo4: " + PlayerStateManager.Instance.distanceFromPlayer(transform));
                    //Destroy(collision.gameObject, 0.1f);
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }
    public void DisableKill()
    {
        doKill = false;
    }
    public void EnableKill()
    {
        doKill = true;

    }

}
