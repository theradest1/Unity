using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Punching : MonoBehaviour
{
    private List<GameObject> Gloves;
    private List<TrailRenderer> trails;
    private List<Vector3> initialTransform;
    private List<Vector3> restingPos;
    private List<Rigidbody> RBs;
    public float punchingSpeed;
    public float returnSpeed;
    public GameObject Enemy;

    public float maxPunchDist;
    List<Collider> Colliders;
    public float rigidbodyOnTime = 2f;
    List<float> rigidbodies = new List<float>{0, 0};
    public GameObject cam;
    public bool punchOnlyWhenReturned = true;
    public float minSlowdown;
    public float maxSlowdown;
    public float slowdownValue;


    //guarding
    public float glovesYFace;
    public float glovesXFace; //for making them go close together to look cool
    List<bool> bodyGuard = new List<bool>{true, true};

    void Start()
    {
        Gloves = new List<GameObject>{GameObject.Find("GloveLeft"), GameObject.Find("GloveRight")};
        trails = new List<TrailRenderer>{Gloves[0].GetComponent<TrailRenderer>(), Gloves[1].GetComponent<TrailRenderer>()};
        RBs = new List<Rigidbody>{Gloves[0].GetComponent<Rigidbody>(), Gloves[1].GetComponent<Rigidbody>()};
        Colliders = new List<Collider>{Gloves[0].GetComponent<Collider>(), Gloves[1].GetComponent<Collider>()};
        initialTransform = new List<Vector3> {Gloves[0].transform.localPosition, Gloves[1].transform.localPosition}; //Remember to get local position (:
        restingPos = initialTransform;
    }

    public void punch(List <Vector2> vectors){
        //Debug.Log(Distance3D(Gloves[i].transform.localPosition - initialTransform[i]));
        for(int i = 0; i <= 1; i++){
            if(Distance3D(Gloves[i].transform.localPosition - initialTransform[i]) <= maxPunchDist && vectors[i] != Vector2.zero){
                //Debug.Log("punch");
                RBs[i].AddRelativeForce(new Vector3(vectors[i].x, 0, vectors[i].y) * punchingSpeed * Time.deltaTime);
                RBs[i].isKinematic = false;
                trails[i].emitting = true;
                rigidbodies[i] = rigidbodyOnTime;
                Colliders[i].enabled = true;
            }
            //Debug.Log(vectors[i]);
        }
    }
    void Update()
    {
        List<float> endTimeScale = new List<float>{1f, 1f};
        for(int i = 0; i <= 1; i++){
            endTimeScale[i] = Distance3D(Gloves[i].transform.position - Enemy.transform.position) / slowdownValue;

            //Debug.Log("dist: " + dist);
            //Debug.Log("slow: " + dist/slowdownValue);
            if(!bodyGuard[i]){
            restingPos = new List<Vector3> {initialTransform[0] + new Vector3(glovesXFace, glovesYFace, 0), initialTransform[1] + new Vector3(-glovesXFace, glovesYFace, 0)};
            }
            else{
                restingPos = initialTransform;
            }

            if(Distance3D(Gloves[i].transform.localPosition - restingPos[i]) >= maxPunchDist){
                rigidbodies[i] = 0;
            }
            if(rigidbodies[i] > 0){
                rigidbodies[i] -= Time.deltaTime;
            }
            if(rigidbodies[i] <= 0 && RBs[i].isKinematic == false){
                RBs[i].isKinematic = true;
                trails[i].emitting = false;
            }

            if(RBs[i].isKinematic){
                Gloves[i].transform.localPosition = moveTowards(Gloves[i].transform.localPosition, restingPos[i], returnSpeed * Distance3D(Gloves[i].transform.localPosition - restingPos[i]));
                Colliders[i].enabled = false;
            }
        }

        if(endTimeScale[0] <= endTimeScale[1]){
            Time.timeScale = endTimeScale[0];
        }
        else{
            Time.timeScale = endTimeScale[1];
        }
        Time.timeScale = Mathf.Clamp(Time.timeScale, minSlowdown, maxSlowdown);
        //Debug.Log(Time.timeScale);
    }

    float Distance3D(Vector3 loc){
        float dist = Mathf.Sqrt(loc.x * loc.x + loc.y * loc.y + loc.z * loc.z);
        return dist;
    }

    public void LeftSwitchGuard(InputAction.CallbackContext context){
        if(context.performed){
            bodyGuard[0] = false;
            Debug.Log("guard face");
        }
        else if(context.canceled){
            bodyGuard[0] = true;
            Debug.Log("guard body");
        }
    }
    public void RightSwitchGuard(InputAction.CallbackContext context){
        if(context.performed){
            bodyGuard[1] = false;
            //Debug.Log("guard face");
        }
        else if(context.canceled){
            bodyGuard[1] = true;
            //Debug.Log("guard body");
        }
    }

    Vector3 moveTowards(Vector3 location, Vector3 target, float speed){
        Vector3 vector = Vector3.MoveTowards(location, target, speed * Time.deltaTime);
        return vector;
    }
}
