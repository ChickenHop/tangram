using UnityEngine;
using UnityEngine.UIElements;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;
    private int correct = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void addTocorrect()
    {
        correct++;
    }

    public void checkIfComplet()
    {
        if (correct == 7)
        {
            UiScript.instance.displayWin();
        }
    }
}
