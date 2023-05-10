//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using System;
using UnityEngine;

namespace AR_Passthrough_SampleScene
{
#if SXR_USE_STEAMVR
using Valve.VR.Extras;
using Valve.VR;
public class Modded_SteamVR_TestTrackedCamera : MonoBehaviour
{
    public float xOffset = .1f;
    public float yOffset = .05f; 
    public float scaleX = .75f;
    public float scaleY = .75f; 
    public float scaler = .02f; 
    public Material material;
    public Transform target;
    public bool undistorted = true;
    public bool cropped = true;

    private void OnEnable()
    {
        // The video stream must be symmetrically acquired and released in
        // order to properly disable the stream once there are no consumers.
        SteamVR_TrackedCamera.VideoStreamTexture source = SteamVR_TrackedCamera.Source(undistorted);
        source.Acquire();

        // Auto-disable if no camera is present.
        if (!source.hasCamera)
            enabled = false;
    }

    private void OnDisable()
    {
        // Clear the texture when no longer active.
        material.mainTexture = null;

        // The video stream must be symmetrically acquired and released in
        // order to properly disable the stream once there are no consumers.
        SteamVR_TrackedCamera.VideoStreamTexture source = SteamVR_TrackedCamera.Source(undistorted);
        source.Release();
    }

    private void Update()
    {
        SteamVR_TrackedCamera.VideoStreamTexture source = SteamVR_TrackedCamera.Source(undistorted);
        Texture2D texture = source.texture;
        if (texture == null)
        {
            return;
        }

        // Apply the latest texture to the material.  This must be performed
        // every frame since the underlying texture is actually part of a ring
        // buffer which is updated in lock-step with its associated pose.
        // (You actually really only need to call any of the accessors which
        // internally call Update on the SteamVR_TrackedCamera.VideoStreamTexture).
        material.mainTexture = texture;

        // Adjust the height of the quad based on the aspect to keep the texels square.
        float aspect = (float)texture.width / texture.height;

        // The undistorted video feed has 'bad' areas near the edges where the original
        // square texture feed is stretched to undo the fisheye from the lens.
        // Therefore, you'll want to crop it to the specified frameBounds to remove this.
        if (cropped)
        {
            VRTextureBounds_t bounds = source.frameBounds;
            material.mainTextureOffset = new Vector2(bounds.uMin+xOffset, bounds.vMin+yOffset);

            float du = bounds.uMax - bounds.uMin ;
            float dv = bounds.vMax - bounds.vMin ;
            material.mainTextureScale = new Vector2(du*scaleX, dv*scaleY);

            aspect *= Mathf.Abs((du*scaleX) / (dv*scaleY));
        }
        else
        {
            material.mainTextureOffset = Vector2.zero + new Vector2(xOffset, yOffset);
            material.mainTextureScale = new Vector2(1, -1);
        }

        target.localScale = new Vector3(scaler * 1, scaler * 1.0f / aspect, 1);

        // Apply the pose that this frame was recorded at.
        // if (source.hasTracking)
        // {
        //     SteamVR_Utils.RigidTransform rigidTransform = source.transform;
        //     target.localPosition = rigidTransform.pos;
        //     target.localRotation = rigidTransform.rot;
        // }
    }
}
#else
    public class Modded_SteamVR_TestTrackedCamera : MonoBehaviour
    {
        private void Start()
        {
            Debug.LogWarning("AR Passthrough currently requires SteamVR package.");
        }
    }
#endif
}