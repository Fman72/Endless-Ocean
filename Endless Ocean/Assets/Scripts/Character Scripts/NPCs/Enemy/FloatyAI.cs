﻿using UnityEngine;
using System.Collections;

public class FloatyAI : EnemyAI
{

    private bool attacking = false;
    private bool bouncing = false;

    private Quaternion groundCheckLocationRotation;
    private float targetHeightFromPlayer = 9f;
    private float verticalMoveSpeedBoost = 2f;

    public int knockBack;
    public int stun;

    private float bounceForce = 300f;




    // Use this for initialization
    new void Start()
    {
        base.Start();
        this.longestRange = 20;
        this.groundCheckLocationRotation = this.groundCheck.rotation;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    new void FixedUpdate()
    {
        //check if player is in range
        if (Vector3.Distance(transform.position, target.position) <= detectRange)
        {
            this.facePlayer(this.target);
            this.makeActionDecision();
        }
    }

    void LateUpdate()
    {
        this.groundCheck.transform.rotation = this.groundCheckLocationRotation;
        //transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);
    }


    //This is the code which manages how the enemy attacks - overrides from NPCBehaviour
    protected override void attackTarget(Transform target)
    {
        //shoot at target, not their feet
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + 1.5f, target.position.z);





        //shoot at target, not their feet
        //Vector3 targetPosition = new Vector3(target.position.x, target.position.y + 1.5f, target.position.z);
        //moveCharacter(0);

        //if (weapon != null)
        //{
        //    if (nextMelee < Time.time)
        //    {
        //        nextMelee = Time.time + weapon.getAttackSpeed();
        //        this.animator.SetTrigger("MeleeAttackTrigger");
        //        weapon.attack(attack, targetPosition);
        //    }
        //}
    }


    //This is where the enemy decides what to do - called from the fixedUpdate - overrides from enemyAI
    protected override void makeActionDecision()
    {
        //Decide on an action
        Debug.Log("Bouncing is " + this.bouncing);
        if (!attacking && !bouncing)
        {
            if (Vector3.Distance(transform.position, target.position) < longestRange && (Mathf.Abs(this.transform.position.y - (this.targetHeightFromPlayer + target.position.y)) < 0.3))
            {
                StartCoroutine(this.attackCoroutine(target.position));
            }
            else
            {
                this.pathToLocation(target.position);
            }
        }
    }

    /// <summary>
    /// Makes the floaty NPC look at the target.
    /// </summary>
    /// <param name="target">The target of the enemy. The player.</param>
    private void facePlayer(Transform target)
    {
        this.transform.LookAt(target);
    }

    #region Movement Functions

    protected void moveCharacter(float horizontalDirection, float verticalDirection)
    {
        //Stun Timer
        if (recoveryTimer != 0)
        {
            horizontalDirection = 0;
        }
        else
        {
            rigidbody.velocity = new Vector3(horizontalDirection * movementSpeed, (verticalDirection * movementSpeed), 0);
        }
    }

    //This is the method used for moving the enemy from current location to a set destination - overrides from NPCBehaviour
    protected override void pathToLocation(Vector3 destination)
    {
        float horizontalDirection = 0;
        float verticalDirection = 0;
        if (this.gameObject.transform.position.x > destination.x)
        {
            horizontalDirection = -1;
        }
        else
        {
            horizontalDirection = 1;
        }

        if (this.gameObject.transform.position.y > (destination.y + this.targetHeightFromPlayer))
        {
            verticalDirection = -1;
        }
        else if (this.gameObject.transform.position.y < (destination.y + this.targetHeightFromPlayer))
        {
            verticalDirection = 1;
        }

        this.moveCharacter(horizontalDirection, verticalDirection);
    }

    #endregion

    /// <summary>
    /// A coroutine that makes the floaty fly at the player in an attack.
    /// </summary>
    /// <param name="target">The transform of the target to attack.</param>
    /// <returns></returns>
    private IEnumerator attackCoroutine(Vector3 target)
    {
        this.attacking = true;
        float attackStartTime = Time.time;
        Vector3 targetPosition;
        if(this.transform.position.x < target.x)
        {
            targetPosition = new Vector3(target.x + 2, target.y, 0);
        }
        else
        {
            targetPosition = new Vector3(target.x - 2, target.y, 0);
        }
        Vector3 newVelocity = Vector3.zero;
        while ((attackStartTime + 2f > Time.time) && attacking)
        {
            Vector3.SmoothDamp(this.transform.position, targetPosition, ref newVelocity, 1f);
            this.GetComponent<Rigidbody>().velocity = newVelocity;
            yield return null;
        }
        this.attacking = false;
    }

    /// <summary>
    /// Makes the floaty boucne away from the player and overrides its normal movement.
    /// </summary>
    /// <param name="other">The object the floaty is bouncing away from.</param>
    /// <returns></returns>
    private IEnumerator bounceCoroutine(Transform other)
    {
        this.bouncing = true;
        Debug.Log("Boucning");
        float bounceStartTime = Time.time;
        Vector3 direction = this.gameObject.transform.position - other.position;
        //this.GetComponent<Rigidbody>().AddForce(direction.normalized * bounceForce);
        while(bounceStartTime + 1f > Time.time)
        {
            Vector3 velocity = Vector3.zero;
            Vector3.SmoothDamp(this.transform.position, direction.normalized * 50, ref velocity, 1f);
            this.GetComponent<Rigidbody>().velocity = velocity;
            yield return null;
        }
        this.bouncing = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.attacking = false;
            StartCoroutine(this.bounceCoroutine(other.transform));
            base.OnTriggerEnter(other);

        }

    }
}