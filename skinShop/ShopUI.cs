using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance;
    [SerializeField] GameObject everything;
    [Header("List of skins that are sold")]
    [SerializeField] private SkinItem[] skinItem;
    [Header("Refferences")]
    [SerializeField] private Transform shopContainer;
    [SerializeField] private GameObject shopItemPrefab;
    [SerializeField] private TextMeshProUGUI coinAmount;
    [SerializeField] private RectTransform scrollRect;


    [SerializeField] public string playPrefabTestString;
    [SerializeField] private float xOffsetForSnapEquippedBird = 730f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        playPrefabTestString = PlayerPrefs.GetString("SkinsStatus", "2000000000000000000000000000000000000000");

    }
    private IEnumerator Start()
    {

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        PopulateShop();
        //Only if testing


    }

    private  void PopulateShop()
    {




        playPrefabTestString = PlayerPrefs.GetString("SkinsStatus", "2000000000000000000000000000000000000000");
        if (skinItem.Length > playPrefabTestString.Length)
        {
            int difference = skinItem.Length - playPrefabTestString.Length;
            for (int i = 0; i < difference; i++)
            {
                playPrefabTestString += "0";
            }
            PlayerPrefs.SetString("SkinsStatus", playPrefabTestString);

        }


        updatePLayerCoinValue();
     
        for (int i = 0; i < skinItem.Length; i++)
        {
            SkinItem si = skinItem[i];
            GameObject itemObject = Instantiate(shopItemPrefab, shopContainer);
            //Skinitem itemObject UI hiearchy goes like this
            /*
             * body(0)
             * name(1)
             * buy button(2)
             * equip button(3)
             * equip text(4)
             * wings sprite(5)
             * Image dark background(6)
             * full dark Image that spawns lower half background(7)
             * speed text(8)
             * health text(9)
             * special note text(10)
             * gravity text(11)
             */

            itemObject.transform.GetChild(0).GetComponent<Image>().sprite = si.bodySprite;
            itemObject.transform.GetChild(5).GetComponent<Image>().color = si.wingColor;
            itemObject.transform.GetChild(5).GetComponent<RectTransform>().localScale = si.wingScale;
            //harcoded this
            //Debug.Log("BREAK BEFORE BUG HERE?");
            itemObject.transform.GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>().text = jumpStrenghtToSpeed(si);
            itemObject.transform.GetChild(9).GetChild(0).GetComponent<TextMeshProUGUI>().text = si.startingHealth.ToString();
            itemObject.transform.GetChild(11).GetChild(0).GetComponent<TextMeshProUGUI>().text = GravityScaleToMetersPerSecond(si);
          //  Debug.Log("BREAK BEFORE BUG HERE 2");

            //if has special note write it down and expand the dark image
            if (!string.IsNullOrEmpty(si.descriptionOfAbility)
)
            {
                itemObject.transform.GetChild(7).gameObject.SetActive(true);
                itemObject.transform.GetChild(10).gameObject.SetActive(true);

                itemObject.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = si.descriptionOfAbility;

            }


            itemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = si.skinName;
            itemObject.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = si.price.ToString();
            itemObject.GetComponent<skinItemUIHolder>().skin = si;
            itemObject.GetComponent<skinItemUIHolder>().index =i;

            //   Debug.Log(si.skinName+itemObject.GetComponent<RectTransform>().anchoredPosition);
            //   \0 is empty 

            //I'm keeping the store memory to a single string in playerPrefs 
            //0==not bought
            //1==bought
            //2==Equipped
            //3==Crown bird?

            /*
             * buy button(2)
             * equip button(3)
             * equip text(4)
             * */
                if (playPrefabTestString[i].Equals('0'))
                {
                    itemObject.transform.GetChild(2).gameObject.SetActive(true);
                    itemObject.transform.GetChild(3).gameObject.SetActive(false);
                    itemObject.transform.GetChild(4).gameObject.SetActive(false);

                }
                if (playPrefabTestString[i].Equals('1'))
                {
                    itemObject.transform.GetChild(2).gameObject.SetActive(false);
                    itemObject.transform.GetChild(3).gameObject.SetActive(true);
                    itemObject.transform.GetChild(4).gameObject.SetActive(false);


                }
                if (playPrefabTestString[i].Equals('2'))
                {
                    PlayerStateManager.Instance.currentSkin= si;

                SaveLoad.Instance.SetshopIndexValue(i);
                    PlayerStateManager.Instance.EquipSkin();
                    itemObject.transform.GetChild(2).gameObject.SetActive(false);
                    itemObject.transform.GetChild(3).gameObject.SetActive(false);
                    itemObject.transform.GetChild(4).gameObject.SetActive(true);
                SnapToEquipedBird(i);


                }
            
        }

            shopContainer.GetChild(shopContainer.childCount-1).gameObject.SetActive(false);
    

    }
    public void unlockFinalBird()
    {
        shopContainer.GetChild(shopContainer.childCount - 1).gameObject.SetActive(true);

    }
    public void showShopUI()
    {
        Debug.Log("playPrefabTestString :" + playPrefabTestString);
        Debug.Log("getString :" + PlayerPrefs.GetString("SkinsStatus", "2000000000000000000000000000000000000000"));

        updatePLayerCoinValue();
        everything.SetActive(true);
        if (!PlayerStateManager.Instance.IsDead())
        {
            PlayerStateManager.Instance.gameObject.GetComponent<PlayerMoveJoystick>().enabled = false;
        }
    }
    public void hideShopUI()
    {
        DeathScreenUI.Instance.UpdateCoinValue();
        everything.SetActive(false);
        if (!PlayerStateManager.Instance.IsDead())
        {
            PlayerStateManager.Instance.gameObject.GetComponent<PlayerMoveJoystick>().enabled = true;
        }
    }

    void updatePLayerCoinValue()
    {
        coinAmount.text = PlayerPrefs.GetInt("Coins").ToString();
    }
    public void buyIndexSkinItem(int index)
    {
        char[] array = playPrefabTestString.ToCharArray();
        array[index] = '1';
        //change item to bought in memory
        playPrefabTestString = new string(array);
        PlayerPrefs.SetString("SkinsStatus", playPrefabTestString);
        updatePLayerCoinValue();

        GameObject itemObject = shopContainer.GetChild(index).gameObject;
        itemObject.transform.GetChild(2).gameObject.SetActive(false);
        itemObject.transform.GetChild(3).gameObject.SetActive(true);
        itemObject.transform.GetChild(4).gameObject.SetActive(false);

    }
    public void SetEquippedIndex(int index)
    {
        //set in string memory and change equipped
        char[] array = playPrefabTestString.ToCharArray();
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == '2')
            {
                //set all other objects to be unequipped
                array[i] = '1';
                GameObject itemObject = shopContainer.GetChild(i).gameObject;
               

                itemObject.transform.GetChild(2).gameObject.SetActive(false);
                itemObject.transform.GetChild(3).gameObject.SetActive(true);
                itemObject.transform.GetChild(4).gameObject.SetActive(false);
            }
        }
        //set current index to be equipped
        array[index] = '2';
        SaveLoad.Instance.SetshopIndexValue(index);
        GameObject iO = shopContainer.GetChild(index).gameObject;
        iO.transform.GetChild(2).gameObject.SetActive(false);
        iO.transform.GetChild(3).gameObject.SetActive(false);
        iO.transform.GetChild(4).gameObject.SetActive(true);
        playPrefabTestString = new string(array);
        PlayerPrefs.SetString("SkinsStatus", playPrefabTestString);

    }
    public string jumpStrenghtToSpeed(SkinItem skin)
    {
        /*if something something JumpPower/(25/320) je kmpH
         * a za mph je JUmpPower/(25/200)
         * */
            return (skin.startingJumpPower / (25f / 320f)).ToString("F1")+ " km/h";
    }
    private string GravityScaleToMetersPerSecond(SkinItem skin)
    {
        /*if something something JumpPower/(25/320) je kmpH
         * a za mph je JUmpPower/(25/200)
         * */
        return (skin.startingAirTime *9.81).ToString("F1") + " m/s";
    }
    public void SnapToEquipedBird(int targetElementNumber)
    {
        RectTransform shopContainerRect= shopContainer.GetComponent<RectTransform>();
        Vector2 startingPos = shopContainerRect.anchoredPosition;

        shopContainerRect.anchoredPosition = new Vector2(startingPos.x - (xOffsetForSnapEquippedBird * targetElementNumber), startingPos.y);
    }
    public SkinItem GetSkin(int index)
    {
        return skinItem[index];
    }
    public void SetCrownOnSkin(SaveObject saveObject)
    {
       GameObject shopPrefab= shopContainer.GetChild(saveObject.shopIndexValue).gameObject;
        shopPrefab.GetComponent<skinItemUIHolder>().setCrown(saveObject.timeFinished);
    }
}
