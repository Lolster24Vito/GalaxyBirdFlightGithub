using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScoreManager : MonoBehaviour
{
   public static PlayerScoreManager Instance;
    private Transform player;
    private TMP_Text scoreText;
    public static float score;
    private float _score;
    float previousScore=0;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        scoreText = this.GetComponent<TMP_Text>();
        scoreText.text = "0.0";
        if (PlayerPrefs.GetInt("Checkpoint", 0) == 1)
        {
            score = SaveLoad.Instance.checkpoint.score;
        }
        else
        {


            score = 0f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // score = player.position.y / 96;

        _score= player.position.y / 96;
        if (_score > previousScore)
        {
            score = player.position.y / 96;
            scoreText.text = score.ToString("0.0");
            previousScore = score;
        }
        }
   public  void resetScore()
    {
        scoreText.text = "0.0";
        previousScore = 0;
        score = 0;

    }
}
