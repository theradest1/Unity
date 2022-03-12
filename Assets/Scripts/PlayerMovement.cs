using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 leanAmount;
    PlayerInput inputs;
    Punching punching;
    Rigidbody rb; 
    public GameObject cam;
    public GameObject lookPlace;
    public GameObject Enemy;
    public GameObject EnemyHead;
    public GameObject punchTarget;

    public float playerSpeed;

    public float leanSpeed;
    public Vector2 leanTarget;

    public float lookSpeedHorizontal;
    public float lookSpeedVertical;
    public float lookSmoothing = .02f;

    private float playerY;
    public Vector2 limitX;
    public Vector2 limitZ;

    public Vector2 speedClamp;
    public float distSpeedMult;
    float distSpeed;

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
        punching = this.GetComponent<Punching>();
        playerY = this.transform.position.y;
        punchTarget = Enemy;
    }

    // Update is called once per frame
    void Update()
    {
        //looking
        lookPlace.transform.position = Vector3.Lerp(lookPlace.transform.position, punchTarget.transform.position, lookSmoothing * Time.deltaTime);
        this.transform.LookAt(lookPlace.transform.position);

        cam.transform.LookAt(lookPlace.transform.position);
        
        move(inputs.Player.Movement.ReadValue<Vector2>());
        dodge(inputs.Player.Dodge.ReadValue<Vector2>());
        punching.punch(new List<Vector2>{inputs.Player.LeftFist.ReadValue<Vector2>(),inputs.Player.RightFist.ReadValue<Vector2>()});
    }

    public void move(Vector2 move){
        distSpeed = Mathf.Clamp(Distance3D(this.transform.position - Enemy.transform.position) * distSpeedMult, speedClamp.x, speedClamp.y);
        Vector3 add = this.transform.position + this.transform.forward * move.y * Time.deltaTime * playerSpeed * distSpeed + this.transform.right * move.x * Time.deltaTime * playerSpeed * distSpeed;
        this.transform.position = new Vector3(add.x, playerY, add.z);
        this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, limitX.x, limitX.y), playerY, Mathf.Clamp(this.transform.position.z, limitZ.x, limitZ.y));
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

    public void switchTarget(InputAction.CallbackContext context){
        if(context.performed){
            punchTarget = EnemyHead;
        }   
        else if(context.canceled){
            punchTarget = Enemy;
        }
    }
}
