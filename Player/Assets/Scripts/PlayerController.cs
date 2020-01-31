using System.Collections;
using System.Collections.Generic;
using BasicWorld.Math;
using BasicWorld.WorldData;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerBody;    
    private float WalkSpeed = 20f;
    private float TurnSpeed = 180f;

    private float _rotate;
    private float _walk;

    private Vector3 _lastPosition;

    public Vec2 Position;
    public Vec2 Velocity;
    public float Rotation;

    // Start is called before the first frame update
    void Start()
    {
        _playerBody = GetComponent<Rigidbody>();
        _lastPosition = transform.position;
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
        // Perform position and velocity update
        var pos = transform.position;
        var vel = (pos - _lastPosition) / Time.fixedDeltaTime;
        _lastPosition = pos;

        // Update visible state
        Position = new Vec2(pos.x, pos.z);
        Velocity = new Vec2(vel.x, vel.z);
        Rotation = transform.rotation.eulerAngles.y;

        // Calculate requested movement
        var rot = transform.rotation * Quaternion.Euler(0, _rotate * Time.fixedDeltaTime, 0);
        var mov = transform.TransformVector(new Vector3(0, 0, _walk * Time.fixedDeltaTime));

        // Request rotate and move of player
        _playerBody.MoveRotation(rot);
        _playerBody.MovePosition(transform.position + mov);
    }
}
