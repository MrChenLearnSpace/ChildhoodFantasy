using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float distance = 15;
    public float rot = 0;
    public float roll = 30f * Mathf.PI * 2 / 360;
    public float Speed = 0.5f;
    public GameObject target;
    public bool isEable = true;
    // Start is called before the first frame update
    void Start()
    {
        target = transform.parent.Find("LookAtPoint").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEable) {
            //-1.5  1.5
            //roll += Input.GetAxis("Mouse Y")*Speed; 
            roll = Mathf.Clamp(roll + Input.GetAxis("Mouse Y") * Speed, -1.5f, 1.5f);
            rot -= Input.GetAxis("Mouse X") * Speed;
            //0.5
            //distance -= Input.mouseScrollDelta.y*Speed;
            distance = Mathf.Clamp(distance - Input.mouseScrollDelta.y * Speed, 0.5f, 4.28f);
        }
    }
    private void LateUpdate() {
        if (target == null)
            return;

        Vector3 targetPos = target.transform.position;
        Vector3 camPos;
        float d = distance * Mathf.Cos(roll);
        float height = distance * Mathf.Sin(roll);
        camPos.x = targetPos.x + d * Mathf.Cos(rot);
        camPos.z = targetPos.z + d * Mathf.Sin(rot);
        camPos.y = targetPos.y + height;
        transform.position = Vector3.Lerp(transform.position,camPos,0.1f);
        transform.LookAt(target.transform);
    }
}
