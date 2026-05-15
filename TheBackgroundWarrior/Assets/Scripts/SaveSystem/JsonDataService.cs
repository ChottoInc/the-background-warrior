using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// "com.unity.nuget.newtonsoft-json": "3.0.2",

public class JsonDataService : IDataService
{
    private const string ENC_KEY = "lOU4bNn7TJyCLHpiW6PFu7Hj55mG39F7Eb5bWujFWcs=";
    private const string ENC_IV = "qObiuxvu1UWiS+QrjC8aJQ==";

    public bool SaveData<T>(string relativePath, T data, bool encrypted)
    {
        string path = Application.persistentDataPath + "/" + relativePath;

        try
        {
            if (File.Exists(path))
            {
                //Debug.Log("File exists, removing and adding again");
                File.Delete(path);
            }
            else
            {
                //Debug.Log("Creating new file");
            }

            using FileStream stream = File.Create(path);

            if(encrypted)
            {
                WriteEncryptedData(data, stream);
            }
            else
            {
                stream.Close();

                File.WriteAllText(path, JsonConvert.SerializeObject(data));
            }
           
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError($"Unable to save due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    private void WriteEncryptedData<T>(T data, FileStream stream)
    {
        using Aes aesProvider = Aes.Create();
        aesProvider.Key = Convert.FromBase64String(ENC_KEY);
        aesProvider.IV = Convert.FromBase64String(ENC_IV);

        //Debug.Log("Start key: " + Convert.ToBase64String(aesProvider.Key));
        //Debug.Log("Start iv: " + Convert.ToBase64String(aesProvider.IV));

        using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
        using CryptoStream cryptoStream = new CryptoStream(
            stream,
            cryptoTransform,
            CryptoStreamMode.Write
        );

        cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));
    }

    public T LoadData<T>(string relativePath, bool encrypted)
    {
        string path = Application.persistentDataPath + "/" + relativePath;

        if (!File.Exists(path))
        {
            Debug.LogError($"Cannot load file at {path}");
            throw new FileNotFoundException($"{path} does not exists");
        }

        try
        {
            T data;

            if (encrypted)
            {
                data = ReadEncryptedData<T>(path);
            }
            else
            {
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            
            return data;
        }
        catch(Exception e)
        {
            Debug.LogError($"Failed to load due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }

    private T ReadEncryptedData<T>(string path)
    {
        byte[] fileBytes = File.ReadAllBytes(path);

        using Aes aesProvider = Aes.Create();
        aesProvider.Key = Convert.FromBase64String(ENC_KEY);
        aesProvider.IV = Convert.FromBase64String(ENC_IV);

        //Debug.Log("Start key: " + Convert.ToBase64String(aesProvider.Key));
        //Debug.Log("Start iv: " + Convert.ToBase64String(aesProvider.IV));

        using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(
            aesProvider.Key,
            aesProvider.IV
        );

        using MemoryStream decryptionStream = new MemoryStream(fileBytes);

        using CryptoStream cryptoStream = new CryptoStream(
           decryptionStream,
           cryptoTransform,
           CryptoStreamMode.Read
       );

        using StreamReader reader = new StreamReader(cryptoStream);

        string result = reader.ReadToEnd();

        //Debug.Log($"Decrypted result (if the following is not legible, probably wrong key or IV): {result}");
        return JsonConvert.DeserializeObject<T>(result);
    }

    
}