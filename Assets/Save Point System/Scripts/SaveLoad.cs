using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoad
{
    private static string path = Application.persistentDataPath + "/saves/";
    private static string extenstion = ".koalas";

    public static void Save<T>(T objToSave, string directory, string key)
    {
        string modPath = path + directory;
        Directory.CreateDirectory(modPath);
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream fileStream = new FileStream(modPath + key + extenstion, FileMode.Create))
        {
            formatter.Serialize(fileStream, objToSave);
            string name = objToSave.GetType().Name;
        }

    }

    public static T Load<T>(string directory, string key)
    {
        string modPath = path + directory;
        Directory.CreateDirectory(modPath);
        BinaryFormatter formatter = new BinaryFormatter();
        T returnValue = default(T);
        using (FileStream fileStream = new FileStream(modPath + key + extenstion, FileMode.Open))
        {
            string name = fileStream.GetType().Name;
            returnValue = (T)formatter.Deserialize(fileStream);
        }

        return returnValue;

    }

    public static bool SaveExists(string directory, string key)
    {
        string modPath = path + directory + key + extenstion;

        return File.Exists(modPath);
    }

    public static void EraseAllFiles()
    {
        DirectoryInfo directory = new DirectoryInfo(path);
        directory.Delete(true);
        Directory.CreateDirectory(path);
    }

}

public static class Directories
{
    public const string PlayerStats = "/PlayerStats/";
}