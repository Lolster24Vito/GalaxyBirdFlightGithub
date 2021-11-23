using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelBoss : MonoBehaviour
{
   [SerializeField] Vector2 MinMaxFromCamera;
   [SerializeField] Vector2 minMaxspeed;
    [SerializeField] float endYPos;
    [SerializeField] float startYPos;
    [SerializeField] bool Topside = false;
    [SerializeField] float maxY = 12960f;
    int dirHelper=1;
    Coroutine helperCoroutine;

    float minimumFromCamera;
    float speed;
    Transform player;
    Vector3 originalPos;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        helperCoroutine=StartCoroutine(lerpBossMinimumFromCamera());
        if (Topside) dirHelper = -1;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float lerpAmount=(player.position.y -startYPos) / (endYPos - startYPos);
        if (lerpAmount < 0.99f)
        {
            speed = Mathf.Lerp(minMaxspeed.x, minMaxspeed.y, lerpAmount);
            if(helperCoroutine==null)
            minimumFromCamera = Mathf.Lerp(MinMaxFromCamera.x, MinMaxFromCamera.y, lerpAmount);
        }
        else
        {
            speed = 1f;
            StartCoroutine(lerpBossMinimumFromCameraToMax());
            if (Vector2.Distance(player.position, transform.position) > 70f)
            {
                gameObject.SetActive(false);
            }
        }
        transform.position = new Vector3(originalPos.x, Mathf.Clamp(transform.position.y + (Time.deltaTime * speed), player.position.y - minimumFromCamera*dirHelper, maxY));
    }
    IEnumerator lerpBossMinimumFromCamera()
    {
        minimumFromCamera=120f;
        while (minimumFromCamera > MinMaxFromCamera.x)
        {
            minimumFromCamera -= 0.5f;
            yield return new WaitForEndOfFrame();
        }
        helperCoroutine = null;
    }
    IEnumerator lerpBossMinimumFromCameraToMax()
    {
        while (minimumFromCamera < 120f)
        {
            minimumFromCamera += 1f;
            yield return new WaitForSeconds(0.1f);
        }
        helperCoroutine = null;
    }
}
