using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionController : MonoBehaviour {
    public Transform LFootTarget;
    public Transform RFootTarget;
    public Animator anim;
    public ActorController ac;
    public Vector3 vector3;
    public float RayDistance = 1;
    public float UpDistance = 0.15f;

    WeaponController wc;
    // Start is called before the first frame update
    void Awake() {
        anim = GetComponent<Animator>();
        ac = transform.parent.parent.GetComponent<ActorController>();
        LFootTarget = transform.DFSFindChildGameObject("足首D.L");
        RFootTarget = transform.DFSFindChildGameObject("足首D.R");
    }

    // Update is called once per frame
    void Update() {

    }
    private void OnAnimatorMove() {
        vector3 = anim.deltaPosition;
        ac.deltaPosition += anim.deltaPosition;
    }
    private void OnAnimatorIK(int layerIndex) {
        if (anim.GetFloat("forward") < 0.1) {
            PlaneIK(RFootTarget, AvatarIKGoal.RightFoot, AvatarIKHint.RightKnee);
            PlaneIK(LFootTarget, AvatarIKGoal.LeftFoot, AvatarIKHint.LeftKnee);
        }
        
    }
    void PlaneIK(Transform foot, AvatarIKGoal g, AvatarIKHint h) {
        var ray = new Ray(foot.transform.position + Vector3.up * 0.5f, Vector3.down);
        var hitInfo = new RaycastHit();

        if (Physics.SphereCast(ray, 0.05f, out hitInfo, RayDistance)) {
            anim.SetIKPositionWeight(g, 1.0f);
            anim.SetIKPosition(g, hitInfo.point + Vector3.up * UpDistance);

        }
        // foot.position = hitInfo.point + Vector3.up * UpDistance;
    }
    
    public void OnWeaponAttackStart() {
        if(wc==null) {
            if(ac.RHandWeaponPos.childCount<1&&ac.LHandWeaponPos.childCount<1) {
                return;
            }
            else if(ac.LHandWeaponPos.childCount < 1) {
                wc =  ac.RHandWeaponPos.GetChild(0).GetComponent<WeaponController>();
            }
            else if (ac.RHandWeaponPos.childCount < 1) {
                wc = ac.LHandWeaponPos.GetChild(0).GetComponent<WeaponController>();
            }
        }
        wc.isUse = true;
        wc.tails.Play();
        //print("OnWeaponAttackStart");
    }
    public void OnWeaponAttackStop() {
        wc.tails.Stop();
        wc.isUse = false;

    }

}
