using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour {

    //Tile Variables
    [SerializeField]
    GameObject tilePrefab;
    GameObject[,] tiles;
    public int gridSize;

    private void Start()
    {
        //An odd number gridSize cannot have an even amount of tiles.
        if (gridSize % 2 == 1)
        {
            Debug.LogWarning("Grid Size must be an even number.");
            gridSize--;
        }
        Generate();
    }

    //Generates a grid based on gridSize.
    public void Generate()
    {
        tiles = new GameObject[gridSize, gridSize];
        int numOfFood = System.Enum.GetValues(typeof(FoodType)).Length;
        int tilePairs = gridSize * gridSize / 2;
        int currentFood = 0;

        //Generate pairs of tiles for each available pair of tiles in the grid.
        for (int i = 0; i < tilePairs; i++)
        {
            //Instantiate a pair of tiles in the grid randomly.
            for (int p = 0; p < 2; p++)
            {
                int x;
                int y;

                do
                {
                    x = Random.Range(0, gridSize);
                    y = Random.Range(0, gridSize);
                } while (tiles[x, y] != null);
                tiles[x, y] = Instantiate(tilePrefab, new Vector3(x + x * 0.25f, -y - y * 0.25f, 0), Quaternion.identity);
                tiles[x, y].GetComponent<Tile>().food = (FoodType)currentFood;
            }
            Debug.Log(currentFood);
            if (currentFood >= numOfFood - 1)
            {
                //Cycle back to the start of the food list.
                currentFood = 0;
            }
            else
            {
                //Move to the next food.
                currentFood++;
            }
        }

    }
}
