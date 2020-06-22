using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour {

    
    private Transform player;
    private NavMeshAgent nav;
    private Animator anim;
    private EnemyHealth enemyHealth;
    private EnemyHealth status;
	// Use this for initialization
	void Start () {
        player = GameManager.instance.Player.transform;
        status = GetComponent<EnemyHealth> ();
        enemyHealth = GetComponent<EnemyHealth> ();
        anim = GetComponent<Animator> ();
        nav = GetComponent<NavMeshAgent> ();
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameManager.instance.GameOver && enemyHealth.IsAlive && !GameManager.instance.GamePause) {
            if (!status.IsStun) {
                nav.enabled = true;
                nav.SetDestination (player.position);
                anim.SetBool ("EndStun", true);
                
            }
            else {
                anim.SetBool ("EndStun", false);
                anim.Play ("Idle");

            }
        }
        else if((!GameManager.instance.GameOver || GameManager.instance.GameOver) && !enemyHealth.IsAlive){ 
            nav.enabled = false;
        }
        else {
            nav.enabled = false;
            anim.Play ("Idle");
        }

      
    }
}
