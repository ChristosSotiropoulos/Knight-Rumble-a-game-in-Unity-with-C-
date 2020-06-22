using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

    [SerializeField]
    private int startingHeath = 20;

    [SerializeField]
    private float timeSinceLastHit = 0.5f;

    [SerializeField]
    private float dissapearSpeed = 2f;




    private UserController damage;
    private AudioSource audio2;
    private float timer = 0f;
    private Animator anim;
    private NavMeshAgent nav;
    private bool isAlive;
    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;
    private bool dissapearEnemy = false;
    private int currentHealth;
    private int healthdamage = 0;
    [SerializeField]
    private ParticleSystem blood;
    private ParticleSystem stun;
    private bool isStun;
    private bool maceattack;
    private UserController usercontroller;
    public GameObject HealthMeter;

    public GameObject Health;
    public GameObject Energy;


    public bool IsAlive
    {
        get { return isAlive; }
    }

    public bool IsStun
    {
        get { return isStun; }
    }
    // Use this for initialization
    void Start () {

        GameManager.instance.RegisterEnemy (this);
        rigidBody = GetComponent<Rigidbody> ();
        capsuleCollider = GetComponent<CapsuleCollider> ();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audio2 = GetComponent<AudioSource> ();
        isAlive = true;
        currentHealth = startingHeath;
        healthdamage = 0;
        stun = GetComponentInChildren<ParticleSystem> ();
        isStun = false;
        maceattack = false;
        usercontroller = GameManager.instance.Player2;
        HealthMeter.SetActive (true);
        //usercontroller.GetComponent<UserController> ();

    }
	

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;

        if (dissapearEnemy) {
            transform.Translate (-Vector3.up * dissapearSpeed * Time.deltaTime);
        }
        if(usercontroller.Attack == 1) {
            Damage1 ();
        }

        if (usercontroller.Attack == 2) {
            Damage2 ();
        }

        if (usercontroller.Attack == 3) {
            Damage3 ();
        }
    }

    

    private void OnTriggerEnter (Collider other) {
         if(timer >= timeSinceLastHit && !GameManager.instance.GameOver) {
            if(other.tag == "PlayerWeapon") {

                takeHit ();
                timer = 0f;
                blood.Play();
            }
            if (other.tag == "Fire") {
               
                takeHit ();
                timer = 0f;
            }
        }
    }


   public void Damage1 () {
   healthdamage = 5;
      maceattack = false;
    }

    public void Damage2 () {
      healthdamage = 10;
     maceattack = true;
  }

  public void Damage3 () {
    healthdamage = 10;
    maceattack = false;
    }

    void takeHit() {
        
        if(currentHealth > 0  ) {
            if (maceattack) {

                StartCoroutine (Stun ());
            }
            audio2.PlayOneShot (audio2.clip);
            anim.Play ("Hurt");
            if(healthdamage == 10) {
                currentHealth -= healthdamage;
                Vector3 originalScale = HealthMeter.transform.localScale;
                Vector3 destinationScale = new Vector3 (originalScale.x * 0.3f, originalScale.y * 0.3f, originalScale.z * 0.3f);
                HealthMeter.transform.localScale = Vector3.Lerp (originalScale, destinationScale, 0.5f);
            }
            if (healthdamage == 5) {
                currentHealth -= healthdamage;
                Vector3 originalScale = HealthMeter.transform.localScale;
                Vector3 destinationScale = new Vector3 (originalScale.x * 0.3f, originalScale.y * 0.3f, originalScale.z * 0.3f);
                HealthMeter.transform.localScale = Vector3.Lerp (originalScale, destinationScale, 0.35f);
            }
        }
        if(currentHealth <= 0) {
            HealthMeter.SetActive (false);
            stun.Stop ();
            isAlive = false;
            KillEnemy ();
            if (Random.value < .6f) Instantiate (Energy, transform.position, Energy.transform.rotation);
            else Instantiate (Health, transform.position, Health.transform.rotation);
           
        }
    }



    void KillEnemy() {
        GameManager.instance.KilledEnemy (this);
        stun.Stop ();
        
        capsuleCollider.enabled = false;
        nav.enabled = false;
        anim.SetTrigger ("EnemyDie");
        rigidBody.isKinematic = true;
        StartCoroutine (removeEnemy());
    }

    IEnumerator removeEnemy () {
        yield return new WaitForSeconds (4f);
        dissapearEnemy = true;
        yield return new WaitForSeconds (2f);
        Destroy (gameObject);
    }

    IEnumerator Stun () {
        isStun = true;
        stun.Play();
        anim.SetBool ("EndStun", false);
        yield return new WaitForSeconds (3f);
        isStun = false;
        stun.Stop ();
        anim.SetBool ("EndStun", true);
    }

}
