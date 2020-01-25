using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerBody;    
    public float WalkSpeed = 5f;

    private float x;
    private float z;

    // Start is called before the first frame update
    void Start()
    {
        _playerBody = GetComponent<Rigidbody>();
    }

    void Update() {
        
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        _playerBody.MovePosition(
            transform.position + transform.TransformVector(
                WalkSpeed * x * Time.deltaTime,
                0,
                WalkSpeed * z * Time.deltaTime));
    }
}
