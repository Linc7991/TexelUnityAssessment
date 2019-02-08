using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType { Taco, Steak, Pizza, Hotdog, Burger, Fries, Chicken, Cheese};

public class Tile : MonoBehaviour {
    
    public FoodType food;


    //Face Plate Variables
    [SerializeField]
    Texture[] foodTextures;
    Material face;

    //Back Plate Variables
    [SerializeField]
    Material[] selectTextures;

    //Animation
    Animator anim;
    bool selected = false;

    /*************************
     * 
     *      Functions
     *      
     *************************/

    //End the tile
    public void Vanish()
    {
        //If "Spin" has already been set to true, destroy self
        if (anim.GetBool("Spin"))
        {
            Destroy(gameObject);
        }
        else
        {
            anim.SetBool("Spin", true);
        }
    }

    //Rotate to a specified angle
    public IEnumerator Turn(float angle)
    {
            while (transform.rotation.y != angle)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * 10);
                yield return new WaitForEndOfFrame();
            }
    }

    //Select the tile
    public void Select()
    {
        transform.GetChild(0).GetComponent<Renderer>().material = selectTextures[1];
        anim.SetBool("Shake", true);
        StartCoroutine(Turn(180));
        selected = true;
    }

    //Unselect the tile
    public void Unselect()
    {
        transform.GetChild(0).GetComponent<Renderer>().material = selectTextures[0];
        anim.SetBool("Shake", false);
        StartCoroutine(Turn(0));
        selected = false;
    }

    /***************************
     * 
     *      Events
     * 
     **************************/

    private void Start()
    {
        //Set the texture of the face to the index of food found in foodTextures.
        face = transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
        face.mainTexture = foodTextures[(int)food];

        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse Clicked.");
        //If the tile is unselected, change to selected.
        if (!selected)
        {
            Select();
        }
    }

    private void OnMouseEnter()
    {
        //Shake while hovering over tile.
        anim.SetBool("Shake", true);
    }

    private void OnMouseExit()
    {
        //If the tile wasn't selected, stop shaking.
        if (!selected)
        {
            anim.SetBool("Shake", false);
        }
    }



}
