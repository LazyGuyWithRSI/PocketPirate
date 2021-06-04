using Google.Play.AppUpdate;
using Google.Play.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateChecker : MonoBehaviour
{
    // Start is called before the first frame update
    AppUpdateManager appUpdateManager;
    void Start()
    {
        appUpdateManager = new AppUpdateManager();
        StartCoroutine(CheckForUpdate());
    }

    IEnumerator CheckForUpdate()
    {
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
          appUpdateManager.GetAppUpdateInfo();

        // Wait until the asynchronous operation completes.
        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
            // Check AppUpdateInfo's UpdateAvailability, UpdatePriority,
            // IsUpdateTypeAllowed(), etc. and decide whether to ask the user
            // to start an in-app update.

            var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();

            // Creates an AppUpdateRequest that can be used to monitor the
            // requested in-app update flow.
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
