using UnityEngine;

namespace sxr_internal {
    public class Timer {
        private float startTime;
        private readonly string timerName;
        private float duration;
        private float pausedTime; 

        public Timer(string timerName, float duration) {
            startTime = Time.time;
            this.timerName = timerName;
            this.duration = duration; }

        public void Restart() { startTime = Time.time; }
        public void StartTimer(float duration)
        {
            this.duration = duration; 
            if (pausedTime == 0) { Restart(); }
            else
            {
                startTime = Time.time - pausedTime + startTime; 
                pausedTime = 0; 
            }
        }
        public void StartTimer(){StartTimer(this.duration);}
        
        public void PauseTimer(){ pausedTime = Time.time; }
        public string GetName() { return timerName; }
        public float GetTimePassed() { return Time.time - startTime; }
        public float GetTimeRemaining()
        { return GetTimePassed() > duration ? 0 : duration - GetTimePassed();}
        public float GetDuration() { return duration; }

        
    }
}
