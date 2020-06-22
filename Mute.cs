using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mute : MonoBehaviour {
    
    public AudioListener audiolistener;
    public bool ismute=false;

    public bool IsMute
    {
        get { return ismute; }
    }
    void Start () {
       
        
	}
	
   public void Mute2 () {
        
            AudioListener.pause = true;
        ismute = true;
        }
    public void NotMute2 () {
        AudioListener.pause = false;
        ismute = false;
    }
   
    }

