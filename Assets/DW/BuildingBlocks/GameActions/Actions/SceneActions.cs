using System.Collections;
using UnityEngine.SceneManagement;

namespace DirtyWorks.GameBlocks
{
    public abstract class BaseSceneAction : ActionBlock
    {
        public bool additiveScene = false;
        public bool sceneByIndex = false;
        public int sceneIndex = 0;
        public string sceneName = "";

        public virtual void DoAction()
        {
            
        }

        public virtual void Run() => DoAction();

        public override IEnumerator RunCoroutine()
        {
            Run();
            yield break;
        }
    }

    [ActionBlock("SceneManager")]
    public class LoadSceneAction : BaseSceneAction, IGameBlock
    {
        public override void DoAction()
        {
            var mode = (additiveScene) ? LoadSceneMode.Additive : LoadSceneMode.Single;
            if (sceneByIndex)
                SceneManager.LoadScene(sceneIndex, mode);
            else
                SceneManager.LoadScene(sceneName, mode);
        }

        public override IEnumerator RunCoroutine()
        {
            Run();
            yield break;
        }
    }

    [ActionBlock("SceneManager")]
    public class LoadSceneAsyncAction : BaseSceneAction, IGameBlock
    {
        public override void DoAction()
        {
            var mode = (additiveScene) ? LoadSceneMode.Additive : LoadSceneMode.Single;
            if (sceneByIndex)
                SceneManager.LoadSceneAsync(sceneIndex, mode);
            else
                SceneManager.LoadSceneAsync(sceneName, mode);
        }

        public override IEnumerator RunCoroutine()
        {
            Run();
            yield break;
        }
    }
}
