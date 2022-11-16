using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSenser : MonoBehaviour
{
    public CapsuleCollider capcol;
    Vector3 point1;
    Vector3 point2;
    public float radius;
    public float height;
    public bool isGround;
    // Start is called before the first frame update
    void Awake()
    {
        //capcol = transform.parent.GetComponent<CapsuleCollider>();
        //radius = capcol.radius;
        //height = capcol.height;
        
    }

    // Update is called once per frame
    private void FixedUpdate() {
        point1 = transform.position ;
        point2 = transform.position + transform.up * (-height);
        Collider[] colliders = Physics.OverlapCapsule(point1, point2, radius);
        
        foreach(var col in colliders) {

           // print(col.name);
           if (col.gameObject.name != "player") {
                isGround = true;
                break;
            }
            isGround = false;
        }
         transform.parent.GetComponent<ActorController>().SetAnimatorIsOnGround(isGround);
    }
}
