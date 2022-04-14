using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DatabaseManagment : MonoBehaviour {

    int left = 0;
    int right = 0;
    bool shouldStartTracking;
    public bool gameOver;
    public string causeOfDefeat;
    public bool gameWon;
    bool gameOverTriggered;
    bool trackingTriggered;
    Score scoreScript;
    public Text scoreText;
    int currentLevel;

    // Use this for initialization
    // SVAKI LOAD/RELOAD SCENE
    void Start () {
        Debug.Log("--NEW SCENE-----------");
        Debug.Log("dbManager start");
        shouldStartTracking = false;
        gameOver = false;
        gameOverTriggered = false;
        trackingTriggered = false;

        currentLevel = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("dbManagment start : currentLevel = " + currentLevel);

        if ( currentLevel == 5 )
        {
            scoreScript = FindObjectOfType<Score>();
            if (scoreScript == null)
            {
                Debug.Log("ERROR : DatabaseManagment.cs couldn't get scoreScipt.");
                scoreText = scoreScript.scoreText;
                if (scoreText == null)
                {
                    Debug.Log("ERROR : scoreText se nije mogao izvuci iz score skripte u dbManagment");
                }
                else if (scoreText.text == "") throw new Exception("GREŠKA : score tekst je prazan");
            }
        }
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Time.timeSinceLevelLoad > 0.1f && !trackingTriggered)
        {
            trackingTriggered = true;
            Debug.Log("starting to track left-right");
            shouldStartTracking = true;
        }

        if ( shouldStartTracking && !gameOver )
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                left++;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                right++;
            }
        }

        if (gameOver && !gameOverTriggered )
        {
            gameOverTriggered = true;
            Debug.Log("dbManagment : game over");
            EndLevelProcedura();
        }
        
	}

    public void EndLevelProcedura()
    {
        int currentUserId = GlobalVariables.id;
        //string currentUserUsername = GlobalVariables.username;  // možda redundantno
        

        // MORAM ZNATI SLJEDECE
        // dali je win ili defeat
        // lijevo desno kolicine
        // što je uzrokovalo defeat
        // koji je igrač ulogiran trenutno
        Debug.Log("game won = " + gameWon);
        Debug.Log("left = " + left + " , right = " + right);

        string conn = "URI=file:E:/Unity/projekt_zavrsni/Assets/Plugins/zavrsni_db.s3db";

        IDbConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();
        Debug.Log("connection state prije prve selekcije : " + dbconn.State);
        dbcmd.CommandText = "SELECT * FROM UserLevel WHERE userid=" + GlobalVariables.id +" AND level=" + currentLevel;  //TODO - preuzmi potrebne podatke iz baze podataka
        Debug.Log(dbcmd.CommandText);

        IDataReader reader = dbcmd.ExecuteReader();

        // DB oznacava da je izvor vrijednosti iz baze podataka

        int lossesDB=-1;
        int gamesDB = -1;
        int lossesFallDB = -1;
        int lossesStaticDB = -1;
        int lossesDynamicDB = -1;
        int leftDB = -1;
        int rightDB = -1;

        short counter = 0;
        while (reader.Read())
        {
            Debug.Log("usli u reader");
            counter++;
            if ( counter > 1 )
            {
                // onda znamo da je error
                Debug.Log("ERROR : trebali smo dobiti samo 1 UserLevel zapis a dobili smo više!");
                throw new Exception("ERROR : trebali smo dobiti samo 1 UserLevel zapis a dobili smo više!");
            }
            //Debug.Log("connection state nakon citanja prve selekcije : " + dbconn.State);
            //Debug.Log(reader.GetValue(0) + " , " + reader.GetValue(0).GetType());
            //Debug.Log(reader.GetValue(1) + " , " + reader.GetValue(1).GetType());
            //Debug.Log(reader.GetValue(2) + " , " + reader.GetValue(2).GetType());
            //Debug.Log(reader.GetValue(3) + " , " + reader.GetValue(3).GetType());
            //Debug.Log(reader.GetValue(4) + " , " + reader.GetValue(4).GetType());
            //Debug.Log(reader.GetValue(5) + " , " + reader.GetValue(5).GetType());
            //Debug.Log(reader.GetValue(6) + " , " + reader.GetValue(6).GetType());
            //Debug.Log(reader.GetValue(7) + " , " + reader.GetValue(7).GetType());
            //Debug.Log(reader.GetValue(8) + " , " + reader.GetValue(8).GetType());
            // ako smo dosli ovdje , onda je ok dohvaćen podatak

            // PROVJERITI JELI INDEKSIRANO KAKO SPADA
            lossesDB = reader.GetInt32(2);            
            gamesDB = reader.GetInt32(3);
            lossesFallDB = reader.GetInt32(4);
            lossesStaticDB = reader.GetInt32(5);
            lossesDynamicDB = reader.GetInt32(6);
            leftDB = reader.GetInt32(7);
            rightDB = reader.GetInt32(8);

            Debug.Log("currentLevel: " + currentLevel + 
                ",GlobalVariables.id: " + GlobalVariables.id + 
                ",lossesDB: " + lossesDB +
                ",gamesDB: " + gamesDB +
                ",lossesFallDB: " + lossesFallDB + 
                ",lossesStaticDB: " + lossesStaticDB +
                ",lossesDynamicDB: " + lossesDynamicDB +
                ",leftDB: " + leftDB + 
                ",rightDB: " + rightDB);

            leftDB += left;
            rightDB += right;
            gamesDB++;
            // left i right spremni za update z bazi podataka

            // za svaki zapis ... napravi ovo
            


        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        //dbcmd.Cancel();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        
        // ako poraz
        if ( !gameWon )
        {
            Debug.Log("cause of defeat : " + causeOfDefeat);
            // inkrementiraj loss counter za level u bazi podataka
            lossesDB++;

            // inkrementiraj counter uzroka poraza u bazi podataka
            if (causeOfDefeat == "Fall") lossesFallDB++;
            else if (causeOfDefeat == "Obstacle") lossesStaticDB++;
            else if (causeOfDefeat == "Ball") lossesDynamicDB++;
            else Debug.Log("ERROR : neispravan causeOfDefeat string");
            
        }
                
        if (currentLevel == 5)
        {
            // ODAVDE NADALJE IF 
            Debug.Log("scoreText.text taman prije pokušaja parsanja u integer highscore : " + scoreText.text);
            int newScore = Int32.Parse(scoreText.text);
            int highscore = -1;

            // dohvati highscore ulogiranog igrača iz baze podataka
            dbconn = new SqliteConnection(conn);
            dbconn.Open();
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = "SELECT id,username,password,highscore FROM User WHERE id = " + currentUserId + ";";  //TODO - preuzmi potrebne podatke iz baze podataka

            Debug.Log(dbcmd.CommandText);
            reader = dbcmd.ExecuteReader();

            Debug.Log("connection state dbconn konekcije prije errora : " + dbconn.State);
            short count = 0;
                while (reader.Read())
                {
                    count++;
                    if (count > 1)
                    {
                        Debug.Log("ERROR : count pri dohvati highscora veci od 1");
                        throw new Exception("ERROR : count pri dohvati highscora veci od 1");
                    }
                    
                    highscore = reader.GetInt32(3);
                }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
            
            Debug.Log("dohvacen zadnji highscore iz baze : " + highscore);
                // usporedi sa zabilježenim scorom
                if (newScore > highscore)
                {
                    dbconn = new SqliteConnection(conn);
                    dbconn.Open();
                    // upload u bazu podataka novi highscore
                    //UPDATE table_name
                    //SET column1 = value1, column2 = value2...., columnN = valueN
                    //WHERE[condition];
                    dbcmd = dbconn.CreateCommand();
                    dbcmd.CommandText = "UPDATE User SET highscore = " + newScore + " WHERE id = " + currentUserId + ";";
                    Debug.Log(dbcmd.CommandText);
                    dbcmd.ExecuteNonQuery();
                    dbcmd.Dispose();
                    dbcmd = null;
                    dbconn.Close();
                    dbconn = null;
                }

        }


        //Debug.Log("connection state prije UPDATE : " + dbconn.State);
        dbconn = new SqliteConnection(conn);
        dbconn.Open();
        dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = "UPDATE UserLevel SET " +
            "losses = " + lossesDB + "," +
            "games = " + gamesDB + "," +
            "lossesFromFall = " + lossesFallDB + "," +
            "lossesFromStatic = " + lossesStaticDB + "," +
            "lossesFromDynamic = " + lossesDynamicDB + "," +
            "left = " + leftDB + "," +
            "right = " + rightDB +
            " WHERE userid = " + currentUserId +
            " AND level = " + currentLevel + ";";
        Debug.Log(dbcmd.CommandText);
        dbcmd.ExecuteNonQuery();
        Debug.Log("connection state nakon egzekucije UPDATE naredbe i prije zatvaranja dbconn konekcije : " + dbconn.State);

        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}
