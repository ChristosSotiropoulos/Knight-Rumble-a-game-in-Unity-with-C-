using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {




    [SerializeField]
    private float Range = 3f;

    //public float timeBetweenAttacks ;
    [SerializeField]
    private float timeBetweenAttacks1 = 1f;
    [SerializeField]
    private float timeBetweenAttacks2 = 1f;

    private Animator anim;
    private GameObject player;
    private bool playerInRange;
    private BoxCollider[] weaponColliders;
    private EnemyHealth enemyHealth;

	// Use this for initialization
	void Start () {
        enemyHealth = GetComponent<EnemyHealth> ();
        weaponColliders = GetComponentsInChildren<BoxCollider> ();
        player = GameManager.instance.Player;
        anim = GetComponent<Animator> ();
        StartCoroutine (attack ());
        playerInRange = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(transform.position, player.transform.position) < Range && enemyHealth.IsAlive) {
            playerInRange = true;
        }
        else {
            playerInRange = false;
            foreach (var weapon in weaponColliders) {
                weapon.enabled = false;
            }
        }
	}

    IEnumerator attack() {
        if(playerInRange && !GameManager.instance.GameOver && !GameManager.instance.GamePause) {
            anim.Play ("Attack");
            float timeBetweenAttacks = Random.Range (timeBetweenAttacks1, timeBetweenAttacks2);
            yield return new WaitForSeconds (timeBetweenAttacks);
        }
        yield return null;
        StartCoroutine (attack ());
    }

    public void EnemyBeginAttack () {
        foreach (var weapon in weaponColliders) {
            weapon.enabled = true;
        }
    }

    public void EnemyEndAttack () {
        foreach (var weapon in weaponColliders) {
            weapon.enabled = false;
        }
    }
}
