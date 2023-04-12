using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Savingandloadingfurniture 
{
    public static void SaveFurniture(Statystykimebla statystykimebla)
    {
        string pathname = statystykimebla.gameobjectstring;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + pathname + ".furniture";
        FileStream stream = new FileStream(path, FileMode.Create);
        Classforsavingdatafurnitures data = new Classforsavingdatafurnitures(statystykimebla);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static Classforsavingdatafurnitures LoadFurniture(Statystykimebla statystykimebla)
    {
        string pathname = statystykimebla.gameobjectstring;
        string path = Application.persistentDataPath + "/" + pathname + ".furniture";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Classforsavingdatafurnitures data = formatter.Deserialize(stream) as Classforsavingdatafurnitures;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
