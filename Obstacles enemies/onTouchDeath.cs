using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onTouchDeath : MonoBehaviour
{

    [SerializeField] GameObject particleOnDeath;
    [SerializeField] Sprite killedByImage;
    private void Start()
    {
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (particleOnDeath != null)
        {
            Instantiate(particleOnDeath, transform.position, Quaternion.identity);
        }
        if (collision.collider.CompareTag("Player"))
        {
            if (killedByImage != null && !PlayerStateManager.Instance.IsDead())
                PlayerStateManager.Instance.setKilledByImage(killedByImage);

            PlayerStateManager.Instance.Death();



        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (particleOnDeath != null)
        {
            Instantiate(particleOnDeath, transform.position, Quaternion.identity);
        }
        if (collision.CompareTag("Player"))
        {
            if (killedByImage != null&&!PlayerStateManager.Instance.IsDead())
                PlayerStateManager.Instance.setKilledByImage(killedByImage);
            PlayerStateManager.Instance.Death();

        }
   

    }
}
