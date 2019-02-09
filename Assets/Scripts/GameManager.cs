using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int games;

    //Tile Variables
    [SerializeField]
    GridGenerator generator;
    Tile tile1;
    Tile tile2;
    public int matches;

    //Time Variables
    public bool timerActive;
    public float secondsPassed;
    public int minutesPassed;

    //UI Variables
    public Text timerText;
    public Text gamesText;
    public Text matchesText;
    public Button ResetButton;
    public Button StartButton;

    /************************
     * 
     *      Functions
     *      
     ***********************/

    //Sets collected tile to tile variables.
    public void SetTile(Tile tile)
    {
        if (tile1 == null)
        {
            tile1 = tile;
        }
        else
        {
            tile2 = tile;
            matches++;

            //Check if the tiles match
            if (tile1.food == tile2.food)
            {
                tile1.Vanish();
                tile1 = null;
                tile2.Vanish();
                tile2 = null;

                if (FindObjectsOfType<Tile>().Length == 2)
                {
                    EndGame();
                }
            }
            else
            {
                //Start over
                tile1.Unselect();
                tile1 = null;
                tile2.Unselect();
                tile2 = null;
            }
        }

    }

    //Start a new game
    public void BeginGame()
    {
        //Set variables
        matches = 0;
        secondsPassed = 0;
        games++;

        //Disable UI Elements
        StartButton.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        gamesText.gameObject.SetActive(false);
        matchesText.gameObject.SetActive(false);

        generator.Generate();
        timerActive = true;
    }

    //End of match
    public void EndGame()
    {
        timerText.gameObject.SetActive(true);
        gamesText.gameObject.SetActive(true);
        matchesText.gameObject.SetActive(true);

        timerActive = false;
        minutesPassed = Mathf.FloorToInt(secondsPassed / 60);
        timerText.text = minutesPassed + ":" + (secondsPassed % 60).ToString("00");
    }

    /*******************************
     * 
     *      Events
     *      
     ******************************/

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.GetComponent<Tile>() != null)
                {
                    SetTile(hit.collider.GetComponent<Tile>());
                }
            }
        }

        if (timerActive)
        {
            secondsPassed += Time.deltaTime;
        }
    }

}
