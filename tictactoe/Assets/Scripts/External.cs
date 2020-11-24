using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class External : MonoBehaviour
{
    #region Previous External Logic
    public GameObject assetDownloader;
    public static string urlFromServer;
    [HideInInspector]
    public bool IssueIDReceived = false;
    string BaseAPI = "api/lockerApi/getGameData?gameId=tictactoe&issueId=";
    string CurrentIssueID = "";
  
    public void DisableFullScreen()
    {
        PlayerPrefs.SetInt("DisableFullScreen", 1);
    }

    public void PlayUnityScene()
    {
        Manager.StartCoroutine(Manager.LoadScene());
    }

    #endregion
    public Text CopyrightTextRef;
    public string CopyrightText;
    public static External Instance;
    public GameManager Manager;
    
    string baseURL;
    [HideInInspector]
    public string BaseUrl
    {
        get { return baseURL; }
        set
        {
            baseURL = value;
        }
    }

    public void AssignCopyright()
    {
        CopyrightTextRef.text = CopyrightText;
    }

    void OnEnable()
    {
        Instance = this;
        AssignCopyright();
        StartCoroutine(GetManagerAfter(1f));
    }

    IEnumerator GetManagerAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(!Manager)
            Manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    
    }
    public void GetBaseUrl(string uRL)
    {
        Debug.Log("The Base Url from server is : " + uRL);
        BaseUrl = uRL;
    }

    public void GetIssueID(string issueID)
    {
        if (!IssueIDReceived)
        {
            IssueIDReceived = true;
            Debug.Log("The issueID from server is : " + issueID + " new version v2");
            CurrentIssueID = issueID;
            urlFromServer = BaseUrl + BaseAPI + CurrentIssueID;
            Debug.Log("complete URL : " + urlFromServer);
            Invoke("StartDownloadWithDelay", 1.4f);
        }
    }

    public void StartDownloadWithDelay()
    {
        StartCoroutine(assetDownloader.GetComponent<AssetDownloader>().GetJsonData());
    }

}


