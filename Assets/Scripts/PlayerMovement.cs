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
    public Vector2 leanTarget;
    Vector2 leanAmount;
    private PlayerInput inputs;
    private Punching punching;

    private void Awake()
    {
        inputs = new PlayerInput();
    }
    private void OnEnable(){
        inputs.Enable();
    }
    private void OnDisable()
    {
        inputs.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        charControl = GetComponent<CharacterController>();
        punching = this.GetComponent<Punching>();
    }

    // Update is called once per frame
    void Update()
    {
        //looking
        lookPlace.transform.position = Vector3.Lerp(lookPlace.transform.position, Enemy.transform.position, lookSmoothing);
        this.transform.LookAt(lookPlace.transform.position);

        cam.transform.LookAt(lookPlace.transform.position);
        
        move(inputs.Player.Movement.ReadValue<Vector2>());
        dodge(inputs.Player.Dodge.ReadValue<Vector2>());
        punching.punch(new List<Vector2>{inputs.Player.LeftFist.ReadValue<Vector2>(),inputs.Player.RightFist.ReadValue<Vector2>()});
    }

    public void move(Vector2 move){
        charControl.Move(this.transform.forward * move.y * Time.deltaTime * playerSpeed + this.transform.right * move.x * Time.deltaTime * playerSpeed); // for character controller (better so far)
        lookPlace.transform.position += this.transform.forward * move.y * Time.deltaTime * playerSpeed + this.transform.right * move.x * Time.deltaTime * playerSpeed;
    }

    public void dodge(Vector2 lean){
        leanAmount = Vector2.Lerp(leanAmount, leanTarget * -lean, leanSpeed);

        this.transform.Rotate(new Vector3(-leanAmount.y, 0, leanAmount.x));
    }

    float Distance3D(Vector3 loc){
        float dist = Mathf.Sqrt(loc.x * loc.x + loc.y * loc.y + loc.z * loc.z);
        return dist;
    }
}
