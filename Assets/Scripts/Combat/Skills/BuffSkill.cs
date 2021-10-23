using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkill : Skill {
    
    // Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		
	}
    
    override protected void useSkill(Vida vida) {
        GameObject skillObject = Instantiate(skillPrefab, vida.transform);
        Buff buff = skillObject.GetComponent<Buff>();
        buff.user = vida;
        SkillManager sm = vida.GetComponent<SkillManager>();
        if(sm) {
            sm.freezeSkill(skillName);
            buff.sm = sm;
        }
        buff.skillName = skillName;
        buff.enabled = true;
    }
}
