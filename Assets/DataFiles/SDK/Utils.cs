using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace sdk
{
    class Utils
    {
        public static byte[] GetStringtoBytes(string str)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            return bytes;
        }

        public static string GetByteToString(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        public static T fromJsonToObject<T>(T obj, string jsonObject)
        {
            return JsonUtility.FromJson<T>(jsonObject);
        }

        public static string fromObjectToJSON<T>(T obj)
        {
            return JsonUtility.ToJson(obj);
        }
        public static void setHeaders(LoginResponseModel loginResponseModelObject)
        {
            PlayerPrefs.SetString("X-I2CE-API-KEY", loginResponseModelObject.api_key);
            PlayerPrefs.SetString("X-I2CE-USER-ID", loginResponseModelObject.user_id);
            PlayerPrefs.SetString("X-I2CE-ENTERPRISE-ID", loginResponseModelObject.enterprise_id);
        }

        public static Dictionary<string, string> getHeadersAsDictionary()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("X-I2CE-API-KEY", PlayerPrefs.GetString("X-I2CE-API-KEY"));
            headers.Add("X-I2CE-USER-ID", PlayerPrefs.GetString("X-I2CE-USER-ID"));
            headers.Add("X-I2CE-ENTERPRISE-ID", PlayerPrefs.GetString("X-I2CE-ENTERPRISE-ID"));
            if (headers.Count == 0)
            {
                Debug.LogWarning("No Headers found");
            }
            return headers;
        }
        public static bool isLoggedIn(string enterprise_id, string user_id)
        {
            if (
                PlayerPrefs.GetString("X-I2CE-ENTERPRISE-ID").Equals(enterprise_id) &&
                PlayerPrefs.GetString("X-I2CE-USER-ID").Equals(user_id) &&
                PlayerPrefs.GetString("X-I2CE-API-KEY").Length == 0
                )
            {
                return true;
            }
            return false;
        }

        string url;
        string filename;



        public string DownloadFileInline(string url)
        {

            if (PlayerPrefs.GetString(url).Length != 0)
            {
                return PlayerPrefs.GetString(url);
            }
            else
            {
                if (url.StartsWith("http://") || url.StartsWith("https://"))
                {

                    WebClient client = new WebClient();
                    Uri uri = new Uri(url);

                    filename = Application.persistentDataPath + "/" + Path.GetFileName(uri.AbsolutePath);//Regex.Replace(url, @"[^0-9a-zA-Z]+", "");
                    Debug.Log("filePath = " + filename);
                    client.DownloadFile(new Uri(url), filename);
                    return filename;
                }
                else
                {
                    return url;
                }
            }
        }
        public IEnumerator GetAssetBundle(string assetBundlePath, Action<GameObject> OnObjectLoaded, string audioPath)
        {
            Debug.Log("called enum "+assetBundlePath);
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(assetBundlePath);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadedBytes);
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                foreach (string go in bundle.GetAllAssetNames())
                {
                    Debug.Log("asdlkjfalskdjfh   =   " + go);
                }
                GameObject newGameObject = bundle.LoadAsset<GameObject>("dave");
                Debug.Log("name = " + newGameObject.name);
                
                newGameObject.GetComponent<Animation>().playAutomatically = false;
                Animation[] a = newGameObject.GetComponentsInChildren<Animation>();
                Animator animator = newGameObject.AddComponent<Animator>();
                AnimationClip aniClip = a[0].GetClip("Armature.002|Armature.002Action");
                animator.runtimeAnimatorController = Resources.Load("DefaultAnimator") as RuntimeAnimatorController;

                

                using (UnityWebRequest www1 = UnityWebRequestMultimedia.GetAudioClip(audioPath, AudioType.WAV))
                {
                    yield return www1.SendWebRequest();

                    if (www1.isNetworkError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        AudioClip myClip = DownloadHandlerAudioClip.GetContent(www1);
                        AudioSource asource = newGameObject.AddComponent<AudioSource>();
                        asource.clip = myClip;
                    }
                }
                // put some parameters on the AnimationEvent
                //  - call the function called PrintEvent()
                //  - the animation on this object lasts 2 seconds
                //    and the new animation created here is
                //    set up to happen 1.3s into the animation

                OnObjectLoaded(newGameObject);

            }
        }
        public IEnumerator GetAndPlayAudio(string audioPath, GameObject gameObject)
        {
            Debug.Log("trying to play audio = " + audioPath);
            using (UnityWebRequest www1 = UnityWebRequestMultimedia.GetAudioClip(audioPath, AudioType.WAV))
            {
                yield return www1.SendWebRequest();

                if (www1.isNetworkError)
                {
                    Debug.Log(www1.error);
                }
                else
                {
                    AudioClip myClip = DownloadHandlerAudioClip.GetContent(www1);
                    AudioSource asource = gameObject.AddComponent<AudioSource>();
                    asource.clip = myClip;
                    talk = true;
                    asource.Play();
                }
            }
        }
        public static bool talk = false;


        static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
        static char[] TRIM_CHARS = { '\"' };

        public static List<Dictionary<string, object>> Read(string file)
        {
            var list = new List<Dictionary<string, object>>();/*
            TextAsset data = Resources.Load(file) as TextAsset;*/
            StreamReader reader = new StreamReader(file);
            string str = reader.ReadToEnd();
            reader.Close();
            var lines = Regex.Split(str, LINE_SPLIT_RE);

            if (lines.Length <= 1) return list;

            string[] header = { "time", "state" };
            for (var i = 0; i < lines.Length; i++)
            {

                var values = Regex.Split(lines[i], SPLIT_RE);
                if (values.Length == 0 || values[0] == "") continue;

                var entry = new Dictionary<string, object>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                    object finalvalue = value;
                    int n;
                    float f;
                    if (int.TryParse(value, out n))
                    {
                        finalvalue = n;
                    }
                    else if (float.TryParse(value, out f))
                    {
                        finalvalue = f;
                    }
                    entry[header[j]] = finalvalue;
                }
                list.Add(entry);
            }
            return list;
        }
    }

   
}