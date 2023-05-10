using UnityEngine;
using UnityEngine.XR;

namespace sxr_internal {
    /// <summary>
    /// Lazy TryGetFeatureValue access...  Automates type specification requirement
    /// and outputs error message
    /// </summary>
    public class OpenXR_InputDeviceFeatureHelper {
        public bool UpdateFeatureValue<T>(InputDevice device, InputFeatureUsage feature, out T output) {
            if (typeof(T) == typeof(bool))
                if (device.TryGetFeatureValue(feature.As<bool>(), out bool featureValue))
                { output = (T) (object) featureValue; return true; }
                else 
                    sxr.DebugLog("Found " + feature.name + " but unable to parse " + typeof(T));
            else if (typeof(T) == typeof(float))
                if(device.TryGetFeatureValue(feature.As<float>(), out float featureValue))
                    { output = (T) (object) featureValue; return true; }
                else 
                    sxr.DebugLog("Found " + feature.name + " but unable to parse " + typeof(T));
            else if (typeof(T) == typeof(uint))
                if(device.TryGetFeatureValue(feature.As<uint>(), out uint featureValue))
                    { output = (T) (object) featureValue; return true; }
                else 
                    sxr.DebugLog("Found " + feature.name + " but unable to parse " + typeof(T));
            else if (typeof(T) == typeof(Quaternion))
                if(device.TryGetFeatureValue(feature.As<Quaternion>(), out Quaternion featureValue))
                    { output = (T) (object) featureValue; return true; }
                else 
                    sxr.DebugLog("Found " + feature.name + " but unable to parse " + typeof(T));
            else if (typeof(T) == typeof(Vector2))
                if(device.TryGetFeatureValue(feature.As<Vector2>(), out Vector2 featureValue))
                    { output = (T) (object) featureValue; return true; }
                else 
                    sxr.DebugLog("Found " + feature.name + " but unable to parse " + typeof(T));
            else if (typeof(T) == typeof(Vector3))
                if(device.TryGetFeatureValue(feature.As<Vector3>(), out Vector3 featureValue))
                    { output = (T) (object) featureValue; return true; }
                else 
                    sxr.DebugLog("Found " + feature.name + " but unable to parse " + typeof(T));
            else if (typeof(T) == typeof(Bone))
                if(device.TryGetFeatureValue(feature.As<Bone>(), out Bone featureValue))
                    { output = (T) (object) featureValue; return true; }
                else 
                    sxr.DebugLog("Found " + feature.name + " but unable to parse " + typeof(T));
            else if (typeof(T) == typeof(Eyes))
                if(device.TryGetFeatureValue(feature.As<Eyes>(), out Eyes featureValue))
                    { output = (T) (object) featureValue; return true; }
                else 
                    sxr.DebugLog("Found " + feature.name + " but unable to parse " + typeof(T));
            else if (typeof(T) == typeof(Hand))
                if(device.TryGetFeatureValue(feature.As<Hand>(), out Hand featureValue))
                    { output = (T) (object) featureValue; return true; }
                else 
                    sxr.DebugLog("Found " + feature.name + " but unable to parse " + typeof(T));
            else if (typeof(T) == typeof(InputTrackingState))
                if(device.TryGetFeatureValue(feature.As<InputTrackingState>(), out InputTrackingState featureValue))
                    { output = (T) (object) featureValue; return true; }
                else 
                    sxr.DebugLog("Found " + feature.name + " but unable to parse " + typeof(T));

            output = default(T);
            return false; }
    }
}