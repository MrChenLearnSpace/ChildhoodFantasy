using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class MsgGamePlayerData : MsgBase {
	public MsgGamePlayerData() { protoName = "MsgGamePlayerData"; }
	public PlayerData data=new PlayerData();
}
public class MsgAllPlayerChat : MsgBase {
	public MsgAllPlayerChat() { protoName = "MsgAllPlayerChat"; }
	public string id = "";
	public string data = "";
}


