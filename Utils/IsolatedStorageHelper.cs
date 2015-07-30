using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace FreeApp.Utils
{
    public static class IsolatedStorageHelper
    {
        public static T GetPrimitive<T>(string key)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
                return (T)IsolatedStorageSettings.ApplicationSettings[key];
            return default(T);
        }

        public static void SavePrimitive<T>(string key, T toSave)
        {
            IsolatedStorageSettings.ApplicationSettings[key] = (object)toSave;
        }

        public static T GetObject<T>(string key)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
                return IsolatedStorageHelper.Deserialize<T>(IsolatedStorageSettings.ApplicationSettings[key].ToString());
            return default(T);
        }

        public static void SaveObject<T>(string key, T objectToSave)
        {
            string str = IsolatedStorageHelper.Serialize((object)objectToSave);
            IsolatedStorageSettings.ApplicationSettings[key] = (object)str;
        }

        public static bool ContainsKey(string key)
        {
            return IsolatedStorageSettings.ApplicationSettings.Contains(key);
        }

        public static void DeleteObject(string key)
        {
            IsolatedStorageSettings.ApplicationSettings.Remove(key);
        }

        public static string Serialize(object objectToSerialize)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                new DataContractJsonSerializer(objectToSerialize.GetType()).WriteObject((Stream)memoryStream, objectToSerialize);
                memoryStream.Position = 0L;
                using (StreamReader streamReader = new StreamReader((Stream)memoryStream))
                    return streamReader.ReadToEnd();
            }
        }

        public static T Deserialize<T>(string jsonString)
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject((Stream)memoryStream);
        }
    }
}
