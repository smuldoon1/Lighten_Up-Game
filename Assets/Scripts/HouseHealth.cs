using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script is attached to an empty game object. It displays a bar at the top with the house health.
// Health is calculated by (all flamable objects health at the start) / (all flamables' objects health left).
// When the health is less than 66% the colour of the bar changes to orange, when lower than 33% it's red.
public class HouseHealth : MonoBehaviour
{
	public GameObject[] flamableObjects; // array of all the objects that candle can burn

	public float totalHealth;
	public float currentHealth;
	public Image healthBar; // health bar displaying amount of health, calculated by current / total flamable objects' health
    private float health66 = 0.0f;
    private float health33 = 0.0f;

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
        health66 = totalHealth / 3 * 2; // this will return 66%
        health33 = totalHealth / 3; // this will return 33%

        //Debug purposes
        //Debug.Log("Total house health: " + totalHealth);
    }

    // Update is called once per frame
    void Update()
    {
		// getting current health
		currentHealth = GetAllFlamablesHealth ();

        // showing current health on the bar, scaling it
		healthBar.transform.GetChild(0).transform.localScale = new Vector3 ((currentHealth / totalHealth), 1, 1);

        // if the healthbar is less than 66% changing bar colour to orange
        if(currentHealth < health66 && currentHealth > health33)
        {
            healthBar.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(255, 162, 0, 255);
        }

        // if the total health is less than 33% changing bar colour to red
        if (currentHealth <= health33)
        {
            healthBar.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }

		//Debug purposes
		//Debug.Log("Current house health: " + currentHealth);
    }
}
