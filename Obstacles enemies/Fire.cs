using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] bool fireOnlyOnFunctions = false;
    [SerializeField] GameObject bullet;
    [SerializeField] float spawnRateSeconds = 1f;
    [SerializeField] float offset = 0f;
    [SerializeField] Transform[] SpawnPosition;
    [SerializeField] int poolSize=5;
    [SerializeField] bool shootAtSameTimeAtPos = true;
    [SerializeField] string shootAnimatorVar ="Shooting";
    [SerializeField] float shootAnimationSecondsOffset = 0.0f;

    [Space(15)]
    [Header("LeanTween")]
    [SerializeField] GameObject spriteHolder;
    [SerializeField] float squashDuration;
    [SerializeField] Vector2 squashAmount = new Vector2(1.5f, 0.5f);
    [SerializeField] float stretchDuration;
    [SerializeField] Vector2 stretchAmount = new Vector2(1.2f, 0.8f);
    [SerializeField] LeanTweenType tweenType;
    [SerializeField] Color squashColor=Color.white;
    Vector2 originalScale;
    Color originalColor;
    

[SerializeField] bool movingEnemy = false;
[SerializeField] bool poolOutsideOfGameObject = false;
    GameObject parentOutside;
    [SerializeField] int numbOfParents = 1;
    private Transform parentPool;


    ParticleSystem particlesFire;

    GameObject[] _bulletPool;
  [SerializeField]  Animator animator=null;
    int _poolCounter = 0;

    int numberOfChildren = 0;

    AudioSource fireAudio;
    float timeOffset;

    Rotate rotate;
    RotateTowardsPlayer rotateTowardsPlayer;
    RotateTowardsPoints rotateTowardsPoints;

  
    // Start is called before the first frame update
    void Start()
    {

        if (offset == 0f)
        {
            timeOffset = Random.Range(0f, 2f);
        }
        else { timeOffset = offset; }

        if (GetComponent<AudioSource>() != null)
        {
            fireAudio = GetComponent<AudioSource>();
        }
        if (movingEnemy)
        {
            //ok so this is here because it more cost effective to just put a empty gameobject into the prefab then to worry about creating new gameobjects
            parentPool = GetLastParentOfTr(transform);
           // parentPool = transform.parent;
        }
        if (poolOutsideOfGameObject)
        {
            parentOutside = new GameObject("Hey guys I'm the keeper of the bullets");
            parentPool = parentOutside.transform;
        }
        numberOfChildren = transform.childCount;
        if (GetComponent<ParticleSystem>())
        {
            particlesFire = GetComponent<ParticleSystem>();
        }
        _bulletPool = new GameObject[poolSize];
        for (int i = 0; i < _bulletPool.Length; i++)
            {

            if (movingEnemy)
            {
                _bulletPool[i] = Instantiate(bullet, parentPool);
            }
            else
            {
                _bulletPool[i] = Instantiate(bullet, transform);
            }
                _bulletPool[i].SetActive(false);
            }

        if (spriteHolder != null)
        {
            originalScale = spriteHolder.transform.localScale;
            if (animator == null)
            {
                if (spriteHolder.transform.GetComponent<Animator>())
                {
                    animator = spriteHolder.transform.GetComponent<Animator>();
                }
            }
            if (spriteHolder.GetComponent<SpriteRenderer>())
            {
                originalColor = spriteHolder.GetComponent<SpriteRenderer>().color;
            }
        }
        if (transform.parent != null)
        {
            if (transform.GetComponentInParent<Rotate>())
            {
                rotate = transform.GetComponentInParent<Rotate>();
            }
            if (transform.GetComponentInParent<RotateTowardsPoints>())
            {
                rotateTowardsPoints = transform.GetComponentInParent<RotateTowardsPoints>();
            }

        }
        if(!fireOnlyOnFunctions)
        StartCoroutine(fireBullet());
    }

    // Update is called once per frame
    IEnumerator fireBullet()
    {
        yield return new WaitForSeconds(timeOffset);
        int spawnPositionIndex=0;
        while (true)
        {
            if (rotate != null) rotate.StopRotating();
            if (rotateTowardsPoints != null) rotateTowardsPoints.StopRotating();
            squash();
            yield return new WaitForSeconds(squashDuration);
            if (animator != null)
            {
                animator.SetBool(Animator.StringToHash(shootAnimatorVar), true);
                yield return new WaitForSeconds(shootAnimationSecondsOffset);
            }

            stretch();
            if (shootAtSameTimeAtPos)
            {
                for (int i = 0; i < SpawnPosition.Length; i++)
                {
                    if (_bulletPool[i] != null)
                    {
                        _bulletPool[i].SetActive(false);
                        if(_bulletPool[i].GetComponent<Rigidbody2D>()!=null)
                        _bulletPool[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    }

                    if (_bulletPool[_poolCounter] == null)
                    {
                        _bulletPool[_poolCounter] = Instantiate(bullet, transform);
                        _bulletPool[_poolCounter].SetActive(false);
                    }
                    _bulletPool[_poolCounter].transform.position = SpawnPosition[i].position;
                    _bulletPool[_poolCounter].transform.rotation = SpawnPosition[i].rotation;
                    if (_bulletPool[_poolCounter].GetComponent<TrailRenderer>()) StartCoroutine(ResetTrail(_bulletPool[_poolCounter].GetComponent<TrailRenderer>()));
                    _bulletPool[_poolCounter].SetActive(true);

                    if (fireAudio != null)
                    {
                        fireAudio.Play();
                    }

                    if (_poolCounter < _bulletPool.Length - 1)
                        _poolCounter++;
                    else
                        _poolCounter = 0;
                }
            }
            else
            {
                if (spawnPositionIndex >= SpawnPosition.Length)
                {
                    //("restarted value");
                    spawnPositionIndex = 0;
                }

                //ok so I instantiate again just in case if the bullet got destroyed by some obstacle
                if (_bulletPool[spawnPositionIndex] != null)
                {
                    _bulletPool[spawnPositionIndex].SetActive(false);
                    if(_bulletPool[spawnPositionIndex].GetComponent<Rigidbody2D>()!=null)
                    _bulletPool[spawnPositionIndex].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }

                if (_bulletPool[_poolCounter] == null)
                {
                    _bulletPool[_poolCounter] = Instantiate(bullet, transform);
                    _bulletPool[_poolCounter].SetActive(false);
                }
                _bulletPool[_poolCounter].transform.position = SpawnPosition[spawnPositionIndex].position;
                _bulletPool[_poolCounter].transform.rotation = SpawnPosition[spawnPositionIndex].rotation;
                if (_bulletPool[_poolCounter].GetComponent<TrailRenderer>()) StartCoroutine(ResetTrail(_bulletPool[_poolCounter].GetComponent<TrailRenderer>()));
                _bulletPool[_poolCounter].SetActive(true);

                if (fireAudio != null)
                {
                    fireAudio.Play();
                }

                if (_poolCounter < _bulletPool.Length - 1)
                    _poolCounter++;
                else
                    _poolCounter = 0;

                spawnPositionIndex++;
               

            }
            if (rotate != null) rotate.ContinueRotating();
            if (rotateTowardsPoints != null) rotateTowardsPoints.ContinueRotating();

            if (particlesFire != null) particlesFire.Play();
            yield return new WaitForSeconds(0.35f);
            if(animator!=null)
            animator.SetBool(Animator.StringToHash(shootAnimatorVar), false);
            
            yield return new WaitForSeconds(spawnRateSeconds-squashDuration-0.35f- shootAnimationSecondsOffset);

        }
    }
    IEnumerator fireBulletOnce()
    {
        yield return new WaitForSeconds(timeOffset);
        int spawnPositionIndex = 0;

            if (rotate != null) rotate.StopRotating();
            if (rotateTowardsPoints != null) rotateTowardsPoints.StopRotating();
            squash();
            yield return new WaitForSeconds(squashDuration);
            if (animator != null)
            {
                animator.SetBool(Animator.StringToHash(shootAnimatorVar), true);
                yield return new WaitForSeconds(shootAnimationSecondsOffset);
            }

            stretch();
            if (shootAtSameTimeAtPos)
            {
                for (int i = 0; i < SpawnPosition.Length; i++)
                {
                    if (_bulletPool[i] != null)
                    {
                        _bulletPool[i].SetActive(false);
                        if (_bulletPool[i].GetComponent<Rigidbody2D>() != null)
                            _bulletPool[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    }

                    if (_bulletPool[_poolCounter] == null)
                    {
                        _bulletPool[_poolCounter] = Instantiate(bullet, transform);
                        _bulletPool[_poolCounter].SetActive(false);
                    }
                    _bulletPool[_poolCounter].transform.position = SpawnPosition[i].position;
                    _bulletPool[_poolCounter].transform.rotation = SpawnPosition[i].rotation;
                    if (_bulletPool[_poolCounter].GetComponent<TrailRenderer>()) StartCoroutine(ResetTrail(_bulletPool[_poolCounter].GetComponent<TrailRenderer>()));
                    _bulletPool[_poolCounter].SetActive(true);

                    if (fireAudio != null)
                    {
                        fireAudio.Play();
                    }

                    if (_poolCounter < _bulletPool.Length - 1)
                        _poolCounter++;
                    else
                        _poolCounter = 0;
                }
            }
            else
            {
                if (spawnPositionIndex >= SpawnPosition.Length)
                {
                    //("restarted value");
                    spawnPositionIndex = 0;
                }

                //ok so I instantiate again just in case if the bullet got destroyed by some obstacle
                if (_bulletPool[spawnPositionIndex] != null)
                {
                    _bulletPool[spawnPositionIndex].SetActive(false);
                    if (_bulletPool[spawnPositionIndex].GetComponent<Rigidbody2D>() != null)
                        _bulletPool[spawnPositionIndex].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }

                if (_bulletPool[_poolCounter] == null)
                {
                    _bulletPool[_poolCounter] = Instantiate(bullet, transform);
                    _bulletPool[_poolCounter].SetActive(false);
                }
                _bulletPool[_poolCounter].transform.position = SpawnPosition[spawnPositionIndex].position;
                _bulletPool[_poolCounter].transform.rotation = SpawnPosition[spawnPositionIndex].rotation;
                if (_bulletPool[_poolCounter].GetComponent<TrailRenderer>()) StartCoroutine(ResetTrail(_bulletPool[_poolCounter].GetComponent<TrailRenderer>()));
                _bulletPool[_poolCounter].SetActive(true);

                if (fireAudio != null)
                {
                    fireAudio.Play();
                }

                if (_poolCounter < _bulletPool.Length - 1)
                    _poolCounter++;
                else
                    _poolCounter = 0;

                spawnPositionIndex++;


            }
            if (rotate != null) rotate.ContinueRotating();
            if (rotateTowardsPoints != null) rotateTowardsPoints.ContinueRotating();

            if (particlesFire != null) particlesFire.Play();
            yield return new WaitForSeconds(0.35f);
            if (animator != null)
                animator.SetBool(Animator.StringToHash(shootAnimatorVar), false);

            yield return new WaitForSeconds(spawnRateSeconds - squashDuration - 0.35f - shootAnimationSecondsOffset);

        
    }



    void squash()
    {
        if (spriteHolder != null)
        {
            LeanTween.color(spriteHolder, squashColor, squashDuration).setEase(tweenType);
            spriteHolder.transform.localPosition -= new Vector3(0f, squashAmount.y, 0f);
        LeanTween.scaleX(spriteHolder, squashAmount.x, squashDuration).setEase(tweenType);
        LeanTween.scaleY(spriteHolder, squashAmount.y, squashDuration).setEase(tweenType);
        }
    }
    void stretch()
    {
        if (spriteHolder != null)
        {
            LeanTween.color(spriteHolder, originalColor, stretchDuration).setEase(tweenType);
            
            spriteHolder.transform.localPosition += new Vector3(0f, squashAmount.y, 0f);
            LeanTween.scaleX(spriteHolder, stretchAmount.x, stretchDuration).setEase(tweenType);
            LeanTween.scaleY(spriteHolder, stretchAmount.y, stretchDuration).setEase(tweenType).setOnComplete(setBackToNormal);
        }
    }
    void setBackToNormal()
    {
        LeanTween.scaleX(spriteHolder, originalScale.x, 0.3f).setEase(tweenType);
        LeanTween.scaleY(spriteHolder, originalScale.y, 0.3f).setEase(tweenType);

    }

     IEnumerator ResetTrail(TrailRenderer trail)
    {
        trail.gameObject.SetActive(false);
        var trailTime = trail.time;
        trail.time = 0;
        yield return new WaitForEndOfFrame();
        trail.time = trailTime;
        trail.gameObject.SetActive(true);

    }
    private Transform GetLastParentOfTr(Transform tr)
    {
        int crashBreaker = 0;
        if (transform.parent == null) { return tr; }
        Transform test = transform.parent;
        while (test != null && crashBreaker < numbOfParents)
        {
            crashBreaker++;
            if (test.parent != null)
            {
                test = test.parent;
            }
            else
            {
                break;
            }
        }
        return test;
    }
    public void SetSpawnRateSeconds(float value)
    {
        spawnRateSeconds = value;
    }

    public void FireOnlyOnce()
    {
        StartCoroutine(fireBulletOnce());
    }
    private void OnDisable()
    {
        if (poolOutsideOfGameObject)
            parentOutside.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        if(poolOutsideOfGameObject)
        Destroy(parentOutside);

    }
}
