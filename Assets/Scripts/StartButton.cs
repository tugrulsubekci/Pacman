using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(StartBtn);
    }

    private void StartBtn()
    {
        SceneManager.LoadScene(1);
    }
}
