﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/// <summary>
/// This is the super class for all items it contains functionality that is shared between all types of items such as flying out of chests and showing tooltips.
/// 
/// IMPORTANT: When overriding the Start and Update methods of this class. Be sure to call the base vesion as well so that the base functionality is not lost.
/// </summary>
[System.Serializable()]
public class Item : MonoBehaviour
{ 

    bool flyingOutOfChest = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    public GameObject tooltip;

    public string itemNameWithoutQuality;

    /// <summary>
    /// Property that constructs an item's name based off it's stats.
    /// </summary>
    public virtual string itemName
    {
        get
        {
            if (quality == ItemQuality.NULL && itemNameWithoutQuality == "")
            {
                return null;
            }
            else if (quality == ItemQuality.NULL)
            {
                return itemNameWithoutQuality;
            }
            return (this.quality + " " + this.itemNameWithoutQuality);
        }
    }

    public string description;

    //main icon
    public Sprite itemIcon;

    public Sprite itemIconCrude;
    public Sprite itemIconBasic;
    public Sprite itemIconImproved;
    public Sprite itemIconLegendary;
    public Sprite itemIconGodly;

    public enum ItemQuality
    {
        NULL = 0,
        Crude = 30,
        Basic = 70,
        Improved = 90,
        Legendary = 100,
        Godly = 1000 //for developer usage
    }

    public ItemQuality quality = ItemQuality.NULL;

    //Used to stack consumables.
    [HideInInspector]
    public int itemCount = 1;

    /// <summary>
    /// Dummy sell value of base class. Will be overriden.
    /// </summary>
    public virtual int sellValue
    {
        get { return 3; }
    }
    /// <summary>
    /// Dummy buy value of base class. Will be overriden.
    /// </summary>
    public virtual int buyValue
    {
        get { return 5; }
    }
    /// <summary>
    /// Default item are not stackable.
    /// </summary>
    public virtual bool stackable
    {
        get { return false; }
    }
    /// <summary>
    /// Default items cannot be reforegd.
    /// </summary>
    public virtual bool reforgable
    {
        get { return false; }
    }
    /// <summary>
    /// Default items cannot be equipped in the on mouseclick slots.
    /// </summary>
    public virtual bool mouseClickEquipable
    {
        get { return false; }
    }

    public Sprite getQualityIcon()
    {
        switch (quality)
        {
            case ItemQuality.NULL:
                return itemIcon;
            case ItemQuality.Crude:
                return itemIconCrude;
            case ItemQuality.Basic:
                return itemIconBasic;
            case ItemQuality.Improved:
                return itemIconImproved;
            case ItemQuality.Legendary:
                return itemIconLegendary;
            case ItemQuality.Godly:
                return itemIconGodly;
            default:
                return itemIcon;
        }
    }

    public Color getQualityColour()
    {
        switch (quality)
        {
            case ItemQuality.NULL:
                return Color.black;
            case ItemQuality.Crude:
                return Color.red;
            case ItemQuality.Basic:
                return Color.white;
            case ItemQuality.Improved:
                return Color.green;
            case ItemQuality.Legendary:
                return Color.blue;
            case ItemQuality.Godly:
                return Color.yellow;
            default:
                return Color.grey;
        }
    }

    /// <summary>
    /// Intializes key variables.
    /// </summary>
    protected virtual void Start()
    {
        if (tooltip == null)
        {
            this.tooltip = Instantiate(Resources.Load("Prefabs/UI/ItemTooltip"), new Vector3(), Quaternion.identity) as GameObject;
            this.tooltip.transform.GetChild(0).GetComponent<Text>().text = this.itemName;
            this.tooltip.transform.GetChild(0).GetComponent<Text>().color = getQualityColour();
            this.itemIcon = getQualityIcon();

            this.tooltip.transform.parent = this.gameObject.transform;
            this.tooltip.SetActive(false);
            //this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }
    }

    /// <summary>
    /// Runs each frame meakes the item fly out of a chest if it is opened.
    /// </summary>
    protected virtual void Update()
    {
        if (flyingOutOfChest)
        {
            this.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints.FreezeRotation);
            if (this.transform.position.y >= targetPosition.y)
            {
                flyingOutOfChest = false;
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                this.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ);

            }
            this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, .4f);
            Debug.Log(Time.deltaTime);
        }
    }

    /// <summary>
    /// Runs after each frame. Updates the positions of the tooltip.
    /// </summary>
    protected void LateUpdate()
    {
        this.repositionTooltip();
    }

    /// <summary>
    /// Returns a bool indicating whether or not the Item can be placed in a quick item slot.
    /// </summary>
    public virtual bool quickItemEquipable
    {
        get { return false; }
    }

    /// <summary>
    /// Starts an item flying out of a chest.
    /// </summary>
    /// <param name="startPosition">The position the item starts at.</param>
    /// <param name="targetPosition">The position the item has to fly to.</param>
    public void startFlyingOutOfChest(Vector3 startPosition, Vector3 targetPosition)
    {
        this.startPosition = startPosition;
        this.targetPosition = targetPosition;
        this.flyingOutOfChest = true;
    }

    /// <summary>
    /// Reposition the items tooltip to be above the item.
    /// </summary>
    public void repositionTooltip()
    {
        if (this.tooltip.activeSelf)
        {
            this.tooltip.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z);
            this.tooltip.transform.rotation = Quaternion.identity;
        }
    }

    /// <summary>
    /// Shows the items tooltip when the player mouses over it.
    /// </summary>
    void OnMouseEnter()
    {
        try
        {
            if ((!((this.transform.parent.gameObject.CompareTag("WeaponMount")) || (this.transform.parent.gameObject.CompareTag("ArmorMount")))))
            {
                this.tooltip.SetActive(true);
            }
        }
        catch (NullReferenceException ex)
        {
            this.tooltip.SetActive(true);
        }

    }

    /// <summary>
    /// Shows the items tooltip when the player mouses off of it.
    /// </summary>
    void OnMouseExit()
    {
        this.tooltip.SetActive(false);
    }
}
