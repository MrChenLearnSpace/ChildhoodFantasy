using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class EnemyManager : Singleton<EnemyManager>
{
    public  List<EnemyController> enemies;
    protected override void Awake() {
        base.Awake();
    }
    public void AddListenObserve(EnemyController obj) {
        enemies.Add(obj);
    } 
    public void RemoveListenObserve(EnemyController obj) {
        enemies.Remove(obj);
    }
    // Start is called before the first frame update
   
}
