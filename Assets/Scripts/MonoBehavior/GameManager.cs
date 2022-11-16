using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager> {
    [Header(" ==== 全局数据 ====")]
    public Weapon weapon;
    public Food food;

    public PlayerData playerData;
    public string id;
    [Header(" ==== 鼠标管理 ====")]
    public bool IsLockMouse = true;
    public bool IsMouse = false;
    [Header(" ==== 聊天室初始化 ====")]

    public GameObject ChatRoomPanel;
    public int currentNum;
    // public List<ChatMassage> chatMassages = new List<ChatMassage>();
    public Queue<ChatMassage> chatMassages = new Queue<ChatMassage>();


    Transform tip;
    Transform panel;

    protected override void Awake() {
        base.Awake();

    }
    private void Start() {
        //网络监听
        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);
        //初始化
        ResManager.Init();//资源总管理在面板管理之前
        PanelManager.Init();
        // BattleManager.Init();
        //打开登陆面板
        PanelManager.Open<LoginPanel>();
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(PanelManager.root);

        tip = PanelManager.root.DFSFindChildGameObject("Tip");
        panel = PanelManager.root.DFSFindChildGameObject("Panel");
        //WeaponData weapond = weapon.dataArray[0];
        //playerData.weaponDatas.Add(weapond);
    }
    void Update() {
        NetManager.Update();

    }

    //关闭连接
    void OnConnectClose(string err) {
        Debug.Log("断开连接");
    }
     
    #region   鼠标临界判定
    public bool MouseEdgeLocked() {
        if (tip.childCount == 0 && PanelIsActive() && panel.GetChild(0).name == "GamePanel(Clone)") {
           // print(panel.GetChild(0).name);
            return true;

        }
        return false;
    }
    bool PanelIsActive() {
        int k = 0;
        if (panel.childCount != 0) {
            for (int i = 0; i < panel.childCount; i++) {
                if (panel.GetChild(i).gameObject.activeSelf) {
                    k++;
                }
            }
        }
        if (k == 1) return true;
        return false;
    }
    #endregion
    //被踢下线
    void OnMsgKick(ProtocolBase msgBase) {
        PanelManager.Open<TipPanel>("被踢下线");
    }
    // Start is called before the first frame update
    void OnMsgAllPlayerChatBackcall(ProtocolBase msgBase) {
        ProtocolBytes protocolBytes = msgBase as ProtocolBytes;
        int start = 0;
        protocolBytes.GetString(start, ref start);
        ChatMassage chatMassage = new ChatMassage();
        chatMassage.id = protocolBytes.GetString(start, ref start);
        chatMassage.massage = protocolBytes.GetString(start, ref start);
        chatMassages.Enqueue(chatMassage);
    }

}

public class ChatMassage {
    public string id;
    public string massage;
}