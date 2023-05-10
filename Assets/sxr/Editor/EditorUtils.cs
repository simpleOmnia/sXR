using UnityEditor;
using UnityEditor.Build;

namespace sxr_internal {
    public static class EditorUtils {
        public static void AddDefineIfNecessary(string define, NamedBuildTarget buildTarget) {
            var defines = PlayerSettings.GetScriptingDefineSymbols(buildTarget);

            if (defines == null) {
                defines = define; }
            else if (defines.Length == 0) {
                defines = define; }
            else {
                if (defines.IndexOf(define, 0) < 0) {
                    defines += ";" + define; } }

            PlayerSettings.SetScriptingDefineSymbols(buildTarget, defines); }

        public static void RemoveDefineIfNecessary(string define, NamedBuildTarget buildTarget) {
            var defines = PlayerSettings.GetScriptingDefineSymbols(buildTarget);

            if (defines.StartsWith(define + ";"))
                defines = defines.Remove(0, define.Length + 1);
            else if (defines.StartsWith(define))
                defines = defines.Remove(0, define.Length);
            else if (defines.EndsWith(";" + define))
                defines = defines.Remove(defines.Length - define.Length - 1, define.Length + 1);
            else {
                var index = defines.IndexOf(define, 0, System.StringComparison.Ordinal);
                if (index >= 0) {
                    defines = defines.Remove(index, define.Length + 1); } }

            PlayerSettings.SetScriptingDefineSymbols(buildTarget, defines); }
    }
}