using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteUser : MonoBehaviour
{
    private FirebaseAuth mAuth;
    public TMP_InputField passwordInput;
    public TMP_Text txt;
    public GameObject passwordErrorPopup;
    public GameObject successPopup;
    private bool isReAuth = true;
    private bool isDelete = false;
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            mAuth = FirebaseAuth.DefaultInstance;
        });
    }

    public async void Delete()
    {
        FirebaseUser user = mAuth.CurrentUser;

        if (user != null)
        {
            // 사용자를 다시 인증
            var credential = EmailAuthProvider.GetCredential(user.Email, passwordInput.text);

            try
            {
                await user.ReauthenticateAsync(credential);
                // 비밀번호 재인증 성공: 계정 삭제 시도
                await user.DeleteAsync();
                // 계정 삭제 성공
                ShowSuccessPopup();
            }
            catch (FirebaseException ex)
            {
                // FirebaseException이 발생하면 비밀번호가 잘못된 것이므로 오류 팝업을 띄웁니다.
                Debug.Log("Reauthentication failed: " + ex.Message);
                // txt.SetText(ex.Message);
                ShowPasswordErrorPopup();
            }
        }
    }

    void ShowPasswordErrorPopup()
    {
        // 비밀번호 오류 팝업을 활성화하고 사용자에게 메시지를 표시하는 코드를 여기에 추가하세요.
        
        passwordErrorPopup.SetActive(true);
    }

    void ShowSuccessPopup()
    {
        // 계정 삭제 성공 팝업을 활성화하고 사용자에게 메시지를 표시하는 코드를 여기에 추가하세요.
        successPopup.SetActive(true);
    }
}