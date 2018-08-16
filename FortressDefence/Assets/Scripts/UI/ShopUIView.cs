using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIView : MonoBehaviour {


    public Sprite upButtonEnabled;
    public Sprite upButtonDisabled;


    public Button upButtonArcher1;
    public Button upButtonArcher2;
    public Button upButtonArcher3;
    public Button upButtonFortress;

	
    public void setUpButtonArcher1(bool value)
    {
        Sprite sprite = (value) ? upButtonEnabled : upButtonDisabled;
        upButtonArcher1.GetComponent<Image>().sprite = sprite;
    }

    public void setUpButtonArcher2(bool value)
    {
        Sprite sprite = (value) ? upButtonEnabled : upButtonDisabled;
        upButtonArcher2.GetComponent<Image>().sprite = sprite;
    }

    public void setUpButtonArcher3(bool value)
    {
        Sprite sprite = (value) ? upButtonEnabled : upButtonDisabled;
        upButtonArcher3.GetComponent<Image>().sprite = sprite;
    }

    public void setUpButtonCastle(bool value)
    {
        Sprite sprite = (value) ? upButtonEnabled : upButtonDisabled;
        upButtonFortress.GetComponent<Image>().sprite = sprite;
    }
}
