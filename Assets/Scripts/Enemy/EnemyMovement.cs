using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public GameObject Player;
    public GameObject lookPlace;

    public float speed;

    public float maxDist;
    public float minDist;

    int strapheDirection = 1;
    public float strapheSpeed;
    public float strapheChangeChance;
    public float strapheChangeCallTime;
    public float punchCallTime;
    int moveDirection = 1;

    public float lookSmoothing;
    List<EnemyGlove> gloves;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ChangeStraphe", 0f, strapheChangeCallTime);
        InvokeRepeating("RandomPunch", 0f, punchCallTime);
        gloves = new List<EnemyGlove>{GameObject.Find("LeftGloveEnemy").GetComponent<EnemyGlove>(), GameObject.Find("RightGloveEnemy").GetComponent<EnemyGlove>()};
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.LookAt(new Vector3(lookPlace.transform.position.x, this.transform.position.y, lookPlace.transform.position.z));
        lookPlace.transform.position = Vector3.Lerp(lookPlace.transform.position, Player.transform.position, lookSmoothing * Time.deltaTime);
        
        float dist = Distance3D(this.gameObject.transform.position - Player.transform.position);
        if(dist > maxDist){
            this.gameObject.transform.position += this.gameObject.transform.forward * speed * Time.deltaTime;
            lookPlace.transform.position += this.gameObject.transform.forward * speed * Time.deltaTime;
        }
        else if(dist < minDist){
            this.gameObject.transform.position -= this.gameObject.transform.forward * speed * Time.deltaTime;
            lookPlace.transform.position += this.gameObject.transform.forward * speed * Time.deltaTime;
        }
        else{
            this.gameObject.transform.position += this.gameObject.transform.right * strapheSpeed * Time.deltaTime * strapheDirection;
            this.gameObject.transform.position += this.gameObject.transform.forward * strapheSpeed * Time.deltaTime * moveDirection;

            lookPlace.transform.position += this.gameObject.transform.right * strapheSpeed * Time.deltaTime * strapheDirection;
            lookPlace.transform.position += this.gameObject.transform.forward * strapheSpeed * Time.deltaTime * moveDirection;
        }
    }

    void RandomPunch(){
        if(Distance3D(this.gameObject.transform.position - Player.transform.position) < maxDist){
            gloves[Random.Range(0, 2)].hit();
        }
    }

    void ChangeStraphe(){
        float rand = Random.Range(0f, 1f);
        if(rand < strapheChangeChance){
            strapheDirection *= -1;
            float dist = Distance3D(this.gameObject.transform.position - Player.transform.position);
            if(dist < maxDist && dist > minDist){
                gloves[Random.Range(0, 2)].hit();
            }
            
        }
        rand = Random.Range(0f, 1f);
        if(rand < strapheChangeChance){
            moveDirection *= -1;
            Debug.Log("change");
        }
    }

    float Distance3D(Vector3 loc){
        float dist = Mathf.Sqrt(loc.x * loc.x + loc.y * loc.y + loc.z * loc.z);
        return dist;
    }
}
