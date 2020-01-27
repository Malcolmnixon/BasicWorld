using System.Collections;
using System.Collections.Generic;
using BasicWorld.Math;
using BasicWorld.WorldData;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerBody;    
    private float WalkSpeed = 40f;
    private float TurnSpeed = 180f;

    private float _rotate;
    private float _walk;

    public Vec2 Position;
    public Vec2 Velocity;

    // Start is called before the first frame update
    void Start()
    {
        _playerBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Calculate rotate and walk based on controls
        float rotate;
        float walk;
        if (Input.GetMouseButton (0)) {

            var halfWidth = (Screen.width / 2);
            var halfHeight = (Screen.height / 2);

            rotate = (Input.mousePosition.x - halfWidth) / halfWidth;
            walk = (Input.mousePosition.y - halfHeight) / halfHeight;
        } else {
            rotate = Input.GetAxis("Horizontal");
            walk = Input.GetAxis("Vertical");
        }

        // Save scaled by configured speeds
        _rotate = rotate * TurnSpeed;
        _walk = walk * WalkSpeed; 
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Move player
        transform.Rotate(0, _rotate * Time.fixedDeltaTime, 0);
        transform.Translate(new Vector3 (0, 0, _walk * Time.fixedDeltaTime));

        // Update position
        Position = new Vec2(transform.position.x, transform.position.z);

        // Update velocity
        var vel = transform.TransformVector(0, 0, _walk);
        Velocity = new Vec2(vel.x, vel.z);
    }
}
