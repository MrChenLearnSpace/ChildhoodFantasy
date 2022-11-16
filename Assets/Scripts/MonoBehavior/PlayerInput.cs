using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("===== 按键设置 =====")]
    public string KeyUp;
    public string KeyDown;
    public string KeyLeft;
    public string KeyRight;
    public string KeyA; 
    public string KeyB;
    public string KeyC;
    public string KeyD;
    [Header("===== 输出信号 =====")]
    public float Dup;
    public float Dright;
    public float SmoothTime;
    public float Dmag;
    public Vector3 Dvec;
    public bool run;
    public bool jump;
    [Header("===== 其他 =====")]
    public bool inputEnable=true;
    float targetDup;
    float targetDright;
    float velocityDup;
    float velocityDright;
    [SerializeField]
    Vector3 directiion;
    Vector3 camDirForward;
    Vector3 camDirRight;
    Camera cam;
    bool lateJump;
    private void Awake() {
        cam = transform.Find("Camera").GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveDirectionInputSingle();
        run = Input.GetKey(KeyA);
        if(Input.GetKey(KeyB)&& Input.GetKey(KeyB) != lateJump) {
            jump = true;
        }
        else {
            jump = false;
        }
        lateJump = Input.GetKey(KeyB);
         
    }
    void MoveDirectionInputSingle() {
        //移动方向信号
        if (inputEnable) {
            targetDup = (Input.GetKey(KeyUp) ? 1 : 0) - (Input.GetKey(KeyDown) ? 1 : 0);
            targetDright = (Input.GetKey(KeyRight) ? 1 : 0) - (Input.GetKey(KeyLeft) ? 1 : 0);
        }
        else {
            targetDup = 0;
            targetDright = 0;
        }
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, SmoothTime);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, SmoothTime);

        directiion = new Vector3(Dup, 0, Dright).normalized;
        camDirForward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
        camDirRight = new Vector3(cam.transform.right.x, 0, cam.transform.right.z).normalized;
        Dmag = Mathf.Sqrt(Dup * Dup + Dright * Dright);
        Dvec = directiion.x * camDirForward + directiion.z * camDirRight;
    }
}
