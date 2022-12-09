using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float moveSpeed;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Move();
    }
    //third person camera controller


    private void Move()
    {
        var z = Input.GetAxis("Vertical");
        var x = Input.GetAxis("Horizontal");
        rb.MovePosition(transform.position + (transform.forward * z+ transform.right * x) * Time.deltaTime * moveSpeed);
    }
}
