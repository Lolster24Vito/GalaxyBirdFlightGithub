using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class galaxyBirdScript : MonoBehaviour
{
    // Start is called before the first frame update
 public void GiveCrown()
    {
        PlayerStateManager.Instance.setCrownOnSkin();
    }
    public void continueMoving()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveJoystick>().continueMovingCinematic();
    }

}
