using Vuforia;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicTargets : MonoBehaviour
{

    private bool mChipsObjectCreated = false;
    public Transform myModelPrefab;

    // Update is called once per frame
    void Update()
    {
        IEnumerable<TrackableBehaviour> trackableBehaviours = TrackerManager.Instance.GetStateManager().GetActiveTrackableBehaviours();

        // Loop over all TrackableBehaviours.
        foreach (TrackableBehaviour trackableBehaviour in trackableBehaviours)
        {
            string name = trackableBehaviour.TrackableName;
            Debug.Log("Trackable name: " + name);

            if (name.Equals("rodion1_5") && !mChipsObjectCreated)
            {
                // chips target detected for the first time
                // augmentation object has not yet been created for this target
                // let's create it
                Transform Quad = GameObject.Instantiate(myModelPrefab) as Transform;
                // attach cube under target
                Quad.transform.parent = trackableBehaviour.transform;

                // Add a Trackable event handler to the Trackable.
                // This Behaviour handles Trackable lost/found callbacks.
                trackableBehaviour.gameObject.AddComponent<DefaultTrackableEventHandler>();

                // set local transformation (i.e. relative to the parent target)
                Quad.transform.localPosition = new Vector3(0, 0.2f, 0);
                Quad.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                Quad.transform.localRotation = Quaternion.identity;
                Quad.gameObject.SetActive(true);

                mChipsObjectCreated = true;
            }
        }
    }
}
