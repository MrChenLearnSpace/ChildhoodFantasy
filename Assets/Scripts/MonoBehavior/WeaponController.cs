using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    public string Name;
    public bool isUse;
    public GameObject User;
    
    public WeaponData weapon;
    public ParticleSystem tails;
    // Start is called before the first frame update
    private void Start() {
        for(int i=0;i< GameManager.Instance.weapon.dataArray.Length;i++) {
            if(GameManager.Instance.weapon.dataArray[i].Name == Name) {
                weapon = WeaponDataClone(GameManager.Instance.weapon.dataArray[i]);
                break;
            }
        }
        tails = transform.DFSFindChildGameObject("Tails")?.GetComponent<ParticleSystem>();

    }
    WeaponData WeaponDataClone(WeaponData weaponData) {
        WeaponData temp = new WeaponData();
        temp.Atk = weaponData.Atk;
        temp.Dft = weaponData.Dft;
        temp.Hpmax = weaponData.Hpmax;
        temp.Info = weaponData.Info;
        return temp;
    }
        private void OnTriggerEnter(Collider other) {
        //print("111");
        if (isUse&& !other.gameObject.Equals(User)) {
            EnemyController ec;
            ActorController ac;
            if (other.TryGetComponent<EnemyController>(out ec)) {
                ec.Damage(User.GetComponent<ActorController>().playerData.atk + weapon.Atk);
            }else if(other.TryGetComponent<ActorController>(out ac)) {
                ac.Damage(User.GetComponent<EnemyController>().currentData.atk + weapon.Atk);
            }
        }
    }
}
