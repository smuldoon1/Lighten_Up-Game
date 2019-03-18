using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBombScript : MonoBehaviour
{
	public float waterAmount = 5.0f; // how much water there is in one 'shot' that can be applied on fired objects
	public float waterRadius = 10.0f; // how far the water can reach 
	public int numberOfBombs = 10; // how many water bombs the player has
	public Text ammoText;

	private float timeAlive = 0.0f;
	private GameObject steamEffect;

	void OnCollisionEnter(Collision collision)
	{
		foreach (Collider col in Physics.OverlapSphere(this.transform.position, waterRadius)) 
		{
			// if the nearby object has flamable tag
			if (col.gameObject.tag == "Flamable") 
			{
				// getting ItemScript script from the object nearby
				ItemScript itemScript = col.GetComponent<ItemScript> ();

				// if the object is on fire
				if (itemScript.onFire) 
				{
					// getting the closest face of the object that is near the water bomb
					Vector3 closestPoint = col.GetComponent<Collider> ().ClosestPointOnBounds (this.gameObject.transform.position);
					// distance between the closest face of the item on fire and the water bomb
					float distance = Vector3.Distance (closestPoint, this.gameObject.transform.position);
					// the closer the object the bigger the number which will lead to more water poured on the nearby objects on fire
					float waterRate = waterRadius - distance; // example 10 - 3, waterRate = 7


					// leting the item script handle extinguishing
					itemScript.amountOfWater += (waterAmount * (waterRate / 10));

                    // adding steam particle effect
                    steamEffect = Instantiate(Resources.Load("Prefabs/Steam"), itemScript.transform.position, Quaternion.identity) as GameObject;

                    Debug.Log ("I'm a water bomb and I exploded on " + col.gameObject.name + " object, distance from the object is " + distance + ", amountfilled = " + itemScript.amountOfWater);
				} // end of !(itemScript.onFire)
			}// end of (tag == "Flamable")
		}// end of foreach

		// had issues with object being destroyed at the start because it collided with the human
		if(timeAlive > 0.2f)
			Destroy (this.gameObject);
	}

    // Update is called once per frame
    void Update()
    {
		timeAlive += Time.deltaTime;

		if (this.gameObject.activeSelf) 
		{
			if(ammoText != null)
				ammoText.text = "AMMO " + numberOfBombs;

		} // end of activeSelf
    }
}
