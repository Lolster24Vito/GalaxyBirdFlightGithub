using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;


[System.Serializable]
public struct Obstacle
{
    public GameObject gameObject;
    public Sprite sprite;
}

public class enemyObstacleContainer : MonoBehaviour
{





    [SerializeField] List<Obstacle> platformObstacles1 = new List<Obstacle>();
    [SerializeField] List<Obstacle> spawnableObstacles1 = new List<Obstacle>();

    [SerializeField] List<GameObject> playerPlatformObstacles=new List<GameObject>();
    [SerializeField] List<GameObject> playerSpawnableObstacles=new List<GameObject>();

    [SerializeField] List<int> playerPlatformObstaclesIndexes = new List<int>();
    [SerializeField] List<int> playerSpawnableObstaclesIndexes = new List<int>();


    [Space(20)]
    [SerializeField] GameObject[] Coin;
    [SerializeField] GameObject HeartCoin;
    [SerializeField] GameObject Star;
    [SerializeField] GameObject Diamond;


    public static enemyObstacleContainer Instance;
    [SerializeField] bool testingEverything=false;
    [SerializeField] int numOfRandomObstaclesAtStart = 2;
    [SerializeField] int numOfRandomSpawnableAtStart = 2;

    [Space(20)]
    [SerializeField] Image[] obstacleImages;
        int signCounter = 0;


    // Start is called before the first frame update
    void Awake()
    {


        if (Instance==null)
        Instance = this;
        else
        {
            Destroy(this);
        }

    }
    private void Start()
    {
        if (testingEverything)
        {

            for (int i = 0; i < platformObstacles1.Count; i++)
            {
                playerPlatformObstacles.Add(platformObstacles1[i].gameObject);
            }
            for (int i = 0; i < spawnableObstacles1.Count; i++)
            {
                playerSpawnableObstacles.Add(spawnableObstacles1[i].gameObject);
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("Checkpoint", 0) == 0)
            {
                for (int i = 0; i < numOfRandomObstaclesAtStart; i++)
                {
                    RandomAddObstacle();
                    // playerPlatformObstacles.Add(platformObstacles[Random.Range(0, platformObstacles.Count)]);
                }
                for (int i = 0; i < numOfRandomSpawnableAtStart; i++)
                {
                    RandomAddSpawnable();

                    //  playerSpawnableObstacles.Add(spawnableObstacles[Random.Range(0, spawnableObstacles.Count)]);
                }
            }
            else
            {
                CheckpointObject checkpoint = SaveLoad.Instance.checkpoint;
                playerSpawnableObstaclesIndexes=checkpoint.playerSpawnableObstaclesRandomIndexes;
                playerPlatformObstaclesIndexes=checkpoint.playerPlatformObstaclesRandomIndexes;
                for (int i = 0; i < checkpoint.playerPlatformObstaclesRandomIndexes.Count; i++)
                {
                    addPlatformObstacle(checkpoint.playerPlatformObstaclesRandomIndexes[i]);

                }
                for (int i = 0; i < checkpoint.playerSpawnableObstaclesRandomIndexes.Count; i++)
                {
                    addSpawnableObstacle(checkpoint.playerSpawnableObstaclesRandomIndexes[i]);
                }

            }
        }
    }

    private void Update()
    {
    }

    // Update is called once per frame
    
  
    public GameObject[] GetPlatformObstacles()
    {
        return playerPlatformObstacles.ToArray();
    }
    public List<GameObject> GetPlatformObstaclesList()
    {
        return playerPlatformObstacles;
    }

    public GameObject[] GetSpawnableObstacles()
    {
        return playerSpawnableObstacles.ToArray();
    }
    public List<GameObject> GetSpawnableObstaclesList()
    {
        return playerSpawnableObstacles;
    }

    public List<Obstacle> getAllPlatformObstacles()
    {
        return platformObstacles1;
    }
    public List<Obstacle> getAllSpawnableObstacles()
    {
        return spawnableObstacles1;
    }
    public List<int> getPlatformIndexes()
    {
        return playerPlatformObstaclesIndexes;
    }
    public List<int> getSpawnableIndexes()
    {
        return playerSpawnableObstaclesIndexes;
    }

    public void RandomAddObstacle()
    {
        //OLD code
        /*
        if (platformObstacles.Count > 0)
        {
            int random = Random.Range(0, platformObstacles.Count);
            playerPlatformObstacles.Add(platformObstacles[random]);
            platformObstacles.RemoveAt(random);
        }
        */ 
        //NEW CODE
        
        if (platformObstacles1.Count > 0)
        {
            int random = Random.Range(0, platformObstacles1.Count);
            playerPlatformObstacles.Add(platformObstacles1[random].gameObject);
            playerPlatformObstaclesIndexes.Add(random);
            ShowObstacleOnSign(platformObstacles1[random].sprite);
            platformObstacles1.RemoveAt(random);

        }
    }
    public void addPlatformObstacle(int index)
    {
        if (platformObstacles1.Count > 0)
        {
            playerPlatformObstacles.Add(platformObstacles1[index].gameObject);
            ShowObstacleOnSign(platformObstacles1[index].sprite);
            platformObstacles1.RemoveAt(index);
        }
    }
    public void addSpawnableObstacle(int index)
    {
        if (spawnableObstacles1.Count > 0)
        {
            playerSpawnableObstacles.Add(spawnableObstacles1[index].gameObject);
            ShowObstacleOnSign(spawnableObstacles1[index].sprite);
            spawnableObstacles1.RemoveAt(index);

        }
    }

    public void RandomAddSpawnable()
    {
        //OLD CODE
        /*
        if (spawnableObstacles.Count > 0)
        {
            int random = Random.Range(0, spawnableObstacles.Count);
            playerSpawnableObstacles.Add(spawnableObstacles[random]);
            spawnableObstacles.RemoveAt(random);
        }*/
        //NEW CODE
        if (spawnableObstacles1.Count > 0)
        {
            int random = Random.Range(0, spawnableObstacles1.Count);
            playerSpawnableObstacles.Add(spawnableObstacles1[random].gameObject);
            playerSpawnableObstaclesIndexes.Add(random);
            ShowObstacleOnSign(spawnableObstacles1[random].sprite);
            spawnableObstacles1.RemoveAt(random);

        }

    }
    public GameObject[] GetCoin()
    {
        return Coin;
    }
    public GameObject GetHeart()
    {
        return HeartCoin;
    }
    public GameObject GetStar()
    {
        return Star;

    }
    public GameObject GetDiamond()
    {
        return Diamond;
    }
    void ShowObstacleOnSign(Sprite sprite)
    {
        if (obstacleImages.Length > signCounter)
        {
            obstacleImages[signCounter].sprite = sprite;
            signCounter++;
        }
    }
}
