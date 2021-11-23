using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forceOnStart : MonoBehaviour
{
    [SerializeField] Vector2 forceAmountMinMax = new Vector2(1,6);
    [SerializeField] ForceMode2D forceMode;
    [SerializeField] float forceAmount;
    [SerializeField] GameObject particleOnTouch;
    bool transformRight = true;
    bool transformUp = false;

    // Start is called before the first frame update
    void OnEnable()
    {

         forceAmount = Random.Range(forceAmountMinMax.x, forceAmountMinMax.y);
        transform.GetComponent<Rigidbody2D>().AddForce(-transform.right * forceAmount,forceMode);
        transform.rotation = Quaternion.identity;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
