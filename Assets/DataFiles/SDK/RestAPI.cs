using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace sdk
{
    class RestAPI
    {
        string json = "{" +
                                    "\"enterprise_id\":\"virtual_mirror\"," +
                                    "\"password\":\"123456\"," +
                                    "\"user_id\":\"soham+vm@i2ce.in\"" +
                                    "}";
        public IEnumerator Login(string user_id, string enterprise_id, string password, Action<LoginResponseModel> callback = null)
        {
            LoginDataModel loginDataModel = new LoginDataModel(user_id, enterprise_id, password);
            //LoginDataModel loginDataModel = Utils.fromJsonToObject<LoginDataModel>(new LoginDataModel(),json);
            string jsonBody = Utils.fromObjectToJSON<LoginDataModel>(loginDataModel);
            Debug.Log("posting with body = " + jsonBody);
            yield return MakeRequest<LoginResponseModel>(callback,"https://test.iamdave.ai/login", "POST", jsonBody, new Dictionary<string, string>(),true);
        }



        public IEnumerator PostConversation<T>(T conversationRequestModelObject, string conversationName, string customerId,Action<ConversationResponseModel> callback = null)
        {
            //LoginDataModel loginDataModel = Utils.fromJsonToObject<LoginDataModel>(new LoginDataModel(),json);
            string jsonBody = "{}";
            if(conversationRequestModelObject != null)
            {
                jsonBody = Utils.fromObjectToJSON<T>(conversationRequestModelObject);
            }
            Debug.Log("posting with body = " + jsonBody);
            yield return MakeRequest<ConversationResponseModel>(callback, "https://test.iamdave.ai/conversation/"+conversationName+"/"+customerId, "POST", jsonBody, Utils.getHeadersAsDictionary());
        }

        public IEnumerator SendWAV(byte[] bytes,string conversationName, string customerId,ConversationRequestModel conversationRequestModelObject)
        {
            Dictionary<string,string> headers = Utils.getHeadersAsDictionary();
            /*validation to find file*/
            //read in file to bytes
            byte[] img = bytes;

            //create multipart list
            List<IMultipartFormSection> requestData = new List<IMultipartFormSection>();
            requestData.Add(new MultipartFormFileSection("file", img,"audio.wav", "audio/wav"));

            string formData = "system_response=" + conversationRequestModelObject.system_response + "&engagement_id=" + conversationRequestModelObject.engagement_id;

            requestData.Add(new MultipartFormDataSection(formData));
            
            //url3 is a string url defined earlier
            UnityWebRequest request = UnityWebRequest.Post("https://test.iamdave.ai/conversation/" + conversationName + "/" + customerId, requestData);
            request.method = "POST";
            //request.SetRequestHeader("Content-Type", "multipart/form-data");

            request.SetRequestHeader("content-type", "multipart/form-data");
            
            foreach (KeyValuePair<string, string> entry in headers)
            {
                Debug.Log("adding header : " + entry.Key + " : " + entry.Value);
                request.SetRequestHeader(entry.Key, entry.Value);
            }

            yield return request.SendWebRequest();
            Debug.Log("uploaded bytes ===== " + request.uploadedBytes);
            Debug.Log("request completed with code: " + request.responseCode);

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                Debug.Log("Request Response: " + request.downloadHandler.text);
            }
        }


        IEnumerator MakeRequest<T>(Action<T> callback = null,string url = null,string method = null,string jsonBody = null, Dictionary<string,string> headers = null,bool isLogin = false)
        {
            string response = "";
            UnityWebRequest www;
            switch (method)
            {
                case "POST":
                    byte[] bytes = Utils.GetStringtoBytes(jsonBody);
                    www = UnityWebRequest.Put(url, bytes);

                    www.SetRequestHeader("content-type", "application/json; charset=UTF-8");
                    www.method = "POST";
                    break;
                case "PATCH":
                case "PUT":
                    byte[] patchBytes = Utils.GetStringtoBytes(jsonBody);
                    www = UnityWebRequest.Put(url, patchBytes);

                    www.SetRequestHeader("accept", "application/json; charset=UTF-8");
                    www.SetRequestHeader("content-type", "application/json; charset=UTF-8");
                    break;
                case "GET":
                    // Defaults are fine for GET
                    www = UnityWebRequest.Get(url);
                    break;

                case "DELETE":
                    // Defaults are fine for DELETE
                    www = UnityWebRequest.Delete(url);
                    break;
              
                default:
                    throw new Exception("Invalid HTTP Method");
            }
            //adding headers for all apis except for login
            if (!isLogin)
            {
                foreach(KeyValuePair<string, string> entry in headers)
                {
                    Debug.Log("adding header : " + entry.Key + " : " + entry.Value);
                    www.SetRequestHeader(entry.Key, entry.Value);
                }
            }
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.uploadedBytes);
                Debug.Log(Utils.GetByteToString(www.downloadHandler.data));
            }
            else
            {

                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
                Debug.Log("Form upload complete!   " + results.Length);
                string converted = Utils.GetByteToString(results);
                Debug.Log(converted);
                if (callback != null)
                {
                    //callback(converted);
                    T obj = (T)Activator.CreateInstance(typeof(T));
                    T objRet = Utils.fromJsonToObject<T>(obj, converted);
                    if (isLogin)
                    {
                        LoginResponseModel loginResponseModelObject = ((LoginResponseModel)(object)objRet);
                        Utils.setHeaders(loginResponseModelObject);
                    }
                    callback(objRet);
                   
                }
            }

            
        }
    }
}
