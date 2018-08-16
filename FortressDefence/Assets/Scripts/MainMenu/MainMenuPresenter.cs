using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

public class MainMenuPresenter : MonoBehaviour {

    [SerializeField]
    private MainMenuView view;
   // private MainMenuModel model;

	void Start () {



        view.playButton.OnClickAsObservable()
                  .Subscribe(_ => StartGame())
                  .AddTo(this);

        view.infoButton.OnClickAsObservable()
                  .Subscribe(_ => OnInfoButtonClicked())
                  .AddTo(this);

        view.exitButton.OnClickAsObservable()
                  .Subscribe(_ => OnExitButtonClicked())
                  .AddTo(this);
		
	}
	
    private void StartGame(){
        SceneManager.LoadScene("Game");
    }

    private void OnInfoButtonClicked(){
        // Do nothing
    }
    private void OnExitButtonClicked(){
        Application.Quit();
    }
}
