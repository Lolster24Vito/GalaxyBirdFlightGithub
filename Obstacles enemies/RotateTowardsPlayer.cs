using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    float targetAngle;
    Transform player;
    public bool rotate = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rotate)
        {
            Vector2 dirToPlayer = (player.position - transform.position).normalized;
            targetAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle), rotationSpeed * Time.fixedDeltaTime);
        }
    }
    public void StopRotating()
    {
        rotate = false;
    }
    public void ContinueRotating()
    {
        rotate = true;
    }
}
