using UnityEngine.SceneManagement; 
using UnityEngine;

public class UIManager : MonoBehaviour {

	public void Exit()
    {
        Application.Quit(); 
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
