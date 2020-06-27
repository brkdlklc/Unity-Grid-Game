using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    [SerializeField]
    private GameObject circle;  // Yaratılacak oyun objesi.

    [SerializeField]
    private int width, height;

    private float margin = 2f; // margin değişkeni circle objeleri arası mesafeyi belirler.

    private int maxWidth = 6;
    private int maxHeight = 7;
    private int minHeight = 3;
    private int minWidth = 3;

    private SaveDataManager saveDataScript; // Grid değiştiği zaman sahnenin üstündeki "saved!" yazısını kaldırmak için tanımlandı.
    private AudioManager audioScript; // Button click ses efektlerini kullanmak için başka bir script tanımlandı.
    void Start()
    {
        saveDataScript = GameObject.Find("SaveDataObject").GetComponent<SaveDataManager>();
        audioScript = GameObject.Find("AudioObject").GetComponent<AudioManager>();
        initializeCircles();
    }

    void Update()
    {

    }

    #region Grid Methods
    public void changeGridSize(int action)
    {
        audioScript.playButtonClickSound();   //Buton click ses efektini aktif eder.
        switch (action)     //action=0 genişliği 1 arttırır, action=1 uzunluğu 1 arttırır, action=2 uzunluğu 1 azaltır, action=3 genişliği 1 azaltır.
        {
            case 0:
                if (width != maxWidth)
                    changeGridWidth(+1);
                break;
            case 1:
                if (height != maxHeight)
                    changeGridHeight(+1);
                break;
            case 2:
                if (height != minHeight)
                    changeGridHeight(-1);
                break;
            case 3:
                if (width != minWidth)
                    changeGridWidth(-1);
                break;
            default:
                break;
        }
    }
    private void changeGridWidth(int widthChange)
    {
        width += widthChange;
        saveDataScript.makeInvisibleSaveText();      // Gridin büyüklüğü değiştiyse sahnede "Saved" yazısı varsa kaldır.
        this.transform.position = new Vector2(0, 0); //Yaratılacak oyun objelerinin pozisyonlarının doğru ayarlanması için parent objesi her zaman ortalandı.
        reArrangeMarginOffset();

        GameObject[] circleList = getAllCircles();
        foreach (GameObject circle in circleList)
        {
            arrangeCirclePosition(circle, circle.GetComponent<CircleManager>().getCoordinate());
        }

        if (widthChange < 0)
        {
            circleList = circleList.Where(c => c.GetComponent<CircleManager>().getCoordinate().x == width).ToArray<GameObject>();    //Genişlik azaltıldığı için grid sisteminin en sondaki oyun objelerini bulur ve sahneden yok eder.
            destroyCircles(circleList);
        }
        else if (widthChange > 0)
        {
            for (int j = 0; j < height; j++)
            {
                createCircle(new Vector2(width - 1, j));   //Genişlik arttırıldığı için grid sisteminin sonuna yeni circle objeleri yaratır.
            }
        }

        rescaleCircles();
        arrangeGridPosition();
        resizeBackgroundImage();
    }
    private void changeGridHeight(int heightChange)
    {
        height += heightChange;
        saveDataScript.makeInvisibleSaveText();         // Gridin büyüklüğü değiştiyse sahnede "Saved" yazısı varsa kaldır.
        this.transform.position = new Vector2(0, 0);   //Yaratılacak oyun objelerinin pozisyonlarının doğru ayarlanması için parent objesi her zaman ortalandı.
        reArrangeMarginOffset();

        GameObject[] circleList = getAllCircles();
        foreach (GameObject circle in circleList)
        {
            arrangeCirclePosition(circle, circle.GetComponent<CircleManager>().getCoordinate());
        }

        if (heightChange < 0)
        {
            circleList = circleList.Where(c => c.GetComponent<CircleManager>().getCoordinate().y == height).ToArray<GameObject>(); //Yükseklik azaltıldığı için grid sisteminin en yukarısındaki oyun objelerini bulur.
            destroyCircles(circleList);
        }
        else if (heightChange > 0)
        {
            for (int i = 0; i < width; i++)
            {
                createCircle(new Vector2(i, height - 1));
            }
        }

        rescaleCircles();
        arrangeGridPosition();
        resizeBackgroundImage();
    }
    private void arrangeGridPosition() // Oyun objelerinin sahneyi ortalayarak dağılması için gereken hesaplama.
    {
        float gridWidth = width * margin;
        float gridHeight = height * margin;
        this.transform.position = new Vector2((-gridWidth + margin) / 2, -(gridHeight - margin) / 2);
    }

    #endregion

    #region Circle Methods
    private void initializeCircles()    // Sahne yüklenirken grid i editörden değerleri girilen width ve height değişkenine göre initialize eden method.
    {
        reArrangeMarginOffset();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                createCircle(new Vector2(i, j));
            }
        }
        rescaleCircles();
        arrangeGridPosition();
        resizeBackgroundImage(); // Oyun objelerinin sayısına göre arkaplan resminin boyutunu değiştirir.
    }
    private void destroyCircles(GameObject[] goList) // Parametrede verilen oyun objesi dizisindeki objeleri sahneden yok eder.
    {
        foreach (GameObject go in goList)
        {
            Destroy(go);
        }
    }
    private void createCircle(Vector2 coordinate)   // Verilen coordinate noktasında circle oyun objesini yaratır ve bu oyun objesinin pozisyonuna margin ekler. Margin değişkeni oyun objelerinin birbirine olan uzaklığını belirtir.
    {
        GameObject newCircle = Instantiate(circle, this.transform);
        arrangeCirclePosition(newCircle, coordinate);
        newCircle.GetComponent<CircleManager>().setCoordinate(new Vector2(coordinate.x, coordinate.y));
        newCircle.name = "Circle (" + coordinate.x + "," + coordinate.y + ")";
    }

    private void arrangeCirclePosition(GameObject circle, Vector2 coordinate)
    {
        circle.transform.position = new Vector2(coordinate.x * margin, coordinate.y * margin);
    }
    private void rescaleCircles()        // Circle objelerinin sahnedeki circle obje sayısına göre yeniden scale değerinin hesaplanmasını sağlar. Min değer = 0.75f.
    {
        GameObject[] circleList = getAllCircles();
        float newScale = (float)(((maxHeight * maxWidth) - (height * width)) * 0.03); //Width, height değeri azaldıkça yani sahnedeki circle obje sayısı azaldıkça scale değerleri artar.
        foreach (GameObject circle in circleList)
        {
            circle.transform.localScale = new Vector3(0.75f + newScale, 0.75f + newScale, 1);
        }
    }
    private void resizeBackgroundImage() // Circle objelerinin arkaplan resmini tekrardan boyutlandırır.
    {
        GameObject circleBackground = GameObject.FindGameObjectWithTag("Image").gameObject;
        circleBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(width + 3 + (width - height) / 2.5f, height + 3 + (height - width) / 2.5f); //Circle objelerinin arkaplanının  width ve height değerlerine göre yeniden boyutlandırılması.
    }
    #endregion

    private void reArrangeMarginOffset()
    {
        float newMargin = (float)((height * width) * 0.02); //Circle objelerinin sayısı arttıkça scale değerleri küçüleceğinden dolayı margin değerinin azaltılması.
        margin = 2f - newMargin;
    }

    #region Get Methods
    public GameObject[] getAllCircles()    // Sahnedeki tüm circle oyun objelerini getirir.
    {
        return GameObject.FindGameObjectsWithTag("Circle");
    }
    public int getWidth()
    {
        return width;
    }
    public int getHeight()
    {
        return height;
    }

    #endregion
}
