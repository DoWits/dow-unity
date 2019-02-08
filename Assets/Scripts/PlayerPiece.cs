using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPiece : BasePiece
{
    protected bool myTurn = true;
    //public GameObject mButtonPrefab;
    Button shootButton;
    ColorBlock enable;
    ColorBlock disable;
    public GameObject Laser;
    public LaserScript laserObject;
    
    public override void Setup(string orientation, Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, GameObject pieceButtons, PlayerButtons playerButtons)
    {
        /*
        GameObject ShootButton = Instantiate(mButtonPrefab);
        ShootButton.transform.SetParent(transform);
        */

      //  Laser = transform.GetChild(0).gameObject;
        
        /*Laser.transform.SetParent(this.transform);
        Laser.transform.localPosition = new Vector3(0, 29, 0);*/

        shootButton = ShootButtonObject.GetComponent<Button>();
        shootButton.GetComponent<RectTransform>().position = new Vector3(0, -250, 0);
        shootButton.onClick.AddListener(() => MovePiece(this, "Shoot"));
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
