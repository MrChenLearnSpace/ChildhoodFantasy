using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySenser : MonoBehaviour
{
    public EnemyController ec;
    private void Awake() {
        ec = transform.parent.GetComponent<EnemyController>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            print("enter");
        }


    }
    private void OnTriggerStay(Collider other) {
        if (ec.AttackObj == null && other.tag == "Player") {
            // print("stay");
            ec.AttackObj = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (ec.AttackObj == other.gameObject) {
            ec.AttackObj = null;
        }
    }
}
