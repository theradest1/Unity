using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioSource ambient;
    public AudioSource hit;
    public AudioSource bored;
    public AudioSource excited;
    public AudioSource excitedUp;
    float excitedTimer;
    public float excitedTimeAdd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(excitedTimer >= 0 && !excited.isPlaying && !excitedUp.isPlaying){
            excited.Play();
        }
        else{
            excitedTimer -= Time.deltaTime;
        }
        //ambient.pitch = Time.timeScale;
    }

    public void hitSounds(){
        hit.Play();
        if(!excited.isPlaying){
            excitedUp.Play();
            excitedTimer = excitedTimeAdd;
            excited.PlayDelayed(1);
        }
        
    }
}
