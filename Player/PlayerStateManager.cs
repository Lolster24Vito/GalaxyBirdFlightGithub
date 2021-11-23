using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


[System.Serializable]
public struct Prices
{
    public int startingPrice;
    public int priceAddAfterBuying;
    public int currentPrice;
    public int maxPrice;
}

public class PlayerStateManager : MonoBehaviour
{
    [System.Serializable]
    public class HighSpeedCoins
    {
        [SerializeField] public GameObject extraCoinUI;
        [SerializeField] public TextMeshProUGUI extraCoinText;
        [SerializeField] public float coinPerSecondHighSpeed;
        [HideInInspector]public int extraHighSpeedCoins =0 ;
        [HideInInspector] public float extraHighSpeedCoinsTimer = 0f;
        [HideInInspector] public bool highSpeedCoins = false;
    }
    private GameObject player;
    private Rigidbody2D playerRB;
    public static PlayerStateManager Instance;
    public GameObject DeathScreen;
    public Animator hitEffect;
    [SerializeField] Transform coinParticleTransform;
    [SerializeField] Transform heartParticleTransform;
    [SerializeField] Transform starParticleTransform;
    [SerializeField] AudioClip starPowerUpAud;
    [SerializeField] AudioClip starPowerDownAud;




    AudioSource coinAudio;
    ParticleSystem coinParticle;

    ParticleSystem starParticle;
    AudioSource starAudio;

    ParticleSystem heartParticle;
    AudioSource heartAudio;

    [SerializeField] TextMeshProUGUI coinText;
   // [SerializeField] GameObject extraCoinUI;
   // [SerializeField] GameObject extraCoinUI2;
   // [SerializeField] TextMeshProUGUI extraCoinText;
   // [SerializeField] TextMeshProUGUI extraCoinText2;
    [SerializeField] Image killedByImage;
    [SerializeField] GameObject tutorialUI;


   // int extraHighSpeedCoins = 0;
   // float extraHighSpeedCoinsTimer = 0;


   // int extraUltraSpeedCoins = 0;
   // float extraUltraSpeedCoinsTimer = 0;
  //  [SerializeField] float coinPerSecondHighSpeed = 2f;
  //  [SerializeField] float coinPerSecondUltraSpeed = 2f;



    [SerializeField] TextMeshProUGUI heartText;

    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip hitSound;


    [SerializeField] AudioClip goldCoinAud;
    [SerializeField] AudioClip silverCoinAud;
    [SerializeField] AudioClip bronzeCoinAud;
    [SerializeField] AudioClip diamondCoinAud;


    [Range(0,1)][SerializeField] float slowMoDeathAmount;
    [Range(0, 5)] [SerializeField] float slowMoDeathDuration;
    [Space(10)]
    [Header("ShopPrices",order =0)]
    [Space(10)]
    [Header("SpeedPrice", order = 1)]
    [SerializeField] public Prices SpeedPrice;
    [Header("HealthPrice")]
    [SerializeField] public Prices HealthPrice;
    [Header("GravityPrice")]
    [SerializeField] public Prices GravityPrice;

    PlayerMoveJoystick playerMove;

    private Coroutine starPowerUpCor;

   [SerializeField] GameOverMap gameoverMap;
    bool dead = false;


    [SerializeField] HighSpeedCoins[] highSpeedCoins = new HighSpeedCoins[4];
    //bool highSpeedCoins = false;
    //bool ultraSpeedCoins = false;

    public static int coinsStartingFromZero;
    public static int health=1;
    [Space(20)]
    [SerializeField] LayerMask unKillableLayer;
    [SerializeField] float unKillableDuration;

    [Space(20)]
    [Header("Skins")]
    [SerializeField] public SkinItem currentSkin;
    [SerializeField] public SpriteRenderer body;
    [SerializeField] public SpriteRenderer wings;

