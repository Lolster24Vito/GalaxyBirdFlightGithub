
using UnityEngine;
[CreateAssetMenu(menuName ="Shop/SkinItem")]
[System.Serializable]
public class SkinItem : ScriptableObject
{
    public string skinName;
    public int price;
 public Sprite bodySprite;
    public Color wingColor;
   [Range(1,80f)] public float startingJumpPower;
    [Range(-10f, 10f)] public float startingAirTime;
    public int startingHealth;
    public string descriptionOfAbility;
    public Vector3 wingScale = Vector3.one;
    public Vector3 wingLocalPosition = Vector3.zero;
    public Vector3 crownPosition = Vector3.zero;
    public Vector3 crownPositionUI = Vector3.zero;

    public PhysicsMaterial2D physicsMaterial;

    [Header(" ADVANCED SETTINGS", order =0)]

    [Header("Floating parameters",order =1)]
    [SerializeField] public float minFloatXSpeed = 1f;
    [SerializeField] public float maxFloatXSpeed = 35f;


    [Header("Floating SlowDown parameters")]
    [SerializeField] public float timeTillMaxGravity = 0.5f;
    [SerializeField] public float floatSlowDownXMultiplier = 1.6f;


    [Header("Floating Gravity parameters")]
    [SerializeField] public float timeLowGravityAfterTouch = 0.4f;
    [SerializeField] public float lowGravityAmountAfterJump = 0.35f;

    [Range(-50, 10)] [SerializeField] public float maxGravityWhileFloating = -7;
    [Range(100, -20)] [SerializeField] public float gravityDirectionMultiplier = 6f;

    [SerializeField] public LayerMask wallLayer = 8;
    [Header("Unique bird options")]
    public bool flightlessBird = false;

}
