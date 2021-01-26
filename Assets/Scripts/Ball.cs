using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool aLive = true;
    private float upForce = 200f;
    private Rigidbody2D rb2d;
    private Vector2 upVector;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        upVector = new Vector2(0f, upForce);
    }

    // Update is called once per frame
    void Update()
    {
        if (aLive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                rb2d.velocity=Vector2.zero;
                rb2d.AddForce(upVector);
            }
        }
    }

    private void OnCollisionEnter2D()
    {
        aLive = false;
    }
}
