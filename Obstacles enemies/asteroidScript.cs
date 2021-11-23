using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroidScript : MonoBehaviour
{
    [SerializeField] float _projectileSpeed = 20f;
    [SerializeField] Vector2 minMaxFromPlayerPos;
    [SerializeField] float distanceTillUnActive = 170f;
    [SerializeField] Vector2 yAxisMinMax;
    [SerializeField] float offset=1.5f;
    [SerializeField] float tempHelper = 0.0f;
    [SerializeField] float xPosMeteor=0.0f;
    [SerializeField] LayerMask checkLayer;
    float checkRadius = 9f;
    Rigidbody2D rb;
    Vector2 startPos;
    Vector2 parentStartPos;
    Transform player;
    
 
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        parentStartPos = transform.parent.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tempHelper < offset)
        {
            tempHelper += Time.fixedDeltaTime;
        }
        else
        {
            rb.MovePosition(transform.position + (transform.up * _projectileSpeed * Time.fixedDeltaTime));

            if (Vector2.Distance(startPos, transform.position) > distanceTillUnActive)
            {
                findPosition();
            }
        }
    }
    void findPosition()
    {
        StartCoroutine(activateINFewSec());
        float yPos = Random.Range(player.position.y + minMaxFromPlayerPos.x, player.position.y + minMaxFromPlayerPos.y);
        for(int i = 0; i < 5; i++)
        {
            if (Physics2D.OverlapCircle(new Vector2(xPosMeteor, yPos), checkRadius,checkLayer))
            {
                Debug.LogWarning(transform.parent.gameObject.name+"___Ypos was crap picked new one for the:" + i);
                yPos = Random.Range(player.position.y + minMaxFromPlayerPos.x, player.position.y + minMaxFromPlayerPos.y);
            }
            else {
                break;
            }
        }
        parentStartPos= new Vector2(parentStartPos.x, Mathf.Clamp(yPos,yAxisMinMax.x,yAxisMinMax.y));
        transform.parent.position = parentStartPos;
        transform.localPosition = Vector2.zero;
        startPos = transform.position;
        

    }
 
    IEnumerator activateINFewSec() {
        transform.GetComponent<TrailRenderer>().enabled = false;
        yield return new WaitForSeconds(0.45f);
        transform.GetComponent<TrailRenderer>().enabled = true;


    }
    private void OnDrawGizmos()
    {
       // Gizmos.DrawSphere(new Vector2(xPosMeteor,transform.parent.position.y), 9f);
    }
}
