using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class MainMenuSceneChange : MonoBehaviour
    {
        public void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }
        public void LoadMap()
        {
            SceneManager.LoadScene(1);
        }

    }
}
