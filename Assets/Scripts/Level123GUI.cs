using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level123GUI : MonoBehaviour {
    
    int currentBuildIndex;
    int currentLevel;
    public Text brPadova;
    public Text brUdaracaUKutiju;
    public Text postotatGubljenjaOdPadova;
    public Text postotatGubljenjaOdKutije;
    public Text postotakPobjeda;
    public Text postotakKretanjaUlijevo;
    public Text postotakKretanjaUdesno;   
    
    private void Start()
    {
        currentLevel = -1;
        currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

        string conn = "URI=file:E:/Unity/projekt_zavrsni/Assets/Plugins/zavrsni_db.s3db";
        IDbConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();
        
        // LEVEL 1 GUI ima buildIndex 9
        if (currentBuildIndex == 9)
        {
            //ucitaj podatke igraca za lvl 1
            dbcmd.CommandText = "SELECT * FROM UserLevel WHERE userid=" + GlobalVariables.id + " AND level=1";
            currentLevel = 1;
        }
        // LEVEL 2 GUI ima buildIndex 12
        else if (currentBuildIndex == 12)
        {
            //ucitaj podatke igraca za lvl 2
            dbcmd.CommandText = "SELECT * FROM UserLevel WHERE userid=" + GlobalVariables.id + " AND level=2";
            currentLevel = 2;
        }
        // LEVEL 3 GUI ima buildIndex 13
        else if (currentBuildIndex == 13)
        {
            //ucitaj podatke igraca za lvl3 
            dbcmd.CommandText = "SELECT * FROM UserLevel WHERE userid=" + GlobalVariables.id + " AND level=3";
            currentLevel = 3;
        }
        else
        {
            // u ovom slucaju error
            Debug.Log("ERROR : neispravan build Index u level123GUI skripti");
            return;
        }

        IDataReader reader = dbcmd.ExecuteReader();
        short counter = 0;
        int lossesDB = -1;
        int gamesDB = -1;
        int lossesFallDB = -1;
        int lossesStaticDB = -1;
        //int lossesDynamicDB = -1;
        int leftDB = -1;
        int rightDB = -1;

        while (reader.Read())
        {
            Debug.Log("usli u reader");
            counter++;
            if (counter > 1)
            {
                // onda znamo da je error
                Debug.Log("ERROR : trebali smo dobiti samo 1 UserLevel zapis a dobili smo više!");
                return;
            }
            Debug.Log("connection state nakon citanja prve selekcije : " + dbconn.State);
            Debug.Log(reader.GetValue(0) + " , " + reader.GetValue(0).GetType());
            Debug.Log(reader.GetValue(1) + " , " + reader.GetValue(1).GetType());
            Debug.Log(reader.GetValue(2) + " , " + reader.GetValue(2).GetType());
            Debug.Log(reader.GetValue(3) + " , " + reader.GetValue(3).GetType());
            Debug.Log(reader.GetValue(4) + " , " + reader.GetValue(4).GetType());
            Debug.Log(reader.GetValue(5) + " , " + reader.GetValue(5).GetType());
            Debug.Log(reader.GetValue(6) + " , " + reader.GetValue(6).GetType());
            Debug.Log(reader.GetValue(7) + " , " + reader.GetValue(7).GetType());
            Debug.Log(reader.GetValue(8) + " , " + reader.GetValue(8).GetType());
            // ako smo dosli ovdje , onda je ok dohvaćen podatak
            
            lossesDB = reader.GetInt32(2);
            gamesDB = reader.GetInt32(3);
            lossesFallDB = reader.GetInt32(4);
            lossesStaticDB = reader.GetInt32(5);
            //lossesDynamicDB = reader.GetInt32(6);
            leftDB = reader.GetInt32(7);
            rightDB = reader.GetInt32(8);

            Debug.Log("currentLevel: " + currentLevel +
                ",GlobalVariables.id: " + GlobalVariables.id +
                ",lossesDB: " + lossesDB +
                ",gamesDB: " + gamesDB +
                ",lossesFallDB: " + lossesFallDB +
                ",lossesStaticDB: " + lossesStaticDB +
                ",leftDB: " + leftDB +
                ",rightDB: " + rightDB);
        }

        float fallLossPerc = ((float)100 * lossesFallDB / lossesDB);
        float staticLossPerc = ((float)100 * lossesStaticDB / lossesDB);
        float winratePerc = ((float)100 * (gamesDB - lossesDB) / gamesDB);
        float leftPerc = ((float)100 * leftDB / (leftDB + rightDB));
        float rightPerc = ((float)100 * rightDB / (leftDB + rightDB));

        if (lossesDB == 0)
        {
            fallLossPerc = 0;
            staticLossPerc = 0;
        }
        if (gamesDB == 0) winratePerc = 0;
        if ((leftDB + rightDB) == 0)
        {
            leftPerc = 0;
            rightPerc = 0;
        }

        brPadova.text = lossesFallDB.ToString();
        brUdaracaUKutiju.text = lossesStaticDB.ToString();
        postotatGubljenjaOdPadova.text = ((int)fallLossPerc).ToString() + "%";
        postotatGubljenjaOdKutije.text = ((int)staticLossPerc).ToString() + "%";
        postotakPobjeda.text = ((int)winratePerc).ToString() + "%";
        postotakKretanjaUlijevo.text = ((int)leftPerc).ToString() + "%";
        postotakKretanjaUdesno.text = ((int)rightPerc).ToString() + "%";

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        //dbcmd.Cancel();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public void returnToLevelList()
    {
        SceneManager.LoadScene(6);
    }
    
}
