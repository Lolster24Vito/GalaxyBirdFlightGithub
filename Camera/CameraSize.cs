using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSize : MonoBehaviour
{

    private static CameraSize _instance;

    public static CameraSize Instance { get { return _instance; } }
    public Vector2 size=new Vector2(56.89885f,100.368f);
   // public SpriteRenderer rink;
    //[SerializeField] Vector2 rinkSize;
    [SerializeField] CinemachineVirtualCamera camera;
    [SerializeField] bool zoomedOutBegining = false;
    public float cameraLensSize;
    [SerializeField] float zoomValue = 2f;

    private bool ZoomOutHelper = false;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    // Use this for initialization
    void Start()
    {
       // rinkSize = rink.bounds.size;
       // Debug.Log("x:"+ rink.bounds.size.x+ "  _y:"+ rink.bounds.size.y);
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio =size.x / size.y;

        if (screenRatio >= targetRatio)
        {
            cameraLensSize = size.y / 2;
            if (zoomedOutBegining)
                camera.m_Lens.OrthographicSize = cameraLensSize;
            else
            {
                camera.m_Lens.OrthographicSize = cameraLensSize/zoomValue;

            }

            //   Camera.main.orthographicSize = rink.bounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            cameraLensSize = size.y / 2 * differenceInSize;
            if (zoomedOutBegining)
                camera.m_Lens.OrthographicSize = cameraLensSize;
            else
                camera.m_Lens.OrthographicSize = cameraLensSize/zoomValue;



            //Camera.main.orthographicSize = rink.bounds.size.y / 2 * differenceInSize;
        }
    }


    public void SetCameraLens(float size)
    {
        camera.m_Lens.OrthographicSize = size;
    }
    public void SetCameraLensToNormal()
    {
        camera.m_Lens.OrthographicSize = cameraLensSize;
    }

    public void ZoomOutLense(float seconds)
    {
        ZoomOutHelper = false;
        StartCoroutine(ZoomOrtographicSmooth(seconds));
    }
    public void ZoomInLense(float seconds)
    {
        ZoomOutHelper = true;
        StartCoroutine(ZoomOrtographicSmooth(seconds));
    }

    public IEnumerator ZoomOrtographicSmooth(  float seconds)
    {
        float elapsedTime = 0;
        float startingPos = camera.m_Lens.OrthographicSize;
        float goToNumber;
        if (ZoomOutHelper)
        {
            goToNumber = cameraLensSize / zoomValue;
        }
        else
        {
            goToNumber = cameraLensSize;

        }
        while (elapsedTime < seconds)
        {
            camera.m_Lens.OrthographicSize = Mathf.Lerp(startingPos, goToNumber, (elapsedTime / seconds));
           
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        camera.m_Lens.OrthographicSize = goToNumber;
    }
}
