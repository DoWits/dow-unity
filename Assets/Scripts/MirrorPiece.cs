using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MirrorPiece : BasePiece
{
    public override void Setup(string orientation, Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, GameObject pieceButtons)
    {
        base.Setup(orientation, newTeamColor, newSpriteColor, newPieceManager, pieceButtons);

        mMovement = new Vector3Int(1, 1, 0);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("m45");

        if(mOrientation.Equals("-1"))
        {
            transform.Rotate(new Vector3(0, 0, 90));
        }

    }

}
