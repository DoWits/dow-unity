using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerPiece : BasePiece
{
    protected bool myTurn = true;
    //public GameObject mButtonPrefab;
    Button shootButton;
    ColorBlock enable;
    ColorBlock disable;
    LaserScript laserObject;

    float increment = 0f;

    Vector3 GetLaserDirection()
    {
        increment = 1f;

        Vector3 LaserPosition = this.transform.position;

        Dictionary<string, Vector3> IncrementList = new Dictionary<string,Vector3>()
        {
            { "up", new Vector3(0,increment,0)},
            { "down", new Vector3(0,-increment,0)},
            { "left", new Vector3(-increment,0,0)},
            { "right", new Vector3(increment,0,0)},

        };
        LaserPosition = IncrementList[this.mOrientation];

        return LaserPosition;
    }

    /* This probably isn't neccessary 
    public Vector3 GetLaserDirection()
    {

        increment = 1f;

        Vector3 LaserDirection = this.transform.position;

        Dictionary<string, Vector3> IncrementList = new Dictionary<string, Vector3>()
        {
            { "up", new Vector3(0,increment,0)},
            { "down", new Vector3(0,-increment,0)},
            { "left", new Vector3(-increment,0,0)},
            { "right", new Vector3(increment,0,0)},

        };
        LaserPosition = IncrementList[this.mOrientation];

        return LaserPosition;
    }*/

    public override void Setup(
        string orientation, Color newTeamColor,
        Color32 newSpriteColor, PieceManager newPieceManager,
        GameObject pieceButtons, PlayerButtons playerButtons)
    {
        /*
        GameObject ShootButton = Instantiate(mButtonPrefab);
        ShootButton.transform.SetParent(transform);
        */

        // Laser = transform.GetChild(0).gameObject;
        // Laser.


        /*Laser.transform.SetParent(this.transform);
        Laser.transform.localPosition = new Vector3(0, 29, 0);*/

        //GameObject Laser = Instantiate(LaserPrefab) as GameObject ;
       // Laser.transform.SetParent(this.transform);

        laserObject = (LaserScript)this.gameObject.GetComponent("LaserScript");

        //laserObject.Setup(this);

        shootButton = ShootButtonObject.GetComponent<Button>();
        shootButton.GetComponent<RectTransform>().position = new Vector3(0, -250, 0);
        shootButton.onClick.AddListener( () =>  MovePiece(this, "Shoot"));
        enable = shootButton.colors;
        disable = shootButton.colors;

        disable.disabledColor = Color.grey;
        disable.normalColor = Color.grey;
        disable.pressedColor = Color.grey;
        disable.highlightedColor = Color.grey;

        mLockMovement = (false);
        base.Setup(orientation, newTeamColor, newSpriteColor, newPieceManager,pieceButtons, playerButtons);

        mMovement = new Vector3Int(1, 1, 0);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("R_Player");

        //testing with colliders
        CircleCollider2D edges = this.gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;
        edges.radius = 15f;


        //Set orientation of Player2's image to down
        if(mOrientation.Equals("down"))
        {
            
            shootButton.GetComponent<RectTransform>().position = new Vector3(0, 250, 0);
            myTurn = false;
            mLockMovement = (true);
            transform.Rotate(new Vector3(0,0,180));
            shootButton.enabled = myTurn;

            shootButton.colors = disable;

        }

    }

    public override IEnumerator ShootingAnimation()
    {
        while (laserObject.IsDoneShooting() == false)
            yield return laserObject.ShootLaserFromPointAnimation(this.transform.position, GetLaserDirection(), this.mCurrentCell.mBoard, (BasePiece)this);

        doneShooting = true;


        laserObject.ResetShooting();

        yield return new WaitUntil(() => IsAllDoneShooting());

    }

    public override void DisableShoot()
    {
        shootButton.enabled = false;
        shootButton.colors = disable;
    }

    public override void EnableShoot()
    {
        shootButton.enabled = true;
        shootButton.colors = enable;
    }
    
    public override void ChangeTurn()
    {
        //The movemenet wil be locked and unlocked based on the value of myTurn
        mLockMovement = (myTurn);
        myTurn = !myTurn;
        shootButton.enabled =  myTurn;
        if (!myTurn)
            shootButton.colors = disable;
        else
            shootButton.colors = enable;
        
    }
  


}
