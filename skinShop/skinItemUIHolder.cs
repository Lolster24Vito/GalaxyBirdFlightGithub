using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class skinItemUIHolder : MonoBehaviour
{
    [SerializeField] public int index;
    [SerializeField] public SkinItem skin;
     float timeFinishedBest;
    bool firstTimeFinished = true;
    public void buyShopSkin()
    {
        if (PlayerPrefs.GetInt("Coins")>= skin.price)
        {
            PlayerStateManager.Instance.ReduceCoins(skin.price);
            ShopUI.Instance.buyIndexSkinItem(index);


        }
    }
    public void EquipSkin()
    {
        PlayerStateManager.Instance.currentSkin = skin;

        ShopUI.Instance.SetEquippedIndex(index);
        if (!PlayerStateManager.Instance.IsDead())
        {
            PlayerStateManager.Instance.EquipSkin();

        }
       // PlayerStateManager.Instance.EquipSkin();
    }
    public void setCrown(float timeFinished)
    {
        if (firstTimeFinished)
        {
            timeFinishedBest = timeFinished;
            firstTimeFinished = false;
        }
        if (timeFinished < timeFinishedBest)
        {
            timeFinishedBest = timeFinished;
        }

        int minutes = Mathf.FloorToInt(timeFinishedBest / 60F);
        int seconds = Mathf.FloorToInt(timeFinishedBest - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        Transform crown = transform.GetChild(12);
        crown.gameObject.SetActive(true);
        crown.GetComponent<RectTransform>().anchoredPosition = skin.crownPositionUI;
        GameObject textG = transform.GetChild(13).gameObject;
        textG.GetComponent<TextMeshProUGUI>().text = "Best time:" + niceTime + " min";
        textG.gameObject.SetActive(true);

    }
}
