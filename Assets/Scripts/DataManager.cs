using UnityEngine;
using System.IO;
using TMPro;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public int highScore;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        // File.Delete(Application.persistentDataPath + "/bestscorefile.json"); // This line can be activated, If you want to delete save file.
    }
    [System.Serializable]
    class SaveData
    {
        public int highScore;
    }
    public void Save()
    {
        SaveData data = new SaveData();
        data.highScore = highScore;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/bestscorefile.json", json);
    }
    public void Load()
    {
        string path = Application.persistentDataPath + "/bestscorefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            highScore = data.highScore;
        }
    }
}