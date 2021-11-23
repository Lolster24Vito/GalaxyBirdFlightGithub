using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGranadeEnemy : MonoBehaviour
{
  [SerializeField]  float forceAmountPerDistance;
    [SerializeField] float explosionRadius;
    [SerializeField] float timeTillExplosion;
    [SerializeField] float triggerExplosionInSeconds;

   [SerializeField] ForceMode2D forceMode;
   Transform player;
    [SerializeField] float distanceToReact=47f;
    [SerializeField] LayerMask raycastLayer;
    [SerializeField] GameObject idk;
    bool fired = false;
   [SerializeField] private float distanceFromPlayer;
    private Rigidbody2D rb;
    bool explode = false;
    [SerializeField]float secondsToPlayer = 0f;
    [SerializeField] GameObject explosionParticle;

    [SerializeField] float squashDuration;
    [SerializeField] Vector2 squashAmount = new Vector2(1.5f, 0.5f);
    [SerializeField] LeanTweenType tweenType;

    Vector2 direction;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        fired = false;
        rb = transform.GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
            distanceFromPlayer = Vector2.Distance(player.position, transform.position);
            if (distanceFromPlayer < distanceToReact && !fired)
            {
                 direction = (player.position - transform.position).normalized;

                RaycastHit2D hit = Physics2D.CircleCast(transform.position,1.5f, direction, distanceToReact, raycastLayer);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                    //rotate to player
                    transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180);
                        fired = true;
                    StartCoroutine(beforeExplosionDelay());
                    audioSource.Play();


                    }
                }



            }
            if (fired)
            {
                secondsToPlayer += Time.fixedDeltaTime;
                if ((distanceFromPlayer < explosionRadius/2f||secondsToPlayer> triggerExplosionInSeconds) &&!explode)
                {
                    rb.velocity = Vector2.zero;
                transform.GetComponent<CircleCollider2D>().enabled = false;
                idk.transform.localScale = Vector2.one * explosionRadius;
                explode = true;
                StartCoroutine(explodeInSec(timeTillExplosion));
                }


            }
        
    }
    IEnumerator beforeExplosionDelay()
    {
        squash();
        yield return new WaitForSeconds(squashDuration);
        rb.AddForce(direction * forceAmountPerDistance * distanceFromPlayer, forceMode);

    }
    IEnumerator explodeInSec(float sec)
    {
        idk.SetActive(true);
        colorChange();
        yield return new WaitForSeconds(sec);
        idk.GetComponent<CircleCollider2D>().enabled = true;
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);

    }
    void colorChange()
    {
        LeanTween.alpha(idk,
            to: 0.6f,
            time: timeTillExplosion
        ).setFrom(0f);
    }
    void squash()
    {

            LeanTween.scaleX(gameObject, squashAmount.x, squashDuration).setEase(tweenType);
            LeanTween.scaleY(gameObject, squashAmount.y, squashDuration).setEase(tweenType).setOnComplete(setBackToNormal);
        
    }
    void setBackToNormal()
    {
        LeanTween.scaleX(gameObject, 1f, 0.3f).setEase(tweenType);
        LeanTween.scaleY(gameObject, 1f, 0.3f).setEase(tweenType);

    }
}
