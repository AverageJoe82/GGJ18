﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    private Manager m;
    private CharacterComponent c;
    public bool active;

    //values used to keep track of watching
    public float watchSpeed;
    private int watching;
    private SpriteRenderer sprite;
    private Vector4 colors;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        m = GameObject.FindWithTag("Manager").GetComponent<Manager>();
        c = gameObject.GetComponent<CharacterComponent>();

        //initialize values related to watching
        watching = 0;
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>(); //bad practice, should try to assign this in the editor but it's a game jam so im not rdoing this
        colors = sprite.color;
        sprite.color = new Vector4(colors.x, colors.y, colors.z, 0);
        sprite.enabled = false;
	}
	
	// FixedUpdate is called a fixed number of times per frame
	void FixedUpdate () {
        if (active)
        {
            //movement
            float moveV = Input.GetAxis("Vertical");// * Time.deltaTime * 75.0f;
            float moveH = Input.GetAxis("Horizontal");// * Time.deltaTime * 75.0f;
            Vector3 movement = new Vector3(moveH, rb.velocity.y/c.speed, moveV);
            rb.velocity = movement * c.speed;
            //swap on space
            if(Input.GetKeyDown("space"))
            {
                m.EntityChangeA(gameObject);
                rb.velocity = Vector3.zero;
            }
            //handle being watched
            if (watching != 0)
            {
                colors = sprite.color;
                sprite.color = new Vector4(colors.x, colors.y, colors.z, colors.w + watchSpeed / 65);
                if (colors.w >= 1)
                {
                    Destroy(gameObject); //game over
                }
            }
            else if (colors.w > 0) 
            {
                colors = sprite.color;
                sprite.color = new Vector4(colors.x, colors.y, colors.z, colors.w - watchSpeed / 65);
                sprite.enabled = false;
            }
        }
    }
    
    //keep a count of the number of things watching the player
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Watch")
        {
            watching++;
            sprite.enabled = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Watch")
            watching--;
    }

    public void SetState(bool boo)
    {
        active = boo;
    }
}


