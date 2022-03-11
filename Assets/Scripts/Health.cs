using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    Sound sound;
    public GameObject player;
    public GameObject hitParticles;
    public GameObject head;

    public Slider healthBar;
    public Canvas healthBarCanvas;
    public CamShake camShake;

    public float maxHealth;
    public float health;
    public float healthHeight;

    public float resistance;
    public float minStrength;
    public float critDmg;

    public bool debugHits;

    // Start is called before the first frame update
    void Start()
    {
        sound = GameObject.Find("SoundManager").GetComponent<Sound>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBarCanvas.transform.position = new Vector3(0, healthHeight, 0) + this.transform.position;
        healthBarCanvas.transform.LookAt(player.transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {   
        float strength = collision.relativeVelocity.magnitude;

        //Debug.Log(strength);
        //Debug.Log(strength);
        if(collision.gameObject.tag == "Glove")
        {
            collision.gameObject.GetComponent<Glove>().ableToPunch = false;
            collision.gameObject.GetComponent<Glove>().RB.isKinematic = true;
            collision.gameObject.GetComponent<Glove>().trail.emitting = false;
            collision.gameObject.GetComponent<Glove>().collider.enabled = false;
            if(strength > minStrength){
            StartCoroutine(camShake.Shake(.1f, strength));
            sound.hitSounds();
            Instantiate(hitParticles, collision.contacts[0].point, healthBarCanvas.transform.rotation);

            if(debugHits){
                if(collision.GetContact(0).thisCollider.gameObject == head){
                    strength *= critDmg;
                    Debug.Log("Head hit, Strength: " + strength + ", Glove: " + collision.gameObject);
                }
                else{
                    Debug.Log("Body hit, Strength: " + strength + ", Glove: " + collision.gameObject);
                }
            }
            

            health -= strength/resistance;

            if(health <= 0){
                health = maxHealth;
            }

            healthBar.value = health/maxHealth;
        }
        

        }
    }
}
