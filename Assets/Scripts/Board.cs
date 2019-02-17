//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

	public GameObject mCellPrefab;
	 

	[HideInInspector]
	public Cell[,] mAllCells = new Cell[4,4];


	public void Create()
	{
        //Create
		for (int y = 0; y < 4; y++) {
			for (int x = 0; x < 4; x++) {

                //Create the cell
                GameObject newCell = Instantiate(mCellPrefab, transform);

                //Position
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);

                //Setup
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);

                
			}
        }
        //Color the board
        for (int x = 0; x < 4; x+=2)
        {
            for (int y = 0; y < 4; y++)
            {
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;

                //Color the cell

                mAllCells[finalX, y].GetComponent<Image>().color = new Color32(230, 220, 187, 255);
            }

        }




	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
