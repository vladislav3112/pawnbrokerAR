using System;
using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;
public class LoadAssetBundle : MonoBehaviour
{
    private string BundleURL;
    public string AssetName;
    string toastString;
    private short[] code = { 0, 0, 0 };//code is downloaded
    private int version;
    AndroidJavaObject currentActivity;
    // specify these in Unity Inspector

    public void show(string toastString)
    {
        //
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        //
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(showToast));
    }
    private void showToast()
    {
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", toastString);

        //
        AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
        toast.Call("show");
    }
    public void OnClickDownload()
    {
        StartCoroutine(DownloadAndCache());

    }

    IEnumerator DownloadAndCache()
    {
        BundleURL = "ftp://89.208.87.29/" + AssetName + ".unity3d";
        // Wait for the Caching system to be ready
        while (!Caching.ready)
            yield return null;
        
        // Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
        using (WWW www = WWW.LoadFromCacheOrDownload(BundleURL, version))
        {
            yield return www;
            if (www.error != null)
            {
                show("WWW download had an error: " + www.error);
                throw new Exception("WWW download had an error:" + www.error);
            }
            AssetBundle bundle = www.assetBundle;
            if (AssetName == "")
            {
                show("empty asset bundle name:");
                throw new Exception("empty asset bundle name:");
            }
            else
            {   //download all prefabs 
                show("no problems with download:");
                if (code[0] == 0 && AssetName == "pawnbroker")
                {// && GameObject.Find("pawnbroker1_target")== null
                 //var prefab = bundle.LoadAsset<GameObject>("pawnbroker1_target");
                 //Instantiate(prefab);
                    var prefab = bundle.LoadAsset<GameObject>("pawnbroker2_target");
                    Instantiate(prefab);
                    Debug.Log("pawnbroker распакована");
                    show("pawnbroker распакована");
                    code[0] = 1;
                }
                else if (code[1] == 0 && AssetName == "rodion")
                {
                    var prefab = bundle.LoadAsset<GameObject>("rodion1_target");
                    Instantiate(prefab);
                    //prefab = bundle.LoadAsset<GameObject>("rodion2_target");
                    //Instantiate(prefab);
                    Debug.Log("rodion распакована");
                    show("rodion распакована");
                    code[1] = 1;
                }
                else if (code[2] == 0 && AssetName == "marmeladov")
                {
                    var prefab = bundle.LoadAsset<GameObject>("marmeladov_target");
                    var parent_obj = GameObject.Find("marmeladov_target");
                    Instantiate(prefab);
                    Debug.Log("marmeladov распакована");
                    show("marmeladov распакована");
                    code[2] = 1;
                    //foreach (obj2.GetComponent)
                }
                else show("ошмбка в имени asset bundle или все asset bundle загружены:" + www.error);



            }// Unload the AssetBundles compressed contents to conserve memory
            bundle.Unload(false);
            version++;
        } // memory is freed from the web stream (www.Dispose() gets called implicitly)
    }

    
}