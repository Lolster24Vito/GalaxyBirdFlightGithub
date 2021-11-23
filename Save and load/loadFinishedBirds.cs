using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadFinishedBirds : MonoBehaviour
{
    [SerializeField] GameObject birdPrefab;
    SaveObjectList saveObjectList;
    SaveObject[] saveObjects;

    [SerializeField] Transform[] positions;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        saveObjectList = SaveLoad.Instance.lSave;
        fillOutTheBirds();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        //saveObjectList = SaveLoad.Instance.lSave;

        //fillOutTheBirds();
    }
   public void fillOutTheBirds()
    {
        saveObjectList = SaveLoad.Instance.lSave;
        //with saveObjectList equip the birds
        if (saveObjectList != null&&saveObjectList.lSaveObjects.Count!=0) { 
        saveObjects = saveObjectList.lSaveObjects.ToArray();

        for (int i = 0; i < saveObjects.Length; i++)
        {
            if (i < positions.Length)
            {
                SpawnBird(saveObjects[i],i);
            }
        }

        }
    }
    void SpawnBird(SaveObject saveObject,int index)    
    {

       GameObject bird= Instantiate(birdPrefab, positions[index].transform);
        SkinItem skin = ShopUI.Instance.GetSkin(saveObject.shopIndexValue);
        //0 body
        //1 is  wings sprite
        bird.transform.GetChild(1).GetComponent<SpriteRenderer>().color = skin.wingColor;
        bird.transform.GetChild(1).localScale = skin.wingScale;
        bird.transform.GetChild(1).localPosition = skin.wingLocalPosition;
        bird.transform.GetChild(4).localPosition = skin.crownPosition;

        bird.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = skin.bodySprite;



    }
}
