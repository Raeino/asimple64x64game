using UnityEngine;

public class NGHelper : MonoBehaviour
{
    [SerializeField] private io.newgrounds.core ngioCore;

    private void Start() {
        ngioCore.onReady(() => {

            ngioCore.checkLogin((bool loggedIn) => {
                if (loggedIn)
                    OnLoggedIn();
                else
                    RequestLogin();
            });
        });
    }

    private void OnLoggedIn() {
        io.newgrounds.objects.user player = ngioCore.current_user;
    }

    private void RequestLogin() {
        ngioCore.requestLogin(OnLoggedIn, OnLoginFailed, OnLoginCancelled);
    }

    private void OnLoginFailed() {
        io.newgrounds.objects.error error = ngioCore.login_error;
    }

    private void OnLoginCancelled() {
        // do nothing
    }

    public void NGSubmitScore(int scoreId, int score) {
        io.newgrounds.components.ScoreBoard.postScore submitScore = new io.newgrounds.components.ScoreBoard.postScore();
        submitScore.id = scoreId;
        submitScore.value = score;
        submitScore.callWith(ngioCore);
    }
}
