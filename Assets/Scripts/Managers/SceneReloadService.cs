using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloadService
{
    private bool _startImmediatelyAfterReload;

    public void ReloadCurrentScene(bool startImmediately = false)
    {
        _startImmediatelyAfterReload = startImmediately;

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool ConsumeStartImmediatelyFlag()
    {
        bool value = _startImmediatelyAfterReload;
        _startImmediatelyAfterReload = false;
        return value;
    }
}