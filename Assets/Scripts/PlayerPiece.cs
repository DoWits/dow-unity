using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPiece : BasePiece
{
    public override void Setup(string orientation, Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, GameObject pieceButtons)
    {
        base.Setup(orientation, newTeamColor, newSpriteColor, newPieceManager,pieceButtons);

        mMovement = new Vector3Int(1, 1, 0);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("R_Player");

        //Set orientation of Player2's image to down
        if(mOrientation.Equals("down"))
        {
            transform.Rotate(new Vector3(0,0,180));
        }

    }
    


}
