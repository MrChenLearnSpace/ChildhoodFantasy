using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkFoot : MonoBehaviour
{
    //public List<Transform> FootTarget;
    public Transform LFootTarget;
    public Transform RFootTarget;
    
    public float RayDistance=1;
    public float UpDistance = 0.15f;
    public string thresholdVariable;
    public Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    //private void LateUpdate() {

    //    PlaneIK(RFootTarget, AvatarIKGoal.RightFoot, AvatarIKHint.RightKnee);
    //    PlaneIK(LFootTarget, AvatarIKGoal.LeftFoot, AvatarIKHint.LeftKnee);
    //    //for (int i = 0; i < FootTarget.Count; i++) {
    //    //    var foot = FootTarget[i];
    //    //    var ray = new Ray(foot.transform.position + Vector3.up * 0.5f, Vector3.down);
    //    //    var hitInfo = new RaycastHit();
    //    //    if (Physics.SphereCast(ray, 0.05f, out hitInfo, RayDistance))
    //    //        foot.position = hitInfo.point + Vector3.up * UpDistance;
    //    //}

    //}

    private void OnAnimatorIK(int layerIndex) {
        if (animator.GetFloat(thresholdVariable) < 0.1) {
            PlaneIK(RFootTarget, AvatarIKGoal.RightFoot, AvatarIKHint.RightKnee);
            PlaneIK(LFootTarget, AvatarIKGoal.LeftFoot, AvatarIKHint.LeftKnee);
        }

    }
    void PlaneIK(Transform foot,AvatarIKGoal g,AvatarIKHint h) {
        var ray = new Ray(foot.transform.position + Vector3.up * 0.5f, Vector3.down);
        var hitInfo = new RaycastHit();

        if (Physics.SphereCast(ray, 0.05f, out hitInfo, RayDistance)) {
            animator.SetIKPositionWeight(g, 1.0f);
            animator.SetIKPosition(g, hitInfo.point+ Vector3.up * UpDistance);
            
        }
        // foot.position = hitInfo.point + Vector3.up * UpDistance;
    }
    // Start is called before the first frame update

}
