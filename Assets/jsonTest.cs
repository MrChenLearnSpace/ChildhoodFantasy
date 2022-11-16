using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jsonTest : MonoBehaviour
{
    public PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
        print( JsonUtility.ToJson(playerData));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
