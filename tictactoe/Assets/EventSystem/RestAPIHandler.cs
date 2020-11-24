using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RestAPIHandler : MonoBehaviour {

	public static RestAPIHandler Instance;
	[HideInInspector]
	public bool UploadedSuccessfully=false;
	[HideInInspector]
	public bool DownloadedSuccessfully = false;

	void Start () {
		Instance = this;
	}

	public IEnumerator PostRequest(string url, string json)
	{
		var uwr = new UnityWebRequest(url, "POST");
		byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
		uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
		uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		uwr.SetRequestHeader("Content-Type", "application/json");

		//Send the request then wait here until it returns
		Debug.Log("waiting");
		yield return uwr.SendWebRequest();
		UploadedSuccessfully = true;
		if (uwr.isNetworkError)
		{
			Debug.Log("Error While Sending: " + uwr.error);
		}
		else
		{
			Debug.Log("Received: " + uwr.downloadHandler.text);
		}
	}

	public IEnumerator GetRequest(string _url,string _key)
	{
		UnityWebRequest uwr = UnityWebRequest.Get(_url);
		uwr.SetRequestHeader("Content-Type", "application/json");
		uwr.SetRequestHeader("secret-key", _key);
		
		//Send the request then wait here until it returns
		Debug.Log("waiting");
		yield return uwr.SendWebRequest();
		if (uwr.isNetworkError)
		{
			Debug.Log("Error While Sending: " + uwr.error);
		}
		else
		{
			DownloadedSuccessfully = true;
			AssetDownloader.instance.jsonData = uwr.downloadHandler.text;
			Debug.Log("Received: " + uwr.downloadHandler.text);
		}
	}

	public IEnumerator GetRequestWithoutKey(string _url)
	{
		UnityWebRequest uwr = UnityWebRequest.Get(_url);
		uwr.SetRequestHeader("Content-Type", "application/json");

		//Send the request then wait here until it returns
		Debug.Log("waiting");
		yield return uwr.SendWebRequest();
		if (uwr.isNetworkError)
		{
			Debug.Log("Error While Sending: " + uwr.error);
		}
		else
		{
			DownloadedSuccessfully = true;
			AssetDownloader.instance.jsonData = uwr.downloadHandler.text;
			Debug.Log("Received: " + uwr.downloadHandler.text);
		}
	}
}
