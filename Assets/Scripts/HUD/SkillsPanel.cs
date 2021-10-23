using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsPanel : MonoBehaviour {
    public Text[] names;
    public Text[] cds;
    public Text[] manaCosts;
    public Text[] descriptions;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void updateSkills(SkillManager skillMan) {
        Skill[] skills = skillMan.skills;
        for(int i = 0; i < skills.Length; i++) {
            names[i].text = skills[i].skillName;
            cds[i].text = skills[i].cooldown.ToString();
            manaCosts[i].text = skills[i].manaCost.ToString();
            descriptions[i].text = skills[i].description;
        }
    }
}
