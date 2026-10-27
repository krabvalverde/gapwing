using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
static class OpenStartScene
{
    static OpenStartScene()
    {
        if (SessionState.GetBool(nameof(OpenStartScene), false)) return;
        SessionState.SetBool(nameof(OpenStartScene), true);
        EditorApplication.delayCall += Open;
    }

    static void Open()
    {
        var active = SceneManager.GetActiveScene();
        if (EditorApplication.isPlayingOrWillChangePlaymode || !string.IsNullOrEmpty(active.path) || active.isDirty) return;
        if (EditorBuildSettings.scenes.Length == 0) return;
        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
    }
}
