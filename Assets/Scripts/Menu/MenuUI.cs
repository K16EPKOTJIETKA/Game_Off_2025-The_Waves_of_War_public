using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public GameObject startMenu;        
    public GameObject tutorialDialog;  

    public GameObject loadingCanvas;      
    public LoadingScreen loadingScreen;  

    public string mainGame = "MainGame";     
    public string tutorialScene = "Tutorial"; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        startMenu.SetActive(true);
        tutorialDialog.SetActive(false);

     
        loadingCanvas.SetActive(false);
    }

    public void OnStartPressed()
    {
        startMenu.SetActive(false);
        tutorialDialog.SetActive(true);
    }

    public void OnTutorialYes()
    {
        tutorialDialog.SetActive(false);
        loadingCanvas.SetActive(true);
        loadingScreen.StartLoading(tutorialScene);
    }

    public void OnTutorialNo()
    {
        tutorialDialog.SetActive(false);
        loadingCanvas.SetActive(true);
        loadingScreen.StartLoading(mainGame);
    }
}

