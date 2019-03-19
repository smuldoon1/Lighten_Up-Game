using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Variables for controlls
    // Set to automatically resort to controller 1
    // Booleans
    // Set to public incase there is an error and one is stuck on
    public bool leftStick = true;
    public bool rightStick = true;
    public bool aPressed, bPressed, xPressed, yPressed = false;
    // Strings
    // Set to public so they can be assigned in the editor to the correct input
    public string moveX = "C1moveX";
    public string moveY = "C1moveY";
    public string horizontal = "C1horizontal";
    public string vertical = "C1vertical";
    public string aButton = "C1A";
    public string bButton = "C1B";
    public string xButton = "C1X";
    public string yButton = "C1Y";
    public GameObject camera;

    // Privates
    private Vector3 startPos;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // If the left stick is set to respond
        if (leftStick)
        {
            // Takes in the input direction
            Vector3 direction = Vector3.zero;
            direction.x = Input.GetAxis(moveX);
            direction.z = Input.GetAxis(moveY);
            playerTransform.position = playerTransform.position + (playerTransform.forward * direction.z + playerTransform.right * direction.x) *(Time.deltaTime * 50);

        }
        // If the right stick is set to respond
        if (rightStick)
        {
            playerTransform.Rotate(new Vector3(0, Input.GetAxis(horizontal), 0));
            camera.transform.Rotate(new Vector3(Input.GetAxis(vertical), 0, 0));
        }

        // If a button is pressed
        if (Input.GetButton(aButton))
        {
            aPressed = true;
        }
        else
        {
            aPressed = false;
        }

        // If b button is pressed
        if (Input.GetButton(bButton))
        {
            bPressed = true;
        }
        else
        {
            bPressed = false;
        }

        // If x button is pressed
        if (Input.GetButton(xButton))
        {
            xPressed = true;
        }
        else
        {
            xPressed = false;
        }

        // If y button is pressed
        if (Input.GetButton(yButton))
        {
            yPressed = true;
        }
        else
        {
            yPressed = false;
        }
    }
}
