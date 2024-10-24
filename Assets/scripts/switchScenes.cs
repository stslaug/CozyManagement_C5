using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class switchScenes : MonoBehaviour
{
    [SerializeField] public string sceneName;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(changeScene);
    }

    private void changeScene()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}