using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnOff : MonoBehaviour
{

    private Button button;
    public Sprite imageOn;
    public Sprite imageOff;
    public bool mute;
    // Use this for initialization
    void Awake()
    {
        button = GetComponent<Button>();
        changeImage();
    }

    public void changeOnOff()
    {
        mute = !mute;
        changeImage();
    }

    public void change(bool valor)
    {
        mute = valor;
        changeImage();
    }

    void changeImage()
    {
        if (mute) button.image.overrideSprite = imageOff;
        else button.image.overrideSprite = imageOn;
    }

}
