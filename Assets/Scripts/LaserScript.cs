using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public GameObject laserStart, laserMiddle, laserEnd;

    // Start is called before the first frame update
    void Start()
    {
        laserStart = Instantiate(laserStart) as GameObject;
        laserStart.transform.SetParent(this.transform);
        laserStart.transform.localPosition = new Vector3(0, 0, 0);
        laserStart.transform.localRotation = Quaternion.identity;


        laserMiddle = Instantiate(laserMiddle) as GameObject;
        laserMiddle.transform.SetParent(this.transform);
        laserMiddle.transform.localPosition = new Vector3(0, 0, 0);
        laserMiddle.transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // Create the laser start from the prefab
        if (laserStart == null)
        {
           
        }
        if (laserMiddle == null)
        {
           
        }

        // Define an "infinite" size, not too big but enough to go off screen
        float maxLaserSize = 20f;
        float currentLaserSize = maxLaserSize;

        // Raycast at the right as our sprite has been design for that
        Vector2 laserDirection = this.transform.up;
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, laserDirection, maxLaserSize);

        if (hit.collider != null)
        {
            // We touched something!

            // -- Get the laser length
            currentLaserSize = Vector2.Distance(hit.point, this.transform.position);

            // -- Create the end sprite
            if (laserEnd == null)
            {
                laserEnd = Instantiate(laserEnd) as GameObject;
                laserEnd.transform.parent = this.transform;
                laserEnd.transform.localPosition = Vector2.zero;
            }
        }
        */
        /*
        else
        {
            // Nothing hit
            // -- No more end
            if (laserEnd != null) Destroy(laserEnd);
        }*/

    }
}
