using ArcanepadSDK.Models;
using ArcanepadSDK.Types;
using UnityEngine;
using UnityEngine.UI;

public class PadManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject CoverPad;
    public GameObject SelectCoursePad;
    public Button ReadyButton;
    public Button WaitButton;
    public Button BackToCoverButton;
    public GameObject GamePad;
    public Button QuitGameButton;


    async void Awake()
    {
        DontDestroyOnLoad(this);

        canvas = GameObject.Find("Canvas");

        CoverPad = canvas.transform.Find("CoverPad").gameObject;
        SelectCoursePad = canvas.transform.Find("SelectCoursePad").gameObject;
        GamePad = canvas.transform.Find("GamePad").gameObject;

        BackToCoverButton = SelectCoursePad.transform.Find("BackToCover").GetComponent<Button>();
        ReadyButton = SelectCoursePad.transform.Find("Ready").GetComponent<Button>();
        WaitButton = SelectCoursePad.transform.Find("Wait").GetComponent<Button>();
        QuitGameButton = GamePad.transform.Find("Quit").GetComponent<Button>();

        CoverPad.SetActive(true);

        // RefreshPadUI(Global.gameState.uiState);

        Arcane.Init(new ArcaneInitParams(deviceType: ArcaneDeviceType.pad, padOrientation: AOrientation.Portrait));

        await Arcane.ArcaneClientInitialized();

        BackToCoverButton.onClick.AddListener(() => Arcane.Msg.EmitToViews(new RefreshUIStateEvent(UIState.GameCover)));

        ReadyButton.onClick.AddListener(() =>
        {
            Arcane.Msg.EmitToViews(new ArcaneBaseEvent("Ready"));
            ReadyButton.gameObject.SetActive(false);
            WaitButton.gameObject.SetActive(true);
        });

        WaitButton.onClick.AddListener(() =>
        {
            Arcane.Msg.EmitToViews(new ArcaneBaseEvent("Wait"));
            ReadyButton.gameObject.SetActive(true);
            WaitButton.gameObject.SetActive(false);
        });

        QuitGameButton.onClick.AddListener(() => Arcane.Msg.EmitToViews(new RefreshUIStateEvent(UIState.SelectCourse)));

        Arcane.Msg.On(GameEvent.RefreshUIState, (RefreshUIStateEvent e) =>
        {
            Arcane.Msg.EmitToViews(new ArcaneBaseEvent("QuitLevel"));
        });

    }

    // void OnRefreshGameState(RefreshGameStateEvent e)
    // {
    //     Global.gameState = e.gameState;

    //     RefreshPadUI(Global.gameState.uiState);
    // }


    void RefreshPadUI(UIState uiState)
    {
        Debug.Log("Refreshed Pad UI State:" + uiState.ToString());

        switch (uiState)
        {
            case UIState.GameCover:
                CoverPad.SetActive(true);

                SelectCoursePad.SetActive(false);
                GamePad.SetActive(false);
                break;

            case UIState.SelectCourse:
                SelectCoursePad.SetActive(true);

                CoverPad.SetActive(false);
                GamePad.SetActive(false);
                break;

            case UIState.InGame:

                GamePad.SetActive(true);

                CoverPad.SetActive(false);
                SelectCoursePad.SetActive(false);
                break;

            // case UIState.Loading:
            //     break;

            case UIState.PlayerDisconnected:
                break;
        }
    }
}
