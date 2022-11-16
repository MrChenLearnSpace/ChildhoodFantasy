using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public PlayerInput pi;
    public float walkSpeed;
    public float jumpVelocity= 1.0f;
    public float runMultiplier=2.0f;
    //public float temp;

    public Animator anim;
    public Rigidbody rigid;
    public bool Lockplanar;
    public Vector3 planarVec;
    public Vector3 thrustVec;
    // Vector3 de;
    public Vector3 deltaPosition;
    public bool canAttack =true;

    public float attackTargetWeight;
    public event Action<string> OnFSMevent;

    public Transform LHandWeaponPos;
    public Transform RHandWeaponPos;

    public PlayerData playerData;
    

    private void Awake() {
       
        model = transform.Find("Model").GetChild(0).gameObject;
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        if(!LHandWeaponPos) {
            LHandWeaponPos = transform.DFSFindChildGameObject( "LWeaponPos");
        }if(!RHandWeaponPos) {
            RHandWeaponPos = transform.DFSFindChildGameObject("RWeaponPos");
        }
    }
    public void Damage(float atkNum) {
        anim.SetTrigger("hit");
        playerData.hp = Mathf.Clamp( playerData.hp - atkNum + playerData.dft,0, playerData.hp);
        if (playerData.hp == 0) {
            anim.SetBool("death", true);
            //pi.inputEnable = false;
            pi.enabled = false;
            this.enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoveDirection();

       
    }
    private void LateUpdate() {
        
        
    }

    private void FixedUpdate() {
        rigid.position += (deltaPosition);
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z)+thrustVec;
         //print(deltaPosition*10000f);
        //print(rigid.velocity);
        thrustVec = Vector3.zero;
        deltaPosition = Vector3.zero;
        //laterVec = rigid.velocity;
    }
    void UpdateMoveDirection() {
        
        anim.SetFloat("forward", pi.Dvec.magnitude*Mathf.Lerp(anim.GetFloat("forward"), pi.run ? 2.0f : 1.0f, 0.5f) );
        if(rigid.velocity.magnitude>6) {
            anim.SetTrigger("roll");
        }
        if(pi.jump) {
            
            anim.SetTrigger("jump");
        }
        if(Input.GetButtonDown(pi.KeyC)&&canAttack&&CheckAnimation("Ground")) {
            anim.SetTrigger("attack");
        }
        if (pi.Dvec != Vector3.zero) {
            #region 旧代码
            //if (Vector3.Dot(pi.Dvec, model.transform.forward) >= -0.8) {
            //    if (Vector3.Lerp(model.transform.forward, pi.Dvec, 0.2f) != Vector3.zero)
            //        model.transform.forward = Vector3.Lerp(model.transform.forward, pi.Dvec, 0.2f);
            //}
            //else {
            //    //  modelDir = Vector3.Lerp(modelDir, new Vector3(camDirForward.x, 0, camDirForward.y), 0.9f);
            //    model.transform.forward = pi.Dvec;
            //}
            #endregion
            model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);
        }
        if (!Lockplanar) {
            planarVec = pi.Dvec * walkSpeed * (pi.run ? runMultiplier : 1.0f);
        }
      
    }

    bool CheckAnimation(string animationName,string animatorLayer = "Base Layer") {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(animatorLayer)).IsName(animationName);
    }

        
    public void OnFSMEventNotify(string onEvent) {

        OnFSMevent.Invoke(onEvent);
    }

    

    public void SetAnimatorIsOnGround(bool signal) {
        anim.SetBool("isGround", signal);
    }

}
