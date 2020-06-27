using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataManager : MonoBehaviour
{
    [Serializable]
    public class JsonDataClass
    {
        public int id=new int();
        public Color32 circleColor=new Color32();
        public Vector2 coordinate=new Vector2();
    }
    [Serializable]
    public class JsonDataListClass
    {
        public int circleCount;
        public int width;
        public int height;
        public List<JsonDataClass> circleList=new List<JsonDataClass>();
    }
    [SerializeField]
    private Text   saveTextField;
    private string savingText="Saving..", savedText = "Saved!", saveFailedText="An error occured.";

    private GridScript gridScript; // Sahnedeki circle objelerinin bilgilerini almak için kullanıldı.
        void Start()
    {
        gridScript = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridScript>();
    }

    void Update()
    {
        
    }
    public void save()
    {
        showSaveText(savingText);
        JsonDataListClass jsonDataList = new JsonDataListClass();
        GameObject[] circleList = gridScript.getAllCircles();
        int circleCount = circleList.Length;
        jsonDataList.circleCount = circleCount;
        jsonDataList.width = gridScript.getWidth();
        jsonDataList.height = gridScript.getHeight();
        try
        {
            for (int i = 0; i < circleCount; i++)                               //Herbir circle objesinin bilgilerinin listeye yazılması.
            {
                JsonDataClass jsonData = new JsonDataClass();
                jsonData.id = i + 1;
                jsonData.circleColor = circleList[i].GetComponent<CircleManager>().getCircleColor();
                jsonData.coordinate = circleList[i].GetComponent<CircleManager>().getCoordinate();
                jsonDataList.circleList.Add(jsonData);
            }

            string jsonString = JsonUtility.ToJson(jsonDataList);
            jsonString = JsonHelper.FormatJson(jsonString); // JSON dosyasını okunabilirliği arttırmak amacıyla formatlı hale getirir.
            File.WriteAllText(Application.dataPath + "/Outputs/circles.json", jsonString); //Circle objelerinin bilgilerini JSON formatında Assets klasörü altındaki Outputs klasörüne yazar.
            showSaveText(savedText);
        }
        catch (Exception ex)
        {
            showSaveText(saveFailedText);
            Debug.LogError(ex.ToString());
        }
    }

    private void showSaveText(string showingText)   //Save text alanını görünür hale getirir.
    {
        saveTextField.text = showingText;
        saveTextField.enabled = true;
    }

    public void makeInvisibleSaveText()     //Save text alanını görünmez hale getirir.
    {
        saveTextField.enabled = false;
    }
}
