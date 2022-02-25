using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punching : MonoBehaviour
{
    private List<GameObject> Gloves;
    private List<ParticleSystem> trails;
    private List<Vector3> initialTransform;
    private List<Vector3> restingPos;
    private List<Rigidbody> RBs;
    public float punchingSpeed;
    public float returnSpeed;

    public float maxPunchDist;
    List<Collider> Colliders;
    public GameObject player;
    public float rigidbodyOnTime = 2f;
    List<float> rigidbodies = new List<float>{0, 0};
    public GameObject cam;
    public bool punchOnlyWhenReturned = true;

    //guarding
    public float glovesYFace;
    public float glovesXFace; //for making them go close together to look cool
    bool bodyGuard = true;

    void Start()
    {
        Gloves = new List<GameObject>{GameObject.Find("GloveLeft"), GameObject.Find("GloveRight")};
        trails = new List<ParticleSystem>{Gloves[0].GetComponent<ParticleSystem>(), Gloves[1].GetComponent<ParticleSystem>()};
        RBs = new List<Rigidbody>{Gloves[0].GetComponent<Rigidbody>(), Gloves[1].GetComponent<Rigidbody>()};
        Colliders = new List<Collider>{Gloves[0].GetComponent<Collider>(), Gloves[1].GetComponent<Collider>()};
        initialTransform = new List<Vector3> {Gloves[0].transform.localPosition, Gloves[1].transform.localPosition}; //Remember to get local position (:
        restingPos = initialTransform;
    }

    public void punch(int i){
        //Debug.Log(Distance3D(Gloves[i].transform.localPosition - initialTransform[i]));
        if(Distance3D(Gloves[i].transform.localPosition - initialTransform[i]) <= .3f || !punchOnlyWhenReturned){
            //Debug.Log("punch");
            RBs[i].AddForce(cam.transform.forward * punchingSpeed * Time.deltaTime);
            RBs[i].isKinematic = false;
            trails[i].enableEmission = true;
            rigidbodies[i] = rigidbodyOnTime;
            Colliders[i].enabled = true;
        }
    }
    void Update()
    {
        if(!bodyGuard){
            restingPos = new List<Vector3> {initialTransform[0] + new Vector3(glovesXFace, glovesYFace, 0), initialTransform[1] + new Vector3(-glovesXFace, glovesYFace, 0)};
        }
        else{
            restingPos = initialTransform;
        }


        for(int i = 0; i <= 1; i++){
            if(Distance3D(Gloves[i].transform.localPosition - restingPos[i]) >= maxPunchDist){
                rigidbodies[i] = 0;
            }
            if(rigidbodies[i] > 0){
                rigidbodies[i] -= Time.deltaTime;
            }
            if(rigidbodies[i] <= 0 && RBs[i].isKinematic == false){
                RBs[i].isKinematic = true;
                trails[i].enableEmission = false;
            }

            if(RBs[i].isKinematic){
                Gloves[i].transform.localPosition = moveTowards(Gloves[i].transform.localPosition, restingPos[i], returnSpeed * Distance3D(Gloves[i].transform.localPosition - restingPos[i]));
                Colliders[i].enabled = false;
            }
        }
    }

    float Distance3D(Vector3 loc){
        float dist = Mathf.Sqrt(loc.x * loc.x + loc.y * loc.y + loc.z * loc.z);
        return dist;
    }

    public void switchGuard(bool up){
        if(up){
            bodyGuard = false;
            Debug.Log("guard face");
        }
        else{
            bodyGuard = true;
            Debug.Log("guard body");
        }
    }

    Vector3 moveTowards(Vector3 location, Vector3 target, float speed){
        Vector3 vector = Vector3.MoveTowards(location, target, speed * Time.deltaTime);
        return vector;
    }
}
