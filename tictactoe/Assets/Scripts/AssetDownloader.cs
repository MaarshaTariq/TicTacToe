using UnityEngine.UI;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System.Linq.Expressions;

public class AssetDownloader : MonoBehaviour
{
    public string TestJsonLink;
    public string JsonBinKey;
    public BackOfficeData backOfficeData;
    public static AssetDownloader instance;
    [HideInInspector]
    public string jsonData;
  
    public List<Sprite> DownloadedAssets = new List<Sprite>();
    public GameObject LoadingScreen;
    public GameObject OpeningScreen;

    public bool IsTest = false;
    private void Awake()
    {
        instance = this;
        Debug.unityLogger.logEnabled = false;
    }

 
    void Start()
    {
        if (IsTest)
        {
            LoadingScreen.SetActive(true);
            StartCoroutine(GetJsonData());
        }
    }

    public IEnumerator DownloadRelatedAssets()
    {
        for (int i = 0; i < backOfficeData.gameContent.Length; i++)
        {
            WWW W = new WWW(backOfficeData.gameContent[i].image_url);
            yield return W;
            Texture2D tex = W.texture;
            Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            mySprite.name = (i+1).ToString();
            DownloadedAssets.Add(mySprite);
        }

        PlayerMarkerFetch.Instance.MarkerImages = new Sprite[DownloadedAssets.Count];
        for (int i = 0; i < DownloadedAssets.Count; i++)
        {
            PlayerMarkerFetch.Instance.MarkerImages[i] = DownloadedAssets[i];
        }

        LoadingScreen.SetActive(false);
        OpeningScreen.SetActive(true);
    }

    public void GetDataFromTempJson()
    {
        RestAPIHandler.Instance.StartCoroutine(RestAPIHandler.Instance.GetRequest(TestJsonLink, JsonBinKey));
    }

    public IEnumerator GetJsonData()
    {
        yield return new WaitForSeconds(1);

        if (External.urlFromServer == null)
        {
               GetDataFromTempJson();

            yield return new WaitUntil(() => RestAPIHandler.Instance.DownloadedSuccessfully);
            backOfficeData = JsonUtility.FromJson<BackOfficeData>(jsonData);
            StartCoroutine(DownloadRelatedAssets());
        }
        else
        {
            string jsonUrl = External.urlFromServer;
            RestAPIHandler.Instance.StartCoroutine(RestAPIHandler.Instance.GetRequestWithoutKey(jsonUrl));
            yield return new WaitUntil(() => RestAPIHandler.Instance.DownloadedSuccessfully);
            backOfficeData = JsonUtility.FromJson<BackOfficeData>(jsonData);
            StartCoroutine(DownloadRelatedAssets());
        }
    }

    [Serializable]
    public class GameContent
    {
        public string contentText;
        public string image_url;
        public string contentType;
        public string createdBy;
        public Time createdDate;
        public string fileName;
        public int gameContentId;
        public string gameSubType;
        public string imageUrl;
        public int issueId;
        public string modifiedBy;
        public Time modifiedDate;
        public string newspaperGameId;
    }

    [Serializable]
    public class BackOfficeData
    {
        public GameContent[] gameContent;
        public string[] colors;
        public string[] shapes;
        public string[] images;
        public string[] numbers;
        public string[] letters;

        public string createdBy;
        public Time createdDate;

        public string displayName;
        public string name;
        public int issueGameId;
        public int issueId;

        public string modifiedBy;
        public Time modifiedDate;

    }
}
