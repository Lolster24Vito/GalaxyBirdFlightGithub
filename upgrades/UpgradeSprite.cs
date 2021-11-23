using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSprite : MonoBehaviour
{

    // Update is called once per frame


    private void OnTriggerStay2D(Collider2D collision)
    {


        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            //We transform the touch position into word space from screen space and store it.
            Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            //Debug.Log("Touched " + touchPosWorld.x + "" +  touchPosWorld.y);
            //We now raycast with this information. If we have hit something we can process it.
            Collider2D hitInformation = Physics2D.OverlapCircle(touchPosWorld2D, 2f);
            if (hitInformation != null && hitInformation.gameObject == gameObject)
            {
                UpgradeUI.Instance.ShowShopUI();
            }

        }
    }
}
