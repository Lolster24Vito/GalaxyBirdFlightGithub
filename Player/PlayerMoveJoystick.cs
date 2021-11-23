using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveJoystick : MonoBehaviour
{
    [System.Serializable]
    public class HighSpeed
    {
        public bool highSpeed=false;
        public float highSpeedTimeCounter=0f;
        public float slowSpeedTimeCounter=0f;
        public float speedAt;

        public HighSpeed(float speedAt1)
        {
            speedAt = speedAt1;
        }

    };

    [SerializeField] FloatingJoystick floatingJoystick;
    [SerializeField] ParticleSystem windFlapParticles;

    [SerializeField] ParticleSystem windLineParticles;
    [SerializeField] GameObject Speed2Particles;
    [SerializeField] ParticleSystem Speed2Particle1;
    [SerializeField] ParticleSystem Speed2Particle2;
    [SerializeField] GameObject Speed2Particle3Sprite;



    // [SerializeField] float windSpawnAtSpeed=21f;
    // [SerializeField] float ultraSpeedVelocity = 44f;


    [SerializeField] Transform groundCheckTr;
    [Header("Jump force parameters")]
    [SerializeField]public float jumpPower=8f;
    //pls remove public here


    [Header("Floating parameters")]
    [SerializeField] float minFloatXSpeed = 1.7f;
    [SerializeField] float maxFloatXSpeed = 40f;


    [Header("Floating SlowDown parameters")]
    [SerializeField] float timeTillMaxGravity = 3.83f;
    [SerializeField] float floatSlowDownXMultiplier = 2f;


    [Header("Floating Gravity parameters")]
    [SerializeField] float timeLowGravityAfterTouch=0.3f;
    [SerializeField] float lowGravityAmountAfterJump = 0.35f;

    [Range(-20, 10)] [SerializeField] float maxGravityWhileFloating = -7;
    [Range(100, -20)] [SerializeField] float gravityDirectionMultiplier = 5.5f;


    [SerializeField] Animator wingAnimator;
    [SerializeField] LayerMask wallLayer;

    [Header("Sound effects")]
    [SerializeField] AudioClip[] flapSound;

    [SerializeField] AudioClip collisionSound;

    [SerializeField] PhysicsMaterial2D physicsMat;
    private bool deleteMeVariableHelper = false;
    private AudioSource _audioSource;


    Rigidbody2D rb;
    LineRenderer lineRenderer;
    Vector2 originalVelocity;
    float floatingMultiplier = 0f;
    float floatingGravity = 0f;

    float refVelocityGravity;
    Vector2 refVelocityForVelocity;

    float refVelocityFloatXMultiplier;

    Vector2 refDirVel;



    Vector2 dir = Vector2.zero;


    Coroutine lowGravityEnumerator;
  
    bool AbleToMove = true;


    public static HighSpeed[] highspeed=new HighSpeed[]{
     new HighSpeed(21f),
     new HighSpeed(39.5f),
     new HighSpeed(48f),
     new HighSpeed(56f)
     };
    //[SerializeField] public static bool highSpeed = false;
    //[SerializeField] public static bool ultraSpeed = false;


   // private float highSpeedTimeCounter = 0f;
  //  private float slowSpeedTimeCounter = 0f;

   // private float ultraSpeedTimeCounter = 0f;
    //private float notUltraSpeedTimeCounter = 0f;


    [SerializeField] public float gravityScale=1f;
    //pls remove public here
    int audioHelperVar = 0;

    Vector3 localScale;
    [SerializeField] float scoreKojiZelim = 45f;
    [Header("Unique birds settings")]
    public bool flightlessBird = false;
    int jumpsFlightless = 1;
  [HideInInspector] public bool onWall = false;

    private  Vector2 velocitySave;
    private  float xPosTrailerSave;
    private  float xScaleTrailerSave;
    private  Quaternion rotationTrailerSave;


    Touch touch;
    // Start is called before the first frame update
    void Start()
    {








        highspeed[0].speedAt = 21f;
        highspeed[1].speedAt = 33f; //old 39.5
        highspeed[2].speedAt = 42f;   //old 48.5
        highspeed[2].speedAt = 50f;   //old 57.5

        Speed2Particle1=Speed2Particles.transform.GetChild(0).GetComponent<ParticleSystem>();
        Speed2Particle2 = Speed2Particles.transform.GetChild(1).GetComponent<ParticleSystem>();
        Speed2Particle3Sprite= Speed2Particles.transform.GetChild(2).gameObject;


        //something
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        _audioSource = this.GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && GetDirection(touch) != Vector2.zero)
        {
            dir = GetDirection(touch);
            lineRenderer.SetPosition(1, Vector3.right * dir.magnitude * (jumpPower / 2));
            if (!Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.7f), 0.25f, wallLayer))
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(dir.x * floatingMultiplier, floatingGravity + (dir.y * gravityDirectionMultiplier)), 0.1f);
                //rb.velocity = new Vector2(floatingMultiplier * dir.x, rb.velocity.y);
                floatingMultiplier = Mathf.Clamp(floatingMultiplier - (Time.fixedDeltaTime * floatSlowDownXMultiplier), minFloatXSpeed, maxFloatXSpeed);
                floatingGravity = Mathf.SmoothDamp(floatingGravity, maxGravityWhileFloating, ref refVelocityFloatXMultiplier, timeTillMaxGravity);
                //   floatingGravity = Mathf.Clamp(floatingGravity + (Time.deltaTime * floatSlowDownYmultiplier), maxGravityWhileFloating, 0);
                wingAnimator.SetBool(Animator.StringToHash("Float"), true);
                wingAnimator.SetBool(Animator.StringToHash("Ground"), false);
                wingAnimator.SetBool(Animator.StringToHash("Swing"), false);



            }
            RotateCharacter();

        }
    }
    // Update is called once per frame
    void Update()
    {
     /*   Debug.Log("Velocity:" + rb.velocity);
        Debug.Log("Velocity magnitude:" + rb.velocity.magnitude);
        Debug.Log("If statement maddness:" + (transform.position.y) + ">"+((PlayerScoreManager.score * 96f) - 0));
     */

        // Debug.Log("mag:"+rb.velocity.magnitude+" X:"+rb.velocity.x+" Y:"+rb.velocity.y);
        if (rb.velocity==Vector2.zero)
        {
            wingAnimator.SetBool(Animator.StringToHash("Ground"), true);
            wingAnimator.SetBool(Animator.StringToHash("Float"), false);
            wingAnimator.SetBool(Animator.StringToHash("Swing"), false);
            
        }
       
        
        if (rb.velocity.magnitude> highspeed[0].speedAt&& !windLineParticles.isPlaying)
        {
            windLineParticles.Play();
        }
        if (rb.velocity.magnitude < highspeed[0].speedAt && windLineParticles.isPlaying)
        {
            windLineParticles.Stop();
        }


        for(int i = 0; i < highspeed.Length; i++)
        {
            if (rb.velocity.magnitude > highspeed[i].speedAt&&Mathf.Abs(rb.velocity.y)>9.5f)
            {
                highspeed[i].highSpeedTimeCounter += Time.deltaTime;
                //                                                                                              before I subtracted 15 but that allowed for a infinite money glith so I lowered it down
                if (highspeed[i].highSpeedTimeCounter > 0.45f && (transform.position.y) > (PlayerScoreManager.score * 96f)-1f)
                {
                    
                    highspeed[i].highSpeed = true;
                    highspeed[i].slowSpeedTimeCounter = 0f;
                }
            }
            if (rb.velocity.magnitude < highspeed[i].speedAt)
            {
                highspeed[i].slowSpeedTimeCounter += Time.deltaTime;
                //origigi 0.9 sekundi 
                if (highspeed[i].slowSpeedTimeCounter > 1.7f)
                {
                    highspeed[i].highSpeed = false;
                    highspeed[i].highSpeedTimeCounter = 0f;
                }
            }

        }
        /*
        if (rb.velocity.magnitude > windSpawnAtSpeed)
        {
            highSpeedTimeCounter += Time.deltaTime;
            if (highSpeedTimeCounter > 0.45f&&(transform.position.y)>(PlayerScoreManager.score*96f)-15f)
            {

                highSpeed = true;
                slowSpeedTimeCounter = 0f;
            }
        }
        if (rb.velocity.magnitude < windSpawnAtSpeed)
        {
            slowSpeedTimeCounter += Time.deltaTime;
            if (slowSpeedTimeCounter > 0.9f)
            {
                highSpeed = false;
                highSpeedTimeCounter = 0f;
            }
        }


        if (rb.velocity.magnitude > ultraSpeedVelocity)
        {
            ultraSpeedTimeCounter += Time.deltaTime;
            if (ultraSpeedTimeCounter > 0.45f && (transform.position.y) > (PlayerScoreManager.score * 96f) - 15f)
            {

                ultraSpeed = true;
                notUltraSpeedTimeCounter = 0f;
            }
        }
        if (rb.velocity.magnitude < ultraSpeedVelocity)
        {
            notUltraSpeedTimeCounter += Time.deltaTime;
            if (notUltraSpeedTimeCounter > 0.9f)
            {
                ultraSpeed = false;
                ultraSpeedTimeCounter = 0f;
            }
        }
        */
        //Debug.Log(slowSpeedTimeCounter + " SLOW SPEED COUNTER");
        //Debug.Log(highSpeedTimeCounter + " HIGH SPEED COUNTER");


        
        if (highspeed[2].highSpeed)
        {
            if (!Speed2Particle1.isPlaying)
            {
                Speed2Particle1.Play();
                Speed2Particle2.Play();
            }
            if (!Speed2Particle3Sprite.activeSelf)
                Speed2Particle3Sprite.SetActive(true);

        }
        else
        {
            if (Speed2Particle1.isPlaying){
                Speed2Particle1.Stop();
                Speed2Particle2.Stop();
            }
            if (Speed2Particle3Sprite.activeSelf)
            {
                Speed2Particle3Sprite.SetActive(false);
            }
        }
        if(AbleToMove)
       if (Input.touchCount > 0)
        {
             touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                lineRenderer.SetPosition(1, Vector2.zero);
                lineRenderer.enabled = true;
                floatingMultiplier = rb.velocity.magnitude * 1.1f;
                floatingGravity = -1f;



            }
          
            if (touch.phase == TouchPhase.Ended)
            {
                
                   
                    if (highspeed[1].highSpeed)
                        PlayerAfterImage.Instance.Dash();
                    //SOUND
                    flapSoundEffect();

                    lineRenderer.enabled = false;
                    lowGravityEnumerator = StartCoroutine(setGravityScaleValues(lowGravityAmountAfterJump, gravityScale, timeLowGravityAfterTouch));
                    Vector2 dir = GetDirection(touch);
                    Vector2 velDirDifference = new Vector2(Mathf.Abs(dir.normalized.x - rb.velocity.normalized.x), Mathf.Abs(dir.normalized.y - rb.velocity.normalized.y));
                    if (velDirDifference.x > 1.1f || velDirDifference.y > 1.1f)
                    {
                    
                        rb.AddForce(GetDirection(touch) * jumpPower * 1.4f, ForceMode2D.Impulse);


                    }
                    else
                    {
                    if (!flightlessBird)
                    {
                        rb.AddForce(GetDirection(touch) * jumpPower, ForceMode2D.Impulse);
                    }
                    else if (flightlessBird)
                    {
                        if (onWall) {
                            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                            onWall = false;
                            rb.AddForce(GetDirection(touch) * jumpPower*4f, ForceMode2D.Impulse); 
                        }
                        else
                        {
                            rb.AddForce(GetDirection(touch) * jumpPower, ForceMode2D.Impulse);
                        }

                    }
                }
                    // GameObject part = Instantiate(particles, transform.position, Quaternion.Euler(new Vector3(180, 90, 90)));
                    // Destroy(part, 5);
                    windFlapParticles.Play();
                    wingAnimator.SetBool(Animator.StringToHash("Swing"), true);
                    wingAnimator.SetBool(Animator.StringToHash("Float"), false);
                    wingAnimator.SetBool(Animator.StringToHash("Ground"), false);


                }
                if (flightlessBird) jumpsFlightless++;
            

        }
    }

    public Vector2 GetDirection(Touch touch)
    {
        /*
        Vector2 cubePos = camera.WorldToScreenPoint(transform.position);
        Debug.Log((touch.position - cubePos).normalized);
        return (touch.position - cubePos).normalized;*/

        return new Vector2(floatingJoystick.Horizontal, floatingJoystick.Vertical);

    }

    IEnumerator setGravityScaleValues(float value1, float value2, float timeWait)
    {

        if (dir.y < 0)
        {
            rb.gravityScale = value1;
        }
        else
        {
            rb.gravityScale = -value1;
        }
        yield return new WaitForSeconds(timeWait);
        rb.gravityScale = value2;


    }
   
    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(new Vector2(transform.position.x, transform.position.y - 0.7f), 0.25f);
       // Gizmos.DrawSphere(transform.position, 3f);
    }
    void RotateCharacter()
    {
        if (dir.x > 0)
        {
            transform.eulerAngles = Vector3.forward * Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.localScale = new Vector3(localScale.x, localScale.y, 1);

        }
        if (dir.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180);
            transform.localScale = new Vector3(-localScale.x, localScale.y, 1);

        }
    }

    public void SlowTime(float howmuch)
    {
        Time.timeScale = howmuch;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
                  _audioSource.pitch = 1f;
                _audioSource.PlayOneShot(collisionSound);
                deleteMeVariableHelper = false;
        if (flightlessBird) { 
            jumpsFlightless = 0;
            rb.velocity = Vector2.zero;
            rb.constraints= RigidbodyConstraints2D.FreezeAll;
            onWall = true;

        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (flightlessBird)
        {
            if(collision.collider.gameObject.layer==10|| collision.collider.gameObject.layer == 15) { 
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                onWall = false;

            }
            //


        }
    }
    public void stopMoving()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

    }
    public void startMovingNormally()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    public void stopMovingCinematic()
    {
        AbleToMove = false;
        stopMoving();

    }
    public void continueMovingCinematic()
    {
        AbleToMove = true;
        startMovingNormally();

    }
    private void flapSoundEffect()
    {
        // float speedPercentage = rb.velocity.magnitude / 25f;
        //  _audioSource.pitch = Mathf.LerpUnclamped(0.8f, 1.3f, speedPercentage);
        if (audioHelperVar >= flapSound.Length)
        {
            audioHelperVar = 0;
        }

            _audioSource.PlayOneShot(flapSound[audioHelperVar]);

        audioHelperVar += 1;

        /*
        int roundDir = Mathf.RoundToInt(dir.y);

        if (roundDir == 1)
        {
            _audioSource.PlayOneShot(jumpHighSound);
        }
        if (roundDir == -1)
        {

            _audioSource.PlayOneShot(jumpLowSound);
        }
        if (roundDir == 0)
        {

            _audioSource.PlayOneShot(jumpMidSound);

        }
        */


    }
    public void setStatsOfSkin(SkinItem skin)
    {
        if (PlayerPrefs.GetInt("Checkpoint", 0) == 0)
        {
            jumpPower = skin.startingJumpPower;
            gravityScale = skin.startingAirTime;
        }
        else
        {
            jumpPower = SaveLoad.Instance.checkpoint.jumpPower;
            gravityScale = SaveLoad.Instance.checkpoint.gravityScale;
        }
        minFloatXSpeed = skin.minFloatXSpeed;
        maxFloatXSpeed = skin.maxFloatXSpeed;
        timeTillMaxGravity = skin.timeTillMaxGravity;
        floatSlowDownXMultiplier = skin.floatSlowDownXMultiplier;
        timeLowGravityAfterTouch = skin.timeLowGravityAfterTouch;
        lowGravityAmountAfterJump = skin.lowGravityAmountAfterJump;
        maxGravityWhileFloating = skin.maxGravityWhileFloating;
        gravityDirectionMultiplier = skin.gravityDirectionMultiplier;
        wallLayer = skin.wallLayer;

        if (skin.physicsMaterial != null)
        {
            GetComponent<BoxCollider2D>().sharedMaterial = skin.physicsMaterial;
        }
        else
        {
            GetComponent<BoxCollider2D>().sharedMaterial = physicsMat;
        }
        flightlessBird = skin.flightlessBird;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public void AddSpeed()
    {
        jumpPower += 1.09f;
        SaveLoad.Instance.checkpoint.jumpPower = jumpPower;

    }
    public void AddGravity()
    {
        gravityScale -= 0.153f;
        SaveLoad.Instance.checkpoint.gravityScale = gravityScale;

    }

    public void testMovePlayerTo15()
    {
        StartCoroutine(movePlayerToScore());
    }
    public void setScoreKojiZelim()
    {
        scoreKojiZelim = 435;
    }
    IEnumerator movePlayerToScore()
    {

        Time.timeScale = 1.1f;
        GetComponent<BoxCollider2D>().enabled = false;
        Vector3 origPos = transform.position;
        float speed = 10f;
        float scoreIncreaser = 0;
        while (scoreIncreaser < scoreKojiZelim)
        {
            scoreIncreaser += Time.deltaTime * speed;
            transform.position = new Vector3(0, origPos.y + (scoreIncreaser * 96f));
            yield return new WaitForEndOfFrame();
        }

        GetComponent<BoxCollider2D>().enabled = true;
        Time.timeScale = 1f;

    }

    public void trailerDebugSaveScreenPosition()
    {
      velocitySave=rb.velocity;
      xPosTrailerSave= transform.position.x;
        xScaleTrailerSave = transform.localScale.x ;
        rotationTrailerSave = transform.rotation;

}

   public void trailerDebugLoadScreenPosition()
    {
       rb.velocity = velocitySave;
        transform.position = new Vector2(xPosTrailerSave, transform.position.y);
       transform.localScale=new Vector2(xScaleTrailerSave,transform.localScale.y);
        transform.rotation = rotationTrailerSave;

    }

}
