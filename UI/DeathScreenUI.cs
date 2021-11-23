using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class DeathScreenUI : MonoBehaviour
{



    public static DeathScreenUI Instance;
    [SerializeField] GameObject DarkenImage;
    [SerializeField] Color StartingColor;
    [SerializeField] Color EndingColor;
    [SerializeField] float duration;
    [SerializeField] float delay;
    [SerializeField] float coinsTextGoingUpTime=1f;
    [SerializeField] LeanTweenType typeCoin;



    [SerializeField] RectTransform shopButton;
    [SerializeField] RectTransform restartButton;

    [SerializeField] TextMeshProUGUI scoreNumber;
    [SerializeField] TextMeshProUGUI highScoreNumber;
    [SerializeField] TextMeshProUGUI coinsNumber;
    [SerializeField] TextMeshProUGUI coinsCollectedNumber;
    [SerializeField] TextMeshProUGUI coinsPlanetBonus;
    RectTransform trCoinsPlanetBonus;


    [SerializeField] GameObject newHighScore;


    [SerializeField] GameObject gameplayUI;
    [SerializeField] CinemachineVirtualCamera camera;
    [SerializeField] float cameraZoomSize = 30f;

    [Header("Map settings")]
    [SerializeField] RectTransform mapEmptyObject;
    GameOverMap gameoverMap;
   float mapMovePerScore1 = GameOverMap.mapMovePerScore;
    [SerializeField] Image birdGameOverSprite;
    [SerializeField] Image birdGameOverWings;
    [SerializeField] RectTransform greenHighScoreLine;


    RectTransform GameOverUI;
    [SerializeField] AudioSource coinsSound;
    [SerializeField] AudioSource diamondSound;


    float xPos;

    // Start is called before the first frame update
    void Start()
    {
        gameoverMap = mapEmptyObject.GetComponent<GameOverMap>();
        newHighScore.SetActive(false);
        Instance = this;
        DarkenImage.GetComponent<Image>().color = StartingColor;
        GameOverUI = transform.GetChild(0).GetComponent<RectTransform>();
        coinsSound = this.GetComponent<AudioSource>();
        mapMovePerScore1 = GameOverMap.mapMovePerScore;
        trCoinsPlanetBonus = coinsPlanetBonus.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowDeathScreen()
    {
        //green line 
        greenHighScoreLine.anchoredPosition = new Vector2((PlayerPrefs.GetFloat("HighScore", 0f) * mapMovePerScore1)+GameOverMap.xZeroPos, 0);
        if (PlayerScoreManager.score > PlayerPrefs.GetFloat("HighScore", 0f))
        {
            PlayerPrefs.SetFloat("HighScore", PlayerScoreManager.score);
            newHighScore.SetActive ( true);
        }

        scoreNumber.text = PlayerScoreManager.score.ToString("F");
        highScoreNumber.text = PlayerPrefs.GetFloat("HighScore",0f).ToString("F");
        //this is the old static version
        //coinsNumber.text = PlayerPrefs.GetInt("Coins").ToString();


        StartCoroutine(UISetCoins());
       // UIsetCoinsLeanTween();

        MusicManager.Instance.MusicMute();
        gameplayUI.SetActive(false);

        xPos = restartButton.anchoredPosition.x;
        GameOverUI.anchoredPosition = new Vector2(1400, GameOverUI.anchoredPosition.y);
     //   mainMenuButton.anchoredPosition = new Vector2(1400, mainMenuButton.anchoredPosition.y);
        restartButton.anchoredPosition = new Vector2(1400, restartButton.anchoredPosition.y);
        shopButton.anchoredPosition = new Vector2(1400, shopButton.anchoredPosition.y);

        //Old code
        //  LeanTween.moveX(mapEmptyObject, -mapMovePerScore1 * PlayerScoreManager.score, PlayerScoreManager.score/ 30f).setDelay(5f);
       StartCoroutine(moveMapToScore());
        scoreIncrease();
        //Original just automatically move
        //mapEmptyObject.anchoredPosition = new Vector2(mapMovePerScore1 * PlayerScoreManager.score, 0);
        birdGameOverSprite.sprite = PlayerStateManager.Instance.currentSkin.bodySprite;
        birdGameOverWings.color = PlayerStateManager.Instance.currentSkin.wingColor;
        birdGameOverWings.rectTransform.localScale = PlayerStateManager.Instance.currentSkin.wingScale;

        GameOverUI.gameObject.SetActive(true);
       // mainMenuButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        shopButton.gameObject.SetActive(true);
        LeanTween.color(DarkenImage.GetComponent<RectTransform>(), EndingColor, duration).setDelay(delay);

        // camera.gameObject.set

        // LeanTween.value(camera.m_Lens.OrthographicSize, from1, to, time, options);
        LeanTween.value(camera.gameObject, camera.m_Lens.OrthographicSize, cameraZoomSize, 1.5f).setOnUpdate((float flt) => {
            camera.m_Lens.OrthographicSize = flt;
        }).setOnComplete(moveUI);
        camera.GetComponent<LockCameraX>().enabled = false;



        //leenTween to show things

    }
    void scoreIncrease()
    {
        LeanTween.value(scoreNumber.gameObject, 0f, PlayerScoreManager.score, PlayerScoreManager.score / 30f).setOnUpdate((float flt) =>
        {
            scoreNumber.text = flt.ToString("F");

        }).setDelay(2f);    
        }

    void moveUI()
    {
        //scoreIncrease();
        LeanTween.moveX(GameOverUI, 0f, 0.3f);
        //LeanTween.moveX(mainMenuButton, 0f, 0.2f).setDelay(0.1f);
        LeanTween.moveX(shopButton, xPos, 0.4f).setDelay(0.2f);
        LeanTween.moveX(restartButton, xPos, 0.5f).setDelay(0.3f);


    }
    public void UpdateCoinValue()
    {
        coinsNumber.text = PlayerPrefs.GetInt("Coins",0).ToString();

    }
    IEnumerator moveMapToScore()
    {
        MapPlanet[] mapPlanet = gameoverMap.planets;
        float score = PlayerScoreManager.score;
        int moneyTextHelper = 0;
        float speedOfAnimation = 3.5f;
        Vector3 oldPosOfBonusCoinText = trCoinsPlanetBonus.localPosition;
        yield return new WaitForSeconds(5f);
        coinsCollectedNumber.text = "0";
        coinsCollectedNumber.gameObject.SetActive(true);

        //lazy code i know but it gets the job done
        int startRunCoins = PlayerStateManager.Instance.startRunCoins;
        int coinsUI = PlayerPrefs.GetInt("Coins") - PlayerStateManager.Instance.startRunCoins-PlayerStateManager.Instance.bonusCoinsForPlanets;



        for (int i = 0; i < mapPlanet.Length; i++)
        {

            trCoinsPlanetBonus.gameObject.SetActive(false);
            coinsPlanetBonus.text = mapPlanet[i].coinsForPlanet.ToString();
            if (score >= mapPlanet[i].scoreItAppearsOn)
            {
                if (mapPlanet[i].coinsForPlanet != 0)
                {
                    LeanTween.moveX(mapEmptyObject, -mapMovePerScore1 * mapPlanet[i].scoreItAppearsOn, 1f / speedOfAnimation).setDelay(0.7f / speedOfAnimation).setOnComplete(() =>
                    {
                        trCoinsPlanetBonus.localPosition = oldPosOfBonusCoinText;
                        trCoinsPlanetBonus.gameObject.SetActive(true);
                        LeanTween.moveY(trCoinsPlanetBonus, 130f, 0.7f / speedOfAnimation).setOnComplete(() =>
                         {
                             coinsSound.Play();
                             moneyTextHelper += mapPlanet[i].coinsForPlanet;
                             
                             coinsCollectedNumber.text = moneyTextHelper.ToString();
                         });

                    });
                    yield return new WaitForSeconds(2.5f / speedOfAnimation);
                }

            }
            else
            {
                LeanTween.moveX(mapEmptyObject, -mapMovePerScore1 * score, 1).setDelay(0.7f / speedOfAnimation);
                //yield return new WaitForSeconds(2.5f/speedOfAnimation);
                //break;
            }
        }

        if (score > 540f)
        {
            float scoreCalc = score - 500;
            coinsPlanetBonus.text = 600.ToString();

            for (int i = 50; i < scoreCalc; i += 50)
            {
                trCoinsPlanetBonus.gameObject.SetActive(false);

                LeanTween.moveX(mapEmptyObject, -mapMovePerScore1 * (500+i), 1f / speedOfAnimation).setDelay(0.7f / speedOfAnimation).setOnComplete(() =>
                {
                    trCoinsPlanetBonus.localPosition = oldPosOfBonusCoinText;
                    trCoinsPlanetBonus.gameObject.SetActive(true);
                    LeanTween.moveY(trCoinsPlanetBonus, 130f, 0.7f / speedOfAnimation).setOnComplete(() =>
                    {
                        coinsSound.Play();
                        moneyTextHelper += 600;
                        coinsCollectedNumber.text = moneyTextHelper.ToString();
                    });

                });
                yield return new WaitForSeconds(2.5f / speedOfAnimation);
            }
        }
        LeanTween.moveX(mapEmptyObject, -mapMovePerScore1 * score, 1).setDelay(0.7f / speedOfAnimation);


        LeanTween.moveY(coinsCollectedNumber.GetComponent<RectTransform>(), 170, 1f).setOnComplete(() => {
            coinsCollectedNumber.gameObject.SetActive(false);
            coinsNumber.text = (startRunCoins + coinsUI+moneyTextHelper).ToString();
            diamondSound.Stop();
            diamondSound.Play();
        }).setEase(typeCoin);
        yield return new WaitForSeconds(1f);
        MusicManager.Instance.MuteEverything();
      //  LeanTween.moveX(mapEmptyObject, -mapMovePerScore1 * 30f, 1).setDelay(15f);
    }
    void UIsetCoinsLeanTween()
    {
        int startRunCoins = PlayerStateManager.Instance.startRunCoins;
        int coinsUI = PlayerPrefs.GetInt("Coins") - PlayerStateManager.Instance.startRunCoins-PlayerStateManager.Instance.bonusCoinsForPlanets;
        /*
        LeanTween.value(this.gameObject, camera.m_Lens.OrthographicSize, cameraZoomSize, 1.5f).setOnUpdate((float flt) => {
            camera.m_Lens.OrthographicSize = flt;
        }).setOnComplete(moveUI);
        */
        LeanTween.value(startRunCoins, startRunCoins + coinsUI, 2f).setOnUpdate((float flt) =>
        {
            coinsNumber.text = Mathf.RoundToInt(flt).ToString();
            coinsSound.Stop();
            coinsSound.Play();
        }).setDelay(1f);

        
    }
    IEnumerator UISetCoins()
    {
        int startRunCoins = PlayerStateManager.Instance.startRunCoins;
        coinsNumber.text = startRunCoins.ToString();

        int coinsUI = PlayerPrefs.GetInt("Coins") - PlayerStateManager.Instance.startRunCoins- PlayerStateManager.Instance.bonusCoinsForPlanets;
        yield return new WaitForSecondsRealtime(3f);
        float waitTime = coinsTextGoingUpTime / coinsUI;
        float coinsCollected = 0;
        int counter = 0;
        Vector3 coinsCollectedOldPos = coinsCollectedNumber.GetComponent<RectTransform>().localPosition;
        if (coinsUI > 0)
        {
            for (float t = 0f; t <= coinsTextGoingUpTime; t += Time.deltaTime)
            {
                coinsCollected = Mathf.Lerp(0, coinsUI, t / coinsTextGoingUpTime);
                coinsCollectedNumber.text = coinsCollected.ToString("0");
                if (counter % 4 == 0)
                {
                    coinsSound.Play();
                    counter = 0;
                }
                counter++;
                yield return null;
            }
        }
        coinsCollectedNumber.text = coinsUI.ToString();
        yield return new WaitForSeconds(1);
        LeanTween.moveY(coinsCollectedNumber.GetComponent<RectTransform>(), 170, 1f).setOnComplete(() => {
          //  coinsCollectedNumber.gameObject.SetActive(false);
            coinsCollectedNumber.GetComponent<RectTransform>().localPosition = coinsCollectedOldPos;
            coinsNumber.text = (startRunCoins + coinsUI).ToString();
            diamondSound.Stop();
            diamondSound.Play();
        }).setEase(typeCoin);



        /*OLD CODE*/
        /*
        for (int i= startRunCoins; i< startRunCoins + coinsUI; i++)
        {
            coinsNumber.text = i.ToString();
            if (i % 3==0)
            {
                coinsSound.Stop();
                coinsSound.Play();
            }
            yield return new WaitForSecondsRealtime(waitTime);
        }
        */
    }
    void deactivateObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
