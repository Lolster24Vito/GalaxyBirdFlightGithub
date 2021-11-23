using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnWallTouchDestroy : MonoBehaviour
{
   [SerializeField] bool destroy = false;
    [SerializeField] int timesItCanTouchWall = 0;
    [SerializeField] GameObject particleEffectsOnTouch;
    int timesItTouchedWall = 0;

    private void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        timesItTouchedWall += 1;
        if (timesItTouchedWall > timesItCanTouchWall)
        {
            timesItTouchedWall = 0;
            if (!destroy)
            {
                gameObject.SetActive(false);
            }
            if (destroy)
            {
                Destroy(gameObject);
            }
            if(particleEffectsOnTouch!=null)
            Instantiate(particleEffectsOnTouch, transform.position, Quaternion.identity);
        }
       
    }
}
