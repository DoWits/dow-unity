using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MirrorPiece : BasePiece
{

    LaserScript laserObject;
    Vector3 laserDirection = Vector3.zero;

    Vector2[] colliderPoints135 = new Vector2[]
       {
            new Vector2(-22f,22f), new Vector2(22f, -22f),
       };

    Vector2[] colliderPoints45 = new Vector2[]
     {
            new Vector2(-22f,-22f), new Vector2(22f, 22f),
     };

    public override void Setup(string orientation, Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, GameObject pieceButtons, PlayerButtons playerButtons)
    {
        base.Setup(orientation, newTeamColor, newSpriteColor, newPieceManager, pieceButtons, playerButtons);

        EdgeCollider2D edges = this.gameObject.AddComponent<EdgeCollider2D>() as EdgeCollider2D;
        mMovement = new Vector3Int(1, 1, 0);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("m45");

        laserObject = (LaserScript)this.gameObject.GetComponent("LaserScript");




        edges.points = colliderPoints45;

        if (mOrientation.Equals("-1"))
        {
            transform.Rotate(new Vector3(0, 0, 90));
            //edges.points = colliderPoints135;

        }


        

       

        


    }

    //Call this funtion if there is an incoming laser
   
    

    public override IEnumerator ShootingAnimation(Vector3 incomingLaserDirection)
    {
        doneShooting = false;
        int multiplier = 1;

        if (this.mOrientation.Equals("+1"))
        {
            multiplier = 1;
        }
        else if (this.mOrientation.Equals("-1"))
        {
            multiplier = -1;
        }
        else
            Debug.LogError("Mirror Doesn't have the right orientation");

        float x = incomingLaserDirection.x;
        float y = incomingLaserDirection.y;
        laserDirection = new Vector3(y * multiplier, x * multiplier, 0);


        //Debug.Log("Collision at mirror detected");

        while (laserObject.IsDoneShooting() == false)
        { yield return laserObject.ShootLaserFromPointAnimation(this.transform.position, laserDirection, this.mCurrentCell.mBoard, (BasePiece)this); }

        laserObject.ResetShooting();
        doneShooting = true;

    }





}
