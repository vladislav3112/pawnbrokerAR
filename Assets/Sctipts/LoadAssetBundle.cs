using System;
using UnityEngine;
using System.Collections;
using Vuforia;
using System.Collections.Generic;
public class LoadAssetBundle : MonoBehaviour
{
    private string BundleURL;
    public string AssetName;
    private short[] code = { 0, 0, 0 };//code is downloaded
    private int version;
    // specify these in Unity Inspector


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
                throw new Exception("WWW download had an error:" + www.error);
            AssetBundle bundle = www.assetBundle;
            if (AssetName == "")
                throw new Exception("empty asset bundle name:" + www.error);

            else
            {   //download all prefabs 
                        if (code[0] == 0 && AssetName == "pawnbroker") {// && GameObject.Find("pawnbroker1_target")== null
                            //var prefab = bundle.LoadAsset<GameObject>("pawnbroker1_target");
                            //Instantiate(prefab);
                            var prefab = bundle.LoadAsset<GameObject>("pawnbroker2_target");
                            Instantiate(prefab);
                            Debug.Log("pawnbroker распакована");
                            code[0] = 1;
                        }
                        else if (code[1] == 0 && AssetName == "rodion")
                        {
                            var prefab = bundle.LoadAsset<GameObject>("rodion1_target");
                            Instantiate(prefab);
                            //prefab = bundle.LoadAsset<GameObject>("rodion2_target");
                            //Instantiate(prefab);
                            Debug.Log("rodion распакована");
                            code[1] = 1;
                        }
                        else if (code[2] == 0 && AssetName == "marmeladov")
                        {
                            var prefab = bundle.LoadAsset<GameObject>("marmeladov_target");
                            var parent_obj = GameObject.Find("marmeladov_target");
                            Instantiate(prefab);
                            Debug.Log("marmeladov распакована");

                            code[2] = 1;
                            //foreach (obj2.GetComponent)
                        }
                        else throw new Exception("ошмбка в имени asset bundle или все asset bundle загружены:" + www.error);
                        


            }// Unload the AssetBundles compressed contents to conserve memory
            bundle.Unload(false);
            version++;
        } // memory is freed from the web stream (www.Dispose() gets called implicitly)
    }

    
}