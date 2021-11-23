using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateAndMoveToPlayer : MonoBehaviour
{
    Transform player;
    Rigidbody2D rb;
   [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform waypoint;
    [SerializeField] Vector2 dirToRight;
   [SerializeField] bool moveWhenPlayerClose = false;
    [SerializeField] float distanceToMove = 53f;
    [SerializeField] LayerMask raycastCheck;
    [SerializeField] float rotationOffset = -90f;


    bool rayCheck = false;
    int dirHelper = 1;

    int frameHelper = 0;
  [SerializeField]  bool rotate = true;

   float targetAngle;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector2 dirToPlayer = (player.position - transform.position).normalized;

        if (!moveWhenPlayerClose || Vector3.Distance(player.position, transform.position)< distanceToMove)
        {


            if (moveWhenPlayerClose && !rayCheck)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position,dirToPlayer,distanceToMove, raycastCheck);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        rayCheck = true;
                    }
                }
            }
            
            if (rayCheck || !moveWhenPlayerClose)
            {
                dirToRight = (waypoint.position - transform.position).normalized;
                rb.velocity = dirToRight * dirHelper * movementSpeed;
            }
        }
        if (rotate)
        {
            targetAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg + rotationOffset;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle), rotationSpeed * Time.fixedDeltaTime);
        }


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        float rotSpeedMultiplied = rotationSpeed * 3f;
        rotationSpeed = rotSpeedMultiplied;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        float rotSpeedMultiplied = rotationSpeed  /3f;
        rotationSpeed = rotSpeedMultiplied;
    }
    public void StopRotating()
    {
        rotate = false;
    }
    public void ContinueRotating()
    {
        rotate = true;
    }
    public void setRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
}
