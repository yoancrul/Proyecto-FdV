using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPersonaje : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            rigidBody.velocity = Vector2.left * moveSpeed;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rigidBody.velocity = Vector2.right * moveSpeed;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.velocity = Vector2.up * moveSpeed;
        }
    }
}
