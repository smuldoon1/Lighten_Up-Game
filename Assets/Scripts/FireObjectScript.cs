using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script should be attached to the candle
public class FireObjectScript : MonoBehaviour
{
	public GameObject[] flamableObjects; // array of all the objects that candle can burn
	GameObject closestObject; // closest flamable object
	Vector3 candleCurrentPos; // candle position
	float distanceDifference = Mathf.Infinity; // distance difference between the candle and nearest flamable object's face
	float timePuttingFire = 0.0f; // how long the candle was lighting up an object
	public Text candleUI; // UI text to light up and object
	public Image fireBar; // fire bar displaying how much is left to fire up the object
	private float amountFilled = 0.0f; // amount of bar to be filled (how much the candle 'fired' the object already)
	private bool resetFireTime = false; // resets the time candle was firing the object - used when it is no longer in collision with that object

	// Finds and returns the closest object that is flamable
	GameObject FindClosestFlamableObject()
	{
		flamableObjects = GameObject.FindGameObjectsWithTag ("Flamable");
		GameObject closestObject = null;
		float distance = Mathf.Infinity;
		Vector3 candlePosition = this.transform.position;

		foreach (GameObject flamableObj in flamableObjects) 
		{
			Vector3 difference = flamableObj.transform.position - candlePosition;
			float currentDistance = difference.sqrMagnitude; // faster than Vector3.Distance(a,b)
			if (currentDistance < distance) 
			{
				closestObject = flamableObj;
				distance = currentDistance;
			}
		}

		// Debug purposes
		//Debug.Log("Distance from " + closestObject.name + " object: " + distance);

		return closestObject;
	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log ("I'm walking on the " + collision.gameObject.name + " object!!!!");
		if(collision.gameObject.tag == "Flamable")
			StartFire ();
		resetFireTime = false;
	}

	void OnCollisionExit(Collision collision)
	{
		Debug.Log ("I'm no longer walking on the " + collision.gameObject.name + "object!!!!");
		if(collision.gameObject.tag == "Flamable")
			resetFireTime = true;
	}

	void StartFire()
	{
		// when the player is just walking on the object
		float timeToFireBoost = 1.0f;

		// getting the closest object's script 
		ItemScript itemScript = closestObject.GetComponent<ItemScript> ();
					
		// returns true while the button is held down
		if (Input.GetKey (KeyCode.E)) {
			// when the player is pressing the button he cannot move and the timeToFireBonus is doubled
			timeToFireBoost = 2.0f;

			/* DISABLE CANDLE CONTROL HERE!
			 * 
			 * 
			 * 
			 *
		} else {
			/* if (candle control disabled)
			 * 		enable candle control
			 * 
			 * 
			 */
		}

		// checking if the timer should be reset
		if (!(resetFireTime))
			timePuttingFire += Time.deltaTime * timeToFireBoost; // multiplying by the boost
		else
			timePuttingFire = 0;

		amountFilled = timePuttingFire / itemScript.timeToFire;

		// Debug purposes
		//Debug.Log("Amount filled = " + amountFilled);

		// if the object was lightened up by the candle long enough
		if (timePuttingFire >= itemScript.timeToFire) 
		{
			// making the itemscript script's variable to true
			itemScript.onFire = true;
			// reseting the variable
			timePuttingFire = 0.0f;
		}

		// UI elements showing up
		if (itemScript.onFire) 
		{
			// Text and firebar disabled
			candleUI.text = "This object is on fire!";
			fireBar.enabled = false;
			fireBar.transform.GetChild (0).gameObject.SetActive (false);
		}
		else 
		{
			candleUI.text = "Press E to light up the " + closestObject.name;
			fireBar.enabled = true;
			fireBar.transform.GetChild (0).gameObject.SetActive (true);
			fireBar.transform.GetChild(0).transform.localScale = new Vector3 (amountFilled, 1, 1);
		}
	}


    // Start is called before the first frame update
    void Start()
    {
		if (flamableObjects == null)
			flamableObjects = GameObject.FindGameObjectsWithTag ("Flamable");
    
		fireBar.enabled = false;
		fireBar.transform.GetChild (0).gameObject.SetActive (false);
	}

    // Update is called once per frame
    void Update()
    {
		candleCurrentPos = this.transform.position;
		closestObject = FindClosestFlamableObject ();

		// the closest point to the bounding box of the collider (so face, not center)
		Vector3 closestPoint = closestObject.GetComponent<Collider> ().ClosestPointOnBounds (candleCurrentPos);
		distanceDifference = Vector3.Distance (closestPoint, candleCurrentPos);

	
		// if candle is close enough to the flamable object
		// if (distanceDifference < 1 && candle isn't jumping)
		if (distanceDifference < 1) 
		{
			StartFire ();
		} 
		else
		{
			fireBar.enabled = false;
			fireBar.transform.GetChild (0).gameObject.SetActive (false);
			candleUI.text = "";
		}

    }
}
