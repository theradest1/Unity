using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public GameObject Player;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.LookAt(new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z));
        this.gameObject.transform.position += this.gameObject.transform.forward * speed * Time.deltaTime;
    }
}
