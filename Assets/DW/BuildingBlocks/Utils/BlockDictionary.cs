using System.Collections.Generic;

namespace DirtyWorks.GameBlocks.Utils
{
    public class BlockDictionary
    {
        public static readonly Dictionary<string, string> BlockName = new Dictionary<string, string>()
        {
            {"PauseForSeconds", "Wait for Seconds" },
            {"PauseForFrames", "Wait for Frames" },
            {"PauseForFixedUpdate", "Wait for FixedUpdate" },
            {"Log", "Console Log" },
            {"CallUnityEvent", "Invoke UnityEvent" },
            {"LoadSceneAction", "Load Scene" }
        };

        public static readonly Dictionary<string, string> TypeIcon = new Dictionary<string, string>()
        {
            {"PauseForSeconds", "PauseButton" },
        };

        public static readonly Dictionary<string, string> AttributeIcon = new Dictionary<string, string>()
        {
            {"Delay", "PauseButton" },
            {"Debug", "console.infoicon" },
        };
    }
}
