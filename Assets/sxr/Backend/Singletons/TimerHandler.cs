using System.Collections.Generic;
using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// [Singleton] Tracks all timers in the scene to allow for sxr.CheckTimer(name) calls.
    /// Use Timer(string timerName, float duration) to instantiate Timer. Once [duration]
    /// seconds has passed and a CheckTimer call is made, will return true and destroy the Timer. 
    /// </summary>
    public class TimerHandler : MonoBehaviour {
        private List<Timer> allTimers = new List<Timer>();

        /// <summary>
        /// Adds a timer to the list of named timers
        /// </summary>
        /// <param name="timerName"></param>
        /// <param name="duration"></param>
        public Timer AddTimer(string timerName, float duration) {
            if(!TimerExists(timerName)) 
                allTimers.Add(new Timer(timerName, duration));
            else
                Debug.LogWarning("Timer with name \"" + timerName +"\" already exists");
            return GetTimer(timerName); 
        }

        public void PauseTimer(string timerName)
        {
            if(TimerExists(timerName))
                GetTimer(timerName).PauseTimer();
            else
                Debug.LogWarning("Attempting to pause timer that does not exist ('"+timerName+"'");
        }
        
        public Timer GetTimer(string timerName) {
            foreach (var timer in allTimers)
                if (timer.GetName() == timerName)
                    return timer;
            Debug.LogWarning("Could not find timer: " + timerName);
            return null; }

        public Timer StartTimer(string timerName, float duration=99999)
        {
            if (!TimerExists(timerName)) AddTimer(timerName, duration);
            else
            {
                Debug.Log("Timer with name '" + timerName + "' already exists." +
                          " Will unpause a paused timer or restart an unpaused timer.");
                GetTimer(timerName).StartTimer(); 
            }

            return GetTimer(timerName); 
        }
        
        /// <summary>
        /// Checks if a timer with provided name is already initiated
        /// </summary>
        /// <param name="timerName"></param>
        /// <returns></returns>
        public bool TimerExists(string timerName) {
            foreach (var timer in allTimers)
                if (timer.GetName() == timerName)
                    return true;
            return false; }

        /// <summary>
        /// Restarts the named timer
        /// </summary>
        /// <param name="timerName"></param>
        public void RestartTimer(string timerName) {
            if(TimerExists(timerName))
                GetTimer(timerName).Restart();
            else
                Debug.Log("Unable to find timer: " + timerName); }
        
        /// <summary>
        /// Checks if Timer.duration (seconds) has passed for the named timer.
        /// Returns true and destroys timer after time has passed
        /// </summary>
        /// <param name="name">Name of Timer to find</param>
        /// <returns>true once Timer.duration seconds has passed</returns>
        public bool CheckTimer(string timerName) {
            foreach (var timer in allTimers) {
                if (timer.GetName() == timerName) {
                    if (timer.GetTimePassed() > timer.GetDuration()) {
                        allTimers.Remove(timer);
                        return true; }

                    return false; } }

            sxr.DebugLog("No timer with name \"" + timerName + "\" found.");
            return false; }

        /// <summary>
        /// Checks how long has passed on the named timer
        /// </summary>
        /// <param name="timerName"></param>
        /// <returns></returns>
        public float GetTimePassed(string timerName) {
            if(TimerExists(timerName))
                return GetTimer(timerName).GetTimePassed();
            
            Debug.Log("Unable to find timer: " + timerName);
            return 0; }

        /// <summary>
        /// Checks how long has passed on the named timer
        /// </summary>
        /// <param name="timerName"></param>
        /// <returns></returns>
        public float GetTimeRemaining(string timerName) {
            if(TimerExists(timerName))
                return GetTimer(timerName).GetTimeRemaining();
            
            Debug.Log("Unable to find timer: " + timerName);
            return 0; }

        // Singleton initiated on Awake()  
        public static TimerHandler Instance;
        void Awake() {
            if ( Instance == null) {Instance = this;  DontDestroyOnLoad(gameObject.transform.root);}
            else Destroy(gameObject); }
    }
}
