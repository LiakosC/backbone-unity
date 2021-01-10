using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClient : MonoBehaviour {

    [Tooltip("Server domain to request. Ex: https://www.google.com")]
    public string domain = "http://192.168.1.114/fblight.devv";

    [Tooltip("Sub url of the app. Example: /account/register. Use it in case you need more than 1 HttpClient in the app.")]
    public string endpoint = "";
    
    // sends a post request to a url
    public void SendPost(string url, Dictionary<string,string> postParams, Action<string> callback) {
        StartCoroutine(_Upload_Post(url, postParams, callback));
    }

    // sends a get request to a url
    public void SendGet(string url, Dictionary<string,string> getParams, Action<string> callback) {
        StartCoroutine(_Upload_Get(url, getParams, callback));
    }

    // sums up domain + endpoint strings.
    private string _EndpointUrl() {
        return this.domain + this.endpoint;
    }

    private IEnumerator _Upload_Post(string url, Dictionary<string,string> postParams, Action<string> callback) {
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
        WWWForm form = new WWWForm();

        //form.AddField("test", "");
        foreach (KeyValuePair<string,string> param in postParams) { // scan through the dict params
            //Debug.Log(param.Key + " | " + param.Value);
            //formData.Add(new MultipartFormDataSection(param.Key, param.Value));
            form.AddField(param.Key, param.Value); // insert the pair to the form
        }

        //formData.Add(new MultipartFormDataSection("test_key", "test_value"));
        //form.AddField("test_key", "test_value");

        string fullUrl = this._EndpointUrl() + url;
        //UnityWebRequest www = UnityWebRequest.Post(fullUrl, formData);
        UnityWebRequest www = UnityWebRequest.Post(fullUrl, form);
        yield return www.SendWebRequest();
        string response = www.downloadHandler.text;
        if (www.isNetworkError || www.isHttpError) {
            // www.error
        } else {
            // ok
        }
        callback(response);
    }

    private IEnumerator _Upload_Get(string url, Dictionary<string,string> getParams, Action<string> callback) {
        string fullUrl = this._EndpointUrl() + url;
        string getQuery = this.GetQuery(getParams);
        UnityWebRequest www = UnityWebRequest.Get(fullUrl + "?" + getQuery);
        yield return www.SendWebRequest();
        string response = www.downloadHandler.text;
        if (www.isNetworkError || www.isHttpError) {
            // www.error
        } else {
            // ok
        }
        callback(response);
    }

    public string GetQuery(Dictionary<string,string> getParams) {
        string queryString = "";
        foreach (KeyValuePair<string,string> param in getParams) {
            string part = param.Key + "=" + param.Value; // "getparam=123"
            queryString += part + "&"; // "getparam=123&getparam2=234&"
        }
        queryString = queryString.Substring(0, queryString.Length - 1); // remove last character "&" 
        return queryString;
    }

}
