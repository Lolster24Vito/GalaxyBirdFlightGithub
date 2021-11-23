using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    struct MusicPerScore
    {
       public AudioClip slowVersion;
        public AudioClip epicVersion;
        public float scoreItAppersOn;

    }
    public static MusicManager Instance;

    [SerializeField] AudioSource slowAudSource;
    [SerializeField] AudioSource epicAudSource;
    [SerializeField] Vector2[] muteMusicMinMax;
    [SerializeField] MusicPerScore[] music;
  [SerializeField]  int musicIndex=0;
    [SerializeField] [Range(0, 2f)] float musicSpeedUp2 = 1.03f;
    [SerializeField][Range(0,2f)] float musicSpeedUp3 = 1.06f;



    public AudioMixer audioMixer;
    // Start is called before the first frame update
    private float prevScore = 0;
   [SerializeField] bool musicEpic = false;
   [SerializeField] bool musicSlow = true;
    Coroutine coroutine;
  [SerializeField]  bool changingMusic = false;
  [SerializeField]  bool muteMusic = false;
    [SerializeField] bool mutedMusicOnce = false;
    private void Start()
    {
        Instance = this;
        
        Application.targetFrameRate = 60;
        audioMixer.SetFloat("slowVolume", -80f);
        audioMixer.SetFloat("epicVolume", -80f);
        if (PlayerPrefs.GetInt("MuteSFX", 1) == 1)
        {
            UnMuteSFXOptions();


        }
        else
        {
            MuteSFXOptions();


        }

        if (PlayerPrefs.GetInt("MuteMusic", 0) == 1)
        {
            MuteMusicOptions();

        }
        else
        {
            UnMuteMusicOptions();


        }

        //  StartCoroutine(StartFade("epicVolume", 1f ,1f));
        //  StartCoroutine(StartFade("slowVolume", 1f, -80f));

        UnMuteEverything();

        if (PlayerPrefs.GetInt("Checkpoint", 0) == 1)
        {
            musicIndex = SaveLoad.Instance.checkpoint.musicIndex;
            slowAudSource.clip = music[musicIndex - 1].slowVersion;
            epicAudSource.clip = music[musicIndex - 1].epicVersion;
            slowAudSource.Play();
            epicAudSource.Play();
        }

    }
    public void MusicMute()
    {
        StopAllCoroutines();
        StartCoroutine(StartFade("epicVolume", 0.5f, -80f));
        StartCoroutine(StartFade("slowVolume", 0.5f, -80f));
    }
   

    // Update is called once per frame
    void Update()
    {
        bool tempMuteMusic = false;
        for (int i = 0; i < muteMusicMinMax.Length; i++)
        {
            if (PlayerScoreManager.score > muteMusicMinMax[i].x && PlayerScoreManager.score < muteMusicMinMax[i].y)
            {
                tempMuteMusic = true;
            }
        }
        muteMusic = tempMuteMusic;
        if (muteMusic&&!mutedMusicOnce)
        {
            mutedMusicOnce = true;
            MusicMute();
        }
        if (!muteMusic)
        {
            if (mutedMusicOnce)
            {
                musicEpic = false;
                musicSlow = false;
            }
                mutedMusicOnce = false;
            //  Debug.Log(PlayerMoveJoystick.highSpeed);
            if (music.Length > musicIndex)
            {
                if (music[musicIndex].scoreItAppersOn < PlayerScoreManager.score)
                {
                    changingMusic = true;
                    StartCoroutine(SetMusic(musicIndex));
                    musicIndex += 1;
                }
            }




            if (PlayerMoveJoystick.highspeed[1].highSpeed && musicEpic == false && !changingMusic)
            {
                //start corutine
                musicEpic = true;
                musicSlow = false;
                StopAllCoroutines();

                StartCoroutine(StartFade("epicVolume", 0.5f, 1f));
                StartCoroutine(StartFade("slowVolume", 0.5f, -80f));



            }
            else if (!PlayerMoveJoystick.highspeed[1].highSpeed && !changingMusic)
            {
                if (musicSlow == false)
                {
                    //start other corutine
                    musicEpic = false;
                    musicSlow = true;
                    StopAllCoroutines();

                    StartCoroutine(StartFade("epicVolume", 0.25f, -80f));
                    StartCoroutine(StartFade("slowVolume", 0.25f, 1f));


                    //  audioMixer.SetFloat("slowVolume", 0f);
                    // audioMixer.SetFloat("epicVolume", -80f);
                    //  StopAllCoroutines();
                    //StartCoroutine("FromEpicToSlow");

                }
            }
            if (PlayerMoveJoystick.highspeed[2].highSpeed&&!PlayerMoveJoystick.highspeed[3].highSpeed)
            {
                audioMixer.SetFloat("MusicPitch", musicSpeedUp2);
            }
            if (PlayerMoveJoystick.highspeed[3].highSpeed)
            {
                audioMixer.SetFloat("MusicPitch", musicSpeedUp3);
            }
            if (!PlayerMoveJoystick.highspeed[3].highSpeed && !PlayerMoveJoystick.highspeed[2].highSpeed)
            {
                audioMixer.SetFloat("MusicPitch", 1f);

            }

        }
    }





    public int getMusicIndex()
    {
        return musicIndex;
    }

    public  IEnumerator StartFade(string exposedParam, float duration, float targetVolume)
    {
//        Debug.Log("ACTIVATED");
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }
    IEnumerator SetMusic(int index)
    {
        StartCoroutine(StartFade("epicVolume", 0.6f, -80f));
        StartCoroutine(StartFade("slowVolume", 0.6f, -80f));
        yield return new WaitForSeconds(0.6f);
        slowAudSource.clip = music[index].slowVersion;
        epicAudSource.clip = music[index].epicVersion;
        slowAudSource.Play();
        epicAudSource.Play();
        changingMusic = false;
        musicEpic = false;
        musicSlow = false;

    }

  public void MuteMusicOptions()
    {
        PlayerPrefs.SetInt("MuteMusic", 1);
        audioMixer.SetFloat("musicVolume", -80);
    }
    public void UnMuteMusicOptions()
    {
        PlayerPrefs.SetInt("MuteMusic",0);
        audioMixer.SetFloat("musicVolume", -11);

    }
    public void MuteSFXOptions()
    {
        PlayerPrefs.SetInt("MuteSFX", 0);
        audioMixer.SetFloat("sfxVolume", -80);

    }
    public void UnMuteSFXOptions()
    {
        PlayerPrefs.SetInt("MuteSFX", 1);
        audioMixer.SetFloat("sfxVolume", -4);
   
    }
    public void MuteEverything()
    {
        audioMixer.SetFloat("MasterVolume", -80f);

    }
    public void UnMuteEverything()
    {
        audioMixer.SetFloat("MasterVolume", 0f);

    }
}
