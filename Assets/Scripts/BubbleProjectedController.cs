using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Projections
{
    public string name;
    public Sprite[] spriteList;

    public bool animation;
    public float freqAnimation;
    public float limitAnimationTime;
}

public class BubbleProjectedController : MonoBehaviour
{
    /*index
        #####################
        #                   #
        # BUBBLE PROJECTED  #
        #                   #
        #####################
    */
    #region BUBBLE_PROJECTED

    private float countTime;
    private float freqTime;

    private int idAnim;
    private int idSprite;
    private bool doAnimation;

    [SerializeField]
    public GameObject bubbleProjected;

    [SerializeField]
    public SpriteRenderer spriteProjected;

    [SerializeField]
    public Projections[] projections;

    public void ShowBubble(bool _val) {
        bubbleProjected.SetActive(_val);
    }

    public void SpriteFlipX(bool _val)
    {
        spriteProjected.flipX = _val;
    }
    
    public void SetProjection(string _name)
    {
        for (int i = 0; i < projections.Length; i++)
        {
            if (projections[i].name == _name)
            {
                spriteProjected.sprite = projections[i].spriteList[0];
                ShowBubble(true);
                if (projections[i].animation)
                {    
                    idAnim = i;
                    doAnimation = true;
                    idSprite = 1;
                }
                
                return;
            }
        }
    }
    #endregion



    /*index
        ########################
        #                      #
        #  FUNCIONES DE UNITY  #
        #                      #
        ########################
    */
    
    private void Start()
    {
        countTime = 0;
        freqTime = 0;
        idSprite = 0;
        doAnimation = false;
        if(gameObject.tag == "Clone")
        {
            bubbleProjected.SetActive(false);
        }
    }

    private void Update()
    {
        if (doAnimation)
        {
            countTime += Time.deltaTime * 1000;
            freqTime += Time.deltaTime * 1000;

            if (freqTime >= projections[idAnim].freqAnimation)
            {
                freqTime = 0;
                spriteProjected.sprite = projections[idAnim].spriteList[idSprite];
                idSprite++;
                if (idSprite>= projections[idAnim].spriteList.Length)
                {
                    idSprite = 0;
                }
            }

            if (countTime >= projections[idAnim].limitAnimationTime)
            {
                ShowBubble(false);
                doAnimation = false;
                countTime = 0;
                freqTime = 0;
                idSprite = 0;
            }
        }



    }
}