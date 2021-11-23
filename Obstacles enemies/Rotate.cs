using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 85f;
    [SerializeField] bool randomRotate = true;
    [SerializeField] bool rotate = true;
    // Start is called before the first frame update
    void Start()
    {
        if (randomRotate)
        {
            bool _randomRot = (Random.Range(0, 2) == 0);
            if (_randomRot) rotationSpeed *= -1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rotate)
        {
            transform.Rotate(Vector3.back * Time.fixedDeltaTime * rotationSpeed);
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
