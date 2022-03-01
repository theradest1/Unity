using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Punching : MonoBehaviour
{

    List<Glove> gloves;
    public float punchingSpeed;
    public float returnSpeed;
    public GameObject Enemy;
    public float maxPunchDist;
    public float rigidbodyOnTime;
    public GameObject cam;
    public bool punchOnlyWhenReturned;
    public float minSlowdown;
    public float maxSlowdown;
    public float slowdownValue;


    //guarding
    public float gloveYFace;
    public float glovesXFace; //for making them go close together to look cool

    void Start()
    {
        gloves = new List<Glove>{GameObject.Find("LeftGlove").GetComponent<Glove>(), GameObject.Find("RightGlove").GetComponent<Glove>()};
    }

    public void punch(List <Vector2> vectors){
        int i = 0;
        foreach(Glove glove in gloves){
            if(Distance3D(glove.transform.localPosition - glove.initialTransform) <= maxPunchDist && vectors[i] != Vector2.zero){
                glove.RB.AddRelativeForce(new Vector3(vectors[i].x, 0, vectors[i].y) * punchingSpeed * Time.deltaTime);
                glove.RB.isKinematic = false;
                glove.trail.emitting = true;
                glove.RBOn = rigidbodyOnTime;
                glove.GetComponent<Collider>().enabled = true;
            }
            i++;
        }
    }
    void Update()
    {
        List<float> endTimeScale = new List<float>{1f, 1f};
        int i = 0;
        foreach(Glove glove in gloves){
            endTimeScale[i] = Mathf.Clamp(Distance3D(glove.transform.position - Enemy.transform.position) / slowdownValue, minSlowdown, maxSlowdown);
            if(!glove.bodyGuard){
                glove.restingPos = glove.initialTransform + new Vector3(gloveYFace, glove.gloveXFace);
            }
            else{
                glove.restingPos = glove.initialTransform;
            }

            if(Distance3D(glove.transform.localPosition - glove.restingPos) >= maxPunchDist){
                glove.RBOn = 0f;
            }
            if(glove.RBOn > 0f){
                glove.RBOn -= Time.deltaTime;
            }
            if(glove.RBOn <= 0 && glove.RB.isKinematic == false){
                glove.RB.isKinematic = true;
                glove.trail.emitting = false;
            }

            if(glove.RB.isKinematic){
                glove.transform.localPosition = moveTowards(glove.transform.localPosition, glove.restingPos, returnSpeed * Distance3D(glove.transform.localPosition - glove.restingPos));
                glove.GetComponent<Collider>().enabled = false;
            }
            i++;
        }

        if(endTimeScale[0] <= endTimeScale[1]){
            Time.timeScale = endTimeScale[0];
        }
        else{
            Time.timeScale = endTimeScale[1];
        }
        //Debug.Log(Time.timeScale);
    }

    float Distance3D(Vector3 loc){
        float dist = Mathf.Sqrt(loc.x * loc.x + loc.y * loc.y + loc.z * loc.z);
        return dist;
    }

    /*public void LeftSwitchGuard(InputAction.CallbackContext context){
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
    }*/

    Vector3 moveTowards(Vector3 location, Vector3 target, float speed){
        Vector3 vector = Vector3.MoveTowards(location, target, speed * Time.deltaTime);
        return vector;
    }
}
