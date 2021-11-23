using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImage : MonoBehaviour
{

    public static PlayerAfterImage Instance;
    [SerializeField]private PlayerSpriteAfterImage[] pool;



    bool activated = false;
    int index = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance!=null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;

    }
public  void Dash()
    {
        for(int i = 0; i < pool.Length; i++)
        {
            pool[i].Dash();
        }
    }
    public void EquipSkin()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i].SetSkins();
        }
    }
}


