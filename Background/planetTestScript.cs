using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetTestScript : MonoBehaviour
{
   [SerializeField] Transform point1;
    [SerializeField] Transform point2;
    [SerializeField] Vector2 scale1;
    [SerializeField] Vector2 scale2;
    [SerializeField] float startScore;
    [SerializeField] float endScore;
    [SerializeField] float lerpTVal;
    [SerializeField] Color color1=Color.white;
    [SerializeField] Color color2= Color.white;
    SpriteRenderer spriteRenderer;
    [SerializeField] List<SpriteRenderer> children = new List<SpriteRenderer>();
    [SerializeField] Color childColor1 = new Color(255, 255, 255, 0);
    [SerializeField] Color childColor2 = Color.white;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<SpriteRenderer>())
            {
                children.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float score=player.position.y / 96;

        lerpTVal = Mathf.Clamp((score - startScore) / (endScore - startScore),0f,1f);
        Vector3 lerpPos= Vector2.Lerp(point1.position, point2.position, lerpTVal);
        transform.position = new Vector3(lerpPos.x, lerpPos.y, lerpPos.z);
        Vector3 lerpScale = Vector2.Lerp(scale1, scale2, lerpTVal);
        spriteRenderer.color = Color.Lerp(color1, color2, lerpTVal);
        if (children.Count > 0)
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].color = Color.Lerp(childColor1, childColor2, lerpTVal);

            }
        }
        //  Debug.Log((PlayerScoreManager.score - startScore) / (endScore - startScore));
        transform.localScale = lerpScale;
        //transform.scale
    }
}
