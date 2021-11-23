using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance;
    RectTransform shopEverything;
    [SerializeField] Text speedPrice;
    [SerializeField] Text healthPrice;
    [SerializeField] Text gravityPrice;
    [SerializeField] TextMeshProUGUI coinNumberText;

    [SerializeField] Color buyColor;
    [SerializeField] Color cannotBuyColor;
    [SerializeField] Color noMoreBuyingColor;


    [SerializeField] Image speedButtonColorImg;
    [SerializeField] Image healthButtonColorImg;
    [SerializeField] Image airTimeButtonColorImg;

    [SerializeField] TextMeshProUGUI currentSpeed;
    [SerializeField] TextMeshProUGUI currentHealth;
    [SerializeField] TextMeshProUGUI currentGravity;




    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        shopEverything = transform.GetChild(0).GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ShowShopUI()
    {
        PlayerStateManager.Instance.gameObject.GetComponent<PlayerMoveJoystick>().enabled = false;
        coinNumberText.text = PlayerStateManager.Instance.GetCoinsNumber().ToString();
        speedPrice.text = PlayerStateManager.Instance.SpeedPrice.currentPrice.ToString();
        healthPrice.text = PlayerStateManager.Instance.HealthPrice.currentPrice.ToString();
        gravityPrice.text = PlayerStateManager.Instance.GravityPrice.currentPrice.ToString();
        currentSpeed.text =( PlayerStateManager.Instance.GetBirdJumpPower() / (25f / 320f)).ToString("F1");
        currentHealth.text = PlayerStateManager.health.ToString();
        currentGravity.text = (PlayerStateManager.Instance.GetBirdGravity() * 9.81f).ToString("F2");

        setPriceColors();
        shopEverything.gameObject.SetActive(true);
        Time.timeScale = 0.01f;


    }
    public void HideShopUI()
    {
        shopEverything.gameObject.SetActive(false);
        PlayerStateManager.Instance.gameObject.GetComponent<PlayerMoveJoystick>().enabled = true;
        Time.timeScale = 1f;
    }
    public void AddSpeed()
    {
        PlayerStateManager.Instance.AddSpeed();
        //speedPrice.text = PlayerStateManager.Instance.SpeedPrice.currentPrice.ToString();
        //coinNumberText.text = PlayerStateManager.coins.ToString();
        ShowShopUI();
        setPriceColors();


    }
    public void AddGravity()
    {
        PlayerStateManager.Instance.AddGravity();
        ShowShopUI();
        setPriceColors();


    }
    public void AddHealth()
    {
        PlayerStateManager.Instance.AddHealth();
        ShowShopUI();
        setPriceColors();

    }
    void setPriceColors()
    {
        float coinAmount = PlayerStateManager.Instance.GetCoinsNumber();
        if (coinAmount >= PlayerStateManager.Instance.SpeedPrice.currentPrice)
        {
            speedButtonColorImg.color = buyColor;
        }
        else
        {
            speedButtonColorImg.color = cannotBuyColor;

        }
        //if (PlayerStateManager.Instance.HealthPrice.currentPrice < 520)
        //{
            if (coinAmount >= PlayerStateManager.Instance.HealthPrice.currentPrice)
            {
                healthButtonColorImg.color = buyColor;
            }
            else
            {
                healthButtonColorImg.color = cannotBuyColor;

            }
        //}
        /*else
        {
            healthButtonColorImg.color = noMoreBuyingColor;
            healthButtonColorImg.GetComponent<Button>().enabled = false;
            healthButtonColorImg.transform.GetChild(0).gameObject.SetActive(false);

        }*/

        if (coinAmount >= PlayerStateManager.Instance.GravityPrice.currentPrice)
        {
            airTimeButtonColorImg.color = buyColor;
        }
        else
        {
            airTimeButtonColorImg.color = cannotBuyColor;

        }
    }

    


}
