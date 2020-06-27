using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleManager : MonoBehaviour
{
    private Vector2 coordinate;
    private SaveDataManager saveDataScript;  // Bir circle objesinin rengi değiştiği zaman sahnenin üstündeki "saved!" yazısını kaldırmak için tanımlandı.
    private ColorManager ColorManagerScript; // Renk paletindeki rengi kullanabilmek için tanımlandı.
    private AudioManager audioScript;        // Button click ses efektlerini kullanmak için tanımlandı.
    void Start()
    {
        saveDataScript= GameObject.Find("SaveDataObject").GetComponent<SaveDataManager>();
        audioScript = GameObject.Find("AudioObject").GetComponent<AudioManager>();
        ColorManagerScript = GameObject.Find("ColorObject").GetComponent<ColorManager>();
    }
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        audioScript.playColorChangeSound();
        setCircleColor(ColorManagerScript.getActiveColor());
        saveDataScript.makeInvisibleSaveText(); // Bir circle objesinin rengi değiştiyse sahnede "Saved" yazısı varsa kaldır.
    }

    public void setCoordinate(Vector2 newCoordinate)
    {
        this.coordinate = newCoordinate;
    }
    public Vector2 getCoordinate()
    {
        return this.coordinate;
    }
    public void setCircleColor(Color32 newColor)
    {
        this.GetComponent<SpriteRenderer>().color = newColor;
    }
    public Color32 getCircleColor()
    {
        return this.GetComponent<SpriteRenderer>().color;
    }
}
