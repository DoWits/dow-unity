

//need to really change the name to piece buttons from player buttons

using UnityEngine;
using UnityEngine.UI;

public class PlayerButtons : MonoBehaviour
{
   
    protected Button[] mButtons;

    public void Setup(GameObject pieceButtonsObject)
    {
        mButtons = pieceButtonsObject.GetComponentsInChildren<Button>();
    }


    void Start()
    {

    }

    

    // Update is called once per frame
    void Update()
    {
        
        
    }


    public void RotateButtons()
    {
        foreach (Button b in mButtons)
        {
            if (b.name.Contains("Rotate"))
            { b.gameObject.transform.Rotate(new Vector3(0, 0, 180));
                b.gameObject.transform.localPosition = -1 * b.gameObject.transform.localPosition;
            }
        }
    }


    //get the specific button to show
    public void Show(string buttonName)
    {
        foreach(Button b in mButtons)
        {
            if (b.name.Equals(buttonName))
            { b.gameObject.SetActive(true); break; }
        }
    }
    //get the specific button to hide
    public void Hide(string buttonName)
    {
        foreach(Button b in mButtons)
        {
            if (b.name.Equals(buttonName))
            { b.gameObject.SetActive(false); break; }
        }
    }
    //Hide all buttons
    public void HideAll()
    {
        foreach(Button b in mButtons)
        {
            b.gameObject.SetActive(false);
        }
    }
    //Show all Buttons
    public void ShowAll()
    {
        foreach (Button b in mButtons)
        {
            b.gameObject.SetActive(true);
        }
    }


}
