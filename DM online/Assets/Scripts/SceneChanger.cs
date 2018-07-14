using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public void ChangeToCollection() {
        SceneManager.LoadScene("Collection");
    }

    public void ChangeToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

}
