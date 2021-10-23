using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public CrystalWall asocCW;
    public int lastHitID = 0;

	virtual public void takeDamage(Weapon attack) {
        
    }
}
