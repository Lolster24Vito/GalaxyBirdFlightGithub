using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelGenerator : MonoBehaviour
{
    private static LevelGenerator _instance;
    public static LevelGenerator Instance { get { return _instance; } }

    [SerializeField] OpenSpace previousLevelEndSpace = OpenSpace.L111;
    [SerializeField]public float LevelLenght=50f;
    [SerializeField]public Vector3 startPos = Vector3.zero;
    [SerializeField] int addNewObstacleEvery =5;
    [SerializeField] Transform wallForPlayer;

    [SerializeField] int LevelCounterForObstacles=0;
    [SerializeField] int[] dontSpawnLevelNumbers;
    [SerializeField] Vector2[] minMaxdontSpawnLevelNumbers;

    [SerializeField] int[] dontAddObstaclesLevelNumbers;
    [SerializeField] Vector2[] bossAreaLevelNumbersMinMax;

    public int numberOfCurrentSpawnedRoom = 0;

    [SerializeField] GameObject blankL111level;
    [SerializeField] GameObject shopPrefabAfter500;
    [Header("Levels 111")]
    [SerializeField] public List<Level> levels111;
    [Header("Levels 001")]

    [SerializeField] public List<Level> levels001;
    [Header("Levels 010")]

    [SerializeField] public List<Level> levels010;
    [Header("Levels 100")]

    [SerializeField] public List<Level> levels100;


    private float spawnPositionHelper = 0f;
    [HideInInspector]public GameObject parentGameobject=null;

  [HideInInspector] public GameObject latestLevel=null;


    private Transform player;

    [SerializeField] float levelSpawnDistance = 130f;

    [SerializeField] float scoreTeleportOffset = 10f;

    private void Awake()
    {
        //120 spike circle turret....
        //123345 two sided rotator, granade, fan
        //69 one sided rot,frog,missile

        // Random.InitState(69);

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Start is called before the first frame update
    void Start()
    {

        //THIS SECTION WORKS
        
        
        
        if (PlayerPrefs.GetInt("Checkpoint", 0) == 0)
        {
            numberOfCurrentSpawnedRoom = 0;
            spawnRandomLevel();
        }

        else
        {
            teleportToScoreInSave(SaveLoad.Instance.checkpoint.teleportScore);
        }
        
        //this is the section for debuging
       // teleportToScoreInSave(649f);



        /*
        tempCounter = 0;
        spawnRandomLevel();

        
        spawnRandomLevel();
        spawnRandomLevel();
        spawnRandomLevel();
        spawnRandomLevel();
        spawnRandomLevel();
        spawnRandomLevel();
        spawnRandomLevel();


        player = GameObject.FindGameObjectWithTag("Player").transform;
        latestLevel = parentGameobject.transform.GetChild(parentGameobject.transform.childCount - 1).transform;


    */
        // latestLevel = parentGameobject.transform.GetChild(parentGameobject.transform.childCount - 1).transform;

    }


    // Update is called once per frame
    void Update()
    {

        /*
        if (numberOfCurrentSpawnedRoom >numberOfRooms)
        {
            this.GetComponent<LevelGenerator>().enabled = false;
        }
        //Get last child Transform
        if(Vector3.Distance(player.position, latestLevel.position) < levelSpawnDistance)
        {
            spawnRandomLevel();
            Destroy(parentGameobject.transform.GetChild(0).gameObject);
            latestLevel = parentGameobject.transform.GetChild(parentGameobject.transform.childCount - 1).transform;
        }
        */


            if (Vector3.Distance(player.position, latestLevel.transform.position) < levelSpawnDistance)
            {
                spawnRandomLevel();

            }
            //originally 200
            if (Vector3.Distance(parentGameobject.transform.GetChild(0).position, player.position) > 250f)
            {
                //4 is originally but i changed it to 5
                if (parentGameobject.transform.childCount > 5)
                {
                    wallForPlayer.position = parentGameobject.transform.GetChild(0).position;
                    wallForPlayer.position += Vector3.up * 96;
                    Destroy(parentGameobject.transform.GetChild(0).gameObject);
                }
            }

        


    }
 public   void teleportToScoreInSave(float score)
    {
        //-2 is added so that there is levels below the saved score
        numberOfCurrentSpawnedRoom = Mathf.FloorToInt(score)-2;
        spawnPositionHelper = (LevelLenght* numberOfCurrentSpawnedRoom);
        player.position = new Vector2(0,(score* LevelLenght) + scoreTeleportOffset);
        
        spawnRandomLevel();
        spawnRandomLevel();
        spawnRandomLevel();
        spawnRandomLevel();
        //load obstacles

    }

    public int getLevelCounterForObstacles()
    {
        return LevelCounterForObstacles;
    }

    public void spawnRandomLevel()
    {

        
            switch (previousLevelEndSpace)
            {
                case OpenSpace.L111:
                    spawnLevel(levels111);
                    break;
                case OpenSpace.L001:
                    spawnLevel(levels001);
                    break;
                case OpenSpace.L010:
                    spawnLevel(levels010);
                    break;
                case OpenSpace.L100:
                    spawnLevel(levels100);
                    break;

            
            }
        numberOfCurrentSpawnedRoom++;

        LevelCounterForObstacles++;

        if (LevelCounterForObstacles==addNewObstacleEvery&& CheckIfShouldAddNewObstacle(numberOfCurrentSpawnedRoom))
        {
            //enemyObstacleContainer.Instance.RandomAddSpawnable();
            enemyObstacleContainer.Instance.RandomAddObstacle();
           // Debug.Log("RoomNumber:" + numberOfCurrentSpawnedRoom + " Obstacle");

        }
        if (LevelCounterForObstacles >= addNewObstacleEvery * 2)
        {
            //enemyObstacleContainer.Instance.RandomAddObstacle();
            if (CheckIfShouldAddNewObstacle(numberOfCurrentSpawnedRoom))
            {
                enemyObstacleContainer.Instance.RandomAddSpawnable();
                //Debug.Log("RoomNumber:" + numberOfCurrentSpawnedRoom + " Spawnable");
            }
            LevelCounterForObstacles = 0;
           // addNewObstacleEvery += 1;
        }

    }


    private void spawnLevel(List<Level> level)
    {
        if (parentGameobject == null) parentGameobject = new GameObject("Levels");


        int randomNumber = Random.Range(0, level.Count);
        if (!CheckForBlankRoom(numberOfCurrentSpawnedRoom + 1)||!CheckIfBossArea(numberOfCurrentSpawnedRoom+1))
        {
            randomNumber = 3;

        }
        if (CheckForBlankRoom(numberOfCurrentSpawnedRoom)&&CheckIfBossArea(numberOfCurrentSpawnedRoom))
        {
            latestLevel = Instantiate(level[randomNumber].levelGameObject, new Vector3(startPos.x, spawnPositionHelper + startPos.y, 0), level[randomNumber].levelGameObject.transform.rotation, parentGameobject.transform);
            previousLevelEndSpace = level[randomNumber].levelEndOpenings;
        }
        else
        {
            if (!CheckForBlankRoom(numberOfCurrentSpawnedRoom))
            {
                latestLevel = Instantiate(blankL111level, new Vector3(startPos.x, spawnPositionHelper + startPos.y, 0), blankL111level.transform.rotation, parentGameobject.transform);
                //after 500 to spawn shop automatically
                if (numberOfCurrentSpawnedRoom > 501&& numberOfCurrentSpawnedRoom%50==0)
                {
                    Instantiate(shopPrefabAfter500, new Vector3(startPos.x, 48f+spawnPositionHelper + startPos.y, 0), shopPrefabAfter500.transform.rotation, latestLevel.transform);
                }
            }
            if (!CheckIfBossArea(numberOfCurrentSpawnedRoom))
            {
                latestLevel = Instantiate(levels111[3].levelGameObject, new Vector3(startPos.x, spawnPositionHelper + startPos.y, 0), blankL111level.transform.rotation, parentGameobject.transform);

            }
            previousLevelEndSpace = OpenSpace.L111;

        }
        spawnPositionHelper += LevelLenght;

        // Debug.Log("Spawned level:" + level[randomNumber].levelGameObject+" With level end:"+ level[randomNumber].levelEndOpenings+"And position:"+ helper + startPos.x);

        //quick remove temporary

    }
    bool CheckForBlankRoom(int levelNumber)
    {

        if (levelNumber > 501&&(levelNumber%50==0||(levelNumber+1)%50==0)|| (levelNumber - 1) % 50 == 0)
        {
            return false;
        }

        for(int i = 0; i < dontSpawnLevelNumbers.Length; i++)
        {
            if (dontSpawnLevelNumbers[i] == levelNumber)
            {
                return false;
            }
        }

        for (int i = 0; i < minMaxdontSpawnLevelNumbers.Length; i++)
        {
            if (minMaxdontSpawnLevelNumbers[i].x < levelNumber && minMaxdontSpawnLevelNumbers[i].y > levelNumber)
            {
                return false;
            }
        }

        return true;

    }
    bool CheckIfBossArea(int levelNumber)
    {
        for (int i = 0; i < bossAreaLevelNumbersMinMax.Length; i++)
        {
            if (levelNumber > bossAreaLevelNumbersMinMax[i].x && levelNumber < bossAreaLevelNumbersMinMax[i].y)
            {
                return false;
            }
        }
        return true;
    }
    bool CheckIfShouldAddNewObstacle(int levelNumber)
    {
        if (levelNumber > 500) return false;

        for (int i = 0; i < dontAddObstaclesLevelNumbers.Length; i++)
        {
            if (dontAddObstaclesLevelNumbers[i] == levelNumber)
            {
                return false;
            }
        }
        return true;

    }




}
