using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class backgroundColorChange : MonoBehaviour
{
    [System.Serializable]
    public struct ScoreColor
    {
        [Header("NEW COLORS")]
        public Color startingColor;
        public float startingColorScore;
        [Space(25)] // 25 pixels of spacing here.
        public Color endingColor;
        public float endingColorScore;


    }
    [SerializeField] int currentIndexBackground=0;
    [SerializeField] int currentIndexTiles = 0;

    [SerializeField] Material mat;
    [SerializeField] ScoreColor[] backgroundColors;
    [SerializeField] ScoreColor[] tileColors;
    [SerializeField] TilemapRenderer tilemapRenderer;



    Camera camera;

    float score=0f;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
        camera.backgroundColor = backgroundColors[0].startingColor;
        tilemapRenderer.sharedMaterial.SetColor("_Color", tileColors[0].startingColor);
        // mat.SetColor(Shader.PropertyToID("_Color"), tileColors[0].startingColor);

    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (currentIndexBackground < backgroundColors.Length)
        {
            ColorLerpBackgroundByScore(backgroundColors[currentIndexBackground]);
        }

        if (currentIndexTiles < tileColors.Length)
        {
            colorLerpTilesByScore(tileColors[currentIndexTiles]);
        }
    }

    void ColorLerpBackgroundByScore(ScoreColor score)
    {
        float lenght = score.endingColorScore - score.startingColorScore;
        float curScore = PlayerScoreManager.score-score.startingColorScore;
        if (curScore >= 0) { 
        float lerpT = curScore / lenght;
        camera.backgroundColor = Color.Lerp(score.startingColor, score.endingColor, lerpT);
        if (camera.backgroundColor.Equals(score.endingColor))
        {
            currentIndexBackground++;
        }
        }
    }
    void colorLerpTilesByScore(ScoreColor score)
    {
        float lenght = score.endingColorScore - score.startingColorScore;
        float curScore = PlayerScoreManager.score - score.startingColorScore;
        if (curScore >= 0)
        {
            float lerpT = curScore / lenght;
            tilemapRenderer.sharedMaterial.SetColor("_Color", Color.Lerp(score.startingColor, score.endingColor, lerpT));
         //   mat.SetColor(Shader.PropertyToID("_Color"), Color.Lerp(score.startingColor, score.endingColor, lerpT));
            if (lerpT>=1)
            {
                currentIndexTiles++;
            }
        }
    }
}
