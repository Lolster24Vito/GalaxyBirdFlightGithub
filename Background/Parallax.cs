using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    private float lenght, startpos;
    private float cameraWidth;
    public GameObject cam;
    public float parallaxEffect;
   [SerializeField] Vector2 ClampY_Min_Max;
    [SerializeField] bool repeat=true;
    [SerializeField] float smoothTime;
    Vector3 velocitySmDamp;

    bool newPosition=true;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.gameObject;
        startpos = transform.position.y;
        cameraWidth = cam.GetComponent<Camera>().orthographicSize/2f;
        lenght = GetComponent<SpriteRenderer>().bounds.size.y ;


    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.y * (1 - parallaxEffect));
        float dist = (cam.transform.position.y * parallaxEffect);
        Vector3 targetPos= new Vector3(transform.position.x, Mathf.Clamp(startpos + dist, ClampY_Min_Max.x, ClampY_Min_Max.y), transform.position.z);

        //transform.position = new Vector3(transform.position.x, Mathf.Clamp(startpos + dist,ClampY_Min_Max.x,ClampY_Min_Max.y), transform.position.z);

        //SmoothDamp paralax effect is noticable when changing position.This is the fix
        if (newPosition)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(startpos + dist,ClampY_Min_Max.x,ClampY_Min_Max.y), transform.position.z);
            newPosition = false;
        }
        else {
        transform.position= Vector3.SmoothDamp(transform.position, targetPos, ref velocitySmDamp, smoothTime);
        }
        //backup
        //transform.position = new Vector3(transform.position.x, Mathf.Clamp(startpos + dist,ClampY_Min_Max.x,ClampY_Min_Max.y), transform.position.z);

        if (repeat)
        {
            if (temp > startpos + lenght)
            {
                //Debug.Log("temp:" + temp + " Startpos+lenght:" + (startpos + lenght));
                startpos += lenght;
                newPosition = true;
            }

            else if (temp < startpos - lenght)
            {
                //Debug.Log("l:"+lenght + " Startpos:" + startpos+"temp:"+temp);

                startpos -= lenght;
                newPosition = true;
            }
        }
    }


    /*
      void Start()
    {
        startpos = transform.position.x;
        cameraWidth = cam.GetComponent<Camera>().orthographicSize/2f;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x ;


    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + lenght)
        {
            //Debug.Log("temp:" + temp + " Startpos+lenght:" + (startpos + lenght));
            startpos += lenght;
        }

        else if (temp < startpos - lenght)
        {
            //Debug.Log("l:"+lenght + " Startpos:" + startpos+"temp:"+temp);

            startpos -= lenght;
        }
        
    }
}
 
    
      */
}
