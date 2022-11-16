using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Solt : MonoBehaviour
{
    public Button button;
    public string itemName;
    public int itemNum = 0;
    public BackpackPanel backpack;
    public Text Description;
    public void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnSoltClick);
        backpack = PanelManager.root.GetComponent<BackpackPanel>();
        Description = backpack.ItemDescription.content.GetComponent<Text>();
    }
    public void OnSoltClick() {
        
        string str = "没找到";
        switch (backpack.Title.text) {
            case "Weapon":
                //print("1111");
                List<WeaponData> weapons= backpack.playerData.weaponDatas;
                for(int i=0;i< weapons.Count;i++) {
                    if(weapons[i].Name == itemName) {
                        str = weapons[i].Info;
                        break;
                    }
                }
                Description.text = str;
                break;
            case "Food":
                
                List<FoodData> foods = backpack.playerData.foodDatas;
                for (int i = 0; i < foods.Count; i++) {
                    if (foods[i].Name == itemName) {
                        str = foods[i].Info;
                        break;
                    }
                }
                Description.text = str; break;
            default:break;
        }

    }
}
