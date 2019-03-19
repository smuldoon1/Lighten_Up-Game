using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script should be attached to Water-Gun object. Shoots water, checks if the player is allowed to reload (enough ammunition)
// and is responsible for displaying the ammunition ammount in the ammoText.
public class WaterGunScript : MonoBehaviour
{
	public int waterAmmoClip = 10; // how much water one clip stores
	public int waterAmmoAll = 20; // how much all water there is
	public float waterAmount = 5.0f; // how much water there is in one 'shot' that can be applied on ONE fired objects
	public float waterRange = 10.0f; // how far the water can reach 

	public Text ammoText; // UI text containing amount of ammunition
	private bool isAbleToShoot = true; // used in coroutine
	private bool isReloading = false; // used in reloading function so that the reloading isn't caught in every frame the player pressed reload button
	private GameObject steamEffect;

	IEnumerator DelayShooting()
	{
		isAbleToShoot = false;
		yield return new WaitForSeconds (0.1f);
		isAbleToShoot = true;
	}

	void Reload()
	{
		// if there actually is need to reload
		if (10 - waterAmmoClip == 0 || waterAmmoAll == 0)
			return;

		// reloads only once and not every frame that captured the user pressing R key
		isReloading = true;

		// ammoToFill = clip size - current clip size
		int ammoToFill = 10 - waterAmmoClip;

        // checking if there is enough ammo in reserve to reload
        if(waterAmmoAll - ammoToFill > 0)
        {
            waterAmmoClip += ammoToFill;
            waterAmmoAll -= ammoToFill;
        }
        else
        {
            waterAmmoClip += waterAmmoAll;
            waterAmmoAll = 0;
        }

		isReloading = false;
	}

	void ShootWater()
	{
		if (waterAmmoClip > 0) 
		{
			//Debug.Log ("Shooting water!");
			waterAmmoClip--;

			RaycastHit hit = new RaycastHit ();

			// raycast
			if (Physics.Raycast (this.transform.position, transform.TransformDirection (Vector3.forward), out hit, waterRange)) 
			{	
				// used to change variable that shows up UI
				ExtinguishObject extinguishObject = GameObject.Find ("Human").GetComponent<ExtinguishObject> ();

				// if the player shot flamable object that is on fire
				if (hit.collider.gameObject.tag == "Flamable") 
				{
					ItemScript itemScript = hit.collider.gameObject.GetComponent<ItemScript> ();	
				
					Debug.Log ("Raycasted flamable object: " + hit.collider.name);

					// used to show up UI elements, when the player points at fired object
					if (itemScript.onFire && extinguishObject != null) 
					{
                        // adding steam effect
                        steamEffect = Instantiate(Resources.Load("Prefabs/Steam"), itemScript.transform.position, Quaternion.identity) as GameObject;

                        extinguishObject.raycastedFire = true;

						// passing the amount of water to itemscript
						itemScript.amountOfWater += waterAmount;

						Debug.Log ("Amount Filled " + itemScript.amountOfWater);
					} 
					else 
					{
						extinguishObject.raycastedFire = false;
					}

				} else if (extinguishObject != null) // checking first if the object was found
					extinguishObject.raycastedFire = false;
			}// end of Physics.Raycast
		}// end of waterAmmoClip > 0
	}

    // Update is called once per frame
    void Update()
	{
		// checking if the gun is activated by player
		if (this.gameObject.activeSelf) 
		{
			if (Input.GetButton ("Fire1") && isAbleToShoot && !(isReloading)) 
			{
				StartCoroutine (DelayShooting ());
				ShootWater ();
			}

			if (Input.GetKey (KeyCode.R) && !(isReloading))
				Reload ();

			// displaying UI text with ammunition
			ammoText.text = "Ammo " + waterAmmoClip + " / " + waterAmmoAll;
		} 
		else 
		{
			ammoText.enabled = false;	
		}
	}
}
