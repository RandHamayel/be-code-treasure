using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AuthView : MonoBehaviour

{
    [Header("Service")]
     public AvatarPicker avatarPicker;
    public AuthService authService;

    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject registerPanel;

    [Header("Login Inputs")]
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;

    [Header("Register Inputs")]
    public TMP_InputField registerNameInput;
    public TMP_InputField registerEmailInput;
    public TMP_InputField registerPasswordInput;

     [Header("Register Avatar")]
    public string selectedAvatarId; // set by AvatarItem click

    /* ===================== COMMON ===================== */
    public TextMeshProUGUI errorText;



    private void Start()
    {
        ShowLogin();
    }
    /* ===================== PANEL SWITCHING ===================== */

    public void ShowLogin()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        errorText.text = "";
    }

    public void ShowRegister()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        avatarPicker.LoadAvatars();
        errorText.text = "";
    }

    /* ===================== LOGIN ===================== */
     public void OnLoginClicked()
    {
        errorText.text = "";

        if (string.IsNullOrEmpty(loginEmailInput.text) ||
            string.IsNullOrEmpty(loginPasswordInput.text))
        {
            errorText.text = "Please fill all fields";
            return;
        }

        LoginRequest req = new LoginRequest(
            loginEmailInput.text,
            loginPasswordInput.text
        );

        authService.Login(
            req,
            OnLoginSuccess,
            OnAuthError
        );
    }

    private void OnLoginSuccess(UserData user)
    {
        Debug.Log($"Login success. User type: {user.user_type}");

        if (user.user_type == "ADMIN")
        {
            SceneManager.LoadScene("AdminDashboardScene");
        }
        else if (user.user_type == "PLAYER")
        {
            SceneManager.LoadScene("MapScene");
            // or AvatarScene ONLY if backend allows changing avatar later
        }
        else
        {
            errorText.text = "Unknown user type";
        }
    }

    /* ===================== REGISTER ===================== */

    public void OnRegisterClicked() {

         if (string.IsNullOrEmpty(selectedAvatarId))
    {
        errorText.text = "Please select an avatar";
        return;
    }

    var req = new RegisterRequest(
        registerNameInput.text,
        registerEmailInput.text,
        registerPasswordInput.text,
        selectedAvatarId
    );

     authService.Register(
        req,
        () => ShowLogin(),
        OnAuthError
     );
    }
      /* ===================== AVATAR SELECTION ===================== */

    // Called from AvatarItem when clicked
    public void OnAvatarSelected(string avatarId)
    {
        selectedAvatarId = avatarId;
        Debug.Log("Selected avatar id: " + avatarId);
    }

    /* ===================== ERROR ===================== */

    private void OnAuthError(string err)
    {
        Debug.LogError(err);
        errorText.text = err;
    }

}
