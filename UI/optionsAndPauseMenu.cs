using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class optionsAndPauseMenu : MonoBehaviour
{
    [SerializeField] GameObject MuteSFX;
    [SerializeField] GameObject MuteMusic;
    [SerializeField] GameObject UnmuteMusic;
    [SerializeField] GameObject UnmuteSFX;
 //   [SerializeField] GameObject PersonalizedAdsOn;
 //   [SerializeField] GameObject PersonalizedAdsOff;
    public static optionsAndPauseMenu Instance;
    [SerializeField] GameObject GNPR_PopUp;
    [SerializeField] GameObject IntroButtons;
    [SerializeField] GameObject rateGameUI;

    [SerializeField] Image ControlerPadImageBackground;
    [SerializeField] Image ControlerPadImage;

    private int rateGameCounter=0;


    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (PlayerPrefs.GetInt("MuteSFX",1)==1)
        {

            MuteSFX.SetActive(true);
            UnmuteSFX.SetActive(false);

        }
        else
        {

            MuteSFX.SetActive(false);
            UnmuteSFX.SetActive(true);
        }

        if (PlayerPrefs.GetInt("MuteMusic", 1) == 1)
        {
            MuteMusic.SetActive(false);
            UnmuteMusic.SetActive(true);

        }
        else
        {
            MuteMusic.SetActive(true);
            UnmuteMusic.SetActive(false);

        }
        /*
        if (PlayerPrefs.GetInt("npa") == 0)
        {
            PersonalizedAdsOn.SetActive(true);
            PersonalizedAdsOff.SetActive(false);
        }
        else
        {
            PersonalizedAdsOn.SetActive(false);
            PersonalizedAdsOff.SetActive(true);
        }*/

        if (PlayerPrefs.GetInt("Checkpoint", 0) == 1)
        {
            IntroButtons.SetActive(false);
        }

        if (PlayerPrefs.GetInt("Invert", 0) == 1)
        {
            //INVERT IS TURNED ON

        }
        else
        {
            //INVERT IS TURNED OFF
        }
        if (PlayerPrefs.GetInt("ControlPad", 0) == 1)
        {

            //ControlPad IS TURNED ON
            ControlerPadImage.color = Color.white;
            ControlerPadImageBackground.color = Color.white;


        }
        else
        {

            ControlerPadImage.color = Color.clear;
            ControlerPadImageBackground.color = Color.clear;

          //  ControlerPadImage.color = new Color(1,1,1,0.5f);
          //  ControlerPadImageBackground.color = new Color(1, 1, 1, 0.5f);
            //ControlPad IS TURNED OFF
        }
        rateGameCounter = PlayerPrefs.GetInt("rateGameCounter", 0);
        if (rateGameCounter > -1)
        {

        
             if (rateGameCounter > 30)
            {
            ShowRateGamePopUp();
            }
            else
            {
            rateGameCounter++;
            PlayerPrefs.SetInt("rateGameCounter", rateGameCounter);
            PlayerPrefs.Save();
            }
        }


    }
    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }
    public void showGNPR_PopUp()
    {
        GNPR_PopUp.SetActive(true);
    }
    public void OnUserClickPersonalizedAds()
    {
        PlayerPrefs.SetInt("npa", 0);
        PlayerPrefs.Save();
        AdsManager.Instance.resetNpaValue();
     //   PersonalizedAdsOn.SetActive(true);
     //   PersonalizedAdsOff.SetActive(false);

    }  
    public void OnUserClickUnPersonalizedAds()
    {
        PlayerPrefs.SetInt("npa", 1);
        PlayerPrefs.Save();
        AdsManager.Instance.resetNpaValue();
    //    PersonalizedAdsOn.SetActive(false);
    //    PersonalizedAdsOff.SetActive(true);

    }
    public void ContinueGame()
    {
        Time.timeScale = 1f;

    }
    public void ShowPrivacyPolicy()
    {
        Application.OpenURL("https://galaxy-bird-flight.flycricket.io/privacy.html");
    }    
    public void ShowTermsOfService()
    {
        Application.OpenURL("https://galaxy-bird-flight.flycricket.io/terms.html");
    }
    public void ShowRateGame()
    {
        rateGameCounter = -100;
        PlayerPrefs.SetInt("rateGameCounter", rateGameCounter);
        PlayerPrefs.Save();

        Application.OpenURL("https://play.google.com/store/apps/details?id=com.VitomirHardi.Mobilegameangelprototype");

    }

    public void RateGameLater()
    {
        rateGameCounter = 0;
        PlayerPrefs.SetInt("rateGameCounter", rateGameCounter);
        PlayerPrefs.Save();
    }
    public void RateGameNever()
    {
        rateGameCounter = -100;
        PlayerPrefs.SetInt("rateGameCounter", rateGameCounter);
        PlayerPrefs.Save();
    }
    public void ShowRateGamePopUp()
    {
        rateGameUI.SetActive(true);
    }

    public void PlayRewardedAd()
    {
        AdsManager.Instance.PlayRewardedVideoAd();
    }
    public void eraseAllData()
    {
        PlayerPrefs.DeleteAll();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        SaveLoad.Instance.deleteEverything();


    }
    public void PauseTime()
    {
        Time.timeScale = 0.0f;

    }
    public void unPauseTime()
    {
        Time.timeScale = 1f;

    }
}
