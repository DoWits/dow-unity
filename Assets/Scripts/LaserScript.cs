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
    
    public void Setup(BasePiece piece)
    {
        this.piece = piece;

    }


    public IEnumerator ShootLaserFromPointAnimation(Vector3 position, Vector3 direction, Board board, BasePiece parent)
    {
        int x = (int)((position.x / 50) + 3)/2;
        int y = (int)((position.y / 50) + 3)/2;
        Cell startCell = board.mAllCells[x, y];


        GameObject laserStart = Instantiate(laserStartPrefab);
        laserStart.transform.SetParent(parent.transform);

        if (piece.getOrientation().Equals("+1") || piece.getOrientation().Equals("-1"))
            laserStart.transform.localPosition = position + 1f * direction;
        else
            laserStart.transform.position = position +  35.1f * direction;
        laserStart.transform.rotation = piece.transform.rotation;



        GameObject laserMiddle = Instantiate(laserMiddlePrefab);
        laserMiddle.transform.SetParent(parent.transform);
        laserMiddle.transform.position = position + 50f * direction;
        laserMiddle.transform.rotation = piece.transform.rotation;
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


        while (laserMiddle.transform.localScale.y < 300f)
        {
            yield return IncreaseMiddle(direction, laserMiddle);

        }

        
        if (laserStart != null)
           Destroy(laserStart);
        if (laserMiddle != null)
           Destroy(laserMiddle);

        yield return 0;
        
    }
    public IEnumerator IncreaseMiddle(Vector3 increment, GameObject laserMiddle)
    {
        laserMiddle.transform.localScale += new Vector3(0,1,0)*5f ;
        laserMiddle.transform.position += increment*2.5f;
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
