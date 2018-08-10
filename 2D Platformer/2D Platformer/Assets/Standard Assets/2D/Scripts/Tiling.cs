using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

    public int offsetX = 2;                 // The offset so that we don't get any weird errors

    // These are used for checking if we need to instantiate things
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    public bool reverseScale = false;       // Used if the object is not tile-able

    private float spriteWidth = 0f;         // Width of our element
    private Camera cam;
    private Transform myTransform;

    void Awake() {
        cam = Camera.main;
        myTransform = transform;
    }

    // Use this for initialization
    void Start() {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update() {
        // Does it still need buddies? If not do nothing
        if (hasALeftBuddy == false || hasARightBuddy == false) {
            // Calculate the cameras extend (half width) of what the camera can see in the world co-ordinated
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            // Calculate the x.position where the camera can see the edge of the sprite element
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

            // checking if we can see the edge of the element and then calling MakeNewBuddy if we can
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false) {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false) {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
    }

    // A function that creates a buddy on the side required
    void MakeNewBuddy(int rightOrLeft) {
        // Calculating the new position for our new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        // Instantiate new buddy and storing in variable
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        // If not tilable lets reverse x size of our object to get rid of seams
        if (reverseScale == true) {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0) {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}