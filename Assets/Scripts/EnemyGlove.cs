using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGlove : MonoBehaviour
{
    public bool ableToPunch;
    public Vector3 initialPos;
    public Vector2 faceGuardPos;
    public bool faceGuard;
    public Rigidbody RB;
    public Collider coll;
    public float strength;
    public float chanceToHit;
    public float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = this.gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 restingPos = initialPos;
        if(faceGuard){
            restingPos += new Vector3(faceGuardPos.x, faceGuardPos.y, 0);
        }
        if(RB.isKinematic){
            this.transform.localPosition = moveTowards(this.transform.localPosition, restingPos, movementSpeed * Distance3D(this.transform.localPosition - restingPos));
            coll.enabled = false;
        }
    }

    public void SwitchGuard(){
        faceGuard = !faceGuard;
    }

    Vector3 moveTowards(Vector3 location, Vector3 target, float speed){
        Vector3 vector = Vector3.MoveTowards(location, target, speed * Time.deltaTime);
        return vector;
    }

    float Distance3D(Vector3 loc){
        float dist = Mathf.Sqrt(loc.x * loc.x + loc.y * loc.y + loc.z * loc.z);
        return dist;
    }
}
