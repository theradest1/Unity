using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    Sound sound;
    public int maxHealth;
    float health;
    public Slider healthBar;

    public float resistance;
    public float minStrength;
    public float camMult;
    public CamShake camShake;


    // Start is called before the first frame update

    void Start(){
        health = maxHealth;
        sound = GameObject.Find("SoundManager").GetComponent<Sound>();
    }
    void OnCollisionEnter(Collision collision)
    { 

        //Debug.Log(strength);
        //Debug.Log(strength);
        if(collision.gameObject.tag == "EnemyGlove" && collision.GetContact(0).thisCollider.gameObject.tag != "Glove")
        {
            float strength = collision.relativeVelocity.magnitude;
            if(strength >= minStrength){
                sound.hitSounds();
                StartCoroutine(camShake.Shake(.1f, strength * camMult));
                health -= strength/resistance;
                healthBar.value = health/maxHealth;
            }
        }
    }
}
