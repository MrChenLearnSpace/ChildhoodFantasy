using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour , IEndObserver {
    public ActorData templateData;
    public ActorData currentData;
    public NavMeshAgent agent;
    public float patrolRange;

    public GameObject AttackObj;
    public Collision collision;


    public Vector3 initPos;
    public float temp;

    Animator anim;
    public WeaponController wc;
    public enum EnemyStates {
        等待,巡逻,追踪,攻击,回初始点,死亡
    };
    public EnemyStates states;
    // Start is called before the first frame update

    #region AnimFlag
    bool death;
    bool fristAttack;
    #endregion

    public virtual void Awake()
    {
        if(currentData  == null) {
            currentData = Instantiate(templateData);
        }
        agent = GetComponent<NavMeshAgent>();
        initPos = transform.position;
        anim = GetComponent<Animator>();
        agent.stoppingDistance = currentData.attRange;
       // print(EnemyManager.IsInitialized);
    }
    protected virtual void Start() {
        EnemyManager.Instance.AddListenObserve(this);
    }
    public void Update() {
        
        StatesSwitch();
        AnimatiorSet();
       // print(agent.velocity.sqrMagnitude);
    }

    private void AnimatiorSet() {
        anim.SetFloat("runSpeed", Mathf.Clamp( agent.velocity.sqrMagnitude * temp,0,3));
        anim.SetBool("death", death);
    }

    public virtual void StatesSwitch() {
        switch (states) {
            case EnemyStates.等待: Idle(); break;
            case EnemyStates.巡逻: Patrol(); break;
            case EnemyStates.追踪: Chase(); break;
            case EnemyStates.攻击: Attack(); break;
            case EnemyStates.回初始点: ReturnToInitial(); break;
            case EnemyStates.死亡: Die(); break;
            default:print("状态溢出"); break;
        }

    }
    private void FixedUpdate() {
       /* Collider[] collisions = Physics.OverlapSphere(transform.position, patrolRange);
        if(AttackObj == null) {
            for (int i = 0; i < collisions.Length; i++) {
                if (collisions[i].tag == "Player") {
                    AttackObj = collisions[i].gameObject;
                    return;
                }
            }
        }
        else {
            for (int i = 0; i < collisions.Length; i++) {
                if(AttackObj == collisions[i].gameObject) {
                    return;
                }
            }
            AttackObj = null;
        }
       */
    }
    #region 状态函数后期可以考虑热更
    void Idle() {
        if(AttackObj!=null) {
            states = EnemyStates.追踪;
        }
    } 
    void Patrol() {

    }
    void Chase() {
        
        if (AttackObj == null) {
            agent.isStopped = true;
            states = EnemyStates.回初始点;
        }
        else {
            if (Vector3.Distance(transform.position, AttackObj.transform.position) < currentData.attRange) {
                agent.isStopped = true;
                states = EnemyStates.攻击;
            }
            else {
                agent.isStopped = false;
                agent.destination = AttackObj.transform.position;
            }
        }

    }
    void Attack() {
        if (Vector3.Distance(transform.position, AttackObj.transform.position) < currentData.attRange) {
            if (!fristAttack) {
                anim.SetTrigger("attack1");
                fristAttack = true;
            }
            else {
                anim.SetTrigger("attack2");

            }

        }
        else {
            fristAttack = false;
            states = EnemyStates.追踪;
        }
    } 
    void ReturnToInitial() {
        if(AttackObj == null) {
            agent.isStopped = false;
            agent.destination = initPos;
            if (Vector3.Distance(initPos, transform.position) < 0.01f) 
            {
                states = EnemyStates.等待;
            }
        }
        else {
            states = EnemyStates.追踪;
        }
    }
    void Die() {
        agent.isStopped = true;
        death = true;


    }
    #endregion
    //private void OnDrawGizmosSelected() {
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, patrolRange);
    //}

    public virtual void Notify() {
        
    }
    //private void OnTriggerEnter(Collider other) {
    //    if (other.tag == "Player") {
    //        print("enter");
    //    }


    //}
    //private void OnTriggerStay(Collider other) {
    //    if (AttackObj==null&& other.tag == "Player") {
    //        // print("stay");
    //        AttackObj = other.gameObject;
    //    }
    //}
    //private void OnTriggerExit(Collider other) {
    //    if (AttackObj == other.gameObject) {
    //        AttackObj = null;
    //    }
    //}
    private void OnDisable() {
        ///EnemyManager.Instance.RemoveListenObserve(this);
    }
    public void Damage(float atkNum) {
        anim.SetTrigger("hit");
        currentData.hp = Mathf.Clamp(currentData.hp - atkNum + currentData.dft, 0,currentData.hp);
        if(currentData.hp == 0) {
            states = EnemyStates.死亡;
            EnemyManager.Instance.RemoveListenObserve(this);
        }
    }
    public void OnWeaponAttackStart() {
        wc.isUse = true;
        //print("OnWeaponAttackStart");
    }
    public void OnWeaponAttackStop() {
        wc.isUse = false;
    }
    //private void OnTriggerStay(Collider other) {

    //    if (other.tag == "Player" && AttackObj == null) {
    //        AttackObj = other.gameObject;
    //    }

    //    print(other.gameObject.name);
    //}
    //private void OnTriggerExit(Collider other) {
    //    if(AttackObj!=null&& other.gameObject == AttackObj) {
    //        AttackObj = null;
    //    }
    //}
    // Update is called once per frame
}
