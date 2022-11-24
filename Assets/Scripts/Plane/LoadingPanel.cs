using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    public Image loading;
    public AsyncOperation async;
    float progressValue =0;
    bool isGetPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        loading.fillAmount = progressValue;
        progressValue += Time.deltaTime * 0.3f;
    }
    public override void OnInit() {
        skinPath = "LoadingPanel";
        layer = PanelManager.Layer.Panel;

    }
    //显示时
    public override void OnShow(params object[] para) {
        loading = skin.transform.Find("Loading").GetComponent<Image>();
        NetManager.AddMsgListener("MsgGetPlayerData", OnMsgGetPlayerData);

        ProtocolBytes protoPlayerData = new ProtocolBytes();
        protoPlayerData.AddString("MsgGetPlayerData");
        NetManager.Send(protoPlayerData);
        StartCoroutine("LoadingSence", "Scene2");
    }

    private void OnMsgGetPlayerData(ProtocolBase msgBase) {
        int start = 0;
        PlayerData LoadPlayerData = GameManager.Instance.playerData;
        ProtocolBytes protoPlayerData = (ProtocolBytes)msgBase;
        protoPlayerData.GetString(start, ref start);
        string data = protoPlayerData.GetString(start, ref start);
        print(data);
        JsonUtility.FromJsonOverwrite(data, LoadPlayerData);

        isGetPlayer = true;
    }



    //关闭时
    public override void OnClose() {
    }
    IEnumerator LoadingSence(string senceName) {

        async = SceneManager.LoadSceneAsync(senceName);
        async.allowSceneActivation = false;
        while (!async.isDone) {
            /* if (async.progress < 0.9f)
                 progressValue = async.progress;
             else
                 progressValue = 1.0f;*/


            // progress.text = (int)(slider.value * 100) + " %";

            if (isGetPlayer&& progressValue >= 0.9f) {


                async.allowSceneActivation = true;

            }
            // print(async.progress);
            //System.Threading.Thread.Sleep(200);
            yield return null;
            //yield return new  WaitForSeconds(1f);

        }

        //  System.Threading.Thread.Sleep(0);
        PanelManager.Open<GamePanel>();
        Close();



    }
}
