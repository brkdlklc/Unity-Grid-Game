using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    [SerializeField]
    private Button colorPickerButton;
    [SerializeField]
    private Image  colorPreviewImage;
    private Color32[] colorList = new Color32[] {new Color32(234,234,234,255) , new Color32(28, 28, 28, 255) , new Color32(47, 183, 63, 255), new Color32(226, 240, 23, 255), new Color32(49, 56, 206, 255), new Color32(220, 19, 19, 255), new Color32(218, 134, 6, 255) }; // White, Black, Blue, Green, Yellow, Red, Orange.
    private int activeColorIndex = 0;

    private AudioManager audioScript; // Button click ses efektlerini kullanmak için başka bir script tanımlandı.
    void Start()
    {
        audioScript = GameObject.Find("AudioObject").GetComponent<AudioManager>();
        initiliazeColors();
    }

    void Update()
    {
        
    }
    private void initiliazeColors()
    {
        setColorOnPickerButton(colorList[activeColorIndex]);
        changeColorPreview();
    }
    private void changeColorPreview()   // Aktif renk değerinin 1 sonraki değerini gösterir.
    {
        if (isItLastColor())
            colorPreviewImage.color = colorList[0];
        else
            colorPreviewImage.color = colorList[activeColorIndex + 1];
    }

    public void changeColor()
    {
        audioScript.playButtonClickSound();

        if (isItLastColor())
            activeColorIndex = 0;
        else
            activeColorIndex++;

        setColorOnPickerButton(colorList[activeColorIndex]);
        changeColorPreview();
    }
    private Boolean isItLastColor()
    {
        return activeColorIndex == colorList.Length - 1;
    }

    public Color getActiveColor()
    {
        return colorList[activeColorIndex];
    }

    public void setColorOnPickerButton(Color32 newColor)
    {
        colorPickerButton.GetComponent<Image>().color = newColor;
    }
}
