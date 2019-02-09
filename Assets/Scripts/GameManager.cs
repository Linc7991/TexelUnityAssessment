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
    public GameObject MouseBlock;

    /************************
     * 
     *      Functions
     *      
     ***********************/

    //Sets collected tile to tile variables.
    public IEnumerator SetTile(Tile tile)
    {
        if (tile1 == null)
        {
            tile1 = tile;
            tile.Select();
        }
        else if (tile != tile1)
        {
            tile2 = tile;
            tile.Select();
            matches++;

            //Check if the tiles match
            if (tile1.food == tile2.food)
            {
                MouseBlock.SetActive(true);
                yield return new WaitForSeconds(1f);
                tile1.Vanish();
                tile1 = null;
                tile2.Vanish();
                tile2 = null;
                MouseBlock.SetActive(false);

                if (FindObjectsOfType<Tile>().Length == 2)
                {
                    EndGame();
                }
            }
            else
            {
                MouseBlock.SetActive(true);
                yield return new WaitForSeconds(1f);

                //Start over
                tile1.Unselect();
                tile1 = null;
                tile2.Unselect();
                tile2 = null;
                MouseBlock.SetActive(false);
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

        //Set texts
        timerText.text = "Time: " + minutesPassed + ":" + (secondsPassed % 60).ToString("00");
        matchesText.text = "Matches: " + matches;
        gamesText.text = "Games this session: " + games;
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
                if (hit.collider.GetComponent<Tile>() != null && tile1 == null || tile2 == null)
                {
                    StartCoroutine(SetTile(hit.collider.GetComponent<Tile>()));
                }
            }
        }

        if (timerActive)
        {
            secondsPassed += Time.deltaTime;
        }
    }

}
