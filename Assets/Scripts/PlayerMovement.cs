using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float playerSpeed;
    Rigidbody rb; 
    CharacterController charControl;
    [SerializeField]
    float lookSpeedHorizontal;
    [SerializeField]
    float lookSpeedVertical;
    public GameObject cam;
    public GameObject lookPlace;
    public GameObject Enemy;
    public float lookSmoothing = .02f;
    public float leanSpeed;
    public float leanTarget;
    float leanAmount;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        charControl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //looking
        lookPlace.transform.position = Vector3.Lerp(lookPlace.transform.position, Enemy.transform.position, lookSmoothing);
        this.transform.LookAt(lookPlace.transform.position);

        cam.transform.LookAt(lookPlace.transform.position);

    }

    public void Jump(InputAction.CallbackContext context){
        Debug.Log("jumped" + context.phase);
    }

    public void move(InputAction.CallbackContext context){
        if(context.performed){
            Debug.Log(context);
            charControl.Move(this.transform.forward * context.ReadValue<Vector2>().y * Time.deltaTime * playerSpeed + this.transform.right * context.ReadValue<Vector2>().x * Time.deltaTime * playerSpeed); // for character controller (better so far)
            //lookPlace.transform.position += this.transform.forward * moveVector.z * Time.deltaTime * playerSpeed + this.transform.right * moveVector.x * Time.deltaTime * playerSpeed;

        
            //leanAmount = Mathf.Lerp(leanAmount, leanTarget * -lean, leanSpeed);
            //this.transform.Rotate(new Vector3(0, 0, leanAmount));
        }
        
    }

    float Distance3D(Vector3 loc){
        float dist = Mathf.Sqrt(loc.x * loc.x + loc.y * loc.y + loc.z * loc.z);
        return dist;
    }
}
