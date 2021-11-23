using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using System;
//using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    private string App_ID = "ca-app-pub-0123";
    private string testinterstitial_ID = "ca-app-pub-3940256099942544/1033173712";
    private string real_interstitial_ID = "ca-app-pub-0123";
    private string testrewardedVideo_ID = "ca-app-pub-3940256099942544/5224354917";
    private string real_rewardedVideo_ID = "ca-app-pub-0123";
    //test reward ""
    //test interstitial 
    [SerializeField] private bool gaveMoney = false;
    public static AdsManager Instance;
    [SerializeField]  float[] timeTillNextVideoAd;
    [SerializeField]  int[] timesPlayedTillNextVideoAd;
     private  float timerHelper;
    private int timesPlayedTimerHelper=0;
    private int timesPlayed=0;
    private int indexTimes=0;
    bool videoAdNotLoaded = false;
    bool rewardvideoAdNotLoaded = false;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    private bool isRewardedAdFinnished = false;
    [SerializeField] int npaValue = -1;
    //"npa"=Non Personalized Ads


    // Start is called before the first frame update
    void Start()
    {

        
        bool firstTimeGDPRRights = false;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            npaValue = PlayerPrefs.GetInt("npa", -1);

            if (npaValue == -1)
            {
                optionsAndPauseMenu.Instance.showGNPR_PopUp();
                firstTimeGDPRRights = true;
                npaValue = 1;
            }

            //this seems scruffy FIX
            timerHelper = PlayerPrefs.GetFloat("TimeTill", 0f);

            timesPlayed = Math.Abs(PlayerPrefs.GetInt("timesPlayed", 0));


            /*for(int i = 0; i < timeTillNextVideoAd.Length; i++)
            {
                if(timerHelper> timeTillNextVideoAd[i])
                {
                    timerHelper = timeTillNextVideoAd[0];
                }
            }*/



            indexTimes = PlayerPrefs.GetInt("indexTimes", 0);
            if (indexTimes > timeTillNextVideoAd.Length)
            {
                indexTimes = 0;
            }
            if (timerHelper == 0f)
            {
                timerHelper = timeTillNextVideoAd[indexTimes];
            }
            //add initialize only if in selected
               MobileAds.Initialize(initStatus => { });
             // MobileAds.Initialize(App_ID);



            this.rewardedAd = new RewardedAd(real_rewardedVideo_ID);
            this.interstitial = new InterstitialAd(real_interstitial_ID);

            #region RewardedAdDeclarations
            // Called when an ad request has successfully loaded.
            this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
            // Called when an ad request failed to load.
            this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
            // Called when an ad is shown.
            //mute audio
            this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
            // Called when an ad request failed to show.
            this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            // Called when the user should be rewarded for interacting with the ad.
            this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            // Called when the ad is closed.
            //unmute audio
            this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
            #endregion
            #region interstitialAdDeclaration
            // Called when an ad request has successfully loaded.
            this.interstitial.OnAdLoaded += HandleOnAdLoaded;
            // Called when an ad request failed to load.
            this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            // Called when an ad is shown.
            this.interstitial.OnAdOpening += HandleOnAdOpened;
            // Called when the ad is closed.
            this.interstitial.OnAdClosed += HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
            #endregion

            if (!firstTimeGDPRRights)
            {
                if (!this.interstitial.IsLoaded())
                    RequestInterstitial();
                if (!this.rewardedAd.IsLoaded())
                    RequestRewardedVideoAd();
            }

        }
        else
        {
            Destroy(this.gameObject);
        }

    }


 
    public void RequestIfNotLoaded()
    {
        if (videoAdNotLoaded)
        {
            RequestInterstitial();
            videoAdNotLoaded = false;
        }
        if (rewardvideoAdNotLoaded){
            RequestRewardedVideoAd();
            rewardvideoAdNotLoaded = false;
        }
    }
    private void RequestRewardedVideoAd()
    {

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().AddTestDevice("000")
            .AddExtra("npa", npaValue.ToString()).
            Build();       
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
        //this.rewardedAd.LoadAd(new AdRequest.Builder().Build(), Test_rewardedVideo_ID);

    }

    private void RequestInterstitial()
    {


        // Initialize an InterstitialAd.


        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().AddTestDevice("000")
            .AddExtra("npa", npaValue.ToString()).
            Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);

    }
   
    public bool CheckAndPlayVideoAd()
    {
       // if (!this.interstitial.IsLoaded())
        //    RequestInterstitial(); 

        timesPlayedTimerHelper++;
        timesPlayed++;
        if ((Time.time > timerHelper)||timesPlayed>timesPlayedTillNextVideoAd[indexTimes])
        {
            indexTimes++;
            if (indexTimes >= timeTillNextVideoAd.Length)
            {
                indexTimes = 0;
            }
            timesPlayedTimerHelper = 0;
            timesPlayed = 0;
            timerHelper = Time.time+timeTillNextVideoAd[indexTimes];
                PlayVideoAd();
            return true;

        }
        return false;

    }
  
    public void PlayVideoAd()
    {
        //   if (!Advertisement.IsReady(videoAd)) { return; }
        //   Advertisement.Show(videoAd);

        //if (!this.interstitial.IsLoaded()) { RequestInterstitial(); }

        if (this.interstitial.IsLoaded())
        {
            MusicManager.Instance.MuteEverything();
            this.interstitial.Show();
        }

    }
    public void PlayRewardedVideoAd()
    {
      //  if (!this.rewardedAd.IsLoaded()) { RequestRewardedVideoAd();
       //     Debug.Log("Request rewarded video");

      //  }

        //  if (!Advertisement.IsReady(rewardedVideoAd)) {  return; }
        if (this.rewardedAd.IsLoaded())
        {
            MusicManager.Instance.MuteEverything();
            this.rewardedAd.Show();
            StartCoroutine(IsRewardAdFinnishedCor());

        }
        gaveMoney = false;
        //Advertisement.Show(rewardedVideoAd);

    }
   
    AdRequest AdRequestBuild()
    {
        return new AdRequest.Builder()
            .AddExtra("npa", npaValue.ToString())
            .Build();
    }
    public void resetNpaValue()
    {
        npaValue = PlayerPrefs.GetInt("npa", 1);
    }
    void OnApplicationQuit()
    {

        saveAdTimers();

        //   if (timerHelper > Time.time)
        //  {

        //}
        //else
        //{
        //  PlayerPrefs.SetFloat("TimeTill", 120f);

        //}
        /*  PlayerPrefs.SetFloat("TimeTill", timerHelper - Time.time);
          PlayerPrefs.SetInt("indexTimes", indexTimes);
          PlayerPrefs.SetInt("timesPlayed", timesPlayed+1);
          PlayerPrefs.Save();
        */
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            saveAdTimers();
        }

    }

    public void saveAdTimers()
    {
        PlayerPrefs.SetFloat("TimeTill", (timerHelper-Time.time));
        PlayerPrefs.SetInt("indexTimes", indexTimes);
        PlayerPrefs.SetInt("timesPlayed", timesPlayed);
        PlayerPrefs.Save();
      //  Debug.Log("Application ending with  " + (timerHelper - Time.time) + " seconds \n timesplayed:" + timesPlayed+" on index "+ indexTimes);
      //  Debug.Log("Prefs ending with  " + PlayerPrefs.GetFloat("TimeTill", 0f) + " seconds \n timesplayed:" + Math.Abs(PlayerPrefs.GetInt("timesPlayed", 0)) + " on index " + PlayerPrefs.GetInt("indexTimes", 0));

        
    }

    IEnumerator IsRewardAdFinnishedCor()
    {
        Debug.Log("Waiting for tank to get empty");
        yield return new WaitUntil(IsRewardedAdFinnished);
        Debug.Log("Tank Empty!");
        gaveMoney = true;
        PlayerPrefs.SetInt("ExtraLife", 1);
        DeathScreenUI.Instance.UpdateCoinValue();
        timerHelper += 150f;
    }


    bool IsRewardedAdFinnished()
    {
        if (!isRewardedAdFinnished) { return false; }
        else { return true; }
    }

    #region interstitialCallBacks
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        videoAdNotLoaded = true;
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        Debug.Log(this.interstitial.IsLoaded() + " before destruction");
        interstitial.Destroy();
        RequestInterstitial();
        Debug.Log(this.interstitial.IsLoaded() + " after destruction");
        PlayerStateManager.Instance.LoadLevel();


    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
    // Update is called once per frame
    #endregion

    #region RewardedVideoCallbacks
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        rewardvideoAdNotLoaded = true;
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
        isRewardedAdFinnished = false;
        timesPlayed = 0;

        // MusicManager.Instance.MuteEverything();

    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        RequestRewardedVideoAd();
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        isRewardedAdFinnished = true;
        //this.rewardedAd = new RewardedAd(Test_rewardedVideo_ID);
    /*    this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        //mute audio
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        //unmute audio
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    */
       // MusicManager.Instance.UnMuteEverything();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
     //   MusicManager.Instance.UnMuteEverything();
        //reset counters so that a ad doesnt appear after watching rewarded ad
        indexTimes++;
        if (indexTimes >= timeTillNextVideoAd.Length)
        {
            indexTimes = 0;
        }
        timesPlayedTimerHelper = 0;
        timesPlayed = 0;
        timerHelper = Time.time + timeTillNextVideoAd[indexTimes];



    }

    #endregion


    /* public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
     {
         switch (showResult)
         {
             case ShowResult.Failed:
                 break;
             case ShowResult.Skipped:
                 break;
             case ShowResult.Finished:
                 if (placementId == rewardedVideoAd)
                 {
                     if (!gaveMoney)
                     {
                         gaveMoney = true;
                        // PlayerStateManager.Instance.AddCoin(120);
                         PlayerPrefs.SetInt("ExtraLife", 1);
                         DeathScreenUI.Instance.UpdateCoinValue();
                         timerHelper += 150f;
                         watchVideoBut.SetActive(false);
                     }
                 }
                 break;
         }
         //throw new System.NotImplementedException();
     }*/
}


