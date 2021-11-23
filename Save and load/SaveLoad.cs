using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;
using TMPro;

public class SaveLoad : MonoBehaviour
{
    private static SaveLoad _instance;

    public static SaveLoad Instance { get { return _instance; } }
    float timer = 0.0f;
    [SerializeField] GameObject savingText;
    public int shopIndexValue;
    [Header("Save")]
    [SerializeField] public SaveObjectList lSave;
    [SerializeField] public TextMeshProUGUI timeAtEndUI;
    string filePath;
    [Header("Checkpoint")]
    [SerializeField]public CheckpointObject checkpoint;
    [SerializeField] public bool dontUseSave = false;
    bool saveWait=false;


    private Transform player;
    private PlayerMoveJoystick playerJoystick;
    Coroutine saveIenumerator;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        filePath = Application.persistentDataPath + "/finished.gd";
        if (dontUseSave)
        {
            PlayerPrefs.SetInt("Checkpoint", 0);
        }
        loadCheckpoint();
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerJoystick = player.GetComponent<PlayerMoveJoystick>();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        LoadFinishedBirds();
        saveWait = true;
        yield return new WaitForSeconds(3f);
        saveWait = false;
        // saveCheckPoint();

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            //save Current score and obstacles
            if(checkpoint!=null)
            writeCheckPoint();

        }
    }
    private void OnApplicationQuit()
    {
        if (checkpoint != null)
            writeCheckPoint();

    }

    public void TriggerFinish()
    {
        SaveFinishedGame(PlayerStateManager.Instance.currentSkin,
        timer, shopIndexValue);


    }
    public void SetshopIndexValue(int index)
    {
        shopIndexValue = index;
    }

    public void saveCheckPoint()
    {
        if (!saveWait||(saveWait&&isSavedProperly()))
        {

      

        checkpoint.score=PlayerScoreManager.score;
        checkpoint.teleportScore = PlayerScoreManager.score;
        checkpoint.hearts = PlayerStateManager.health;

        checkpoint.playerPlatformObstaclesRandomIndexes = enemyObstacleContainer.Instance.getPlatformIndexes();
        checkpoint.playerSpawnableObstaclesRandomIndexes= enemyObstacleContainer.Instance.getSpawnableIndexes();
        checkpoint.nextAllowedHeart = LevelMaker.nextAllowedHeartScore;
        checkpoint.nextAllowedDiamond = LevelMaker.nextAllowedDiamondScore;

        checkpoint.LevelCounterForObstacles=LevelGenerator.Instance.getLevelCounterForObstacles();
        checkpoint.coinsStartingFromZero = PlayerStateManager.coinsStartingFromZero;
        checkpoint.startRunCoins = PlayerStateManager.Instance.startRunCoins;
        checkpoint.SpeedPrice = PlayerStateManager.Instance.SpeedPrice;
        checkpoint.HealthPrice = PlayerStateManager.Instance.HealthPrice;
        checkpoint.GravityPrice = PlayerStateManager.Instance.GravityPrice;
        checkpoint.jumpPower = playerJoystick.jumpPower;
        checkpoint.gravityScale = playerJoystick.gravityScale;
        checkpoint.musicIndex = MusicManager.Instance.getMusicIndex();
            checkpoint.timer = timer;
            checkpoint.savedProperly = 22.22f;
        PlayerPrefs.SetInt("Checkpoint", 1);

        AdsManager.Instance.saveAdTimers();

            if (saveIenumerator!=null)StopCoroutine(saveIenumerator);
            saveIenumerator = StartCoroutine(textSaving());
            writeCheckPoint();
        }
        else
        {
        }

    }
    private void writeCheckPoint()
    {
        BinaryFormatter bf = new BinaryFormatter();

        using (var file = File.Open(Application.persistentDataPath + "/check.gd", FileMode.OpenOrCreate))
        {

            bf.Serialize(file, checkpoint);
        }

    }
    IEnumerator textSaving()
    {
        savingText.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        savingText.SetActive(false);
        saveIenumerator = null;
    }
    public void deleteCheckpoint()
    {
        checkpoint = null;
        File.Delete(Application.persistentDataPath + "/check.gd");

    }
    public void deleteEverything()
    {
        checkpoint = null;
        File.Delete(Application.persistentDataPath + "/check.gd");
        File.Delete(Application.persistentDataPath + "/finished.gd");

    }


    private void loadCheckpoint()
    {
        if (File.Exists(Application.persistentDataPath + "/check.gd"))
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/check.gd", FileMode.Open);
                if (file.Length != 0)
                {


                    //Debug.Log("OKIiiii 3");


                    checkpoint = (CheckpointObject)bf.Deserialize(file);
                    //Debug.Log("OKIiiii 4");
                    if (checkpoint != null)
                    {
                        timer += checkpoint.timer;
                      //  Debug.Log("OKI poki 5");




                    }
                    else
                    {
                        PlayerPrefs.SetInt("Checkpoint", 0);
                    }
                    if (!isSavedProperly())
                    {
                        //Debug.Log("Caught you");
                        PlayerPrefs.SetInt("Checkpoint", 0);
                    }
                }
                file.Close();
            }
            catch (SerializationException)
            {
                PlayerPrefs.SetInt("Checkpoint", 0);
                //Debug.LogWarning("GOTTIIIII Finally i have found the corruption");
            }
            

        }



    }
    public void SaveFinishedGame
       (SkinItem skinitemm, float timeFinishedd, int shopIndexValuee)
    {
       
        if (lSave == null)
        {

            lSave = new SaveObjectList();
            lSave.lSaveObjects = new List<SaveObject>();
            //Debug.Log("EYOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        }
        bool shouldAddToList = true;
        for(int i = 0; i < lSave.lSaveObjects.Count; i++)
        {
            if (shopIndexValue == lSave.lSaveObjects[i].shopIndexValue)
            {
                shouldAddToList = false;
                if (timeFinishedd< lSave.lSaveObjects[i].timeFinished)
                {
                    //change time
                    lSave.lSaveObjects.RemoveAt(i);
                    shouldAddToList = true;
                }

            }
        }
        if(shouldAddToList)
        lSave.lSaveObjects.Add(new SaveObject(timeFinishedd, shopIndexValuee));

        BinaryFormatter bf = new BinaryFormatter();

        using (var file = File.Open(Application.persistentDataPath + "/finished.gd", FileMode.OpenOrCreate))
        {
            bf.Serialize(file, lSave);
        }
        // file is automatically closed after reaching the end of the using block
        //  FileStream file = File.Create(Application.persistentDataPath + "/finished.gd"); //you can call it anything you want
        //  Debug.Log("Ok8");
        //  bf.Serialize(file, lSave);
        //  file.Close();

    }
    private bool isSavedProperly()
    {
        if (checkpoint.savedProperly != 22.22f || checkpoint.playerSpawnableObstaclesRandomIndexes.Count == 0)
        {
            return false;
        }
        return true;

    }

    /*
    public void SaveFinishedGame    
        (SkinItem skinitemm,float timeFinishedd,int shopIndexValuee)
    {
        Debug.Log("Ok1");        
        if (lSave == null)
        {
            Debug.Log("Ok2");

            lSave = new SaveObjectList();
            lSave.lSaveObjects = new List<SaveObject>();
            Debug.Log("EYOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        }
        Debug.Log("Ok3");
        Debug.Log(skinitemm.name);
        Debug.Log(timeFinishedd);
        Debug.Log(shopIndexValuee);

        lSave.lSaveObjects.Add(new SaveObject(skinitemm, timeFinishedd, shopIndexValuee));
        Debug.Log("Ok4");

        string json = JsonUtility.ToJson(lSave);
        Debug.Log(json);
        Debug.Log("Ok5");
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        File.WriteAllText(filePath, json);

    }
    */

    public void LoadFinishedBirds()
    {
        if (File.Exists(Application.persistentDataPath + "/finished.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/finished.gd", FileMode.Open);
            if (file.Length != 0)
            {




                lSave = (SaveObjectList)bf.Deserialize(file);
                if (lSave != null)
                {

                    for (int i = 0; i < lSave.lSaveObjects.Count; i++)
                    {

                        //  lSave.lSaveObjects[i].skin = ShopUI.Instance.GetSkin(lSave.lSaveObjects[i].shopIndexValue);
                        ShopUI.Instance.SetCrownOnSkin(lSave.lSaveObjects[i]);
                        if (ShopUI.Instance.GetSkin(lSave.lSaveObjects[i].shopIndexValue).Equals(PlayerStateManager.Instance.currentSkin))
                        {
                            PlayerStateManager.Instance.setCrownOnSkin();
                        }
                    }
                }
            }
            file.Close();

        }
        else
        {
            lSave = new SaveObjectList();
        }

        if (lSave.lSaveObjects.Count > 17)
        {
            ShopUI.Instance.unlockFinalBird();
        }
    }
    /*
     
    public void LoadFinishedBirds()
    {
        if(File.Exists(Application.dataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
            SaveObjectList saveObject = JsonUtility.FromJson<SaveObjectList> (saveString);
            lSave = saveObject;
            if (lSave != null)
            {

                for (int i = 0; i < lSave.lSaveObjects.Count; i++)
                {

                  //  lSave.lSaveObjects[i].skin = ShopUI.Instance.GetSkin(lSave.lSaveObjects[i].shopIndexValue);
                    ShopUI.Instance.SetCrownOnSkin(lSave.lSaveObjects[i]);
                }
            }
        }
        else
        {
            lSave= new SaveObjectList(); 
        }
    }
      */

    public void setTimeOnFinish() {

        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timeAtEndUI.text = niceTime;
    }


}
[System.Serializable]
public class SaveObject
{
   // public SkinItem skin;
    public float timeFinished;
    public int shopIndexValue;

    public SaveObject(float timeFinishedd,
     int shopIndexValuee){
      //  this.skin = skinn;
        this.timeFinished = timeFinishedd;
        this.shopIndexValue = shopIndexValuee;
        }

}
[System.Serializable]
public class SaveObjectList
{
    public List<SaveObject> lSaveObjects;
    public SaveObjectList()
    {
        lSaveObjects = new List<SaveObject>();
    }
}
[System.Serializable]
public class CheckpointObject
{
    public float score;
    public float teleportScore;
    public int hearts;

    public List<int> playerPlatformObstaclesRandomIndexes;
    public List<int> playerSpawnableObstaclesRandomIndexes;
   // public List<Obstacle> platformObstacles1;
    public int musicIndex;
   // public List<Obstacle> spawnableObstacles1;

    public int LevelCounterForObstacles;
    public float nextAllowedHeart;
    public float nextAllowedDiamond;



    public int coinsStartingFromZero;
    public int startRunCoins;

     public Prices SpeedPrice;
     public Prices HealthPrice;
     public Prices GravityPrice;
    public float jumpPower;
    public float gravityScale;

    public float timer;
    public float savedProperly=11.11f;

}