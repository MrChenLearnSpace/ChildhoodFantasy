using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Character/Character Data", fileName = "New ActorData")]
[System.Serializable]
public class ActorData : ScriptableObject {
    public float Maxhp = 1000 ;
    public float hp = 1 ;
    public float atk = 0;//attack
    public float dft = 0;//defend
    public float attRange = 1;
   


}
