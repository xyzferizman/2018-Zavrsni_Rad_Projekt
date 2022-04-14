using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System.Collections;

public class LoginScreenGUIManager : MonoBehaviour {

    public Text errorText;  // MOZDA NEPOVEZANO
    public InputField username;
    public InputField password;
    public float textDelay = 5f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && username.isFocused)
        {
            password.Select();
        }

        else if (Input.GetKeyDown(KeyCode.Tab) && password.isFocused)
        {
            username.Select();
        }

        if (Input.GetKeyDown(KeyCode.Return)) tryToLogin();
    }

    public void tryToLogin()
    {
        Debug.Log("tryToLogin()");

        string tryUserName = username.text;
        string tryPassWord = password.text;
        Debug.Log("tryUserName = '" + tryUserName + "' , tryPassWord = '" + tryPassWord + "'");

        

        
        
        string passwordFromDatabase = "";
        short counter = 0;
        int idFromDatabase = -1;

        string conn = "URI=file:E:/Unity/projekt_zavrsni/Assets/Plugins/zavrsni_db.s3db";
        IDbConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = "SELECT id,username,password FROM User WHERE username = '" + tryUserName + "'";  //preuzmi potrebne podatke iz baze podataka
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            counter++;
            if ( counter > 1 )
            {
                Debug.Log("POGREŠKA : SQL upit koji je trebao dobiti 1 zapis dobio ih je više. LoginScreenGUIManager skripta");
                return;
            }
            Debug.Log(reader.GetValue(0) + " " + reader.GetValue(1) + " " + reader.GetValue(2));
            passwordFromDatabase = reader.GetString(2);
            idFromDatabase = reader.GetInt32(0);
            Debug.Log("izvuko password from database : " + passwordFromDatabase);
        }
        Debug.Log("prije reader.close u tryToLogin");
        reader.Close();
        reader = null;
        Debug.Log("poslije reader.close u tryToLogin");
        #region maintenance db konekcije
        //reader.Close();
        //reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        #endregion
        Debug.Log("na kraju tryToLogin");

        if ( counter == 0 )
        {
            Debug.Log("POGREŠKA : SQL upit nije našao zapis sa danim username-om. LoginScreenGUIManager skripta");
            StartCoroutine(UsernameNijePronadjen());
            return;
        }
        // u ovom trenutku imamo row iz baze sa tim usernameom , taj row postoji i jedinstven je
        // sada provjeravamo password
        if (password.text.Equals(passwordFromDatabase))
        {
            // SADA JE LOGIN USPJEŠAN
            GlobalVariables.id = idFromDatabase;
            GlobalVariables.username = tryUserName;
            Debug.Log("u global variables spremljeni : id = " + GlobalVariables.id + " , username = " + GlobalVariables.username);
            //errorText.text = "DevEnv : USPJEŠAN LOGIN!!";
            SceneManager.LoadScene(11);
        }
        else
        {
            StartCoroutine(KrivaLozinka());
        }

        
    }

    bool firstErrActive = false;
    bool secondErrActive = false;

    IEnumerator UsernameNijePronadjen()
    {
        firstErrActive = true;
        secondErrActive = false;
        errorText.text = "Pogreška : Korisničko ime nije pronađeno u bazi podataka.";
        yield return new WaitForSecondsRealtime(textDelay);
        if (secondErrActive) yield break;
        errorText.text = "";
    }

    IEnumerator KrivaLozinka()
    {
        firstErrActive = false;
        secondErrActive = true;
        errorText.text = "Pogreška : Kriva lozinka.";
        yield return new WaitForSecondsRealtime(textDelay);
        if (firstErrActive) yield break;
        errorText.text = "";
    }

    public void createAccountGUI()
    {
        // ucitaj scenu za kreaciju accounta
        Debug.Log("createAccountGUI()");
        SceneManager.LoadScene(7);
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame()");
        Application.Quit();
    }
}
