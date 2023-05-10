using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.ReorderableList;
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

        private List<List<string>> listOfFolderLists = new List<List<string>>();

        /// <summary>
        /// Plays a random sound from the specified folder. Requires a specified number of sounds
        /// before repeating the same sound in the folder.
        /// </summary>
        /// <param name="folderName"> Name of Resources/[folderName] to search for sounds</param>
        /// <param name="numberBetweenRepeats">Number of sounds before repeating the same sound</param>
        /// <returns></returns>
        public bool PlayRandomSoundInFolder(string folderName, int numberBetweenRepeats)
        {
            AudioClip[] allSounds = Resources.LoadAll<AudioClip>("Sounds" + folderName);

            if (allSounds.Length < 1) {
                Debug.Log("Could not find Resources folder with name: Sounds/" + folderName);
                allSounds = Resources.LoadAll<AudioClip>(folderName);
                if(allSounds.Length < 1){
                    Debug.Log("Could not find folder with name: "+folderName);
                    return false; }}

            // Pick a sound:
            var sound = allSounds[Random.Range(1, allSounds.Length)];
            
            // Check if the sound's folder has previously called sounds: 
            for (int i=0; i<listOfFolderLists.Count; i++) {
                // If a list starts with folderName, the folder has been used previously
                if (listOfFolderLists[i][0] == folderName) {
                    if (listOfFolderLists[i].Count < numberBetweenRepeats) {
                        Debug.LogError("Not enough sounds to have " + numberBetweenRepeats 
                                            + " sounds between repeats for folder: \"" + folderName +"\"");
                        return false; }

                    
                    
                    // Keep checking until new sound is found: 
                    while (listOfFolderLists[i].Contains(sound.name))
                        sound = allSounds[Random.Range(1, allSounds.Length)]; 
                        
                    sxr.PlaySound((folderName=="" ? "" : folderName + Path.DirectorySeparatorChar) + sound.name); 
                    listOfFolderLists[i].Add(sound.name);


                    if (listOfFolderLists[i].Count > numberBetweenRepeats+1)
                        listOfFolderLists[i].RemoveAt(1);
                    
                    return true; } }

            // Folder name has not been used previously
            sxr.PlaySound((folderName=="" ? "" : folderName + Path.DirectorySeparatorChar) + sound.name); 
            listOfFolderLists.Add(new List<string>{folderName, sound.name});
            return true; 
        }

        /// <summary>
        /// Plays one of the sounds provided by sXR
        /// </summary>
        /// <param name="sound"></param>
        public void PlayBuiltinSound(sxr_internal.ProvidedSounds sound) {
            switch (sound) {
                case sxr_internal.ProvidedSounds.Beep: Beep();
                    break;
                case sxr_internal.ProvidedSounds.Buzz: Buzz();
                    break;
                case sxr_internal.ProvidedSounds.Ding: Ding();
                    break;
                case sxr_internal.ProvidedSounds.Stop: Stop();
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

            AudioClip customClip = Resources.Load<AudioClip>("Sounds/" + soundName);
            Debug.Log(customClip);
            if (customClip == null) {
                Debug.Log("Failed to find sound clip " + soundName + " in a fuckin resources folder");
                foreach (var customSound in sxrSettings.Instance.audioClips){
                    if (customSound.name == soundName) {
                        customClip = customSound;
                        soundPlayer.PlayOneShot(customClip);
                        return; }}

                Debug.Log("Failed to find clip in sxrSettings audioClips, cannot play sound");
                
                AudioClip[] customclips = Resources.LoadAll<AudioClip>("Sounds/" + soundName);
            }
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