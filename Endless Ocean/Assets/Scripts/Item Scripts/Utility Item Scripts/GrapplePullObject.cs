﻿using UnityEngine;
using System.Collections;

/// <summary>
/// This class moves an object towards the player at a gradually increasing speed. This speed is capped at maxSpeed - it is used move objects the player is pulling with the grapple.
/// </summary>
public class GrapplePullObject : MonoBehaviour {

    public float velocityIncrement;
    public GameObject objectToMoveTowards;
    public bool moveInYAxis = false;
    private Rigidbody playerRigidbody;

    /// <summary>
    /// Initalizes key varialbes for the class as mono behaviour classes cannot have constructors.
    /// </summary>
    public void init(GameObject objectToMoveTowards, float velocity, bool moveInYAxis, Rigidbody playerRigidbody)
    {
        this.velocityIncrement = velocity;
        this.objectToMoveTowards = objectToMoveTowards;
        this.moveInYAxis = moveInYAxis;
        this.playerRigidbody = playerRigidbody;
    }

	/// <summary>
    /// Runs once each frame and moves the object towards the player.
    /// </summary>
	void FixedUpdate () {
        if (Vector3.Distance(objectToMoveTowards.transform.position, this.transform.position) > 5) {
            Vector3 direction = (objectToMoveTowards.transform.position - this.transform.position);
            if (!moveInYAxis) {
                direction.y = 0;
            }
            Vector3 tempVelocity = this.GetComponent<Rigidbody>().velocity + Vector3.ClampMagnitude((direction * velocityIncrement), .4f);
            tempVelocity.x = Mathf.Clamp(tempVelocity.x, -(playerRigidbody.gameObject.GetComponent<PlayerController>().movementSpeed), playerRigidbody.gameObject.GetComponent<PlayerController>().movementSpeed);
            this.GetComponent<Rigidbody>().velocity = tempVelocity;
        }
	}
}
