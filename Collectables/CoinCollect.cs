using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    [SerializeField] int amount;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerStateManager.Instance.AddCoinToPlayer(amount);
        Destroy(transform.parent.gameObject);
       
            //PlayerStateManager.Instance.Death();

        

    }
}
