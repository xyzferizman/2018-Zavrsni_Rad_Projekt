using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;


public class highscoreGUImanager : MonoBehaviour {

    public Text greenText;
    public Text redText;

    public Text[] imena;
    public Text[] bodovi;

    private void Start()
    {
        greenText.enabled = false;
        redText.enabled = false;
        bool igracUTop5 = false;
        // preuzeti TOP 5
        string conn = "URI=file:E:/Unity/projekt_zavrsni/Assets/Plugins/zavrsni_db.s3db";
        IDbConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        dbcmd.CommandText = "select username,highscore "+
                               "from User "+
                                   "order by highscore desc ";

        IDataReader reader = dbcmd.ExecuteReader();

        string username;
        int highscore;
        string highscoreString;

        int indexIgracNadjen=-1;

        short counter = 0;
        while(reader.Read())
        {
            
            username = reader.GetString(0);
            highscore = reader.GetInt32(1);
            highscoreString = highscore.ToString();

            //Debug.Log(!igracUTop5 + " , " + GlobalVariables.username + " , " + username);
            if (username.Equals(GlobalVariables.username)) indexIgracNadjen = counter+1;
            
            if (counter < 5)
            {
                imena[counter].text = username;
                bodovi[counter].text = highscoreString;
            }
            

            
            counter++;
        }

        if (indexIgracNadjen == -1)
        {
            throw new Exception("igrac nije pronađen a trebao je biti !!");
        }
        else if (indexIgracNadjen <= 5 )
        {
            greenText.enabled = true;
            greenText.text = "Bravo! Nalazite se u highscore tablici na " + indexIgracNadjen + ".mjestu.";
        }
        else if (indexIgracNadjen > 5 )
        {
            redText.enabled = true;
            redText.text = "Niste u highscore tablici! Nalazite se na " + indexIgracNadjen + ". mjestu.";
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        //dbcmd.Cancel();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }

    public void returnToLevel5()
    {
        SceneManager.LoadScene(10);
    }

}
