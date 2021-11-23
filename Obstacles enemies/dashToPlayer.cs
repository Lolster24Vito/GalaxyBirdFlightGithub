using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public struct EventWithTime
{
    public float time;
    public UnityEvent unityEvent;
}
public class dashToPlayer : MonoBehaviour
{
    [SerializeField] float speedByDistance;
    [SerializeField] float minDistanceToPlayer;
    [SerializeField] float timeBetweenDashes;
    [SerializeField] float timeBeforeDash;
    [SerializeField] float timeAfterDash=0f;
    [SerializeField] float timeAfterAfterDash=0f;
    [SerializeField] Sprite dashSprite;
    [SerializeField] bool raycastBeforeDashing = true;
    [SerializeField] float distanceToDash = 40f;
    [SerializeField] public UnityEvent OnDash;
    [SerializeField] public UnityEvent OnBeforeDash;
    [SerializeField] public UnityEvent OnAfterDash;
    [SerializeField] public UnityEvent OnAfterAfterDash;
    [SerializeField] public EventWithTime[] AfterDashArrayAfter;

    Sprite regularSprite;
    [SerializeField] LayerMask raycastLayer;
    Vector3 scaleNormal;
    float randomOffset;
    Transform player;
    bool fired = false;
    Rigidbody2D rb;
   [SerializeField] SpriteRenderer renderer;
    AudioSource audio;

    [SerializeField] Vector2 squashAmount = new Vector2(1.5f, 0.5f);
    [SerializeField] LeanTweenType tweenType;

    // Start is called before the first frame update
    void Start()
    {
     if(GetComponent<AudioSource>())audio = GetComponent<AudioSource>();
        //renderer = transform.GetComponentInChildren<SpriteRenderer>();
        if(renderer!=null)
        regularSprite = renderer.sprite;

        randomOffset = Random.Range(0, 1);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        if (renderer != null)
            scaleNormal = renderer.transform.localScale;
        else
            scaleNormal = Vector2.one;
    }
    private void Update()
    {
        if (!fired&&raycastBeforeDashing)
        {
            if (Vector3.Distance(player.position, transform.position) < distanceToDash)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, minDistanceToPlayer, raycastLayer);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        StartCoroutine(Dash());
                        fired = true;
                    }
                }
            }
        }
        if (!raycastBeforeDashing&&!fired)
        {
            StartCoroutine(Dash());
            fired = true;
        }
    }

    IEnumerator Dash()
    {
        yield return new WaitForSeconds(randomOffset);
        while (true)
        {
            float distanceFromPlayer = Vector3.Distance(player.position, transform.position);
            if (distanceFromPlayer < minDistanceToPlayer)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, minDistanceToPlayer, raycastLayer);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        //rotate to player
                        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180);
                        rb.velocity = Vector2.zero;
                        if(renderer!=null)
                        renderer.sprite = dashSprite;

                        squash();
                        OnBeforeDash.Invoke();
                        yield return new WaitForSeconds(timeBeforeDash);
                        if (audio != null) audio.Play();
                        rb.AddForce(direction * speedByDistance, ForceMode2D.Impulse);
                        OnDash.Invoke();
                       // Debug.Log(direction * speedByDistance);
                       if(renderer!=null)
                        renderer.sprite = regularSprite;
                        //StartCoroutine(dash());
                        // audioSource.Play();
                        OnAfterDash.Invoke();
                        yield return new WaitForSeconds(timeAfterDash);
                        OnAfterAfterDash.Invoke();
                        yield return new WaitForSeconds(timeAfterAfterDash);
                        for(int i = 0; i < AfterDashArrayAfter.Length; i++)
                        {
                            AfterDashArrayAfter[i].unityEvent.Invoke();
                            yield return new WaitForSeconds(AfterDashArrayAfter[i].time);
                        }

                    }
                }
            }
            yield return new WaitForSeconds(timeBetweenDashes);
        }
    }
    IEnumerator DashOnce()
    {
        yield return new WaitForSeconds(randomOffset);
            float distanceFromPlayer = Vector3.Distance(player.position, transform.position);
            if (distanceFromPlayer < minDistanceToPlayer)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, minDistanceToPlayer, raycastLayer);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        //rotate to player
                        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180);
                        rb.velocity = Vector2.zero;
                        if (renderer != null)
                            renderer.sprite = dashSprite;

                        squash();
                        OnBeforeDash.Invoke();
                        yield return new WaitForSeconds(timeBeforeDash);
                        if (audio != null) audio.Play();
                        rb.AddForce(direction * speedByDistance, ForceMode2D.Impulse);
                        OnDash.Invoke();
                        // Debug.Log(direction * speedByDistance);
                        if (renderer != null)
                            renderer.sprite = regularSprite;
                        //StartCoroutine(dash());
                        // audioSource.Play();
                        OnAfterDash.Invoke();
                       

                    }
                }
            }
            yield return new WaitForSeconds(timeBetweenDashes);
        
    }
    public void DashOnceF()
    {
        StartCoroutine(DashOnce());
    }
    void squash()
    {
        if (renderer != null)
        {
            LeanTween.scaleX(renderer.gameObject, squashAmount.x, timeBeforeDash).setEase(tweenType);
            LeanTween.scaleY(renderer.gameObject, squashAmount.y, timeBeforeDash).setEase(tweenType).setOnComplete(setBackToNormal);
        }

    }
    void setBackToNormal()
    {
        if (renderer != null)
        {
            LeanTween.scaleX(renderer.gameObject, scaleNormal.x, 0.3f).setEase(tweenType);
            LeanTween.scaleY(renderer.gameObject, scaleNormal.y, 0.3f).setEase(tweenType);
        }
    }
    public void setTimeAfterDash(float value)
    {
        timeAfterDash = value;
    }
    public void setTimeAfterAfterDash(float value)
    {
        timeAfterAfterDash = value;
    }
    public void freezePositionRigidBody()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
    public void unfreezePositionRigidBody()
    {
        rb.constraints = RigidbodyConstraints2D.None;
    }

}
