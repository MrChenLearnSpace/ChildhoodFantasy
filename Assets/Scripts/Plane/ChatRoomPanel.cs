using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatRoomPanel : BasePanel
{
    public ScrollRect Chat;
    public Button Quit;
    public Button Send;
    public InputField InputMassge;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnInit() {
        skinPath = "ChatRoomPanel";
        layer = PanelManager.Layer.Panel;
    }
    //显示时
    public override void OnShow(params object[] para) {
        Chat = transform.DFSFindChildGameObject("Chat").GetComponent<ScrollRect>();
        Quit = transform.DFSFindChildGameObject("Quit").GetComponent<Button>();
        Send = transform.DFSFindChildGameObject("Send").GetComponent<Button>();
        InputMassge = transform.DFSFindChildGameObject("InputMassage").GetComponent<InputField>();
        GameManager.Instance.ChatRoomPanel = skin;

        Quit.onClick.AddListener(OnQuit);

    }
    //关闭时
    public override void OnClose() {
    }
    public void OnSend() {

    }public void OnQuit() {
        skin.SetActive(false);
    }
    public void Refresh() {

        // GameObject go =Instantiate( ResManager.AB["ui"].LoadAsset<GameObject>("OneMassage"),Chat.content);
        while (GameManager.Instance.chatMassages.Count > 0) {
            ChatMassage chatMassage = GameManager.Instance.chatMassages.Dequeue();
            GameObject go = Instantiate(ResManager.AB["ui"].LoadAsset<GameObject>("OneMassage"), Chat.content);
            go.transform.Find("idName").GetComponent<Text>().text = chatMassage.id;
            go.transform.Find("massage").GetComponent<Text>().text = chatMassage.massage;
        }
    }
}
