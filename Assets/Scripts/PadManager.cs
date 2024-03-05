using ArcanepadSDK.Models;
using ArcanepadSDK.Types;
using UnityEngine;
using UnityEngine.UI;

public class PadManager : MonoBehaviour
{
    public GameObject canvas;
    public Button StartSelectCourseButton;
    public Button BackToCoverButton;
    public Button ReadyButton;
    public Button WaitButton;
    // public TextMeshProUGUI LogsText;
    public GameObject CoverPad;
    public GameObject SelectCoursePad;

    async void Awake()
    {
        DontDestroyOnLoad(this);

        canvas = GameObject.Find("Canvas");

        CoverPad = canvas.transform.Find("CoverPad").gameObject;
        SelectCoursePad = canvas.transform.Find("SelectCoursePad").gameObject;

        StartSelectCourseButton = CoverPad.transform.Find("StartSelectCourse").GetComponent<Button>();
        BackToCoverButton = SelectCoursePad.transform.Find("BackToCover").GetComponent<Button>();
        ReadyButton = SelectCoursePad.transform.Find("Ready").GetComponent<Button>();
        WaitButton = SelectCoursePad.transform.Find("Wait").GetComponent<Button>();

        RefreshPadUI(Global.gameState.uiState);

        Arcane.Init(new ArcaneInitParams(deviceType: ArcaneDeviceType.pad, padOrientation: AOrientation.Portrait));

        await Arcane.ArcaneClientInitialized();

        StartSelectCourseButton.onClick.AddListener(() => Arcane.Msg.EmitToViews(new RefreshUIStateEvent(UIState.SelectCourse)));
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

        Arcane.Msg.On(GameEvent.RefreshUIState, (RefreshUIStateEvent e) =>
        {
            Global.gameState.uiState = e.uiState;

            RefreshPadUI(Global.gameState.uiState);
        });

    }

    // void OnRefreshGameState(RefreshGameStateEvent e)
    // {
    //     Global.gameState = e.gameState;

    //     RefreshPadUI(Global.gameState.uiState);
    // }


    void RefreshPadUI(UIState uiState)
    {
        switch (uiState)
        {
            case UIState.GameCover:
                CoverPad.SetActive(true);
                SelectCoursePad.SetActive(false);
                break;

            case UIState.SelectCourse:
                CoverPad.SetActive(false);
                SelectCoursePad.SetActive(true);

                break;

            case UIState.Loading:
                break;

            case UIState.PlayerDisconnected:
                break;
        }
    }
}
