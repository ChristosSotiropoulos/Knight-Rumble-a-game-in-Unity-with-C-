using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

    public static Music me;
    public AudioSource audio1;


	// Use this for initialization
	void Start () {
        audio1 = GetComponent<AudioSource>();
        DontDestroyOnLoad (gameObject);
        if (me) Destroy (gameObject);
        else me = this;
	}


    // Update is called once per frame
    void Update () {
		
	}
}
