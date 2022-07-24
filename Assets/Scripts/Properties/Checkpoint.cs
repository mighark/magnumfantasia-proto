using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int number;
    public StageFinish gameController;
    public GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("checkpoint", number + 1) == number) {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.CompareTag("Player")){
            gameController.setCheckpoint(number, collider.transform.position);
            gameObject.SetActive(false);
            Instantiate(text, transform.position, transform.rotation);
        }
    }
}
