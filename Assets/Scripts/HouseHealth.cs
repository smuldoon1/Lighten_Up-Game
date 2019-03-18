using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseHealth : MonoBehaviour
{
	public GameObject[] flamableObjects; // array of all the objects that candle can burn

	public float totalHealth;
	public float currentHealth;
	public Image healthBar; // health bar displaying amount of health, calculated by current / total flamable objects' health

	// finds all flamable objects on the scene and stores their health
	float GetAllFlamablesHealth()
	{
		float amountOfHealth = 0.0f;
		flamableObjects = GameObject.FindGameObjectsWithTag ("Flamable");

		foreach (GameObject flamableObj in flamableObjects) 
		{
			// adding the health from attached script
			ItemScript itemScript = flamableObj.GetComponent<ItemScript> ();
			amountOfHealth += itemScript.health;
		}

		return amountOfHealth;
	}

    // Start is called before the first frame update
    void Start()
    {
		totalHealth = GetAllFlamablesHealth ();

		//Debug purposes
		Debug.Log("Total house health: " + totalHealth);
    }

    // Update is called once per frame
    void Update()
    {
		// getting current health
		currentHealth = GetAllFlamablesHealth ();

		healthBar.transform.GetChild(0).transform.localScale = new Vector3 ((currentHealth / totalHealth), 1, 1);

		//Debug purposes
		Debug.Log("Current house health: " + currentHealth);
    }
}
