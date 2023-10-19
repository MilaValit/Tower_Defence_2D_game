using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    /// <summary>
    /// Контроллер переходов между уровнями. Должен быть с пометкой DoNotDetroyOnLoad
    /// И лежать в сцене с главным меню. LevelController дернет завершение уровня.
    /// </summary>
    public class LevelSequenceController : SingletonBase<LevelSequenceController>
    {
        public static string MainMenuSceneNickname = "LevelMap";
        
        public Episode CurrentEpisode { get; private set; }
        
        public int CurrentLevel { get; private set; }

        public bool LastLevelResult { get; private set; }

        public PlayerStatistics LevelStatistics { get; private set; }

        public static SpaceShip PlayerShip { get; set; }      

        public void StartEpisode(Episode e)
        {
            CurrentEpisode = e;
            CurrentLevel = 0;

            //сбрасывает статы перед началом эпизода
            LevelResultController.ResetPlayerStats();

            SceneManager.LoadScene(e.Levels[CurrentLevel]);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
            //SceneManager.LoadScene(0);
        }

        public void FinishCurrentLevel(bool success)
        {
            LevelResultController.Instance.Show(success);            
        }

        public void AdvanceLevel()
        {
            CurrentLevel++;
            if (CurrentEpisode.Levels.Length <= CurrentLevel)
            {
                SceneManager.LoadScene(MainMenuSceneNickname);
            }
            else
            {
                SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
            }
        }       
    }
}
