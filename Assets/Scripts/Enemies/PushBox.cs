using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBox : MonoBehaviour {
    public float pushForceClose = 10.0f;
    public float pushForceFar = 5.0f;
    public float closeThres = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void OnTriggerStay2D(Collider2D collider) {
        float push = pushForceFar;
        if (collider.CompareTag("Player") && collider.attachedRigidbody.velocity.y < 0) {
            float distance = collider.transform.position.x - transform.position.x;
            
            //if close to pushbox, push farther to get you out
            if(Mathf.Abs(distance) < closeThres) {
                push = pushForceClose;

            }
            
            //push to right or left depending on position
            if(distance < 0) {
                push = -push;
            }
            
            collider.GetComponent<JugadorControl>().shortKnockback(push);
        }
    }
}
