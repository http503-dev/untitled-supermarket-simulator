using Firebase.Database;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using TMPro;
using Firebase;
using Firebase.Extensions;
using Object = System.Object;
using UnityEngine.SceneManagement;

/// <summary>
/// Database Manager Script
/// </summary>
public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;
    
    // Database reference variables
    private DatabaseReference reference;
    private FirebaseAuth auth;
    private FirebaseUser user;
    
    // UI Screens
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject signupScreen;
    [SerializeField] private GameObject loginScreen;
    [SerializeField] private GameObject resetPasswordScreen;
    
    // Signup
    [SerializeField] private TMP_InputField signupUsernameInputField;
    [SerializeField] private TMP_InputField signupEmailInputField;
    [SerializeField] private TMP_InputField signupPasswordInputField;
    [SerializeField] private TMP_InputField signupConfirmPasswordInputField;
    [SerializeField] private TextMeshProUGUI signupValidationText;
    
    // Login
    [SerializeField] private TMP_InputField loginEmailInputField;
    [SerializeField] private TMP_InputField loginPasswordInputField;
    [SerializeField] private TextMeshProUGUI loginValidationText;
    
    // Reset Password
    [SerializeField] private TMP_InputField resetPasswordEmailInputField;
    [SerializeField] private TextMeshProUGUI resetPasswordValidationText;

    public LobbyMenu lobbyMenu;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        
        auth.SignOut();
        
        // if (auth.CurrentUser != null)
        // {
        //     // update last login
        //     Dictionary<string, Object> childUpdates = new Dictionary<string, Object>();
        //     childUpdates["players/" + auth.CurrentUser.UserId + "/lastLoginDate/"] = ConvertNowToTimeStamp();
        //
        //     reference.UpdateChildrenAsync(childUpdates);
        //     
        //     // skips login screen and goes to home screen if user is already signed in
        //     loginScreen.SetActive(false);
        //     signupScreen.SetActive(false);
        //     homeScreen.SetActive(true);
        //     ResetFields();
        // }

        AuthStateChanged(this, null);
        reference.Child("players").ValueChanged += HandlePlayerValueChanged;
    }
    
    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    // Handle removing subscription and reference to the Auth instance.
    void OnDestroy() {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    void HandlePlayerValueChanged(object send, ValueChangedEventArgs
        args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
    }
    
    // Account Interaction Functions

    /// <summary>
    /// Account Sign Up
    /// </summary>
    public void SignUp()  
    {
        Debug.Log("Submit Values (Signup)");
        if (!string.IsNullOrWhiteSpace(signupUsernameInputField.text) && 
            !string.IsNullOrWhiteSpace(signupEmailInputField.text) && 
            !string.IsNullOrWhiteSpace(signupPasswordInputField.text) && 
            !string.IsNullOrWhiteSpace(signupConfirmPasswordInputField.text)) // checks if all fields has been entered
        {
            if (signupPasswordInputField.text == signupConfirmPasswordInputField.text) // checks if passwords match
            {
                string username = signupUsernameInputField.text;
                string email = signupEmailInputField.text;
                string password = signupPasswordInputField.text;
                
                auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
                if (task.IsCanceled) {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted) {
                    signupValidationText.text = HandleAuthExceptions(task.Exception);

                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                if (task.IsCompleted)
                {
                    signupScreen.SetActive(false);
                    homeScreen.SetActive(true);
                    auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
                        if (task.IsCanceled) {
                            Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                            return;
                        }
                        if (task.IsFaulted)
                        {
                            Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                            string errortext = HandleAuthExceptions(task.Exception).ToString();
                            return;
                        }
                        Firebase.Auth.AuthResult result = task.Result;
                        Debug.LogFormat("User signed in successfully: {0} ({1})",
                            result.User.DisplayName, result.User.UserId);
                        WriteNewPlayer(
                                result.User.UserId,
                                username,
                                "", 
                                false,
                                0,
                                0,
                                0,
                                0,
                                0f,
                                0,
                                0,
                                0
                            );
                        ResetFields();
                    });
                }

                // Firebase user has been created.
                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("auth user created successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
                });
            }
            else
            {
                signupValidationText.text = "Passwords do not match";
            }
        }
        else
        {
            signupValidationText.text = "Please fill all fields";
        }
    }

    /// <summary>
    /// Account Log In
    /// </summary>
    public void Login()
    {
        Debug.Log("Submit Values (Login)");
        if (!string.IsNullOrWhiteSpace(loginEmailInputField.text) &&
            !string.IsNullOrWhiteSpace(loginPasswordInputField.text)) // checks if all fields has been entered
        {
            auth.SignInWithEmailAndPasswordAsync(loginEmailInputField.text, loginPasswordInputField.text)
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                        return;
                    }

                    if (task.IsFaulted)
                    {
                        loginValidationText.text = HandleAuthExceptions(task.Exception);
                        Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                        return;
                    }

                    if (task.IsCompleted)
                    {
                        loginScreen.SetActive(false);
                        homeScreen.SetActive(true);
                        ResetFields();
                    }

                    Firebase.Auth.AuthResult result = task.Result;
                    Debug.LogFormat("User signed in successfully: {0} ({1})",
                        result.User.DisplayName, result.User.UserId);
                });
        }
        else
        {
            loginValidationText.text = "Please fill all fields";
        }
    }

    /// <summary>
    /// Sends Email to the User to reset their password
    /// </summary>
    public void ResetPassword()
    {
        if (!string.IsNullOrWhiteSpace(resetPasswordEmailInputField.text)) // checks if all fields has been entered
        {
            auth.SendPasswordResetEmailAsync(resetPasswordEmailInputField.text).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    resetPasswordValidationText.text = HandleAuthExceptions(task.Exception);
                    Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                    return;
                }

                if (task.IsCompleted)
                {
                    Debug.Log("Password reset email sent successfully.");
                    loginScreen.SetActive(true);
                    resetPasswordScreen.SetActive(false);
                    ResetFields();
                }
            });
        }
        else
        {
            resetPasswordValidationText.text = "Please fill all fields";
        }
    }
    
    private void WriteNewPlayer(
        string uid,
        string name,
        string storeName,
        bool isAdmin,
        int shiftsCompleted,
        int customersServed,
        int itemsScanned,
        int mistakesMade,
        float profitsEarned,
        int highScore,
        int proficiencyScore,
        int averageTimePerTransaction)
    {
        PlayerData.MistakesMade mistakes = new PlayerData.MistakesMade(
            0,
            0,
            0,
            0,
            0,
            0
        );
        
        PlayerData player = new PlayerData(
            name,
            storeName,
            isAdmin,
            shiftsCompleted,
            customersServed,
            itemsScanned,
            mistakes, 
            profitsEarned,
            highScore,
            proficiencyScore,
            averageTimePerTransaction
        );
        
        Leaderboard leaderboardEntry = new Leaderboard(name, highScore, profitsEarned, shiftsCompleted);
        
        string playerJson = JsonUtility.ToJson(player);
        string leaderboardJson = JsonUtility.ToJson(leaderboardEntry);
        Debug.Log(playerJson);
        Debug.Log(leaderboardJson);
        reference.Child("players").Child(uid).SetRawJsonValueAsync(playerJson);
        reference.Child("leaderboard").Child(uid).SetRawJsonValueAsync(leaderboardJson);
    }

    /// <summary>
    /// resets text fields
    /// </summary>
    public void ResetFields()
    {
        signupUsernameInputField.text = "";
        signupEmailInputField.text = "";
        signupPasswordInputField.text = "";
        signupConfirmPasswordInputField.text = "";
        signupValidationText.text = "";
        loginEmailInputField.text = "";
        loginPasswordInputField.text = "";
        loginValidationText.text = "";
        resetPasswordValidationText.text = "";
        resetPasswordEmailInputField.text = "";
    }
    
    //Account Validation Functions
    public string HandleAuthExceptions(System.AggregateException e)
    {
        string validationText = "";

        if (e != null)
        {
            FirebaseException firebaseEx = e.GetBaseException() as FirebaseException;

            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            Debug.LogError("Error in auth.... error code: " + errorCode);

            switch (errorCode)
            {
                case AuthError.WrongPassword:
                    validationText += "Wrong Password";
                    break;
                case AuthError.UserNotFound:
                    validationText += "User does not exist, please create an account";
                    break;
                case AuthError.InvalidEmail:
                    validationText += "Invalid Email";
                    break;
                case AuthError.WeakPassword:
                    validationText += "Weak Password (Minimum 8 Characters, requires upper case letters, lower case letters, numbers)";
                    break;
                case AuthError.EmailAlreadyInUse:
                    validationText += "Email is already in use, try logging in";
                    break;
                case AuthError.UserMismatch:
                    validationText += "User Mismatch";
                    break;
                case AuthError.Failure:
                    validationText += "Failed to login...";
                    break;
                default:
                    validationText += "Issue in authentication: " + errorCode;
                    break;
            }
        }
        return validationText;
    }
    
    // Game session listen method
    public async Task GameSessionListen()
    {
        var sessionRef = reference.Child($"sessions/{auth.CurrentUser.UserId}");

        try
        {
            var snapshot = await sessionRef.GetValueAsync();
            if (snapshot == null)
            {
                Debug.LogError("Snapshot is null.");
                return;
            }

            Debug.Log("Session snapshot received.");

            if (!snapshot.Exists)
            {
                Debug.Log("Session does not exist. Creating it...");
        
                SessionData sessionData = new SessionData(
                    false,
                    false,
                    ConvertNowToTimeStamp(),
                    new SessionData.Customer()
                );

                string json = JsonUtility.ToJson(sessionData);

                Debug.Log(auth.CurrentUser.UserId);
                await reference.Child("sessions").Child(auth.CurrentUser.UserId).SetRawJsonValueAsync(json);
                
                Debug.Log("Session created successfully.");
            }
            else
            {
                Debug.Log("Session already exists.");
            }

            // Listen for data changes
            sessionRef.ValueChanged += (sender, args) =>
            {
                if (args.Snapshot.Exists)
                {
                    Debug.Log("Session data changed: " + args.Snapshot.GetRawJsonValue());
                    Debug.Log(args.Snapshot.Child("webConnected"));
                    Debug.Log(args.Snapshot.Child("webConnected").Value.ToString());
                    bool webConnected = bool.Parse(args.Snapshot.Child("webConnected").Value.ToString());
                    lobbyMenu.ChangeWebReadyStatus(webConnected);
                }
                else
                {
                    Debug.Log("Session data has been deleted.");
                }
            };
        }
        catch (FirebaseException firebaseError)
        {
            Debug.LogError("Firebase Error: " + firebaseError.Message);
        }
        catch (Exception error)
        {
            Debug.LogError("Error checking or creating session: " + error.Message);
        }
    }

    public Task UpdateVRReadyStatus(bool status)
    {
        var updates = new Dictionary<string, object>
        {
            { $"/sessions/{auth.CurrentUser.UserId}/gameConnected", status }
        };

        return reference.UpdateChildrenAsync(updates);
    }

    public async void QuitGame()
    {
        if (auth.CurrentUser != null)
        {
            string userId = auth.CurrentUser.UserId;

            try
            {
                // Reference to the UID node under sessions
                var sessionRef = reference.Child($"sessions/{userId}");
                await sessionRef.RemoveValueAsync();

                Debug.Log($"Session for user {userId} deleted successfully.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error deleting session for user {userId}: {e.Message}");
            }
        }

        // Exit the application
        Application.Quit();
    }

    /// <summary>
    /// Converts current time to epoch timestamp
    /// </summary>
    /// <returns></returns>
    public string ConvertNowToTimeStamp()
    {
        DateTimeOffset dto = new DateTimeOffset(DateTime.UtcNow);
        // Get the unix timestamp in seconds
        return dto.ToUnixTimeSeconds().ToString();
    }
}
