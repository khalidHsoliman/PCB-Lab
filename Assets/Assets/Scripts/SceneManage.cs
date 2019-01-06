using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneManage : MonoBehaviour {

    public void Exit()
    {
        Application.Quit();
    }

    public void Load(int index)
    {
        SceneManager.LoadScene(index);
    }
}
