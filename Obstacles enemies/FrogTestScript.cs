using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogTestScript : MonoBehaviour
{
    [SerializeField] float timeBetweenJumps;
    [SerializeField] Vector2 randomJumpTimeOffsetMinMax;

    [SerializeField] float forceX;
    [SerializeField] float distanceYForceMultiplier;
    [SerializeField] float maxForceY;

    [SerializeField] Vector3 jumpDir;

    [SerializeField] Transform rightPosDirectionChange;
    [SerializeField] float rightCheckRadious;

    [SerializeField] Transform downPosJumpCheck;
    [SerializeField] float downPosJumpCheckRadious;

    bool turnOnGround = false;
    bool canJump = false;
    bool helperAnimBool = false;
    float offset = 0f;

    [Space(20)]
    [Header("Jump squashing and stretching")]
    [SerializeField] Animator animator;
    [SerializeField] float squashDuration;
    [SerializeField] Vector2 squashAmount=new Vector2(1.5f,0.5f);
    [SerializeField] float stretchDuration;
    [SerializeField] Vector2 stretchAmount = new Vector2(1.2f, 0.8f);

    [SerializeField] LeanTweenType tweenType;

    GameObject spriteHolder;
    bool turnedOnce=false;

    GameObject player;

    Rigidbody2D rb;
    ConstantForce2D constantForce;
    AudioSource jumpAudio;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<AudioSource>())  jumpAudio = GetComponent<AudioSource>();
        spriteHolder = animator.gameObject;
        offset = Random.Range(randomJumpTimeOffsetMinMax.x, randomJumpTimeOffsetMinMax.y);
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        turnedOnce = false;
        StartCoroutine(jump());


    }

    // Update is called once per frame
    void Update()
    {
        if(Physics2D.OverlapCircle(rightPosDirectionChange.position, rightCheckRadious))
        {
            turnOnGround = true;
           /* Vector3 scale = transform.localScale;
            transform.localScale=new Vector3(scale.x * -1, scale.y, scale.z);
            forceX = forceX * -1;
            */
        }
        if (Physics2D.OverlapCircle(downPosJumpCheck.position, downPosJumpCheckRadious))
        {
            if (canJump != helperAnimBool)
            {
                animator.SetBool(Animator.StringToHash("Jumping"), true);
                helperAnimBool = canJump;
            }
            canJump = true;
            if (turnOnGround&&!turnedOnce)
            {
                Vector3 scale = transform.localScale;
                transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
                forceX = forceX * -1;
                turnOnGround = false;
                turnedOnce = true;

            }
            
        }
        else { canJump = false;
            if (canJump != helperAnimBool)
            {
                animator.SetBool(Animator.StringToHash("Jumping"), false);
                helperAnimBool = canJump;
            }

        }

        //othervalueidk = transform.right;
    }

    Vector2 forceAmount()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
        float distance = Vector2.Distance(hit.point, transform.position);
        Vector2 forceByDistance = new Vector2( forceX,Mathf.Clamp(distance * distanceYForceMultiplier,0f,maxForceY));
        return forceByDistance;

    }
    IEnumerator jump()
    {



           

        while (true)
        {
            yield return new WaitForSeconds(timeBetweenJumps+ offset-squashDuration);


            if (Vector2.Distance(player.transform.position, transform.position) < 75f&&canJump) {

                squash();
                yield return new WaitForSeconds(squashDuration);
                stretch();
                //add so that it's constant to the distance of up wall
                Vector2 forceAmountSomething = forceAmount();

                Vector2 up = transform.up * forceAmountSomething.y;
                Vector2 right = transform.right * forceAmountSomething.x;
                jumpDir = up + right;

                rb.AddForce(jumpDir, ForceMode2D.Impulse);
                if(jumpAudio!=null) jumpAudio.Play();
                turnedOnce = false;
                /*
                //just trying new code this is the previous working version
                Vector2 up = transform.up * force.y;
                Vector2 right = transform.right * force.x;
                jumpDir = up + right;

                Debug.Log(up + "=up  " + right + "=right");
                rb.AddForce(jumpDir, ForceMode2D.Impulse);
                */

                // rb.AddRelativeForce(force, ForceMode2D.Impulse);
            }
        }
    }


    void squash()
    {
        spriteHolder.transform.localPosition-=new Vector3(0f, squashAmount.y, 0f);
        LeanTween.scaleX(spriteHolder, squashAmount.x, squashDuration).setEase(tweenType);
        LeanTween.scaleY(spriteHolder, squashAmount.y, squashDuration).setEase(tweenType);
    }
    void stretch()
    {
        LeanTween.scaleX(spriteHolder, stretchAmount.x, stretchDuration).setEase(tweenType);
        LeanTween.scaleY(spriteHolder, stretchAmount.y, stretchDuration).setEase(tweenType).setOnComplete(setBackToNormal);
    }
    void setBackToNormal()
    {
        spriteHolder.transform.localPosition += new Vector3(0f, squashAmount.y, 0f);
        LeanTween.scaleX(spriteHolder, 1f, 0.3f).setEase(tweenType);
        LeanTween.scaleY(spriteHolder, 1f, 0.3f).setEase(tweenType);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(rightPosDirectionChange.position, rightCheckRadious);
    }
}
