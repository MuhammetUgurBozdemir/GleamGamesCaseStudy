using System.Threading;
using UnityEngine;
using Zenject;

public class LevelController : IInitializable
{
    private LevelSettings levelSettings;
    private LevelView currentLevelView;

    DiContainer container;
    
    private CancellationTokenSource cancellationTokenSource;
    

    public LevelController(LevelSettings _levelSettings, DiContainer _container)
    {
        levelSettings = _levelSettings;
        container = _container;
    }

    public void Initialize()
    {
        currentLevelView = container.InstantiatePrefabForComponent<LevelView>(levelSettings.Levels[0]);
        currentLevelView.Init();
    }

    public void CheckForLevelEnd()
    {
        if(currentLevelView.CheckForLevelEnd()) NextLevel();
    }

    private void NextLevel()
    {
        var nextLevelIndex = currentLevelView.LevelIndex + 1;
        
        if (nextLevelIndex >= levelSettings.Levels.Length)
        {
            nextLevelIndex = 0;
        }
        
        currentLevelView.Dispose();
        currentLevelView = container.InstantiatePrefabForComponent<LevelView>(levelSettings.Levels[nextLevelIndex]);
        currentLevelView.Init();
    }
}