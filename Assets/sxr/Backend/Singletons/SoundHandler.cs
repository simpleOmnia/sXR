using System.IO;
using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// [Singleton] Handles sounds, has four built in (beep/ding/buzz/stop)
    /// or can look up songs by filename in any folder named \"Resources\".
    /// When writing filename, do not include the extension (e.g. .mp3, .wav, etc)
    /// Should be attached to the same object as the audio listener
    /// Contains:
    ///     void Beep()
    ///     void Buzz()
    ///     void Ding()
    ///     void Stop()
    ///     void CustomSound(int soundNumber)
    ///     void CustomSound(string soundName)
    /// On Awake:
    ///     Initializes singleton
    /// On Start:
    ///     N/A
    /// On Update:
    ///     N/A
    /// </summary>
    public class SoundHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource soundPlayer;
        [SerializeField] private AudioClip beep, ding, buzz, stop=null;

        /// <summary>
        /// Plays one of the sounds provided by sXR
        /// </summary>
        /// <param name="sound"></param>
        public void PlayBuiltinSound(sxr.ProvidedSounds sound) {
            switch (sound) {
                case sxr.ProvidedSounds.Beep: Beep();
                    break;
                case sxr.ProvidedSounds.Buzz: Buzz();
                    break;
                case sxr.ProvidedSounds.Ding: Ding();
                    break;
                case sxr.ProvidedSounds.Stop: Stop();
                    break; } }
        /// <summary>
        /// Plays beep sound effect
        /// </summary>
        public void Beep()
            {soundPlayer.PlayOneShot(beep);}
        
        /// <summary>
        /// Plays buzz sound effect
        /// </summary>
        public void Buzz()
            {soundPlayer.PlayOneShot(buzz);}
        
        /// <summary>
        /// Plays ding sound effect
        /// </summary>
        public void Ding() {soundPlayer.PlayOneShot(ding);}

        /// <summary>
        /// Plays the safety wall "STOP" message
        /// </summary>
        public void Stop() { soundPlayer.PlayOneShot(stop); }
        
        /// <summary>
        /// Plays sound effect at given index of customClips array
        /// </summary>
        /// <param name="soundNumber">index of sound effect to play</param>
        /// <returns></returns>
        public void CustomSound(int soundNumber)
            {soundPlayer.PlayOneShot(sxrSettings.Instance.audioClips[soundNumber]);}

        /// <summary>
        /// Plays a custom sound, first searches by filename (no extension, e.g. .mp3, .wav, etc)
        /// If it cannot find the filename in any folder named "Resources", will check to see if
        /// any custom sound clips listed in sxrSettings have the specified filename
        /// </summary>
        /// <param name="soundName"></param>
        public void CustomSound(string soundName) {
            AudioClip customClip = Resources.Load<AudioClip>("Sounds" + Path.DirectorySeparatorChar + soundName);
            if (!customClip) {
                sxr.DebugLog("Failed to find sound clip " + soundName + " in a resources folder");
                foreach (var customSound in sxrSettings.Instance.audioClips)
                    if (customSound.name == soundName) {
                        customClip = customSound;
                        soundPlayer.PlayOneShot(customClip);
                        return; }

                sxr.DebugLog("Failed to find clip in sxrSettings audioClips, cannot play sound"); }

            soundPlayer.PlayOneShot(customClip); }

        void Start() {
            if(!soundPlayer){  
                gameObject.AddComponent<AudioSource>();
                var audioSource = gameObject.GetComponent<AudioSource>(); }

            if (!beep) beep = Resources.Load<AudioClip>("Sounds" + Path.DirectorySeparatorChar + "beep");
            if (!ding) ding = Resources.Load<AudioClip>("Sounds" + Path.DirectorySeparatorChar + "ding");
            if (!buzz)stop = Resources.Load<AudioClip>("Sounds" + Path.DirectorySeparatorChar + "buzz");
            if (!stop)stop = Resources.Load<AudioClip>("Sounds" + Path.DirectorySeparatorChar + "stop");
        }
        // Singleton initiated on Awake()
        public static SoundHandler Instance; 
        private void Awake() {  if ( Instance == null) {Instance = this; DontDestroyOnLoad(gameObject.transform.root);} else Destroy(gameObject);  }
    }
}