using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullScript : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] Transform rightTurnCheckPos;
    [SerializeField] float rightTurnCheckRadious;

    [SerializeField] Transform rightFloorCheckPos;
    [SerializeField] float rightFloorCheckRad;
    [SerializeField] LayerMask rightSideWallLayer;


    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRad;
    [SerializeField] LayerMask groundCheckWallLayer;


    [SerializeField] float waitTime=1f;
    [Space(20)]

    [SerializeField] LayerMask playerLayer;
    [SerializeField] float rushModeSpeedMultiplier;
    [SerializeField] float rushModeTime;
    [SerializeField] float rushModeDelay;

    [SerializeField] float rushModeRadius;
    [SerializeField] Color rushModeColor;

    [Space(20)]
    [SerializeField] float squashTime;
    [SerializeField] float normalTime;

    [SerializeField] Vector2 squashAmount;
    [SerializeField] Vector2 stretchAmount;

    [SerializeField] GameObject spriteHolder;
    [SerializeField] LeanTweenType tweenType;

    [SerializeField] float dot;
    ParticleSystem smokeAngryParticles;
    Transform Player;
    Coroutine coroutine =null;
    bool _rushMode;
    float speedHelperKeeper;
    WallGravity wallGravity;
    Animator animator;

    Rigidbody2D rb;
    AudioSource runningAudio;
    [SerializeField] AudioSource rushAudio;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        wallGravity = GetComponent<WallGravity>();
        smokeAngryParticles = GetComponent<ParticleSystem>();
        animator = spriteHolder.GetComponent<Animator>();
        runningAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(Player.position, transform.position) < 70f&&!_rushMode)
        {
             dot = Vector2.Dot(transform.right * transform.localScale.x, (transform.position - Player.position).normalized);

            if (dot < -0.985f)
            {                       //origin,direction,distance,layermask
                 RaycastHit2D hit = Physics2D.CircleCast(transform.position + transform.right,rushModeRadius, transform.right*transform.localScale.x, 100f, playerLayer);
                // Physics2D.CircleCast()
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        spriteHolder.GetComponent<SpriteRenderer>().color = rushModeColor;
                        _rushMode = true;
                        StopAllCoroutines();
                        speed = speedHelperKeeper;
                        coroutine = null;
                        StartCoroutine(rushMode());
                    }

                }
            }
        }

        Vector2 right = transform.right * speed;
        //rb.velocity=right;
        //rb.position = new Vector2(transform.position.x, transform.position.y) + (right * speed * Time.fixedDeltaTime);
        // rb.MovePosition(transform.position + (transform.right * speed ));

        //if isn't in air
        if (Physics2D.OverlapCircle(groundCheck.position,groundCheckRad, groundCheckWallLayer))
        {

            // rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
            rb.velocity = transform.right * speed;

            if (runningAudio != null && !runningAudio.isPlaying&&coroutine==null) runningAudio.Play();

            if (!_rushMode)
            {
                if (Physics2D.OverlapCircle(rightTurnCheckPos.position, rightTurnCheckRadious, rightSideWallLayer) || !Physics2D.OverlapCircle(rightFloorCheckPos.position, rightFloorCheckRad, rightSideWallLayer))
                {
                    rb.velocity = Vector2.zero;
                    if (coroutine == null)
                    {
                        coroutine = StartCoroutine(waitAfterSeeingWall());
                    }





                }
            }
            else
            {
               // wallGravity.SetGravityDirection();
            }
        }
        else
        {
            //if is in air 

            if(runningAudio!=null)runningAudio.Stop();
        }


    }
    private void OnDrawGizmos()
    {
          Gizmos.DrawSphere(rightTurnCheckPos.position, rightTurnCheckRadious);
          Gizmos.DrawSphere(rightFloorCheckPos.position, rightFloorCheckRad);
          Gizmos.DrawSphere(groundCheck.position, groundCheckRad);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + transform.right, transform.right*100f*transform.localScale.x );
                     //   RaycastHit2D hit = Physics2D.CircleCast(transform.position + transform.right, 0.3f, transform.right, 100f, playerLayer);


    }
    IEnumerator rushMode()
    {
        animator.SetBool(Animator.StringToHash("Running"), false);
        speedHelperKeeper = speed;
        speed = 0;
        squashRush();
        smokeAngryParticles.Play();
        yield return new WaitForSeconds(rushModeDelay);
        rushAudio.Play();
        animator.SetBool(Animator.StringToHash("Running"), true);
        speed = speedHelperKeeper;
        speed = speed * rushModeSpeedMultiplier;
        yield return new WaitForSeconds(rushModeTime);
        speed = speed / rushModeSpeedMultiplier;
        _rushMode = false;
        spriteHolder.GetComponent<SpriteRenderer>().color = Color.white;

    }
    IEnumerator waitAfterSeeingWall()
    {
        runningAudio.Stop();
        animator.SetBool(Animator.StringToHash("Running"), false);
        speed = speed * -1;
        speedHelperKeeper = speed;
        speed = 0;

        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
        yield return new WaitForSeconds(waitTime-squashTime);
        squash();
        yield return new WaitForSeconds(squashTime);
        animator.SetBool(Animator.StringToHash("Running"), true);
        speed = speedHelperKeeper;
       // runningAudio.Play();
        coroutine = null;

    }

    void squash()
    {
        LeanTween.scaleX(spriteHolder, squashAmount.x, squashTime).setEase(tweenType);
        LeanTween.scaleY(spriteHolder, squashAmount.y, squashTime).setEase(tweenType).setOnComplete(stretch);

    }
    void squashRush()
    {
        LeanTween.scaleX(spriteHolder, squashAmount.x, rushModeDelay).setEase(tweenType);
        LeanTween.scaleY(spriteHolder, squashAmount.y, rushModeDelay).setEase(tweenType).setOnComplete(returnToNormal);

    }

    void stretch()
    {
        LeanTween.scaleX(spriteHolder, stretchAmount.x, squashTime).setEase(tweenType);
        LeanTween.scaleY(spriteHolder, stretchAmount.y, squashTime).setEase(tweenType).setOnComplete(returnToNormal);

    }
    void returnToNormal()
    {
        LeanTween.scaleX(spriteHolder, 1, normalTime).setEase(tweenType);
        LeanTween.scaleY(spriteHolder, 1, normalTime).setEase(tweenType);

    }


}
