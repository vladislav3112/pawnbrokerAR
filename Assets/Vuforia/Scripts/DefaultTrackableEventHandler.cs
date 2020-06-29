/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    public UnityEngine.Video.VideoPlayer videoPlayer;
    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;
    
    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS
    public AudioSource music;
    int i = 0;
    private bool[] isSceneHandled = { false, false, false, false, false };
    
    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }
    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            string targetName = mTrackableBehaviour.TrackableName;
            Debug.Log("Trackable " + targetName + " found");
            //attach all from asset bundle:
            
            i = 0;
            if (!isSceneHandled[0] && GameObject.Find("pawnbroker2_target(Clone)") != null && targetName == "pawnbroker2_1")
            {
                isSceneHandled[0] = true;
                var assetObj = GameObject.Find("pawnbroker2_target(Clone)");
                Transform currObj = assetObj.gameObject.transform.GetChild(0);
                if (currObj != null)
                {

                    currObj.parent = this.gameObject.transform;
                    currObj.localScale = new Vector3(0.4f, 0.3f, 1);
                    currObj.localPosition = new Vector3(-0.036f, 0, -0.131f);
                    currObj.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
                    videoPlayer = currObj.gameObject.GetComponent<UnityEngine.Video.VideoPlayer>();
                    videoPlayer.playOnAwake = false;
                }
            }
            if (!isSceneHandled[1] && GameObject.Find("pawnbroker1_target(Clone)") != null && targetName == "pawnbroker1_1")
            {
                isSceneHandled[1] = true;
                var assetObj = GameObject.Find("pawnbroker1_target(Clone)");
                Transform currObj = assetObj.gameObject.transform.GetChild(0);
                currObj = assetObj.gameObject.transform.GetChild(0);
                if (currObj != null)
                {
                    currObj.parent = this.gameObject.transform;
                    currObj.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    currObj.localPosition = new Vector3(-0.03f, 0.04f, 0.017f);
                    currObj.localRotation = Quaternion.Euler(new Vector3(75, 20, -2));

                }
            }
            if (!isSceneHandled[2] && GameObject.Find("rodion1_target(Clone)") != null && targetName == "rodion1_5")
            {
                isSceneHandled[2] = true;
                var assetObj = GameObject.Find("rodion1_target(Clone)");
                Transform currObj = assetObj.gameObject.transform.GetChild(0);
                if (currObj != null)
                {
                    currObj.parent = this.gameObject.transform;
                    currObj.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                    currObj.localPosition = new Vector3(-0.048f, 0.006f, -0.058f);
                    currObj.localRotation = Quaternion.Euler(new Vector3(90, 0, 90));
                }    
            }

            OnTrackingFound();

        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();            
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        //custom
        if (videoPlayer != null) videoPlayer.Play();
        if (music != null) music.Play();
        if (mTrackableBehaviour.TrackableName == "pawnbroker1_1" || mTrackableBehaviour.TrackableName == "pawnbroker2_1")
            GameObject.Find("ButtonDescription").GetComponentInChildren<Text>().text = "Дом Алёны Ивановны, старухи-процентщицы";
        else if (mTrackableBehaviour.TrackableName == "rodion2_2" || mTrackableBehaviour.TrackableName == "rodion1_5")
            GameObject.Find("ButtonDescription").GetComponentInChildren<Text>().text = "Дом Раскольникова, Родиона Романовича";
        else if (mTrackableBehaviour.TrackableName == "marmeladov1_2")
            GameObject.Find("ButtonDescription").GetComponentInChildren<Text>().text = "Marmeladov_destiny";

        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
        }


    protected virtual void OnTrackingLost()
    {
        //custom
        if (videoPlayer != null) videoPlayer.Stop();
        if (music != null) music.Stop();

        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

    #endregion // PROTECTED_METHODS
}
