using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerBody;    
    private float WalkSpeed = 40f;
    private float TurnSpeed = 180f;

    private float rotate;
    private float z;

    // Start is called before the first frame update
    void Start()
    {
        _playerBody = GetComponent<Rigidbody>();
    }

    void Update() {
 
        if (Input.GetMouseButton (0)) {

            var halfWidth = (Screen.width / 2);
            var halfHeight = (Screen.height / 2);

            var directionX = Input.mousePosition.x < halfWidth ? -1 : 1;
            var directionY = Input.mousePosition.y < halfHeight ? -1 : 1;

            rotate = (Input.mousePosition.x - halfWidth) / halfWidth;
            z = (Input.mousePosition.y - halfHeight) / halfHeight;
        } else {
            rotate = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }
        
        rotate = rotate * TurnSpeed * Time.deltaTime;
        z = z * WalkSpeed * Time.deltaTime; 
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Rotate(0, rotate, 0);
        transform.Translate(new Vector3 (0, 0, z));
    }

}
