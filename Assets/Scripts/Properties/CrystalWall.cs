using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalWall : MonoBehaviour {
    private Animator[] animators;
    
    private int numLeft;
    public Enemy[] enemies;
    
    
    void Start() {
        animators = GetComponentsInChildren<Animator>();
        
        foreach(Enemy enemy in enemies) {
            enemy.asocCW = this;
        }
        numLeft = enemies.Length;
    }

	public void update() {
        numLeft--;
        
        if(numLeft <= 0) {
            foreach(Animator animator in animators) {
                animator.SetTrigger("Fade");
            }
            Invoke("fade", 1f);
        }
    }
    
    void fade() {
        gameObject.SetActive(false);
    }
}
