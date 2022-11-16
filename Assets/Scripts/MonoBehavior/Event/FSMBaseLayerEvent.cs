using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBaseLayerEvent : MonoBehaviour ,IAnimatorFSMEvent {

    public ActorController ac;

    private void Awake() {
        ac = GetComponent<ActorController>();
        AddListen();
    }
    // Start is called before the first frame update


    // Update is called once per frame
    public  void AddListen() {
        
        ac.OnFSMevent += OnJumpEnter;
        ac.OnFSMevent += OnFallEnter;
     
        ac.OnFSMevent += OnRollEnter;
        ac.OnFSMevent += OnRollExit;
        ac.OnFSMevent += OnJabEnter;
        ac.OnFSMevent += OnJabExit;
        ac.OnFSMevent += OnGroundEnter;
        ac.OnFSMevent += OnGroundExit;
    }
    void OnJumpEnter(string eventName) {
        if(eventName == "OnJumpEnter") {
            ac.Lockplanar = true;
            ac.pi.inputEnable = false;
            ac.thrustVec = new Vector3(0, ac.jumpVelocity, 0);
        }

    }
    private void OnFallEnter(string eventName) {
        if (eventName == "OnFallEnter") {
            ac.pi.inputEnable = false;

            ac.Lockplanar = true;
        }
    }
    void OnGroundEnter(string eventName) {
        if (eventName == "OnGroundEnter") {
            ac.canAttack = true;
            ac.pi.inputEnable = true;
            ac.Lockplanar = false;
        }
    }
    void OnGroundExit(string eventName) {
        if (eventName == "OnGroundExit") {
            ac.canAttack = false;
        }
    }

    void OnRollEnter(string eventName) {
        if (eventName == "OnRollEnter") {
            ac.pi.inputEnable = false;
            ac.Lockplanar = true;
            ac.planarVec += (new Vector3(transform.forward.x, 0, ac.model.transform.forward.z).normalized) * 2.0f;
            //print(planarVec);
        }

    }
    void OnRollExit(string eventName) {
        if (eventName == "OnRollExit") {
            ac.anim.ResetTrigger("roll");
        }
    }
    void OnJabEnter(string eventName) {
        if (eventName == "OnJabEnter") {
            //Lockplanar = true;
            //pi.inputEnable = false;
            ac.thrustVec = new Vector3(0, ac.jumpVelocity, 0);
        }
    }
    void OnJabExit(string eventName) {
        if (eventName == "OnJabExit") {
            ac.anim.ResetTrigger("jump");
        }
    }
}
