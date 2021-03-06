﻿using UnityEngine;
using System.Collections;

/// <summary>
/// This class contains fucntions for managing spawning items when NPC's die.
/// </summary>
public class OnDeathSpawner : MonoBehaviour
{

    public delegate void SpawnerCoroutineCallback();

    SpawnerCoroutineCallback callbackCountIncrementerHandler;

    public int callbacksReturned = 0;

    /// <summary>
    /// A callback used to check how many callbacks have returned.
    /// </summary>
    public void callbackCountIncrementer()
    {
        this.callbacksReturned++;
    }


    /// <summary>
    /// Deletes the gameobject once all the callbacks have returned.
    /// </summary>
    void FixedUpdate()
    {
        if(callbacksReturned > 2)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Starts the item dropping co routines that spawn pickups from dying NPC's.
    /// </summary>
    /// <param name="maxHealth">The max health of the NPC that died.</param>
    /// <param name="itemPossibilities">The chances that items might drop.</param>
    /// <param name="itemDrops">The possible items and NPC might drop.</param>
    public void startItemSpawningCoroutines(int maxHealth, int[] itemPossibilities, string[] itemDrops)
    {
        callbackCountIncrementerHandler = callbackCountIncrementer;
        System.Random random = new System.Random();
        StartCoroutine(ExpSphereSpawner.spawnExpOrbsCoroutine(random.Next(0, maxHealth / 2), this.transform, callbackCountIncrementerHandler));
        StartCoroutine(TreasureSpawner.spawnTreasureCoroutine(random.Next(0, maxHealth / 2), this.transform, callbackCountIncrementerHandler));
        StartCoroutine(AmmoSpawner.spawnAmmoCoroutine(random.Next(0, maxHealth / 10), this.transform, callbackCountIncrementerHandler));
        ItemSpawner.spawnSpecificItems(itemDrops, itemPossibilities, this.transform);
    }

}
