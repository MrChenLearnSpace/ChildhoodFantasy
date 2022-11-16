using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMAttackLayerEvent : MonoBehaviour, IAnimatorFSMEvent {
    public ActorController ac;

   

    private void Awake() {
        ac = GetComponent<ActorController>();
        AddListen();
    }
    public void AddListen() {
        ac.OnFSMevent += OnAttack1hAEnter;
        ac.OnFSMevent += OnAttackIdleEnter;
        ac.OnFSMevent += OnAttack1hAUpdate;
        ac.OnFSMevent += OnAttackIdleUpdate;
    }
    void OnAttack1hAEnter(string eventName) {
        if(eventName == "OnAttack1hAEnter") {
            ac.pi.inputEnable = false;
            ac.Lockplanar = true;
            ac.planarVec = Vector3.zero;
            ac.attackTargetWeight = 1.0f;
            
        }
    }
    void OnAttack1hAUpdate(string eventName) {
        if (eventName == "OnAttack1hAUpdate")
            ac.anim.SetLayerWeight(ac.anim.GetLayerIndex("AttackLayer"), Mathf.Lerp(ac.anim.GetLayerWeight(ac.anim.GetLayerIndex("AttackLayer")), ac.attackTargetWeight, 0.1f));
    }
    void OnAttackIdleEnter(string eventName) {
       
        if (eventName == "OnAttackIdleEnter") {
            ac.attackTargetWeight = 0.0f;
            ac.pi.inputEnable = true;
            ac.Lockplanar = false;
        }
    }
    void OnAttackIdleUpdate(string eventName) {
        if(eventName == "OnAttackIdleUpdate")
        ac.anim.SetLayerWeight(ac.anim.GetLayerIndex("AttackLayer"), Mathf.Lerp(ac.anim.GetLayerWeight(ac.anim.GetLayerIndex("AttackLayer")), ac.attackTargetWeight, 0.1f));

    }

    // Start is called before the first frame update

}