    public int startRunCoins = 0;
    public int bonusCoinsForPlanets = 0;
    bool starPowerUped = false;
  


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        player = this.gameObject;
        playerRB = player.GetComponent<Rigidbody2D>();
       // highSpeedCoins = false;
       // extraHighSpeedCoinsTimer = 0;
    }
    private void Start()
    {
        coinAudio = coinParticleTransform.GetComponent<AudioSource>();
        coinParticle = coinParticleTransform.GetComponent<ParticleSystem>();
        starParticle= starParticleTransform.GetComponent<ParticleSystem>();
        starAudio = starParticleTransform.GetComponent<AudioSource>();
        heartParticle = heartParticleTransform.GetComponent<ParticleSystem>();
        heartAudio = heartParticleTransform.GetComponent<AudioSource>();

        //Only for testing
        //  PlayerPrefs.SetInt("Coins",27000);
        playerMove = this.GetComponent<PlayerMoveJoystick>();
        coinsStartingFromZero =0;
        if (PlayerPrefs.GetInt("ExtraLife", 0)==1)
        {
            coinsStartingFromZero = 120;
        }
        startRunCoins = PlayerPrefs.GetInt("Coins");

        if (PlayerPrefs.GetInt("Checkpoint", 0) == 0)
        {
            health = 1;
            SpeedPrice.currentPrice = SpeedPrice.startingPrice;
            HealthPrice.currentPrice = HealthPrice.startingPrice;
            GravityPrice.currentPrice = GravityPrice.startingPrice;
        }
        else
        {
            CheckpointObject checkpoint = SaveLoad.Instance.checkpoint;
            coinsStartingFromZero = checkpoint.coinsStartingFromZero;
            startRunCoins = checkpoint.startRunCoins;
            SpeedPrice.currentPrice = checkpoint.SpeedPrice.currentPrice;
            HealthPrice.currentPrice = checkpoint.HealthPrice.currentPrice;
            GravityPrice.currentPrice = checkpoint.GravityPrice.currentPrice;
            health = checkpoint.hearts;
        }

        heartText.text = health.ToString();

        coinText.text = coinsStartingFromZero.ToString();
       // PlayerPrefs.SetInt("Coins", coinsStartingFromZero + startRunCoins);


        if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 1)
        {
            tutorialUI.SetActive(true);
            PlayerPrefs.SetInt("FirstTimePlaying", 0);
        }

        if (AdsManager.Instance != null)
        {
           AdsManager.Instance.RequestIfNotLoaded();
        }

        

    }
    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (PlayerMoveJoystick.highspeed[i].highSpeed && !highSpeedCoins[i].highSpeedCoins)
            {
                highSpeedCoins[i].highSpeedCoins = true;
                highSpeedCoins[i].extraCoinUI.SetActive(true);
            }
            if (!PlayerMoveJoystick.highspeed[i].highSpeed && highSpeedCoins[i].highSpeedCoins)
            {
                AddCoin(highSpeedCoins[i].extraHighSpeedCoins);
                highSpeedCoins[i].highSpeedCoins = false;
                highSpeedCoins[i].extraCoinUI.SetActive(false);
                highSpeedCoins[i].extraHighSpeedCoins = 0;
                highSpeedCoins[i].extraCoinText.text = highSpeedCoins[i].extraHighSpeedCoins.ToString();


            }
            if (highSpeedCoins[i].highSpeedCoins)
            {
                highSpeedCoins[i].extraHighSpeedCoinsTimer += Time.deltaTime;
                if (highSpeedCoins[i].extraHighSpeedCoinsTimer > highSpeedCoins[i].coinPerSecondHighSpeed && !dead)
                {
                    highSpeedCoins[i].extraHighSpeedCoins += 1;
                    highSpeedCoins[i].extraCoinText.text = highSpeedCoins[i].extraHighSpeedCoins.ToString();
                    highSpeedCoins[i].extraHighSpeedCoinsTimer = 0;
                }
            }

        }


    }
    public void setKilledByImage(Sprite obstacle)
    {
        killedByImage.sprite = obstacle;
    }
    public  void Death()
    {
        for(int i = 0; i < highSpeedCoins.Length; i++)
        {
            highSpeedCoins[i].highSpeedCoins = false;
        }
        health-=1;
        if(SaveLoad.Instance.checkpoint!=null)
        SaveLoad.Instance.checkpoint.hearts = health;
        heartText.text = health.ToString();
        if (playerMove.flightlessBird)
        {
            playerMove.onWall = false;
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;

        }
        if (health <= 0)
        {

            if (!dead)
            {
                PlayerPrefs.SetInt("Checkpoint", 0);
                SaveLoad.Instance.deleteCheckpoint();
                Die();
                
            }
        }
        if (health > 0)
        {
            this.GetComponent<AudioSource>().PlayOneShot(hitSound);
            hitEffect.SetTrigger(Animator.StringToHash("Hit"));
            StartCoroutine(slowDownTime(slowMoDeathDuration));
            StartCoroutine(unkillablePlayer(unKillableDuration));


        }





    }
    IEnumerator slowDownTime(float seconds)
    {

        Time.timeScale = slowMoDeathAmount;
        yield return new WaitForSeconds(seconds);
        Time.timeScale = 1f;

        //DeathScreen.SetActive(true);
        //DeathScreen.GetComponent<Animator>().SetBool("IsActive", true);
    }
    IEnumerator unkillablePlayer(float seconds)
    {

        //UnKillableLayer
        gameObject.layer=13;
        yield return new WaitForSecondsRealtime(seconds);
        //Player layer
        if(!starPowerUped)
        gameObject.layer = 9;

        //DeathScreen.SetActive(true);
        //DeathScreen.GetComponent<Animator>().SetBool("IsActive", true);
    }
    IEnumerator starPowerUpUnKill(float seconds)
    {
        SpriteRenderer starCircle =starParticleTransform.GetComponent<SpriteRenderer>();
        LeanTween.cancel(starCircle.gameObject);
        starPowerUped = true;
        //UnKillableLayer
        gameObject.layer = 13;
        //play particleEffects add later
        Color tempColor = starCircle.color;
        tempColor.a = 0.7f;
        starCircle.color = tempColor;
        starCircle.enabled = true;
        LeanTween.alpha(starParticleTransform.gameObject, 0.2f, seconds);
        starAudio.clip = starPowerUpAud;
        starAudio.Play();
        yield return new WaitForSeconds(seconds - 0.5f);
        starParticleTransform.GetComponent<SpriteRenderer>().enabled = false;


        starAudio.clip = starPowerDownAud;
        starAudio.Play();
        yield return new WaitForSeconds(0.5f);
        starPowerUped = false;
        //Player layer
        gameObject.layer = 9;

        //DeathScreen.SetActive(true);
        //DeathScreen.GetComponent<Animator>().SetBool("IsActive", true);
    }
   
   

    private void Die()
    {


        int extraCoins = 0;
        for (int i = 0; i < highSpeedCoins.Length; i++)
        {
            extraCoins += highSpeedCoins[i].extraHighSpeedCoins;
        }
        //  int  coinsHelper = PlayerPrefs.GetInt("Coins") + extraCoins;

        /*
        for(int i = 0; i < GameOverMap.Instance.planets.Length; i++)
        {
            Debug.Log(GameOverMap.Instance.planets[i].rTransform.gameObject.name);
        }
        */
        MapPlanet[] mapPlanet = gameoverMap.planets;
        float score = PlayerScoreManager.score;
        for (int i = 0; i < mapPlanet.Length; i++)
        {
            if (score >= mapPlanet[i].scoreItAppearsOn)
            {
                bonusCoinsForPlanets += mapPlanet[i].coinsForPlanet;
            }
        }
        if (score > 540f)
        {
            float scoreCalc = score - 500;

            for(int i = 50; i < scoreCalc; i += 50)
            {
                bonusCoinsForPlanets += 600;
            }
        }
        AddCoin(extraCoins);
        AddCoin(bonusCoinsForPlanets);
        //  PlayerPrefs.SetInt("Coins", coinsHelper+ bonusCoinsForPlanets);
        for (int i = 0; i < highSpeedCoins.Length; i++)
        {
            highSpeedCoins[i].extraHighSpeedCoins = 0;

        }
        dead = true;
        this.GetComponent<AudioSource>().pitch = 1f;
        this.GetComponent<AudioSource>().PlayOneShot(deathSound);
        hitEffect.SetTrigger(Animator.StringToHash("Hit"));
        StartCoroutine(slowDownTime(slowMoDeathDuration));
        Vector2 vel = playerRB.velocity;
        player.GetComponent<PlayerMoveJoystick>().enabled = false;
        playerRB.AddForce((player.transform.up + (-player.transform.localScale)) * 8f, ForceMode2D.Impulse);
        DeathScreenUI.Instance.ShowDeathScreen();
        transform.GetComponent<LineRenderer>().enabled = false;

    }
    public void StarPowerUp()
    {
        starParticle.Play();
        starAudio.Play();
        if (starPowerUped)
        {
            StopCoroutine(starPowerUpCor);
        }
        starPowerUpCor=StartCoroutine(starPowerUpUnKill(8f));
    }
    public void Respawn()
    {
        bool value= AdsManager.Instance.CheckAndPlayVideoAd();


        Time.timeScale = 1f;
        if (!value)
            LoadLevel();

    }
    public void LoadLevel() {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public void AddCoinToPlayer(int amount)
    {
        coinsStartingFromZero+= amount;
        coinParticle.Play();
        switch (amount)
        {
            case 5:
                coinAudio.clip = goldCoinAud;
                break;
            case 1:
                coinAudio.clip = bronzeCoinAud;
                break;
            case 3:
                coinAudio.clip = silverCoinAud;
                break;
            case 70:
                coinAudio.clip = diamondCoinAud;
                break;
        }
        coinAudio.Play();
        coinText.text = coinsStartingFromZero.ToString();
   PlayerPrefs.SetInt("Coins",coinsStartingFromZero+ startRunCoins);
        //SOUND
        //Activate Particle effects
        //UI change number
        //Potentially UI Animation
        //playerPrefs
    }
    public void AddCoin(int amount)
    {
        coinsStartingFromZero+=amount;
      //  coinParticleEffect.GetComponent<ParticleSystem>().Play();
        coinText.text = coinsStartingFromZero.ToString();
        PlayerPrefs.SetInt("Coins", coinsStartingFromZero+ startRunCoins);
        //SOUND
        //Activate Particle effects
        //UI change number
        //Potentially UI Animation
        //playerPrefs
    }

    //bad function
    public void AddHighSpeedCoin()
    {
        coinsStartingFromZero++;
        coinText.text = coinsStartingFromZero.ToString();
        PlayerPrefs.SetInt("Coins", coinsStartingFromZero+ startRunCoins);
        //SOUND
        //Activate Particle effects
        //UI change number
        //Potentially UI Animation
        //playerPrefs
    }
    public void ReduceCoins(int amount)
    {
        coinsStartingFromZero=coinsStartingFromZero-amount;
        PlayerPrefs.SetInt("Coins", coinsStartingFromZero + startRunCoins);
        if (coinsStartingFromZero < 0)
        {
            coinsStartingFromZero = 0;
            startRunCoins= PlayerPrefs.GetInt("Coins");
        }
        coinText.text = coinsStartingFromZero.ToString();
        //SOUND
        //Activate Particle effects
        //UI change number
        //Potentially UI Animation
        //playerPrefs
    }
    public bool IsDead()
    {
        return dead;
    }
    public void AddHealth()
    {
        if (GetCoinsNumber() >= HealthPrice.currentPrice)
        {
            ReduceCoins(HealthPrice.currentPrice);
            //coins -= HealthPrice.currentPrice;
            health++;

            HealthPrice.currentPrice = Mathf.Clamp(HealthPrice.currentPrice + HealthPrice.priceAddAfterBuying,
                0,HealthPrice.maxPrice);
            SaveLoad.Instance.checkpoint.hearts = health;
            SaveLoad.Instance.checkpoint.HealthPrice = HealthPrice;
            SaveLoad.Instance.saveCheckPoint();

            // Mathf.Clamp(HealthPrice.currentPrice, 0, HealthPrice.maxPrice);
            heartText.text = health.ToString();
            coinText.text = coinsStartingFromZero.ToString();

        }

        //SOUND
        //activate particle effect
        //UI activate
        //Potentially UI Animation

    }
    //delete later
    public void setHealth(int number)
    {
        health = number;
    }

    public void HeartCoin()
    {

        health++;
        heartText.text = health.ToString();
        heartParticle.Play();
        heartAudio.Play();
    }
    public void AddSpeed()
    {
        if (GetCoinsNumber() >= SpeedPrice.currentPrice)
        {


            ReduceCoins(SpeedPrice.currentPrice);
            //coins -= SpeedPrice.currentPrice;
            playerMove.AddSpeed();
            SpeedPrice.currentPrice= Mathf.Clamp(SpeedPrice.currentPrice + SpeedPrice.priceAddAfterBuying,
                0,SpeedPrice.maxPrice);
            //Mathf.Clamp(SpeedPrice.currentPrice, 0, SpeedPrice.maxPrice);
            SaveLoad.Instance.checkpoint.SpeedPrice = SpeedPrice;

            coinText.text = coinsStartingFromZero.ToString();
            SaveLoad.Instance.saveCheckPoint();

        }

    }
    public void AddSpeedDebug()
    {
       
            playerMove.AddSpeed();
        

    }

    public void AddGravity()
    {

        if (GetCoinsNumber() >= GravityPrice.currentPrice)
        {
            ReduceCoins(GravityPrice.currentPrice);
            //coins -= GravityPrice.currentPrice;
            playerMove.AddGravity();
            GravityPrice.currentPrice = Mathf.Clamp(GravityPrice.currentPrice + GravityPrice.priceAddAfterBuying,
                0,GravityPrice.maxPrice);

            SaveLoad.Instance.checkpoint.GravityPrice = GravityPrice;

            // Mathf.Clamp(GravityPrice.currentPrice, 0, GravityPrice.maxPrice);
            coinText.text = coinsStartingFromZero.ToString();
            SaveLoad.Instance.saveCheckPoint();


        }

    }
    public void EquipSkin()
    {
        if (currentSkin != null)
        {
            body.sprite = currentSkin.bodySprite;
            wings.color = currentSkin.wingColor;
            wings.transform.localScale = currentSkin.wingScale;
            wings.transform.localPosition = currentSkin.wingLocalPosition;
            PlayerAfterImage.Instance.EquipSkin();
            if (PlayerPrefs.GetInt("Checkpoint", 0) == 0)
            {
                health = currentSkin.startingHealth + PlayerPrefs.GetInt("ExtraLife", 0);
                PlayerPrefs.SetInt("ExtraLife", 0);
            }
            else
            {
                health = SaveLoad.Instance.checkpoint.hearts;

            }
            heartText.text = health.ToString();
            playerMove.setStatsOfSkin(currentSkin);
            //in transform hierachy crown is the 10th child
            if (checkIfCrownedBird(currentSkin))
            {
                transform.GetChild(10).transform.localPosition = currentSkin.crownPosition;
                transform.GetChild(10).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(10).gameObject.SetActive(false);


            }
        }
    }
    public void setCrownOnSkin()
    {
        if (currentSkin != null)
        {
            transform.GetChild(10).transform.localPosition = currentSkin.crownPosition;
            transform.GetChild(10).gameObject.SetActive(true);
        }
    }
    bool checkIfCrownedBird(SkinItem currentSkin)
    {
        SaveObjectList savedList = SaveLoad.Instance.lSave;
        if (savedList!=null)
     {

        for(int i = 0; i < savedList.lSaveObjects.Count; i++)
        {
                SkinItem skinToCheck = ShopUI.Instance.GetSkin(savedList.lSaveObjects[i].shopIndexValue);
                                      //  lSave.lSaveObjects[i].skin = ShopUI.Instance.GetSkin(lSave.lSaveObjects[i].shopIndexValue);

            if (skinToCheck.Equals(currentSkin))
            {
                return true;
            }
        }

     }

        return false;
    }
    public float GetBirdJumpPower()
    {
        return playerMove.jumpPower;
    }
    public int GetBirdHealth() {
        return health;
    }
    public float GetBirdGravity()
    {
        return playerMove.gravityScale;
    }
    public float GetCoinsNumber()
    {
        int extraCoins = 0;
        for(int i = 0; i < highSpeedCoins.Length; i++)
        {
            extraCoins += highSpeedCoins[i].extraHighSpeedCoins;
            highSpeedCoins[i].extraHighSpeedCoins=0;
        }
        coinsStartingFromZero += extraCoins;
        return (coinsStartingFromZero);
    }
    public float distanceFromPlayer(Transform testi)
    {
        return Vector3.Distance(this.transform.position, testi.position);
    }

}
