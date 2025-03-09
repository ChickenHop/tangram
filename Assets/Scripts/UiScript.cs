using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UiScript : MonoBehaviour
{

    private VisualElement winScreen;
    public static UiScript instance;
    private bool finised=false;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        winScreen = root.Q<VisualElement>("WinScreen");
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.R) && finised)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }

    public void displayWin()
    {
        winScreen.style.display = DisplayStyle.Flex;
        finised = true;
    }

}
