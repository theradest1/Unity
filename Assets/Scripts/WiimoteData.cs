using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

//https://www.reddit.com/r/Unity3D/comments/1syswe/ps4_controller_map_for_unity/

public class WiimoteData : MonoBehaviour
{

    private Wiimote wiimote;

    List<bool> PastAOn = new List<bool> {false, false};
    List<Vector3> steps = new List<Vector3> {new Vector3(0, 0, 0), new Vector3(0, 0, 0)};
    List<bool> rumble = new List<bool> {false, false};
    List<GameObject> model;
    private List<Quaternion> initialRot;
    private List<Quaternion> pastRot = new List<Quaternion> {Quaternion.identity, Quaternion.identity};

    public float smoothing = .99f;
    public float punchDist = 1f;
    public GameObject GloveManager;
    public GameObject Player;
    public int calibrationMode = 1;

    public bool pcInput = false;
    public bool ps4Input = true;

    void Start() {
        model = new List<GameObject> {GameObject.Find("modelLeft"), GameObject.Find("modelRight")};
        initialRot = new List<Quaternion> {model[0].transform.rotation, model[1].transform.rotation};
        WiimoteManager.FindWiimotes();
        Debug.Log(WiimoteManager.Wiimotes.Count + " Remotes connected");
        if(WiimoteManager.Wiimotes.Count > 0){
            WiimoteManager.Wiimotes[0].SendPlayerLED(true, false, false, false);
            WiimoteManager.Wiimotes[1].SendPlayerLED(false, false, false, true);
        }
        else{
            if(pcInput){
                Debug.Log("Wii remotes not being used (keyboard and mouse)");
            }
            else{
                Debug.Log("Not enough wii remotes connected, set pcInput to true to be able to use keyboard and mouse");
            }
        }
    }

    void Update()
    {   
        //Keyboard/mouse returns
        
        /*
        if(pcInput){
            Player.GetComponent<PlayerMovement>().move(new Vector3(BoolToInt(Input.GetKey("d")) - BoolToInt(Input.GetKey("a")), 0, BoolToInt(Input.GetKey("w")) - BoolToInt(Input.GetKey("s"))).normalized, BoolToInt(Input.GetKey("right")) - BoolToInt(Input.GetKey("left")));
            
            if(Input.GetKey("q") || Input.GetKey("e")){
                GloveManager.GetComponent<Punching>().punch(BoolToInt(Input.GetKey("e")));
            }
            if(Input.GetKey("up") || Input.GetKey("down")){
                GloveManager.GetComponent<Punching>().switchGuard(Input.GetKey("up"));
            }
        }

        if(ps4Input){
            Player.GetComponent<PlayerMovement>().move(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized, 0); //BoolToInt(Input.GetKey("leanLeft")) - BoolToInt(Input.GetKey("leanRight")));
        }*/
    }

    float Distance3D(Vector3 loc){
        float dist = Mathf.Sqrt(loc.x * loc.x + loc.y * loc.y + loc.z * loc.z);
        return dist;
    }

    int BoolToInt(bool Bool){
        if(Bool){
            return 1;
        }
        return 0;
    }

    void OnApplicationQuit() {
		if (wiimote != null) {
			WiimoteManager.Cleanup(wiimote);
	        wiimote = null;
		}
	}

    private Vector3 GetAccelVector(Wiimote wiimote)
    {
        float accel_x;
        float accel_y;
        float accel_z;

        float[] accel = wiimote.Accel.GetCalibratedAccelData();
        accel_x = accel[0];
        accel_y = -accel[2];
        accel_z = -accel[1];

        return new Vector3(accel_x, accel_y, accel_z).normalized;
    }
}
