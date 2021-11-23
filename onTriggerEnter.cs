using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class onTriggerEnter : MonoBehaviour
{
    bool enteredOnce = false;
    [SerializeField] bool onlyPlayerTrigger = false;
    [SerializeField] public UnityEvent OnPlayerTouchTrigger;
    [SerializeField] TextMeshProUGUI text;
    
    // Start is called before the first frame update
    private void Start()
    {
        enteredOnce = false;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enteredOnce)
        {
            bool check = true;
            if (onlyPlayerTrigger)
            {
                if (!collision.CompareTag("Player"))
                {
                    check = false;
                }
            }
            if (check)
            {
                OnPlayerTouchTrigger.Invoke();
                enteredOnce = true;
            }

        }
    }
    void hideText()
    {
        LeanAlphaText(text, 0f, 5f);
    }
    public LTDescr LeanAlphaText(TextMeshProUGUI textMesh, float to, float time)
    {
        var _color = textMesh.color;
        var _tween = LeanTween
            .value(textMesh.gameObject, _color.a, to, time)
            .setOnUpdate((float _value) => {
                _color.a = _value;
                textMesh.color = _color;
            });
        return _tween;
    }
    public void SetPlaceText(string placeName)
    {
        placeName = placeName.Replace('$', '\n'); // converts "clown city$part I" into two lines.
        text.text = placeName;
        LeanAlphaText(text, 1f, 5f).setOnComplete(hideText);

    }

   public void zoomOut(float seconds)
    {
        CameraSize.Instance.ZoomOutLense(seconds);
    }
    public void zoomIn(float seconds)
    {
        CameraSize.Instance.ZoomInLense(seconds);
    }
}
