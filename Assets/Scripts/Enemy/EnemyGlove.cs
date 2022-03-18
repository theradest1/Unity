using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGlove : MonoBehaviour
{
    public bool ableToPunch;
    public Vector3 initialPos;
    public Vector2 faceGuardPos;
    public Rigidbody RB;
    public Collider coll;
    public float strength;
    public float hitChance;
    public float movementSpeed;
    public float switchChance;
    public bool punching;
    public GameObject enemy;
    public EnemyMovement enemyMovement;
    public float maxDist;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = this.gameObject.transform.localPosition;
        enemyMovement = enemy.GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 restingPos = initialPos;
        if(enemyMovement.faceGuard){
            restingPos += new Vector3(faceGuardPos.x, faceGuardPos.y, 0);
        }

        if(Distance3D(this.transform.localPosition - restingPos) <= .1){
            coll.enabled = true;
            ableToPunch = true;
        }
        else if(Distance3D(this.transform.localPosition - restingPos) > maxDist){
            StopPunching();
        }
        else if(RB.isKinematic){
            this.transform.localPosition = moveTowards(this.transform.localPosition, restingPos, movementSpeed * Distance3D(this.transform.localPosition - restingPos));
            coll.enabled = false;
        }

        if(punching){
            RB.AddForce(enemy.transform.forward * strength * Time.deltaTime);
        }
    }

    public void SwitchGuard(){
        if(Random.Range(0f, 1f) <= switchChance){
            enemyMovement.faceGuard = !enemyMovement.faceGuard;
        }
    }
    public void hit(){
        if(ableToPunch && Random.Range(0f, 1f) < hitChance){
            punching = true;
            coll.enabled = true;
            RB.isKinematic = false;
            Debug.Log("punching");
        }
    }

    Vector3 moveTowards(Vector3 location, Vector3 target, float speed){
        Vector3 vector = Vector3.MoveTowards(location, target, speed * Time.deltaTime);
        return vector;
    }

    float Distance3D(Vector3 loc){
        float dist = Mathf.Sqrt(loc.x * loc.x + loc.y * loc.y + loc.z * loc.z);
        return dist;
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Glove" || collision.gameObject.tag == "Player"){
            StopPunching();
            Debug.Log(collision.gameObject);
        }
    }

    void StopPunching(){
        ableToPunch = false;
        punching = false;
        coll.enabled = false;
        RB.isKinematic = true;
    }
    
}
