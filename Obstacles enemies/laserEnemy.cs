using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class laserEnemy : MonoBehaviour
{
    [SerializeField] bool fireOnlyOnFunctions=false;
    [SerializeField] float timeBetweenShots;
    [SerializeField] float chargeUp;
    [SerializeField] Transform LaserTr;
    [SerializeField] LeanTweenType tweenTypeLaser;
    [SerializeField] float distanceMax = 120f;
    [SerializeField] LayerMask checkLayer;
    [SerializeField] float laserWidthOnFire = 1.4f;
    [SerializeField] public UnityEvent OnFire;
    [SerializeField] public UnityEvent OnAfterFire;
    [SerializeField] Animator animator;


    bool movHelp = false;
     RotateTowardsPoints rotateTowardsPoints;
    Rotate rotate;
    rotateAndMoveToPlayer _rotateAndMoveToPlayer;
    RotateTowardsPlayer rotateTowardsPlayer;
    Vector2 startingScaleLaser;
    [SerializeField] Vector2 chargeUpScaleLaser = Vector2.zero;
        [SerializeField] float squashDuration=0;
    [SerializeField] Vector2 squashAmount = Vector2.zero;
    [SerializeField] float stretchDuration=0f;
    [SerializeField] Vector2 stretchAmount = Vector2.zero;
    [SerializeField] GameObject tweenObjectCharge;
    [SerializeField] float returnToNormalDuration = 0f;

    Vector2 startScaleSprite;

    BoxCollider2D laserBoxCol;
    SpriteRenderer laserSprite;
    Color startingColor;
    AudioSource laserFire;
    AudioSource laserChargeUp;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent != null)
        {
            if(transform.GetComponentInParent<RotateTowardsPoints>())
            rotateTowardsPoints = transform.parent.GetComponent<RotateTowardsPoints>();
            if (transform.GetComponentInParent<Rotate>())
                rotate= transform.parent.GetComponent<Rotate>();
            if (transform.GetComponentInParent<rotateAndMoveToPlayer>())
                _rotateAndMoveToPlayer = transform.parent.GetComponent<rotateAndMoveToPlayer>();
            if (transform.GetComponentInParent<RotateTowardsPlayer>())
                rotateTowardsPlayer = transform.parent.GetComponent<RotateTowardsPlayer>();


        }

        laserBoxCol = LaserTr.GetComponent<BoxCollider2D>();
        laserSprite = LaserTr.GetComponent<SpriteRenderer>();
        startingColor = laserSprite.color;
        laserFire = LaserTr.GetComponent<AudioSource>();
        laserChargeUp = this.GetComponent<AudioSource>();
        startingScaleLaser = LaserTr.localScale;
        if(!fireOnlyOnFunctions)
        StartCoroutine(shootLaser());

        //these ifs are just so that I don't mess up already established enemies,chargeUpScaleLaser by default is zero  
        //
        if (chargeUpScaleLaser != Vector2.zero)
        {
            startingScaleLaser = chargeUpScaleLaser;
        }
        if (tweenObjectCharge != null)
        {
            startScaleSprite = tweenObjectCharge.transform.localScale;
        }
    }

    // Update is called once per frame

    IEnumerator shootLaser()
    {
        while (true)
        {
            laserBoxCol.enabled = false;
            laserSprite.color = startingColor;
            LaserTr.localScale = new Vector3(0f, LaserTr.localScale.y);
            yield return new WaitForSeconds(timeBetweenShots - chargeUp);
            if (animator != null) animator.SetBool(Animator.StringToHash("Shoot"), true);
            if (rotateTowardsPoints!=null)
            rotateTowardsPoints.StopRotating();
            if (rotate != null)
            rotate.StopRotating();
            if (_rotateAndMoveToPlayer != null) _rotateAndMoveToPlayer.StopRotating();
            if (rotateTowardsPlayer != null) rotateTowardsPlayer.StopRotating();

            setPositionOfLaser();
            laserChargeUp.Play();
            LeanTween.alpha(LaserTr.gameObject, 0.4f, chargeUp).setEase(tweenTypeLaser); ;
            LeanTween.scaleX(LaserTr.gameObject, startingScaleLaser.x, chargeUp).setEase(tweenTypeLaser);
        

            yield return new WaitForSeconds(chargeUp);


            OnFire.Invoke();
            setPositionOfLaser();
            laserFire.Play();
            if (animator != null) animator.SetBool(Animator.StringToHash("Shoot"), false);
            LaserTr.localScale = new Vector3(laserWidthOnFire, LaserTr.localScale.y);
            Color help = startingColor;
            help.a = 1f;
            laserSprite.color = help;
            laserBoxCol.enabled = true;
            yield return new WaitForSeconds(0.45f);
            OnAfterFire.Invoke();
            if (rotateTowardsPoints != null)
                rotateTowardsPoints.ContinueRotating();
            if (rotate != null)rotate.ContinueRotating();
            if (_rotateAndMoveToPlayer != null) _rotateAndMoveToPlayer.ContinueRotating();
            if (rotateTowardsPlayer != null) rotateTowardsPlayer.ContinueRotating();

            laserBoxCol.enabled = false;


        }
    }

    IEnumerator shootLaserOnce()
    {
        
            laserBoxCol.enabled = false;
            laserSprite.color = startingColor;
            LaserTr.localScale = new Vector3(0f, LaserTr.localScale.y);
            yield return new WaitForSeconds(timeBetweenShots - chargeUp);
            if (animator != null) animator.SetBool(Animator.StringToHash("Shoot"), true);
            if (rotateTowardsPoints != null)
                rotateTowardsPoints.StopRotating();
            if (rotate != null)
                rotate.StopRotating();
            if (_rotateAndMoveToPlayer != null) _rotateAndMoveToPlayer.StopRotating();
            if (rotateTowardsPlayer != null) rotateTowardsPlayer.StopRotating();

            setPositionOfLaser();
            laserChargeUp.Play();
            LeanTween.alpha(LaserTr.gameObject, 0.4f, chargeUp).setEase(tweenTypeLaser); ;
            LeanTween.scaleX(LaserTr.gameObject, startingScaleLaser.x, chargeUp).setEase(tweenTypeLaser);
        if (tweenObjectCharge != null)
            LeanTween.scale(tweenObjectCharge, squashAmount, squashDuration);

        yield return new WaitForSeconds(chargeUp);
        if (tweenObjectCharge != null)
            LeanTween.scale(tweenObjectCharge, stretchAmount, stretchDuration).setOnComplete(returnToNormalScale);

        OnFire.Invoke();
            setPositionOfLaser();
            laserFire.Play();
            if (animator != null) animator.SetBool(Animator.StringToHash("Shoot"), false);
            LaserTr.localScale = new Vector3(laserWidthOnFire, LaserTr.localScale.y);
            Color help = startingColor;
            help.a = 1f;
            laserSprite.color = help;
            laserBoxCol.enabled = true;
            yield return new WaitForSeconds(0.45f);
            OnAfterFire.Invoke();
            if (rotateTowardsPoints != null)
                rotateTowardsPoints.ContinueRotating();
            if (rotate != null) rotate.ContinueRotating();
            if (_rotateAndMoveToPlayer != null) _rotateAndMoveToPlayer.ContinueRotating();
            if (rotateTowardsPlayer != null) rotateTowardsPlayer.ContinueRotating();

            laserBoxCol.enabled = false;
        laserBoxCol.enabled = false;
        laserSprite.color = startingColor;
        LaserTr.localScale = new Vector3(0f, LaserTr.localScale.y);



    }
    public void shootTheLaserOnce()
    {
        StartCoroutine(shootLaserOnce()); ;

    }

    void setPositionOfLaser()
    {
        float distanceFromPoint;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, distanceMax, checkLayer);
        if (hit.point != Vector2.zero)
        {
            distanceFromPoint = Mathf.Clamp(Vector2.Distance(transform.position, hit.point), 0, distanceMax);
        }
        else
        {
            distanceFromPoint = distanceMax;
        }
        LaserTr.localPosition = new Vector2(0, 3.42f + distanceFromPoint / 2);
        LaserTr.localScale = new Vector2(LaserTr.localScale.x, distanceFromPoint / 2);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.up * 120f);
    }
    public void SetTimeBetweenShots(float value)
    {
        timeBetweenShots = value;
    }
    private void returnToNormalScale()
    {
        if (tweenObjectCharge != null)
            LeanTween.scale(tweenObjectCharge, startScaleSprite, returnToNormalDuration);
    }

}
