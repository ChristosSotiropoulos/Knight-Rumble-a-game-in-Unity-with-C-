using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;



public class UserController : MonoBehaviour {

    [SerializeField]
    int startingHealth = 100;
    [SerializeField]
    float timeSinceLastHit = 2f;
    [SerializeField]
    Slider healthSlider;
    [SerializeField]
    private AudioClip HitSound;
    [SerializeField]
    private AudioClip FireSound;
    [SerializeField]
    private AudioClip Swipe1;
    [SerializeField]
    private AudioClip Swipe2;

    private int currentHealth;
    private AudioSource audio2;
    private float timer = 0f;
    private BoxCollider[] swordColliders;
    

    [SerializeField]
    int startingEnergy = 100;
    [SerializeField]
    Slider energySlider;
    private int currentEnergy;
    [SerializeField]
    private ParticleSystem Fire;
    public GameObject FireCollider;
    [SerializeField]
    private GameObject FireLight;


    [SerializeField]
    private float mspeed;
    private Animator anim;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    

    public float grav = 6.0F;
    [SerializeField]
    Transform obj;
    [SerializeField]
    Joystick joystick;
    public float turretTraverseSpeed = 200f;
    public int damage = 0;
    [SerializeField]
    private ParticleSystem blood;
    public int attack;



    public int Attack
    {
        get  { return attack; }
        
    }
    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (value < 0)
                currentHealth = 0;
            else
                currentHealth = value;
        }
    }

    public int CurrentEnergy
    {
        get { return currentEnergy; }
        set
        {
            if (value < 0)
                currentEnergy = 0;
            else
                currentEnergy = value;
        }
    }




    void Start () {
        mspeed = 6f;

        anim = GetComponent<Animator> ();
        controller = GetComponent<CharacterController> ();
        currentEnergy = startingEnergy;
        currentHealth = startingHealth;
        Fire = GetComponentInChildren<ParticleSystem> ();
        FireCollider.SetActive (false);
        audio2 = GetComponent<AudioSource> ();
        swordColliders = GetComponentsInChildren<BoxCollider> ();
        FireLight.SetActive (false);
        attack = 0;
   
    }


    private void OnTriggerEnter (Collider other) {
        if (timer >= timeSinceLastHit && !GameManager.instance.GameOver) {
            if (other.tag == "Humanoid") {
                takeHit1 ();
                timer = 0;
                blood.Play ();
            }

        
        if (other.tag == "Troll") {
                takeHit2 ();
                timer = 0;
                blood.Play ();
            }
        
    
            if (other.tag == "Dragon") {
                takeHit3 ();
                timer = 0;
                blood.Play ();
            }
              
        }
    }


    void takeHit1 () {
        if (currentHealth > 0) {
            GameManager.instance.PlayerHit (currentHealth);
            anim.Play ("knight_hit");
            currentHealth -= 8;
            healthSlider.value = currentHealth;
            audio2.clip = HitSound;
            audio2.PlayOneShot (audio2.clip);
        }
        if (currentHealth <= 0) {
            killPlayer ();
        }
    }

    void takeHit2 () {
        if (currentHealth > 0) {
            GameManager.instance.PlayerHit (currentHealth);
            anim.Play ("knight_hit");
            currentHealth -= 10;
            healthSlider.value = currentHealth;
            audio2.clip = HitSound;
            audio2.PlayOneShot (audio2.clip);
        }
        if (currentHealth <= 0) {
            killPlayer ();
        }
    }

    void takeHit3 () {
        if (currentHealth > 0) {
            GameManager.instance.PlayerHit (currentHealth);
            anim.Play ("knight_hit");
            currentHealth -= 12;
            healthSlider.value = currentHealth;
            audio2.clip = HitSound;
            audio2.PlayOneShot (audio2.clip);
        }
        if (currentHealth <= 0) {
            killPlayer ();
        }
    }

    void killPlayer () {
        GameManager.instance.PlayerHit (currentHealth);
        anim.SetTrigger ("HeroDie");
        controller.enabled = false;
        audio2.PlayOneShot (audio2.clip);
    }


    void MoveTurret (Vector3 target) {
        Vector3 lookAt = transform.InverseTransformPoint (target);
        lookAt.y = 0;
        lookAt = transform.TransformPoint (lookAt);

        Quaternion rotation = transform.rotation;
        transform.LookAt (lookAt, transform.up);
        Quaternion lookRotation = transform.rotation;
        transform.rotation = Quaternion.RotateTowards (rotation, lookRotation, turretTraverseSpeed * Time.deltaTime);
    }

    private void Update () {
        if (!GameManager.instance.GameOver &&!GameManager.instance.GamePause) {
            timer += Time.deltaTime;
            if (joystick.IsMoving == true) {
                Vector3 moveDirection = new Vector3 (CrossPlatformInputManager.GetAxis ("Horizontal"), 0, CrossPlatformInputManager.GetAxis ("Vertical"));

                if (moveDirection.magnitude > 1f) moveDirection = moveDirection.normalized;
                moveDirection *= (mspeed * Time.deltaTime);
                moveDirection += grav * Vector3.down;
                controller.Move (moveDirection);
                MoveTurret (transform.position + moveDirection);
                anim.SetBool ("isWalking", true);
            }
            else {
                anim.SetBool ("isWalking", false);
            }
        }


    }

    public void Attack1 () {
        if (!GameManager.instance.GameOver && !GameManager.instance.GamePause) {
            anim.Play ("knight_attackNormal");
            audio2.clip = Swipe1;
            audio2.PlayOneShot (audio2.clip);
            damage = 5;
            attack = 1;
        }

    }

    public void Attack2 () {
        if (!GameManager.instance.GameOver && !GameManager.instance.GamePause) {
            if (currentEnergy > 4) {
                anim.Play ("knight_attackSpecial 0");
                currentEnergy -= 5;
                energySlider.value = currentEnergy;
                audio2.clip = Swipe2;
                audio2.PlayOneShot (audio2.clip);
                damage = 10;
                attack = 2;
            }
        }
    }

    public void Attack3 () {
        if (!GameManager.instance.GameOver && !GameManager.instance.GamePause) {
            if (currentEnergy > 19) {

                attack = 3;
                StartCoroutine (Firecasting ());

                currentEnergy -= 20;
                energySlider.value = currentEnergy;
                damage = 10;
            }

        }

    }
    IEnumerator Firecasting () {
        FireLight.SetActive (true);
        FireCollider.SetActive (true);
        audio2.clip = FireSound;
        audio2.PlayOneShot (audio2.clip);
        Fire.Play ();
        yield return new WaitForSeconds (1.5f);
        FireCollider.SetActive (false);
        FireLight.SetActive (false);
    }
    public void BeginAttack () {
        foreach (var weapon in swordColliders) {
            weapon.enabled = true;
        }
    }

    public void EndAttack () {
        foreach (var weapon in swordColliders) {
            weapon.enabled = false;
        }

    }

    public void PowerUpHealth () {
        int randomHealth = Random.Range (8, 14);
        if(currentHealth <= 100) {
            currentHealth += randomHealth;
            healthSlider.value = currentHealth;
        }

    }

    public void PowerUpEnergy () {
        int randomEnergy = Random.Range (10, 16);
        if(currentEnergy <= 100) {
            currentEnergy += randomEnergy;
            energySlider.value = currentEnergy;

        }

    }

}
