using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class LevelMaker : MonoBehaviour
{


    float maxPointUsage =800f;
    [SerializeField] Vector2 minMaxObstacleNumber;
    [SerializeField] Vector2 minMaxEnemyNumber;
    [SerializeField] Vector2 minMaxCoinNumber;


    [SerializeField] Transform[] pointsObstacles;
    [SerializeField] Transform[] pointsEnemies;
    [SerializeField] Transform[] pointsCoins;

    [SerializeField] bool testingMax = false;
    public static float nextAllowedHeartScore = -50f;
    public static float nextAllowedDiamondScore = -50f;
    public static bool checkedLoad = false;

    const float heartDistanceMinimum = 10f;
    const float diamondDistanceMinimum = 10f;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Checkpoint", 0) == 1)
        {
            nextAllowedHeartScore = SaveLoad.Instance.checkpoint.nextAllowedHeart;
            nextAllowedDiamondScore = SaveLoad.Instance.checkpoint.nextAllowedDiamond;
        }

            /*
        _numbOfObstacles = Random.Range(Mathf.RoundToInt(minMaxObstacleNumber.x), Mathf.RoundToInt(minMaxObstacleNumber.y));
        _numbOfEnemies = Random.Range(Mathf.RoundToInt(minMaxEnemyNumber.x), Mathf.RoundToInt(minMaxEnemyNumber.y));

        int[] previouslyPickedNumbers = new int[_numbOfObstacles];
        for(int i = 0; i < _numbOfObstacles; i++)
        {
            int randomNumber=Random.Range(0, HandPlacedStuff.Length);
            int randomNumberForPoints = Random.Range(0, pointsObstacles.Length);
            while (checkArray(previouslyPickedNumbers, randomNumberForPoints))
            {
                randomNumberForPoints= Random.Range(0, pointsObstacles.Length);
            }

            previouslyPickedNumbers[i] = randomNumberForPoints;
            Instantiate(HandPlacedStuff[randomNumber], pointsObstacles[randomNumberForPoints]);
        }
        */
            /*
            if (LevelGenerator.numberOfCurrentSpawnedRoom > 262)
            {
                int randomWillSpawn = Random.Range(0, 3); // [0,1,2]
                if (randomWillSpawn >= 1)
                {
                    int randomObs = Random.Range(0, enemyObstacleContainer.Instance.GetPlatformObstacles().Length);

                    makeLevel(minMaxObstacleNumber, pointsObstacles, enemyObstacleContainer.Instance.GetPlatformObstacles()[randomObs]);

                }
                if (randomWillSpawn <= 1)
                {
                    int randomObs = Random.Range(0, enemyObstacleContainer.Instance.GetSpawnableObstacles().Length);

                    makeLevel(minMaxEnemyNumber, pointsEnemies, enemyObstacleContainer.Instance.GetSpawnableObstacles()[randomObs]);

                }




            }
            else
            {*/
            makeLevel(minMaxObstacleNumber, pointsObstacles, enemyObstacleContainer.Instance.GetPlatformObstacles());

            makeLevel(minMaxEnemyNumber, pointsEnemies, enemyObstacleContainer.Instance.GetSpawnableObstacles());
            //makeLevel();
      //  }
        makeLevel(minMaxCoinNumber, pointsCoins, enemyObstacleContainer.Instance.GetCoin(),
            enemyObstacleContainer.Instance.GetHeart(), enemyObstacleContainer.Instance.GetStar(),enemyObstacleContainer.Instance.GetDiamond());

      //  StartCoroutine(changeLevelTrailerShot());


    }

    IEnumerator changeLevelTrailerShot()
    {
        while (true)
        {
            for (int i = 0; i < pointsObstacles.Length; i++)
            {
                if (pointsObstacles[i].childCount >= 3)
                {
                    Destroy(pointsObstacles[i].GetChild(2).gameObject);
                }
            }
            for (int i = 0; i < pointsEnemies.Length; i++)
            {
                if (pointsEnemies[i].childCount >= 1)
                {
                    Destroy(pointsEnemies[i].GetChild(0).gameObject);
                }
            }

            makeLevel(minMaxObstacleNumber, pointsObstacles, enemyObstacleContainer.Instance.GetPlatformObstacles());

            makeLevel(minMaxEnemyNumber, pointsEnemies, enemyObstacleContainer.Instance.GetSpawnableObstacles());
            enemyObstacleContainer.Instance.RandomAddObstacle();
            enemyObstacleContainer.Instance.RandomAddSpawnable();
            yield return new WaitForSeconds(0.5f);

        }


    }
 
    
    void makeLevel(Vector2 minMaxNumberOfPoints,Transform[] Points, GameObject[] StuffToSpawn)
    {
        if (StuffToSpawn.Length>0)
        {
            int _objectToSpawnIndex;
            int minByScore = Mathf.RoundToInt(Mathf.Lerp(minMaxNumberOfPoints.x, minMaxNumberOfPoints.y-1, PlayerScoreManager.score / maxPointUsage));

            // int _numbOfChoosenPoints = Random.Range(Mathf.RoundToInt(minMaxNumberOfPoints.x), Mathf.RoundToInt(minMaxNumberOfPoints.y));
            //old way
             int _numbOfChoosenPoints = Random.Range(minByScore, Mathf.RoundToInt(minMaxNumberOfPoints.y));
            if (testingMax)
            {
                _numbOfChoosenPoints = Mathf.RoundToInt(minMaxObstacleNumber.y);
            }
            int[] randomPoints = Shuffle(Points.Length);
            for (int i = 0; i < _numbOfChoosenPoints; i++)
            {
                _objectToSpawnIndex = Random.Range(0, StuffToSpawn.Length);


                Instantiate(StuffToSpawn[_objectToSpawnIndex], Points[randomPoints[i]]);

            }
        }
    }
    void makeLevel(Vector2 minMaxNumberOfPoints, Transform[] Points, GameObject[] StuffToSpawn,GameObject heart,GameObject starPowerUp,GameObject diamond)
    {
        if (StuffToSpawn.Length > 0)
        {
            int _objectToSpawnIndex;
            int minByScore = Mathf.RoundToInt(Mathf.Lerp(minMaxNumberOfPoints.x, minMaxNumberOfPoints.y - 1, PlayerScoreManager.score / maxPointUsage));

            // int _numbOfChoosenPoints = Random.Range(Mathf.RoundToInt(minMaxNumberOfPoints.x), Mathf.RoundToInt(minMaxNumberOfPoints.y));
            //old way
            int _numbOfChoosenPoints = Random.Range(minByScore, Mathf.RoundToInt(minMaxNumberOfPoints.y));
            if (testingMax)
            {
                _numbOfChoosenPoints = Mathf.RoundToInt(minMaxObstacleNumber.y);
            }
            int[] randomPoints = Shuffle(Points.Length);
            for (int i = 0; i < _numbOfChoosenPoints; i++)
            {
                _objectToSpawnIndex = Random.Range(0, StuffToSpawn.Length);

                int randomPowerUpNumber = Mathf.RoundToInt(Random.Range(1, 77));
                if (randomPowerUpNumber == 11&&PlayerScoreManager.score>=nextAllowedHeartScore)
                {
                    nextAllowedHeartScore = PlayerScoreManager.score + heartDistanceMinimum;
                    Debug.LogWarning("HEART next allowed:" + nextAllowedHeartScore + "   curScore:" + PlayerScoreManager.score);
                    Instantiate(heart, Points[randomPoints[i]]);
                    continue;
                }
                if (randomPowerUpNumber == 7)
                {
                    Debug.LogWarning("STAR");
                    Instantiate(starPowerUp, Points[randomPoints[i]]);
                    continue;
                }
                if (randomPowerUpNumber == 42 && PlayerScoreManager.score >= nextAllowedDiamondScore)
                {
                    nextAllowedDiamondScore = PlayerScoreManager.score + diamondDistanceMinimum;
                    Debug.LogWarning("Diamondo");
                    Instantiate(diamond, Points[randomPoints[i]]);
                    continue;
                }
                Instantiate(StuffToSpawn[_objectToSpawnIndex], Points[randomPoints[i]]);

            }
        }
    }

    void makeLevel(Vector2 minMaxNumberOfPoints, Transform[] Points, GameObject StuffToSpawn)
    {
        /*
            int _objectToSpawnIndex;
            int _numbOfChoosenPoints = Random.Range(Mathf.RoundToInt(minMaxNumberOfPoints.x), Mathf.RoundToInt(minMaxNumberOfPoints.y));
            if (testingMax)
            {
                _numbOfChoosenPoints = Mathf.RoundToInt(minMaxEnemyNumber.y);
            }
            int[] randomPoints = Shuffle(Points.Length);
            for (int i = 0; i < _numbOfChoosenPoints; i++)
            {
               

                Instantiate(StuffToSpawn, Points[randomPoints[i]]);

            }
        */
        int minByScore = Mathf.RoundToInt(Mathf.Lerp(minMaxNumberOfPoints.x, minMaxNumberOfPoints.y - 1, PlayerScoreManager.score / maxPointUsage));

        // int _numbOfChoosenPoints = Random.Range(Mathf.RoundToInt(minMaxNumberOfPoints.x), Mathf.RoundToInt(minMaxNumberOfPoints.y));
        //old way
        int _numbOfChoosenPoints = Random.Range(minByScore, Mathf.RoundToInt(minMaxNumberOfPoints.y));
        if (testingMax)
        {
            _numbOfChoosenPoints = Mathf.RoundToInt(minMaxObstacleNumber.y);
        }
        int[] randomPoints = Shuffle(Points.Length);
        for (int i = 0; i < _numbOfChoosenPoints; i++)
        {
            //_objectToSpawnIndex = Random.Range(0, StuffToSpawn.Length);


            Instantiate(StuffToSpawn, Points[randomPoints[i]]);

        }

    }

    bool checkArray(List<int> array,int value)
    {
        bool hasValue = false;
        if (array.Count != 0)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i] == value)
                {
                    hasValue = true;
                }
            }
        }
        return hasValue;
    }
    int[] Shuffle(int max)
    {
        int[] shuffledArray = new int[max];
        for (int i = 0; i < max; i++)
        {
            shuffledArray[i] = i;
        }
        for (int i = 0; i < max; i++)
        {
            int shuffleNumber = Random.Range(0, max);
            int a = shuffledArray[i];
            shuffledArray[i] = shuffledArray[shuffleNumber];
            shuffledArray[shuffleNumber] = a;
        }
        return shuffledArray;
    }
   
}
