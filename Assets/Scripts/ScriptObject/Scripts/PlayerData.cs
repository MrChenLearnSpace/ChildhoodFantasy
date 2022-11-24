using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Character/Player Data", fileName = "New PlayerData")]
[System.Serializable]
public class PlayerData : ActorData {
    // Start is called before the first frame update
    public List<WeaponData> weaponDatas = new List<WeaponData>();
    public List<FoodData> foodDatas = new List<FoodData>();
}
