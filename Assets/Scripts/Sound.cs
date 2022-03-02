using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioSource ambient;
    public AudioSource hit;
    public AudioSource bored;
    public AudioSource excited;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ambient.pitch = Time.timeScale;
    }

    public void hitSounds(){
        hit.Play();
        excited.PlayDelayed(1);
    }
}
