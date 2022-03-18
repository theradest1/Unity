using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Punching : MonoBehaviour
{
    List<Glove> gloves;

    public GameObject Enemy;
    public GameObject cam;

    public float maxPunchDist;
    public float punchingSpeed;
    public float returnSpeed;
    public float rigidbodyOnTime;
    public bool punchOnlyWhenReturned;

    public float minSlowdown;
    public float maxSlowdown;
    public float slowdownValue;

    //PlayerIndex playerIndex;
    //GamePadState state;

    void Start()
    {
        gloves = new List<Glove>{GameObject.Find("LeftGlove").GetComponent<Glove>(), GameObject.Find("RightGlove").GetComponent<Glove>()};
    }

    public void punch(List <Vector2> vectors){
        int i = 0;
        foreach(Glove glove in gloves){
            if(Distance3D(glove.transform.localPosition - glove.initialTransform) <= maxPunchDist && vectors[i] != Vector2.zero && glove.ableToPunch){
                glove.RB.AddRelativeForce(new Vector3(vectors[i].x, 0, vectors[i].y) * punchingSpeed * Time.deltaTime);
                glove.RB.isKinematic = false;
                glove.trail.emitting = true;
                glove.RBOn = rigidbodyOnTime;
                glove.coll.enabled = true;
            }
            if(vectors[i] == Vector2.zero){
                glove.ableToPunch = false;
            }
            i++;
        }
    }
    void Update()
    {
        //playerIndex = (PlayerIndex)0;
        //state = GamePad.GetState(playerIndex);
        //Debug.Log(state.Triggers.Left);
        //GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);

        List<float> endTimeScale = new List<float>{1f, 1f};
        int i = 0;
        foreach(Glove glove in gloves){
            endTimeScale[i] = Mathf.Clamp(Distance3D(glove.transform.position - Enemy.transform.position) / slowdownValue, minSlowdown, maxSlowdown);

            if(Distance3D(glove.transform.localPosition - glove.restingPos) >= maxPunchDist){
                glove.RBOn = 0f;
            }
            if(glove.RBOn > 0f){
                glove.RBOn -= Time.deltaTime;
            }
            else if(glove.RBOn <= 0 && glove.RB.isKinematic == false){
                glove.RB.isKinematic = true;
                glove.trail.emitting = false;
            }

            if(glove.RB.isKinematic){
                glove.transform.localPosition = moveTowards(glove.transform.localPosition, glove.restingPos, returnSpeed * Distance3D(glove.transform.localPosition - glove.restingPos));
                glove.coll.enabled = false;
            }
            if(Distance3D(glove.transform.localPosition - glove.restingPos) <= .1f){
                glove.ableToPunch = true;
                glove.coll.enabled = true;
            }
            i++;
        }

        //if(endTimeScale[0] <= endTimeScale[1]){
        //    Time.timeScale = endTimeScale[0];
        //}
        //else{
        //    Time.timeScale = endTimeScale[1];
        //}
        //Debug.Log(Time.timeScale);
    }

    float Distance3D(Vector3 loc){
        float dist = Mathf.Sqrt(loc.x * loc.x + loc.y * loc.y + loc.z * loc.z);
        return dist;
    }

    public void LeftSwitchGuard(InputAction.CallbackContext context){
        if(context.performed){
            gloves[0].restingPos = gloves[0].initialTransform + new Vector3(gloves[0].gloveXFace, gloves[0].gloveYFace, 0f);
        }
        else if(context.canceled){
            gloves[0].restingPos = gloves[0].initialTransform;
        }
    }
    public void RightSwitchGuard(InputAction.CallbackContext context){
        if(context.performed){
            gloves[1].restingPos = gloves[1].initialTransform + new Vector3(-gloves[1].gloveXFace, gloves[1].gloveYFace, 0f);
        }
        else if(context.canceled){
            gloves[1].restingPos = gloves[1].initialTransform;
        }
    }

    Vector3 moveTowards(Vector3 location, Vector3 target, float speed){
        Vector3 vector = Vector3.MoveTowards(location, target, speed * Time.deltaTime);
        return vector;
    }
}
