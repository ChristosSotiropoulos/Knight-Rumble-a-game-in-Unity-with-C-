using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPower : MonoBehaviour {

    private GameObject player;
    private UserController playerHealth;

    public float rotspeed = 180f;
    public float vspeed = 20f;
    public float grav = 10f;
    public float minY = 2.5f;

    // Use this for initialization
    void Start () {
        player = GameManager.instance.Player;
        playerHealth = player.GetComponent<UserController> ();
	}
	
	// Update is called once per frame
	void Update () {
        transform.localEulerAngles += rotspeed * Time.deltaTime * Vector3.up;
        Vector3 p = transform.position;
        p.y += vspeed * Time.deltaTime;
        if (p.y < minY) p.y = minY;
        transform.position = p;
        vspeed -= grav * Time.deltaTime;
    }

    void OnTriggerEnter (Collider other) {
      if(other.gameObject == player) {
            playerHealth.PowerUpHealth ();
            Destroy (gameObject);
        }   
    }

}
