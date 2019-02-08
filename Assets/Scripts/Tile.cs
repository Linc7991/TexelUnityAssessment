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
        //If the back of the tile is unselected, change to selected.
        if (transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == selectTextures[0])
        {
            transform.GetChild(0).GetComponent<Renderer>().material = selectTextures[1];
            anim.SetBool("Shake", true);
            StartCoroutine(Turn(true));
            selected = true;
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

    public IEnumerator Turn(bool faceUp)
    {
        if (faceUp)
        {
            while (transform.rotation.y < 180)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * 10);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (transform.rotation.y > 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 10);
                yield return new WaitForEndOfFrame();
            }
        }
    }

}
