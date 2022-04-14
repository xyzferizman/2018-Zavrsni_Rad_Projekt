using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
using System.Collections;

public class CreateAccountGUIManager : MonoBehaviour {

    public GameObject errorText;
    public GameObject successText;

    public float textDelay = 5f;
    public InputField usernameIF;
    public InputField passwordIF;
    bool errorOccured ;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && usernameIF.isFocused)
        {
            passwordIF.Select();
        }

        else if (Input.GetKeyDown(KeyCode.Tab) && passwordIF.isFocused)
        {
            usernameIF.Select();
        }

        if (Input.GetKeyDown(KeyCode.Return)) TryToCreateAccount();
    }

    public void ReturnToLogin()
    {
        Debug.Log("ReturnToLogin()");
        this.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
        Debug.Log("this.gameObject.name :" + this.gameObject.name + " ,activeInHierarchy: " + this.gameObject.activeInHierarchy + " ,activeSelf: " + gameObject.activeSelf);
        gameObject.GetComponent<CreateAccountGUIManager>().enabled = false;
        Debug.Log(gameObject.GetComponent<CreateAccountGUIManager>().enabled);
    }

    public void TryToCreateAccount()
    {
        errorOccured = false;
        Debug.Log("TryToCreateAccount()");

        string username = usernameIF.text;
        string password = passwordIF.text;
        
        // dohvati podatke iz baze
        string conn = "URI=file:E:/Unity/projekt_zavrsni/Assets/Plugins/zavrsni_db.s3db"; //Path to database.
        IDbConnection dbconn;
        // uklonio "redundant" cast (IDbConnection)

        dbconn = new SqliteConnection(conn);
        Debug.Log(dbconn.Database);

        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = "SELECT Username FROM User";
        
        IDataReader reader = dbcmd.ExecuteReader();
        
        while (reader.Read())
        {
            string currentReadUsername = reader.GetString(0);
            Debug.Log("currentReadUsername is:" + currentReadUsername);

            // ako vec postoji username ili email baci error
            if ( username.Equals(currentReadUsername) )
            {
                errorOccured = true;
                StartCoroutine(Error());   // start korutinu za error text
                break;
            } 
        }
        reader.Close();
        reader = null;

        // ako nema istog username-a u bazi podataka
        if (!errorOccured)
        {
            // postavi novi acc u bazu
            dbcmd.CommandText = "INSERT INTO User (username,password,highscore) VALUES('" + username + "','" + password + "', 0 )";
            Debug.Log("uneseno u db usera:" + dbcmd.ExecuteNonQuery());

            // dohvatiti ID novonapravljenog usera
            dbcmd.CommandText = "SELECT id FROM User WHERE username = '" + username + "'";
            reader = dbcmd.ExecuteReader();
            int newUserId = -1;
            while (reader.Read())
            {

                newUserId = reader.GetInt32(0);
                Debug.Log("id novog usera dohvacen ... " + newUserId);
            }


            reader.Close();
            reader = null;

            // popuniti userLevel tablicu sa podacima novog usera za svaki level
            for (int i = 1; i <= 5; ++i)
            {
                // po defaultu su sve vrijednosti 0
                dbcmd.CommandText = "INSERT INTO UserLevel (level,userid) VALUES (" + i + "," + newUserId + ");";
                Debug.Log("uneseno u db levela ,a level " + i + " , executeNonQuery: " + dbcmd.ExecuteNonQuery());
            }

            // daj poruku korisniku o uspjehu
            StartCoroutine(Success());
        }
        

        #region maintenance db konekcije
        //reader.Close();
        //reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        #endregion
        Debug.Log("zatvoreno sve u vezi baze podataka nakon stvaranja acca");
        
    }

    IEnumerator Error()
    {
        if (successText.activeInHierarchy) successText.SetActive(false);
        errorText.SetActive(true);
        yield return new WaitForSecondsRealtime(textDelay);
        errorText.SetActive(false);
    }

    IEnumerator Success()
    {
        if (errorText.activeInHierarchy) errorText.SetActive(false);
        successText.SetActive(true);
        yield return new WaitForSecondsRealtime(textDelay);
        successText.SetActive(false);
    }
}
