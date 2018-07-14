using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DMNetworkManagerHUD : MonoBehaviour {

    [SerializeField]
    private GameObject menuCanvas;
    [SerializeField]
    private Button quitButton;
    public Button QuitButton => quitButton;

    private void Start() {
        CloseMenu();
    }

    private void Update() {
        if (Input.GetButtonDown("Menu") && NetworkManager.singleton.isNetworkActive) {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }

    public void CloseMenu() {
        menuCanvas.SetActive(false);
    }

}
