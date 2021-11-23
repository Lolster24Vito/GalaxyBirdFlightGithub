using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGravity : MonoBehaviour
{
    [SerializeField] float gravityForce=9.81f;
    [SerializeField] float maxGravityForce = 18f;

    [SerializeField] Transform childGravityChecker;
    [SerializeField] float radius = 1f;
    [Range(0,1)][SerializeField] float gravityIncreasePercentage = 1f;

   public bool inAir = false;


    ConstantForce2D constantForce;
    // Start is called before the first frame update
    void Start()
    {
        constantForce = GetComponent<ConstantForce2D>();

        SetGravityDirection();
            
    }

    // Update is called once per frame
    void Update()
    {
        if (!Physics2D.OverlapCircle(childGravityChecker.position, radius))
        {
            inAir = true;
            constantForce.force =Vector2.ClampMagnitude( constantForce.force+(constantForce.force * Time.deltaTime),maxGravityForce);
            
        }
        else
        {
            if (inAir)
            {
                SetGravityDirection();
                inAir = false;
            }
        }
    }

    public void SetGravityDirection()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up);

        //can use relative force
        constantForce.force = -hit.normal * gravityForce;
        //     Debug.Log("sooo test:" + hit.normal + " name:" + hit.transform.gameObject.name);
        //set angle
        transform.rotation = Quaternion.FromToRotation(Vector3.up,hit.normal);

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(childGravityChecker.position, radius);
    }
}
