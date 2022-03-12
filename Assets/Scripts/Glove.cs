using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glove : MonoBehaviour
{
    
    public float rigidbodyOn;
    public bool bodyGuard = true;
    public GameObject gloveObject;
    public TrailRenderer trail;
    public Collider coll;
    public Rigidbody RB;
    public Vector3 initialTransform;
    public Vector3 restingPos;
    public float RBOn;
    public float gloveXFace;
    public float gloveYFace;
    public bool ableToPunch;

    void Start()
    {
        gloveObject = this.gameObject;
        initialTransform = gloveObject.transform.localPosition;
        restingPos = initialTransform;
    }

    void Update()
    {
        
    }
}
