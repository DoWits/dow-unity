using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour
{
    public GameObject laserStartPrefab, laserMiddlePrefab, laserEndPrefab;
    public BasePiece piece;

    public float laserStartX, laserStartY;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
 /*   
    public void Setup(BasePiece piece)
    {
       // this.piece = piece;

    }
    */

    public IEnumerator ShootLaserFromPointAnimation(Vector3 position, Vector3 direction, Board board, BasePiece parent)
    {
        GameObject laserEnd, laserStart, laserMiddle, laserStartExtenstion;

        //Put up the starting part of the laser
        Quaternion rotation = parent.transform.rotation;
         laserStart = Instantiate(laserStartPrefab);
        laserStart.transform.SetParent(parent.transform);
        laserStart.transform.rotation = rotation;

        //Create a little extenstion of the start
        laserStartExtenstion = Instantiate(laserMiddlePrefab);
        laserStartExtenstion.transform.SetParent(parent.transform);
        laserStartExtenstion.transform.rotation = rotation;
        int laserExtensionLength = 0;

        
        if (parent.getOrientation().Equals("+1") || parent.getOrientation().Equals("-1"))
        {
            laserStart.transform.position = position + 8f * direction;

            laserStartExtenstion.transform.position = position + 13f * direction;
            laserStartExtenstion.transform.localScale = new Vector3(20, 1, 0);
            laserExtensionLength = 38;

            float x = direction.x;
            float y = direction.y;

            if (x > 0)
                rotation = Quaternion.Euler(0, 0, 270);
            else if (x < 0)
                rotation = Quaternion.Euler(0, 0, 90);
            else if (y > 0)
                rotation = Quaternion.Euler(0, 0, 0);
            else if (y < 0)
                rotation = Quaternion.Euler(0, 0, 180);
            else
                Debug.LogError("Faulty mirror rotation");

            laserStart.transform.rotation = rotation;
            laserStartExtenstion.transform.rotation = rotation; 

        }

        
        else
        {
            laserStart.transform.position = position + 35.1f * direction;
            laserStartExtenstion.transform.position = position + 40f * direction;
            laserStartExtenstion.transform.localScale = new Vector3(20f, 1, 0);
            laserExtensionLength = 10;
            laserStartExtenstion.transform.rotation = parent.transform.rotation;


        }
        while (laserStartExtenstion.transform.localScale.y < laserExtensionLength)
            yield return IncreaseLaser(direction, laserStartExtenstion);




        laserMiddle = Instantiate(laserMiddlePrefab);
        laserMiddle.transform.SetParent(parent.transform);
        laserMiddle.transform.position = position + 50f * direction;
        laserMiddle.transform.rotation = rotation;
        laserMiddle.transform.localScale = new Vector3(20, 1, 1);


        //go in the same direction till you meet the end of the board or a piece


        //if you meet the end of the board instantiate the endLaser prefab


        /*
         * if you meet a mirror, instantiate the startLaser prefab to another gameobject and then yield reutrn ShootLaserFromPointAnimation
         * with parameters the Vector postion with new directions
         */

        /*
         * if you meet the a player piece (doesn't matter which) start up the endLaser and exit after that 
         * 
         */

        int X = (int)direction.x;
        int Y = (int)direction.y;

        //Collider2D collider = laserMiddle.GetComponent<Collider2D>();


        RaycastHit2D hit = Physics2D.Raycast(position + 65f * direction, direction, laserMiddle.transform.localScale.y);
       
        while (hit.collider==null)
        {
            
            yield return IncreaseLaser(direction, laserMiddle);
            hit = Physics2D.Raycast(position + 65f * direction, direction, laserMiddle.transform.localScale.y);

        }

        GameObject laserTarget = hit.collider.transform.gameObject;
        if(laserTarget.name.Contains("Manager"))
        {
            Debug.Log("Hit the edge");

            laserEnd = Instantiate(laserEndPrefab);
            laserEnd.transform.SetParent(parent.transform);
            laserEnd.transform.localScale = new Vector3(25, 24.5f, 0);
            laserEnd.transform.position = (Vector3)hit.point - 16f * direction;
            laserEnd.transform.rotation = rotation;

            while (laserEnd.transform.localScale.y < 25)
                yield return IncreaseLaser(direction, laserEnd, 0.0125f);

            if (laserEnd != null)
                Destroy(laserEnd);
        }
        else if (laserTarget.name.Contains("M"))
        {

            GameObject laserEndAtMirror = Instantiate(laserStartPrefab);
            laserEndAtMirror.transform.SetParent(parent.transform);
            laserEndAtMirror.transform.localScale = new Vector3(20, 1, 0);
            laserEndAtMirror.transform.position = hit.collider.transform.position - 24.5f*direction;
            laserEndAtMirror.transform.rotation = rotation;
            laserEndAtMirror.transform.Rotate(new Vector3(0, 0, 180));


            while (laserEndAtMirror.transform.localScale.y < 25)
                yield return IncreaseLaser(direction, laserEndAtMirror);


            // put a yield return to MirrorPiece instead of the below
            hit.collider.SendMessage("OnIncomingLaser",direction);

            if (laserEndAtMirror != null)
                Destroy(laserEndAtMirror);

          //  LaserScript MirrorReflection = new LaserScript();
          //  MirrorReflection.Setup((BasePiece)laserTarget);

        }
        else
        {
            laserEnd = Instantiate(laserEndPrefab);
            laserEnd.transform.SetParent(parent.transform);
            laserEnd.transform.localScale = new Vector3(25, 24f, 0);
            laserEnd.transform.position = (Vector3)hit.point - 12f * direction;
            laserEnd.transform.rotation = rotation;

            while (laserEnd.transform.localScale.y < 25)
                yield return IncreaseLaser(direction, laserEnd, 0.003125f);

            if (laserEnd != null)
                Destroy(laserEnd);

            Debug.Log("Hit a player");
        }

      //  if(hit.collider.transform.parent. == )


        if (laserStart != null)
           Destroy(laserStart);
        if (laserMiddle != null)
           Destroy(laserMiddle);
        if (laserStartExtenstion != null)
            Destroy(laserStartExtenstion);

        yield return 0;
        
    }
    public IEnumerator IncreaseLaser(Vector3 increment, GameObject laser, float rate = 7)
    {
        laser.transform.localScale += new Vector3(0,1,0)*rate ;
        laser.transform.position += increment*rate/2;
        yield return 0;
    }



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
