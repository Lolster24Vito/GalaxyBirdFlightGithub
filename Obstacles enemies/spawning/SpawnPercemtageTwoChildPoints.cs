using UnityEngine;
using System.Collections;


public class SpawnPercemtageTwoChildPoints : MonoBehaviour
{
    [SerializeField] GameObject gameObjectToPosition;
    [SerializeField] Vector2 minMaxsegmentsPerPoints = new Vector2(3, 7);
    [SerializeField] Vector3 offset = Vector2.zero;
    [SerializeField] float AngleOffset = 0f;

    [SerializeField] bool testing = false;

    int segmentsPerPoints = 3;

    Vector2[] PointsFromParent = new Vector2[2];


    
    // Start is called before the first frame update
    void Start()
    {
        testing = false;
        PointsFromParent[0] = transform.parent.GetChild(0).position;
        PointsFromParent[1] = transform.parent.GetChild(1).position;

        segmentsPerPoints = Random.Range(Mathf.FloorToInt(minMaxsegmentsPerPoints.x), Mathf.FloorToInt(minMaxsegmentsPerPoints.y));

            Vector2 dir=(PointsFromParent[0] - PointsFromParent[1]).normalized;
            float angle =(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg)-180f;
        angle += AngleOffset;
            //Instantiate(randomPoint, pos[i], Quaternion.Euler(Vector3.forward*angle));
            //Instantiate(randomPoint, pos[i+1], Quaternion.Euler(Vector3.forward * angle));

            float lerpT = 1 / (segmentsPerPoints + 1f);
            for (float j = lerpT; j < 1f; j = j +lerpT )
            {
                Vector3 midPoint = Vector3.Lerp(PointsFromParent[0], PointsFromParent[1], j);
                GameObject _obstacle=Instantiate(gameObjectToPosition, midPoint, Quaternion.Euler(Vector3.forward * angle),transform);
                _obstacle.transform.localPosition += offset;

            }



        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnValidate()
    {
        if (testing)
        {

            foreach (Transform child in transform)
            {
                StartCoroutine(Destroy(child.gameObject));
            }


            PointsFromParent[0] = transform.parent.GetChild(0).position;
            PointsFromParent[1] = transform.parent.GetChild(1).position;

            segmentsPerPoints = Random.Range(Mathf.FloorToInt(minMaxsegmentsPerPoints.x), Mathf.FloorToInt(minMaxsegmentsPerPoints.y));

            Vector2 dir = (PointsFromParent[0] - PointsFromParent[1]).normalized;
            float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 180f;
            angle += AngleOffset;
            //Instantiate(randomPoint, pos[i], Quaternion.Euler(Vector3.forward*angle));
            //Instantiate(randomPoint, pos[i+1], Quaternion.Euler(Vector3.forward * angle));
           
                float lerpT = 1 / (segmentsPerPoints + 1f);
                for (float j = lerpT; j < 1f; j = j + lerpT)
                {
                    Vector3 midPoint = Vector3.Lerp(PointsFromParent[0], PointsFromParent[1], j);
                    GameObject _obstacle = Instantiate(gameObjectToPosition, midPoint, Quaternion.Euler(Vector3.forward * angle), transform);
                    _obstacle.transform.localPosition += offset;

                }
            
           


        }
    }

    //for the editor
    IEnumerator Destroy(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }
    /*
    void getPoints(SpriteShapeController copyFrom, Vector3[] pasteTo)
    {
        int numberOfPoints = copyFrom.spline.GetPointCount();

        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector3 pos = copyFrom.spline.GetPosition(i);
            pasteTo[i]= pos+transform.parent.position;
        }

    }*/
}
