using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GamePanel : BasePanel {
    public Button ActivityEntryBtn;
    public Button BattlePassBtn;
    public Button GachaBtn;
    public Button HandbookBtn;
    public Button BackpackBtn;
    public Button PlayerBoyBtn;
    public Button MPWorldBtn;
    public Button LeaveBtn;
    public Button ChatBtn;
    public Image Hp;
    public ActorController ac;
    public ActorData data;
    public CameraFollow cameraFollow;
    // Start is called before the first frame update
    void Start()
    {
        ac = GameObject.Find("player").GetComponent<ActorController>();
    }

    // Update is called once per frame

    //初始化时
    public override void OnUpdate() {
        OnMouseLocked();
        Hp.fillAmount = ac.playerData.hp / ac.playerData.Maxhp;
    }
    public override void OnInit() {
        skinPath = "GamePanel";
        layer = PanelManager.Layer.Panel;
    }
    private void OnMouseLocked() {
        
        //if (Input.GetKeyDown(KeyCode.LeftAlt)) {
        //    OnMouseLocked(true);
        //}
        //if (GameManager.Instance.IsLockMouse&& Input.GetKeyUp(KeyCode.LeftAlt)) {
        //   OnMouseLocked(false);
        //}
        //if(Input.GetKeyUp(KeyCode.LeftAlt)) {
            if (GameManager.Instance.MouseEdgeLocked()) {
                OnMouseLocked(Input.GetKey(KeyCode.LeftAlt));
            }
        //}
    }
    public void OnMouseLocked(bool sign) {
        if (sign) {         
            Cursor.lockState = CursorLockMode.None;
        }
        else {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = sign;
        if (cameraFollow != null)
            cameraFollow.isEable = !sign;
        else
            print("cameraFollow is null");
        ac.pi.inputEnable = !sign;
        ac.canAttack = !sign;
       // ActorController ac = data.


    }
    //显示时
    public override void OnShow(params object[] para) {
        ActivityEntryBtn = transform.DFSFindChildGameObject("ActivityEntryBtn").GetComponent<Button>();
        BattlePassBtn = transform.DFSFindChildGameObject("BattlePassBtn").GetComponent<Button>();
        GachaBtn = transform.DFSFindChildGameObject("GachaBtn").GetComponent<Button>();
        HandbookBtn = transform.DFSFindChildGameObject("HandbookBtn").GetComponent<Button>();
        BackpackBtn = transform.DFSFindChildGameObject("BackpackBtn").GetComponent<Button>();
        PlayerBoyBtn = transform.DFSFindChildGameObject("PlayerBoyBtn").GetComponent<Button>();
        MPWorldBtn = transform.DFSFindChildGameObject("MPWorldBtn").GetComponent<Button>();
        LeaveBtn = transform.DFSFindChildGameObject("LeaveBtn").GetComponent<Button>();
        ChatBtn = transform.DFSFindChildGameObject("ChatBtn").GetComponent<Button>();
        Hp = transform.DFSFindChildGameObject("Hp").GetComponent<Image>();


        ActivityEntryBtn.onClick.AddListener(OnDefault);
        BackpackBtn.onClick.AddListener(OnBackpackBtn);
        BattlePassBtn.onClick.AddListener(OnDefault);
        GachaBtn.onClick.AddListener(OnDefault);
        HandbookBtn.onClick.AddListener(OnDefault); 
        PlayerBoyBtn.onClick.AddListener(OnDefault); ;
        MPWorldBtn.onClick.AddListener(OnDefault); ;
        LeaveBtn.onClick.AddListener(OnLeave);
        ChatBtn.onClick.AddListener(OnChat);

        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
            cameraFollow.isEable = true;
        else
            print("cameraFollow is null");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

 

    //关闭时
    public override void OnClose() {
    }
    public void  OnBackpackBtn() {
        GameManager.Instance.IsLockMouse = false;

        PanelManager.Open<BackpackPanel>();
        
    }
    public void OnDefault() {
        GameManager.Instance.IsLockMouse = false;
        PanelManager.Open<TipPanel>("还没做，敬请期待");
        
    }
    private void OnLeave() {
        Application.Quit();
    }
    void OnChat() {
        if(GameManager.Instance.ChatRoomPanel == null) {
            PanelManager.Open<ChatRoomPanel>();
        }
        else {
            
            GameManager.Instance.ChatRoomPanel.SetActive(true);
            PanelManager.root.GetComponent<ChatRoomPanel>().Refresh();
        }
    }
}
