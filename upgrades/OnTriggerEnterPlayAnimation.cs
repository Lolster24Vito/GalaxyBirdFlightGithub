using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterPlayAnimation : MonoBehaviour
{
    [SerializeField] string animationName = "Sleeping";
    [SerializeField] public UnityEvent OnPlayerEnterTrigger;
    [SerializeField] public UnityEvent OnPlayerLeaveTrigger;
    private Animator anim;


    // Start is called before the first frame update
    private void Start()
    {
        if (transform.GetComponent<Animator>())
        {
            anim = GetComponent<Animator>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnPlayerEnterTrigger.Invoke();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnPlayerLeaveTrigger.Invoke();
    }
    public void setAnimatorBool(bool value)
    {
        anim.SetBool(animationName, value);
    }
}
