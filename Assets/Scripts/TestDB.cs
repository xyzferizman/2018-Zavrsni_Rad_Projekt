using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.UI;
using System.Collections;

public class TestDB : MonoBehaviour {

    //bool mozeDalje = false;
    public Text scoreText;
    public Text debugText1;
    public Text debugText2;
    public Text debugText3;
    int counter = 0;
    
    // Use this for initialization
    void Start () {

        
        Debug.Log("Application.dataPath:" + Application.dataPath);

        // BUILD VERZIJA RADILA PROBLEME ZBOG Application.dataPath ... STOGA ZAMIJENIO HARDCODED VERZIJOM CONNSTRINGA I SAD RADI
        string conn = "URI=file:E:/Unity/projekt_zavrsni/Assets/Plugins/my_database.s3db"; //Path to database.
        IDbConnection dbconn;
        // uklonio "redundant" cast (IDbConnection)
        dbconn = new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        debugText1.text = "connection state: " + dbconn.State;
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT id,Username,Password " + "FROM Users";
        dbcmd.CommandText = sqlQuery;
        debugText2.text = "db command text: " + dbcmd.CommandText;
        
        IDataReader reader = dbcmd.ExecuteReader();
        debugText3.text = ("reader == null : " + (reader == null));

        while (reader.Read())
        {
           
            int id = reader.GetInt32(0);
            string username = reader.GetString(1);
            string password = reader.GetString(2);

            if (counter++ == 0)
            {
                //debugText3.text = "value= " + id + ",  username =" + username + ",  password =" + password;
                scoreText.text = "value= " + id + ",  username =" + username + ",  password =" + password;
            }

            Debug.Log("value= " + id + ",  username =" + username + ",  password =" + password );
            
            //StartCoroutine(Cekanje());
            // POKUSAJ STOPIRANJA PROGRAMA KROZ RADNO CEKANJE KOJI SJEBAVA UNITY I MORAM RADIT END TASK NA UNITY
            //while(true)
            //{
            //    if (mozeDalje) break;
            //}
            Debug.Log("CHECKPOINT");

        }


        #region dodavanje novog
        // pokusaj dodavanja podatka iz programa
        //dbcmd = dbconn.CreateCommand();
        //sqlQuery = "INSERT INTO Users VALUES(4,'Marko','6543')";
        //dbcmd.CommandText = sqlQuery;
        //int rezultat = dbcmd.ExecuteNonQuery();

        //Debug.Log("Unesen podatak , rezultat non-querya je : " + rezultat);

        //Debug.Log("\nPonovan pokušaj čitanja\n");

        //dbcmd = dbconn.CreateCommand();
        //sqlQuery = "SELECT id,Username,Password " + "FROM Users";
        //dbcmd.CommandText = sqlQuery;
        //reader = dbcmd.ExecuteReader();

        //while (reader.Read())
        //{
        //    int id = reader.GetInt32(0);
        //    string username = reader.GetString(1);
        //    string password = reader.GetString(2);

        //    Debug.Log("value= " + id + ",  username =" + username + ",  password =" + password);
        //}

        //dbcmd = dbconn.CreateCommand();
        //sqlQuery = "DELETE FROM Users WHERE id=4";
        //dbcmd.CommandText = sqlQuery;
        //dbcmd.ExecuteNonQuery();
        #endregion

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
    }

    IEnumerator Cekanje()
    {
        Debug.Log("korutina pocela");
        yield return new WaitForSecondsRealtime(2f);
        Debug.Log("korutina nastavila");
        //mozeDalje = true;
    }


}
