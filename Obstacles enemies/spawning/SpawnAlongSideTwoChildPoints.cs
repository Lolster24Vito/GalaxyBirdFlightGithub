using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpawnAlongSideTwoChildPoints : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject idkTest;
    [SerializeField] float offset;
    [SerializeField] bool testing=false;

    Vector2[] PointsFromParent=new Vector2[2];

    void Start()
    {
        testing = false;
        PointsFromParent[0] = transform.parent.GetChild(0).position;
        PointsFromParent[1] = transform.parent.GetChild(1).position;

        Vector2 dif = (PointsFromParent[0] - PointsFromParent[1]);
        int numberOfTimesItWillRepeat = Mathf.RoundToInt(Vector2.Distance(PointsFromParent[0],PointsFromParent[1])/offset);
        Vector2 dir = dif.normalized;
        Vector2 spawnPos = PointsFromParent[0];
        float lerpAmount = 1f / numberOfTimesItWillRepeat;
        float lerpHelper = 0;
        for (int i = 0; i <numberOfTimesItWillRepeat; i++)
        {
            spawnPos=Vector2.Lerp(PointsFromParent[0],PointsFromParent[1], lerpHelper);
            GameObject idkkk = Instantiate(idkTest, transform);
            idkkk.transform.position = spawnPos;
            lerpHelper += lerpAmount;
        }
       




        /*  SpriteShapeController parentSpriteShape = transform.parent.GetComponent<SpriteShapeController>();
          SpriteShapeController gbSp = gameObject.GetComponent<SpriteShapeController>();
          getPoints(parentSpriteShape, gbSp);
     */
    }
    private void OnValidate()
    {
        if (testing)
        {

            PointsFromParent[0] = transform.parent.GetChild(0).position;
            PointsFromParent[1] = transform.parent.GetChild(1).position;

            Vector2 dif = (PointsFromParent[0] - PointsFromParent[1]);
            int numberOfTimesItWillRepeat = Mathf.RoundToInt(Vector2.Distance(PointsFromParent[0], PointsFromParent[1]) / offset);
            Vector2 dir = dif.normalized;
            Vector2 spawnPos = PointsFromParent[0];
            float lerpAmount = 1f / numberOfTimesItWillRepeat;
            float lerpHelper = 0;
            for (int i = 0; i < numberOfTimesItWillRepeat; i++)
            {
                spawnPos = Vector2.Lerp(PointsFromParent[0], PointsFromParent[1], lerpHelper);
                GameObject idkkk = Instantiate(idkTest, transform);
                idkkk.transform.position = spawnPos;
                lerpHelper += lerpAmount;
            }
        }
    }


    /*
     void getPoints(SpriteShapeController copyFrom,SpriteShapeController pasteTo)
     {
       pasteTo.spline.Clear();
         int numberOfPoints= copyFrom.spline.GetPointCount();
         pasteTo.spline.Clear();

           for (int i = 0; i < numberOfPoints; i++)
           {
               Vector3 pos = copyFrom.spline.GetPosition(i);
             pasteTo.spline.InsertPointAt(i, pos); 
           }

     }*/
}
