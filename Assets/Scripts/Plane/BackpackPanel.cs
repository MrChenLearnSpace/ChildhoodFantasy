using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackPanel : BasePanel
{
    public Button Refuse;
    public Text Title;
    public ScrollRect WeaponBox;
    public ScrollRect FoodBox;
    public ScrollRect ItemDescription;
    public Button WeaponBtn;
    public Button FoodBtn;
    public Image image;
    public PlayerData playerData;
    public Transform panel;
    Dictionary<string, int> fooddata= new Dictionary<string, int>();
    // Start is called before the first frame update
    public override void OnInit() {
        skinPath = "BackpackPanel";
        layer = PanelManager.Layer.Panel;
      //  panel = skin.transform;

    }
    public override void OnShow(params object[] args) {
        panel = skin.transform;
        Refuse = panel.Find("Refuse").GetComponent<Button>();
        Title = panel.Find("Title").GetComponent<Text>();
        WeaponBox = panel.Find("WeaponBox").GetComponent<ScrollRect>();
        FoodBox = panel.Find("FoodBox").GetComponent<ScrollRect>();
        ItemDescription = panel.Find("ItemDescription").GetComponent<ScrollRect>();
        WeaponBtn = panel.Find("WeaponBtn").GetComponent<Button>();
        FoodBtn = panel.Find("FoodBtn").GetComponent<Button>();
        playerData = GameManager.Instance.playerData;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //加载背包
        //weaponback
        //List<WeaponData> weaponDatas = GameManager.Instance.playerData.weaponDatas;
        List<WeaponData> weaponDatas = playerData.weaponDatas;
        List<FoodData> resFoodDatas = playerData.foodDatas;
        RefreshBackpackWeapon(weaponDatas);
        RefreshBackpackItem(FoodBox.content, fooddata, resFoodDatas);
        //加载监听
        Refuse.onClick.AddListener(OnRefuse);
        WeaponBtn.onClick.AddListener(OnWeaponBtn);
        FoodBtn.onClick.AddListener(OnFoodBtn);
        
        //显示需要的
        OnWeaponBtn();
    }
    public override void OnClose() {
        //GameManager.Instance.IsLockMouse = true;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        //Camera.main.GetComponent<CameraFollow>().isEable = true;
        //print("true");
    }
    public void OnRefuse() {
        Close();
    }
    public void OnWeaponBtn() {
        WeaponBox.gameObject.SetActive(true);
        FoodBox.gameObject.SetActive(false);
        Title.text = "Weapon";
    } 
    public void OnFoodBtn() {
        WeaponBox.gameObject.SetActive(false);
        FoodBox.gameObject.SetActive(true);
        Title.text = "Food";

    }



    public void RefreshBackpackWeapon(List<WeaponData>  weaponDatas ) {
        for(int i = 0; i < WeaponBox.content.transform.childCount; i++) {
            Destroy(WeaponBox.content.transform.GetChild(i).gameObject);
        }
        
        for (int i = 0; i < weaponDatas.Count; i++) {
            GameObject go = Instantiate(ResManager.AB["ui_bag"].LoadAsset<GameObject>("Solt"), WeaponBox.content.transform);
            go.transform.Find("Image").GetComponent<Image>().sprite = ResManager.AB["ui"].LoadAsset<Sprite>(weaponDatas[i].Pir);
            go.GetComponent<Solt>().itemName = weaponDatas[i].Name;
            go.transform.Find("num").GetComponent<Text>().text = "1";
        }
    }
    public void RefreshBackpackItem(Transform context,Dictionary<string,int> pairs,List<FoodData> resPair) {
        for (int i = 0; i < context.childCount; i++) {
            Destroy(context.GetChild(i).gameObject);
        }
        pairs.Clear();
        for (int i= 0;i < resPair.Count;i++) {
            if(!pairs.ContainsKey(resPair[i].Name)) {
                GameObject go = Instantiate(ResManager.AB["ui_bag"].LoadAsset<GameObject>("Solt"), context);
                go.transform.Find("Image").GetComponent<Image>().sprite = ResManager.AB["ui"].LoadAsset<Sprite>(resPair[i].Pir);
                go.GetComponent<Solt>().itemName = resPair[i].Name;
                //print(resPair[i].Name);
                pairs.Add(resPair[i].Name, 1);
            }
            else {
                pairs[resPair[i].Name]++;
            }
        }
        for(int i = 0; i < context.childCount; i++) {
            Transform child = context.GetChild(i);
            child.Find("num").GetComponent<Text>().text = pairs[child.GetComponent<Solt>().itemName].ToString();
        }
    }
}
