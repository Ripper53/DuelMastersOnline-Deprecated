using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerManager : MonoBehaviour {
    [SerializeField]
    private Button serverButton;
    [SerializeField]
    private Button hostButton;
    [SerializeField]
    private Button clientButton;
    [SerializeField]
    private Button stopClientSearchingButton;

    [SerializeField]
    private GameObject networkMenu;
    [SerializeField]
    private InputField networkAddressInputField;

    private Button quitButton;

    private void Start() {
        quitButton = GameObject.FindGameObjectWithTag("Quit").GetComponent<Button>();
    }

    private void Update() {
        if (Input.GetButtonDown("Menu")) {
            networkMenu.SetActive(!networkMenu.activeSelf);
        }
    }

    public void StartServer() {
        string networkAddress = networkAddressInputField.text;
        if (networkAddress != "") {
            NetworkManager.singleton.networkAddress = networkAddress;
        }
        NetworkManager.singleton.StartServer();
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(NetworkManager.singleton.StopServer);
    }

    public void StartHost() {
        string networkAddress = networkAddressInputField.text;
        if (networkAddress != "") {
            NetworkManager.singleton.networkAddress = networkAddress;
        }
        NetworkManager.singleton.StartHost();
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(NetworkManager.singleton.StopHost);
    }

    public void StartClient() {
        string networkAddress = networkAddressInputField.text;
        if (networkAddress != "") {
            NetworkManager.singleton.networkAddress = networkAddress;
        }
        NetworkManager.singleton.StartClient();
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(NetworkManager.singleton.StopClient);
        stopClientSearchingButton.gameObject.SetActive(true);
        Interactable(false);
    }

    public void StopClientFromSearching() {
        NetworkManager.singleton.StopClient();
        stopClientSearchingButton.gameObject.SetActive(false);
        Interactable(true);
    }

    public void Interactable(bool value) {
        //serverButton.interactable = value;
        hostButton.interactable = value;
        clientButton.interactable = value;
    }

}
