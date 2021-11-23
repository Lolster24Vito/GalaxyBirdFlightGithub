using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct MapPlanet
{
    public RectTransform rTransform;
    public float scoreItAppearsOn;
    public int coinsForPlanet;
    public void SetTransform(float mapMovePerScore, float xZeroPos)
    {
        Vector3 oldPos = rTransform.transform.localPosition;
        rTransform.transform.localPosition = new Vector3(xZeroPos + (mapMovePerScore * scoreItAppearsOn), oldPos.y);
    }
}
public class GameOverMap : MonoBehaviour
{

    public static GameOverMap Instance;
    [SerializeField] public  MapPlanet[] planets;
    [SerializeField] public static float mapMovePerScore= 11f;
    [SerializeField] public static float xZeroPos = -120f;
    float sum = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].SetTransform(mapMovePerScore, xZeroPos);
            sum += planets[i].coinsForPlanet;

        }
    }



}

