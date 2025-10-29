using System.Collections;
using UnityEngine.SceneManagement;

namespace DirtyWorks.GameBlocks
{
    public class BaseSceneAction : ActionBlock
    {
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
            if (sceneByIndex)
                SceneManager.LoadScene(sceneIndex);
            else
                SceneManager.LoadScene(sceneName);
        }

        public override IEnumerator RunCoroutine()
        {
            Run();
            yield break;
        }
    }

    [ActionBlock("SceneManager")]
    public class LoadAdditiveSceneAction : BaseSceneAction, IGameBlock
    {
        public override void DoAction()
        {
            if (sceneByIndex)
                SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
            else
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        public override IEnumerator RunCoroutine()
        {
            Run();
            yield break;
        }
    }
}
