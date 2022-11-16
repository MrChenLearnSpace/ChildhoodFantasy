using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHp : MonoBehaviour
{
    public ActorData actorData;
    Image hp;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        //if(actorData ==null)
        actorData = transform.parent.parent.GetComponent<EnemyController>().currentData;
        hp = GetComponent<Image>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.forward = -cam.transform.forward;
        hp.fillAmount = actorData.hp / actorData.Maxhp;
    }
}
