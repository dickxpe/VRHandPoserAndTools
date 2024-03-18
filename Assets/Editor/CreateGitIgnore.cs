using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
public class MenuTest : MonoBehaviour
{
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("Assets/Create/GitIgnore")]
    internal static async Task CreateGitIgnore()
    {
        UnityWebRequest.ClearCookieCache();


        UnityWebRequest request = UnityWebRequest.Get("https://raw.githubusercontent.com/github/gitignore/main/Unity.gitignore");
        UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();

        // Catch the event as it's own method
        asyncOperation.completed += DownloadAndWriteFile;

        while (!asyncOperation.isDone)
        {
            await Task.Delay(100);
        }
        Debug.Log(".gitignore has been created");


    }

    private static void DownloadAndWriteFile(AsyncOperation obj)
    {
        obj.completed -= DownloadAndWriteFile;

        // Cast it back to a request
        UnityWebRequestAsyncOperation asyncRequestObj = (UnityWebRequestAsyncOperation)obj;
        UnityWebRequest request = asyncRequestObj.webRequest;
        System.IO.File.WriteAllText(".gitignore", request.downloadHandler.text);
    }
}