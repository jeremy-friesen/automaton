using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveSystem{
    public static void Save(PlayerData data){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.s";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();

        //Debug.Log("Data Saved");
    }

    public static PlayerData Load(){
        //Debug.Log(Application.persistentDataPath);
        string path = Application.persistentDataPath + "/player.s";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            // save file updating
            //Save(data);

            //Debug.Log("Data Loaded:\nMax Level: " + data.MaxLevel + "\nLast Level: " + data.LastLevel);

            return data;
        }else{
            //Debug.Log("No save file found, creating new.");
            return new PlayerData(-1, 0);
        }
    }
}
