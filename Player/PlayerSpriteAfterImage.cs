using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteAfterImage : MonoBehaviour
{
    [SerializeField] private float startAlpha = 0.8f;
    [SerializeField] private float alphaMultiplier;
    private SpriteRenderer wingSprite;
    private SpriteRenderer bodySprite;
    GameObject player;
    SpriteRenderer playerWingSprite;
    public static bool activated = true;
    float alpha;
    [SerializeField] float delayStart = 0.1f;
    float delayStartHelper;
    bool waited = false;
    private void Start()
    {

       // yield return new WaitForSeconds(delayStart);
        player = GameObject.FindGameObjectWithTag("Player");
        wingSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        bodySprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
        alpha = startAlpha;
        delayStartHelper = delayStart;
        waited = true;
        SetSkins();


    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(delayStart);
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
        wingSprite.color = new Color(wingSprite.color.r, wingSprite.color.g, wingSprite.color.b, startAlpha);
        bodySprite.color = new Color(bodySprite.color.r, bodySprite.color.g, bodySprite.color.b, startAlpha);
        wingSprite.sprite = playerWingSprite.sprite;
        alpha = startAlpha;
        waited = true;
    }

    // Start is called before the first frame update
    public void SetSkins()
    {
        bodySprite.sprite = player.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite;
        playerWingSprite = player.transform.GetChild(1).GetComponent<SpriteRenderer>();
        wingSprite.transform.localScale = playerWingSprite.transform.localScale;
        wingSprite.color = new Color(playerWingSprite.color.r, playerWingSprite.color.g, playerWingSprite.color.b, startAlpha);
        bodySprite.color = new Color(playerWingSprite.color.r, playerWingSprite.color.g, playerWingSprite.color.b, startAlpha);

    }


    // Update is called once per frame
    void Update()
    {
        if (waited)
        {
            if (alpha > 0.01f)
            {
                alpha -= alpha * alphaMultiplier;
                wingSprite.color = new Color(wingSprite.color.r, wingSprite.color.g, wingSprite.color.b, alpha);
                bodySprite.color = new Color(bodySprite.color.r, bodySprite.color.g, bodySprite.color.b, alpha);

            }
            else waited = false;
            

            if (activated)
            {
                /*
                if (wingSprite.color.a < 0.05f)
                {
                    transform.position = player.transform.position;
                    transform.rotation = player.transform.rotation;
                    wingSprite.color = new Color(wingSprite.color.r, wingSprite.color.g, wingSprite.color.b, startAlpha);
                    bodySprite.color = new Color(bodySprite.color.r, bodySprite.color.g, bodySprite.color.b, startAlpha);
                    wingSprite.sprite = playerWingSprite.sprite;
                    alpha = startAlpha;

                }
                */

            }



        }
        if (!waited && activated)
        {
        }

    }

    public void Dash()
    {
        StartCoroutine(Wait());
    }
}
