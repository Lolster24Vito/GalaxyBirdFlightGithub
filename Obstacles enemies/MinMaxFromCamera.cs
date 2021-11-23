using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxFromCamera : MonoBehaviour
{
    [SerializeField] Vector2 yMinMax;
    [SerializeField] Vector2 xMinMax;
    Transform camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector2(
            Mathf.Clamp(pos.x, camera.position.x + xMinMax.x, camera.position.x + xMinMax.y),
            Mathf.Clamp(pos.y, camera.position.y + yMinMax.x, camera.position.y + yMinMax.y));
    }

    public void setYmin(float value)
    {
        yMinMax.x = value;
    }
    public void setYMax(float value){
        yMinMax.y = value;
    }

    public void setYminSmooth(float value)
    {
        StartCoroutine(lerpYMin(value, 0.5f));
    }
    public void setYMaxSmooth(float value)
    {
        StartCoroutine(lerpYMax(value, 0.5f));
    }
    IEnumerator lerpYMin(float value,float lerpDuration)
    {
        float min = yMinMax.x;
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            yMinMax.x = Mathf.Lerp(min, value, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        //yMinMax.x = value;


    }
    IEnumerator lerpYMax(float value, float lerpDuration)
    {
        float max = yMinMax.y;
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            yMinMax.y = Mathf.Lerp(max, value, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        //yMinMax.x = value;


    }

}
