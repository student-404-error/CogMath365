using UnityEngine;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using TMPro;
using System;
using System.Threading.Tasks;

public class FirebaseAuthManager : MonoBehaviour
{
    public GameObject Popup; // 로그인 실패시 팝업창
    public GameObject PopupInvalidName; // 올바르지 않은 이름 입력시 팝업창
    public TMP_Text ErrorMessage; // 에러 메세지
    
    private FirebaseAuth mAuth;
    private FirebaseUser mUser;
    private static FirebaseAuthManager _instance = null;
    private GameManager mGameManager;
    public TMP_InputField email;
    public TMP_InputField pass;
    public Action<bool> loginState;
    private DateSave mDataSave;
    private DatabaseReference mReference;
    private SceneManager mSceneManager;
    private string mEm, mPa;
    public string UserId => mUser.UserId;
    public static FirebaseAuthManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FirebaseAuthManager>();
            }
            return _instance;
        }
    }
    public void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            mReference = FirebaseDatabase.GetInstance(app, "https://calcgame-fffc9-default-rtdb.firebaseio.com").RootReference;
            mAuth = FirebaseAuth.DefaultInstance;
        });
    }
    private async Task DoAsyncWork()
    {
        // 비동기 작업 수행
        await Task.Delay(200); // 예시로 0.2초 대기
        Debug.Log("Async work completed.");
    }
    public async void Awake()
    {
        mSceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        mDataSave = GameObject.Find("DataManager").GetComponent<DateSave>();
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mEm = email.text;
        mPa = pass.text;
        
        await DoAsyncWork();
        try
        {
            if (mAuth.CurrentUser != null)
            {
                // 이미 로그인된 유저가 있다면, 다음 화면으로 이동 등의 처리를 해줄 수 있습니다.
                mSceneManager.MoveMain();
            }
            else
            {
                mAuth.StateChanged += AuthStateChanged;
                AuthStateChanged(this, null);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    private void OnDestroy()
    {
        // Firebase 인증 이벤트 리스너 해제
        mAuth.StateChanged -= AuthStateChanged;
    }

    private void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        // 사용자 로그인 상태가 변경될 때 호출됩니다.
        if (mAuth.CurrentUser != null)
        {
            // 사용자가 로그인한 경우
            Debug.Log("사용자 로그인됨: " + mAuth.CurrentUser.Email);
            // 여기에서 사용자 정보 업데이트 등 필요한 작업 수행
            mGameManager.userName = mAuth.CurrentUser.Email.Replace("@gmail.com", "");
        }
        else
        {
            // 사용자가 로그아웃한 경우
            Debug.Log("사용자 로그아웃됨");
            // 여기에서 로그아웃 후 작업 수행
        }
    }
    public void Init()
    {
        mAuth = FirebaseAuth.DefaultInstance;

        if (mAuth.CurrentUser != null)
        {
            Logout();
        }

        mAuth.StateChanged += OnChanged;
    }

    private void OnChanged(object sender, EventArgs e)
    {
        if (mAuth.CurrentUser != null)
        {
            bool signed = (mAuth.CurrentUser != mUser && mAuth.CurrentUser != null);
            if (!signed && mUser != null)
            {
                loginState?.Invoke(false);
            }

            mUser = mAuth.CurrentUser;
            if (signed)
            {
                loginState?.Invoke(true);
            }
        }
    }
    // Start is called before the first frame update
    
    // 한글 및 특수 문자 확인 메소드
    private bool ContainsInvalidCharacters(string str)
    {
        foreach (char c in str)
        {
            // ASCII 코드로 기본 영어 문자와 숫자를 확인하고, 그 외 문자는 모두 제외시킵니다.
            if (!((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')))
            {
                return true; // 한글, 특수문자, 기타 문자가 포함되어 있음
            }
        }
        return false; // 유효한 문자만 포함
    }
    
    // Update is called once per frame
    public async void Create()
    {
        // 이메일에 영어, 숫자 이외의 문자가 있다면 에러 출력
        if (ContainsInvalidCharacters(this.email.text))
        {
            PopupInvalidName.SetActive(true);
            // 팝업 또는 경고 메시지 표시
            Debug.LogWarning("Username contains invalid characters! Please use only English and numbers.");
            return;
        }
        
        string email = this.email.text+"@gmail.com";
        string password = pass.text;
        
        try
        {
            var registrationTask = mAuth.CreateUserWithEmailAndPasswordAsync(email, password);

            await registrationTask;

            if (registrationTask.IsCompleted)
            {
                FirebaseUser newUser = registrationTask.Result.User;
                Debug.Log($"User {this.email.text} registered successfully!");
                Popup.SetActive(true);
                
                ErrorMessage.SetText($"User {this.email.text} registered successfully!");
            }
            else
            {
                Popup.SetActive(true);
                Debug.LogError("Registration failed.");
            }
        }
        catch (Exception ex)
        {
            Popup.SetActive(true);
            Debug.Log($"Error during registration: {ex.Message}");
            ErrorMessage.SetText(ex.Message);
        }
    }
    public async void Login()
    {
        string email = this.email.text+"@gmail.com";
        string password = pass.text;

        try
        {
            var signInTask = mAuth.SignInWithEmailAndPasswordAsync(email, password);

            await signInTask;

            if (signInTask.IsCompleted)
            {
                FirebaseUser newUser = signInTask.Result.User;
                mDataSave.InitializeFirebaseData();
                Debug.Log($"User {newUser.Email} SignIn successfully!");
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main"); // 로그인 성공 시에만 씬 이동
            }
            else
            {
                Popup.SetActive(true);
                Debug.LogError("SignIn failed.");
            }
        }
        catch (Exception ex)
        {
            Popup.SetActive(true);
            Debug.Log($"Error during SignIn: {ex.Message}");
            ErrorMessage.SetText(ex.Message);
        }
    }


    public void Logout()
    {
        mAuth.SignOut();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
    }
}