//IF I EVER NEED TO SWITCH TO UNITY ADS
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    private string playStoreID = "3783165";
    private string videoAd = "video";
    private string rewardedVideoAd = "rewardedVideo";
    [SerializeField] private bool gaveMoney = false;
    public static AdsManager Instance;
    [SerializeField]  float timeTillNextVideoAd;
    [SerializeField] GameObject watchVideoBut;
[SerializeField] public static float timerHelper=0f;
    public static int timesPlayed=0;
    [SerializeField] bool timerVideoAds = false;

    public bool isTestAd;

    // Start is called before the first frame update
    void Start()
    {

        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this.gameObject);
        }
        if (timerHelper == 0f)
        {
            timerHelper = timeTillNextVideoAd - 60f;
        }
        Advertisement.AddListener(this);
        InitializeAdvertisment();
    }

    // Update is called once per frame

    public void CheckAndPlayVideoAd()
    {
        timesPlayed++;
        if (Time.time > timerHelper&&timesPlayed>=2)
        {
            timesPlayed = 0;
            timerHelper = Time.time+timeTillNextVideoAd;
            if (timerVideoAds)
            {
                PlayVideoAd();
            }
        }

    }
    private void InitializeAdvertisment()
    {
        Advertisement.Initialize(playStoreID, isTestAd);
    }
    public void PlayVideoAd()
    {
        if (!Advertisement.IsReady(videoAd)) { return; }
        Advertisement.Show(videoAd);
    }
    public void PlayRewardedVideoAd()
    {
        Debug.Log("Play rewarded video");
        if (!Advertisement.IsReady(rewardedVideoAd)) {  return; }
        gaveMoney = false;
        Advertisement.Show(rewardedVideoAd);

    }


    public void OnUnityAdsReady(string placementId)
    {
    //    throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidError(string message)
    {
      //  throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //MUTE AUDIO PLS 
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Failed:
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Finished:
                if (placementId == rewardedVideoAd)
                {
                    if (!gaveMoney)
                    {
                        gaveMoney = true;
                       // PlayerStateManager.Instance.AddCoin(120);
                        PlayerPrefs.SetInt("ExtraLife", 1);
                        DeathScreenUI.Instance.UpdateCoinValue();
                        timerHelper += 150f;
                        watchVideoBut.SetActive(false);
                    }
                }
                break;
        }
        //throw new System.NotImplementedException();
    }
}
*/