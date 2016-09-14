﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public abstract class NPCBehaviour : CharacterSuper
{
    public String[] itemDrops = {"Prefabs/Consumables/TestPotion"};

    public int[] itemPossibilites = {100};

    private int maxAmountOfTreasure;
    private int maxAmountOfExp;
    private int maxAmountOfAmmo;

    //float healthbar above enemy
    public Image healthBar;
    protected bool tracking = false;

    // Use this for initialization
    new void Start () {
        // healthBar = transform.FindChild("EnemyCanvas").FindChild("HealthBG").FindChild("HealthFG").GetComponent<Image>();
        base.Start();
        tracking = false;
    }
	
	// Update is called once per frame
	protected void FixedUpdate() {
	    
	}

    protected void pathToLocation(Vector3 destination)
    {
        float direction = 0;
        if (rigidbody.position.x > destination.x)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        moveCharacter(direction);
        tracking = false;
    }

    protected void attackTarget(Transform target)
    {
        if (weapon != null)
        {
            if (nextMelee < Time.time)
            {
                nextMelee = Time.time + weapon.getAttackSpeed();
                this.animator.SetTrigger("MeleeAttackTrigger");
                weapon.attack(attack, target.position);
                moveCharacter(0);
            }
        }
    }

    /// <summary>
    /// Kills the NPC and makes them drop exp, treasure and equipment.
    /// </summary>
    public override void die()
    {
        System.Random random = new System.Random();
        ExpSphereSpawner.spawnExpOrbs(random.Next(0, maxHealth/2), this.transform);
        TreasureSpawner.spawnTreasure(random.Next(0, maxHealth/2), this.transform);
        AmmoSpawner.spawnAmmo(random.Next(0, maxHealth/10), this.transform);
        ItemSpawner.spawnSpecificItems(itemDrops, itemPossibilites, this.transform);
        Destroy(this.gameObject);
    }
}
