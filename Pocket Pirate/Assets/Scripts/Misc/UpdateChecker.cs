using Google.Play.AppUpdate;
using Google.Play.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UpdateChecker : MonoBehaviour
{
    public StringReference VersionReference;
    public Button UpdateButton;

    AppUpdateManager appUpdateManager;
    private const string UpdateVersionURL = "https://lazyguywithrsi.github.io/";
    private const string StoreURL = "https://play.google.com/store/apps/details?id=com.JustSomeDev.PocketPirate";

    void Start()
    {
        //Debug.Log("MY TEST - Update Checker checking update");
        //appUpdateManager = new AppUpdateManager();
        //StartCoroutine(CheckForUpdate());

        StartCoroutine(GetRequest(UpdateVersionURL));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    string versionText = webRequest.downloadHandler.text;
                    int newestVersion = convertVersionToInt(versionText);
                    int currentVersion = convertVersionToInt(VersionReference.Value);
                    Debug.Log("new version: " + newestVersion + ", current version: " + currentVersion);
                    if (currentVersion < newestVersion)
                    {
                        UpdateButton.gameObject.SetActive(true);
                        UpdateButton.onClick.AddListener(OnUpdateButtonClick);
                    }

                    break;
            }
        }
    }

    private int convertVersionToInt(string version)
    {
        string[] splitText = version.Split('.');
        int newestVersion = 0;
        newestVersion += 10000 * int.Parse(splitText[0]);
        newestVersion += 100 * int.Parse(splitText[1]);
        newestVersion += 1 * int.Parse(splitText[2]);
        return newestVersion;
    }

    void OnUpdateButtonClick()
    {
        Application.OpenURL(StoreURL);
    }

    IEnumerator CheckForUpdate()
    {
        Debug.Log("MY TEST - In CheckForUpdate");

        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
          appUpdateManager.GetAppUpdateInfo();

        // Wait until the asynchronous operation completes.
        yield return appUpdateInfoOperation;
        Debug.Log("MY TEST - Update info success? " + appUpdateInfoOperation.IsSuccessful);
        if (appUpdateInfoOperation.IsSuccessful)
        {
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
            // Check AppUpdateInfo's UpdateAvailability, UpdatePriority,
            // IsUpdateTypeAllowed(), etc. and decide whether to ask the user
            // to start an in-app update.

            var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();

            // Creates an AppUpdateRequest that can be used to monitor the
            // requested in-app update flow.
            Debug.Log("MY TEST - Update available? " + appUpdateInfoResult.UpdateAvailability);
            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                var startUpdateRequest = appUpdateManager.StartUpdate(
                  // The result returned by PlayAsyncOperation.GetResult().
                  appUpdateInfoResult,
                  // The AppUpdateOptions created defining the requested in-app update
                  // and its parameters.
                  appUpdateOptions);
                yield return startUpdateRequest;

                // If the update completes successfully, then the app restarts and this line
                // is never reached. If this line is reached, then handle the failure (for
                // example, by logging result.Error or by displaying a message to the user).
            }
        }
        else
        {
            // Log appUpdateInfoOperation.Error.
        }
    }
}
