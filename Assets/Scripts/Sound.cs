using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioSource ambient;
    public AudioSource hit;
    public AudioSource bored;
    public AudioSource excited;
    float excitedTimer;
    public float excitedTimeAdd;
    public Vector2 volumeChear;
    public float excitedSpeedUp;
    public float excitedSpeedDown;
    public float chantingStartTime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(excitedTimer >= 0){
            excited.volume = Mathf.Clamp(excited.volume + excitedSpeedUp * Time.deltaTime, volumeChear.x, volumeChear.y);
            if(!excited.isPlaying){
                excited.Play();
            }
        }
        else{
            excited.volume = Mathf.Clamp(excited.volume - excitedSpeedDown * Time.deltaTime, volumeChear.x, volumeChear.y);
            if(excitedTimer < -chantingStartTime && !bored.isPlaying){
                bored.Play();
            }
        }
        
        excitedTimer -= Time.deltaTime;
        //Debug.Log("Timer: " + excitedTimer);
        //Debug.Log("Timer: " + excitedTimer);
    }

    public void hitSounds(){
        hit.Play();
        excitedTimer = excitedTimeAdd; 
    }
}
