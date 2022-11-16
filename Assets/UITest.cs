using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{
    public BasePanel panel;
    public Weapon weapon;
    public Food food;

    public PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
        ResManager.Init();//资源总管理在面板管理之前
        PanelManager.Init();
        WeaponData weapond = weapon.dataArray[0];
        playerData.weaponDatas.Add(weapond);
        panel.OnInit();
        panel.OnShow();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)) {
            WeaponData weapond = weapon.dataArray[0];
            playerData.weaponDatas.Add(weapond);
            panel.OnShow();
        }
        if(Input.GetKeyDown(KeyCode.J)) {
            FoodData foods = food.dataArray[0];
            playerData.foodDatas.Add(foods);
            panel.OnShow();
        }
        
    }
}
