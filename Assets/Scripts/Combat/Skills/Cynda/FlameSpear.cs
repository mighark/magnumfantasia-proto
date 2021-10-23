using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSpear : Buff {
    //public Vida user;
    //public SkillManager sm;
    public GameObject flames;
    public Vector3 offset;
    public Vector3 rotation;
    //public string skillName;

	// Use this for initialization
	void OnEnable() {
		user.onAttackEffect += useSkill; 
	}
	
	// Update is called once per frame
	void Update() {
		
	}
    
    void OnDisable() {
        user.onAttackEffect -= useSkill; 
    }
    
    void useSkill() {
        Vector3 position = user.transform.position - new Vector3(offset.x * user.transform.localScale.x, offset.y, offset.z);
        Vector3 adjRotation = rotation * user.transform.localScale.x;
        GameObject flameSpear = MonoBehaviour.Instantiate(flames, position, Quaternion.Euler(adjRotation));
        Weapon flameWeapon = flameSpear.GetComponent<Weapon>();
        flameWeapon.setUser(user);
        if(sm) {
            sm.unfreezeSkill(skillName);
        }
        Destroy(gameObject);
    }
}
