using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should be attached to flamable item. Note that the object must have "Flamable" tag as well. Fires nearby flamable objects,
// Contains item's health, time to fire, time to extinguish, if was previosuly extinguished, fire spread radius, how much
// water was already poured over it, instantiates fire particle effect and changes item's colour to black while on fire.
public class ItemScript : MonoBehaviour
{
	public float health = 100.0f; // should be 100
	public float timeToFire = 10.0f; // differs on items, time in seconds the candle needs to set the item on fire
	public float timePuttingFire = 0.0f; // amount of time the object is putting on another object nearby
	public float waterAmountNeeded = 20.0f; // differs on items, amount of water the human needs to use to extinguish the fire
	public float durability = 1.0f; // the multiplier on how fast the item should burn
	public bool onFire = false; // boolean value to check if the item is on fire
	public bool extinguished = false; // boolean value to check if the item has been extinguished
	public bool wasExtinguished = false; // checks if the item was previously extinguished, doubles the timeToFire variable, can happen only once throughout the game
	public float fireSpreadRadius = 10.0f;
	public float amountOfWater = 0.0f;

	private GameObject[] flamableObjects;
	private GameObject fireEffect; // fire particle effect
	private bool fireEffectOn = false; // used to check if the fire particle effect was already istantiated
	private Color blackColor = Color.black; // changes the color of the object 
	private Renderer rend;
	private Color startColor; // this is the material the object has at the start

	// Puts on fire nearby items. Slowly changes the nearby objects' timeToFire (which will make it even easier to lighten up when candle approaches), the closer to the burning object the bigger change
	void FireNearbyItems()
	{
		foreach (Collider col in Physics.OverlapSphere(this.transform.position, fireSpreadRadius)) 
		{
			// if the nearby object has flamable tag
			if (col.gameObject.tag == "Flamable") 
			{
				// getting ItemScript script from the object nearby
				ItemScript itemScript = col.GetComponent<ItemScript> ();

				// if the object isn't yet on fire
				if (!(itemScript.onFire)) 
				{
					// getting the closest face of the object that is near to the fire
					Vector3 closestPoint = col.GetComponent<Collider> ().ClosestPointOnBounds (this.gameObject.transform.position);
					// distance between the closest face and the object on fire
					float distance = Vector3.Distance (closestPoint, this.gameObject.transform.position);
					// the closer the object the bigger the number which will lead to more quickly putting the nearby object on fire
					float fireRate = fireSpreadRadius - distance; // example 10 - 3, fireRate = 7
					// putting fire on the object, the further the object the longer it's going to take to light it up. // EXAMPLE 0.1 * (1 / 5) = 0.02, the object is 5 units away
					timePuttingFire += Time.deltaTime * (0.1f / fireRate);

					//Debug.Log ("I'm " + this.gameObject.name + " and I'm lighting up " + col.gameObject.name + " object, distance from the object is " + distance + ", fireRate = " + fireRate + " timePuttingFire: " + timePuttingFire);

					// if the amount of time putting on other object exeeds the needed to fire it up
					if (timePuttingFire >= itemScript.timeToFire) {
						// making the itemscript script's variable to true
						itemScript.onFire = true;
						// reseting the variable
						timePuttingFire = 0.0f;
					}
				} // end of !(itemScript.onFire)
			}// end of (tag == "Flamable")
		}// end of foreach
	}

	// blocking any action on this object, performed when the object was fully burnt
	void BlockActionOnThisObject()
	{
		this.health = 0.0f;
		this.timeToFire = 0.0f;
		this.waterAmountNeeded = 0.0f;
		this.durability = 0.0f;
		this.onFire = false;
		this.extinguished = false;
		this.amountOfWater = 0.0f;

		// replacing the tag so that the candle's function won't find it as a flamable object
		this.transform.gameObject.tag = "Burnt";

		// destroying the fire particle effect
		if (GameObject.Find("Fire(Clone)"))
			Destroy(fireEffect);
	}

	void ItemOnFire()
	{
		if (!(fireEffectOn)) 
		{
			// needs to be in Resources folder
			fireEffect = Instantiate (Resources.Load("Prefabs/Fire"), this.transform.position, Quaternion.identity) as GameObject;
			//fireEffect.transform.localScale = this.gameObject.transform.localScale;
		}

		this.health -= this.durability * Time.deltaTime;

		// changing the alpha channel
		if (blackColor.a < 1)
			blackColor.a += (this.durability * Time.deltaTime) / 100;
		else
			blackColor.a = 1;

		// Debug purposes
		//Debug.Log("Alpha colour : " + blackColor.a);

		// blending from the start material to black material, blackColor.a value is between 0 and 1 therefore it can be used in Lerp(). Value 0 makes startColor 100% visible, value 1 makes blackColor 100% visible.
		rend.material.color = Color.Lerp(startColor, blackColor, blackColor.a);

		// Debug purposes
		//Debug.Log("Item: " + this.name + " health: " + this.health);
	}

	// Start is called before the first frame update
    void Start()
    {
		// setting the alpha channel to 0 - transparent
		blackColor.a = 0;
		rend = this.GetComponent<Renderer> ();
		startColor = rend.material.color;
		flamableObjects = GameObject.FindGameObjectsWithTag ("Flamable");
	}
		
    // Update is called once per frame
    void Update()
    {
		// when the item is on fire
		if (this.onFire) 
		{
			ItemOnFire ();
			FireNearbyItems ();
			fireEffectOn = true;
		} else
		{
			fireEffectOn = false;
			if (GameObject.Find("Fire(Clone)") != null)
				Destroy(fireEffect);
		}

		// when the item's health reaches 0
		if (this.health <= 0) 
		{
			BlockActionOnThisObject();
		}

		// if the item extinguished the next time it will take double amount of time to set it on fire. Can happen one time throughout the game that's why immediately after checking the wasExtinguished value is changed to true.
		if (this.extinguished) 
		{
			onFire = false;
			amountOfWater = 0.0f;

			if (!(wasExtinguished))
				timeToFire *= 2;	
			wasExtinguished = true; // this then would stay true through the whole game and the if statement before it will only be called once.
		}

		// if the item is filled with enough water it's extinguished. checking onFire just to be sure, but should be checked in other scripts before accessing the amountOfWater
		if (onFire && amountOfWater >= waterAmountNeeded)
			this.extinguished = true;

		// if the user hasn't finished extinguishing the object it will slowly lose the amountOfWater, so he will have to start all over again if he doesn't try to extinguish the object for long
		if (amountOfWater > 0) 
		{
			//Debug.Log ("amount of water: " + amountOfWater);
			amountOfWater -= Time.deltaTime * 0.2f;
		}
    }
}
