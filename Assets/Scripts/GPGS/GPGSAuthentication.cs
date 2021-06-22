using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GPGSAuthentication : MonoBehaviour
{
    /// <summary>
    /// Platform for Google Play Games Services. MUST BE INITIALIZED ONLY ONCE
    /// </summary>
    public static PlayGamesPlatform platform;

    // Start is called before the first frame update
    void Start()
    {
        if (platform == null)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;

            platform = PlayGamesPlatform.Activate();
        }

        Social.Active.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("LOGGED IN SUCCESFULLY");
            }
            else
            {
                Debug.Log("FAILED TO LOG IN");
            }
        });
    }
}
