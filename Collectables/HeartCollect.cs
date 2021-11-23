using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerStateManager.Instance.HeartCoin();
        Destroy(transform.parent.gameObject);


    }
}
