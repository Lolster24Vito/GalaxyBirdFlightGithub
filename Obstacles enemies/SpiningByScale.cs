using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiningByScale : MonoBehaviour
{
    [SerializeField] float speed = 4f;
    private float direction = 1;
    private Vector3 scale;
    private Vector3 startingScale;
    // Start is called before the first frame update
    private void Start()
    {
        startingScale = transform.localScale;
    }
    private void Update()
    {
        scale = transform.localScale;

        transform.localScale = new Vector3(transform.localScale.x + (direction * speed * Time.deltaTime), scale.y, scale.z);
        if (transform.localScale.x > startingScale.x)
        {
            transform.localScale = startingScale;
            direction *= -1;
        }
        if (transform.localScale.x < -startingScale.x)
        {
            transform.localScale = new Vector2(-startingScale.x, startingScale.y);
            direction *= -1;
        }
    }


   
}
