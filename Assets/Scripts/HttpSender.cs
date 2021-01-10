using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpSender : MonoBehaviour {

    [Tooltip("Link HttpSenderManager")]
    public HttpSenderManager manager;

    [Tooltip("Sub url of the app. Example: /account/register")]
    public string endpoint = "";
    
    public void SendPost(string getUrl, Dictionary<string,string> postParams, Action<string> callback) {
        StartCoroutine(_Upload(getUrl, postParams, callback));
    }

    private string _EndpointUrl() {
        return manager.HomeURL() + this.endpoint;
    }

    private IEnumerator _Upload(string getUrl, Dictionary<string,string> postParams, Action<string> callback) {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
        //WWWForm form = new WWWForm();

        foreach (KeyValuePair<string,string> param in postParams) {
            formData.Add(new MultipartFormDataSection(param.Key, param.Value));
            //form.AddField(param.Key, param.Value);
        }

        //formData.Add(new MultipartFormDataSection("test_key", "test_value"));
        //form.AddField("test_key", "test_value");
        //getUrl = "?gettest=yo";

        string fullUrl = this._EndpointUrl() + getUrl;
        UnityWebRequest www = UnityWebRequest.Post(fullUrl, formData);
        //UnityWebRequest www = UnityWebRequest.Post(fullUrl, form);
        yield return www.SendWebRequest();

        string response = www.downloadHandler.text;
        //string response = www.downloadHandler.data;

        if (www.isNetworkError || www.isHttpError) {
            //Debug.Log(www.error);
        } else {
            //Debug.Log("Form upload complete!");
        }
        callback(response);
    }

}
