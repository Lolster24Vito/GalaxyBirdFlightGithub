using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerStateManager.Instance.StarPowerUp();
        Destroy(transform.parent.gameObject);


    }
}
