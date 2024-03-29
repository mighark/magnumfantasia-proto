﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vida : MonoBehaviour {
    ///private Vector3 posicionOriginal;
    public int maxHealth = 10;
    public int currentHealth;
    public int maxMana;
    public int currentMana = 0;
    public int attack;
    public int defense;
    public int magatk;
    public int magdef;
    //public int crit;
    /*public float invulnerabilidadTimer = 0.5f;*/
    
    private bool invulnerabilidad = false;
    
    public delegate void effect();
    public event effect onAttackEffect;
    public event effect onHitEffect; 

    //private int nextCrit = 0;
    /*private Animator animator;
    private SpriteRenderer mySpriteRenderer;
    private AudioSource[] sonidos;
    public float timeDeath = 0.45f;*/

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    
    public void damageToHealth(int damage){
        if(!invulnerabilidad){
            currentHealth -= damage;
        }
    }
    
    public void gainMana(int mana) {
        currentMana += mana;
        if(currentMana > maxMana) {
            currentMana = maxMana;
        } else if(currentMana < 0) {
            currentMana = 0;
        }
    }
    
    public void startAttack() {
        if(onAttackEffect != null) {
            onAttackEffect();
        }
    }
    
    public void attackHit() {
        if(onHitEffect != null) {
            onHitEffect();
        }
    }
    
    /*public void critUp() {
        nextCrit += crit;
    }
    
    public void critReset() {
        nextCrit = 0;
    }
    
    public bool isCrit() {
        if(nextCrit >= 100) {
            return true;
        }
        return false;
    }
    
    public bool critHit() {
        if(nextCrit >= 100) {
            nextCrit -= 100;
            return true;
        }
        return false;
    }*/
}
