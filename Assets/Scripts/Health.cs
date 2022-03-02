using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public float height;
    public Slider healthBar;
    public Canvas healthBarCanvas;
    public float resistance;
    public GameObject player;
    public GameObject hitParticles;
    public float minStrength;
    Sounds sounds;

    // Start is called before the first frame update
    void Start()
    {
        sounds = GameObject.Find("SoundManager").GetComponent<Sounds>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBarCanvas.transform.position = new Vector3(0, height, 0) + this.transform.position;
        healthBarCanvas.transform.LookAt(player.transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {   
        float strength = collision.relativeVelocity.magnitude;

        //Debug.Log(strength);
        //Debug.Log(strength);
        if(strength > minStrength){
            sounds.hitSounds();
            Instantiate(hitParticles, collision.contacts[0].point, healthBarCanvas.transform.rotation);
            health -= strength/resistance;

            if(health <= 0){
                health = maxHealth;
            }

            healthBar.value = health/maxHealth;

        }
    }
}
