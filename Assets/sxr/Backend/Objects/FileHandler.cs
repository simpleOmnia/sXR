using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// Used to handle writing to/reading from files.  
    /// </summary>
    public class FileHandler {
        /// <summary>
        /// Creates file (or directory) if it doesn't exist
        /// Then appends line 
        /// </summary>
        public void AppendLine(string path, string newLine) {
            if (CheckPath(path) & !File.Exists(path))
                using (StreamWriter file = File.CreateText(path))
                    file.WriteLine(newLine);
            else if (CheckPath(path))
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@path, true)) 
                    file.WriteLine(newLine);   }

        public void OverwriteFile(string path, string toWrite) {
            using (StreamWriter file = File.CreateText(path))
                file.WriteLine(toWrite); }

        public void RemoveLastLine(string path, int numLines) {
            List<string> lineList = new List<string>(File.ReadAllLines(path));
            if (lineList.Count > 0) {
                lineList.RemoveAt(lineList.Count - numLines);
                File.WriteAllLines(path, lineList.ToArray()); }
            else File.Delete(path); }
        public void RemoveLastLine(string path) {
            RemoveLastLine(path, 1); }

        public bool CheckPath(string path) {
            string directory = Path.GetDirectoryName(path); 
            if(directory!=null & !Directory.Exists(directory) & Directory.Exists(path.Split('/')[0]))
                Directory.CreateDirectory(directory);
            else if(!Directory.Exists(directory))
                Debug.Log("Could not create directory for: " + path);
            return Directory.Exists(directory); }

        public string ReadLastLine(string path) {
            if (File.Exists(path)) {
                List<string> lineList = new List<string>(File.ReadAllLines(path));
                return lineList.Count > 0 ? lineList[^1] : ""; }
            return ""; } 
    }    
}
