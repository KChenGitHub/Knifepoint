using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public int maxHP;
    [SerializeField] private int currHP; //set from the editor
    [SerializeField] private GameObject[] heartIconList;

    [Header("Attack")]
    [SerializeField] private BoxCollider knifeAttackHitbox;
    [SerializeField] private GameObject stabText;
    private bool canMeleeAttack;

    [Header("Knife Throwing")]
    [SerializeField] public bool canThrowKnife;
    [SerializeField] private float throwForce;

    [Header("Objects")]
    private Main main;
    [SerializeField] private FPSController FPSController;
    [SerializeField] private GameObject playerHand; //The arm/hand model
    [SerializeField] private GameObject handKnife; //The knife attached to the hand model
    [SerializeField] private GameObject throwKnife; //Knife prefab for throwing
    [SerializeField] private GameObject knifeThrowPoint;

    #region Start and Controls
    // Start is called before the first frame update
    void Start()
    {
        canThrowKnife = true;
        canMeleeAttack = true;
        knifeAttackHitbox.name = "knifey";
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (canMeleeAttack)
            {
                StartCoroutine(MeleeAttack());
            }
            
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (canThrowKnife)
            {
                ThrowKnife();
            }
        }
    }

    public void SetMain(Main mainRef)
    {
        main = mainRef;
    }
    #endregion

    #region Attacks

    private IEnumerator MeleeAttack()
    {
        knifeAttackHitbox.enabled = true;
        canThrowKnife = false;
        canMeleeAttack = false;
        stabText.SetActive(true);
        yield return new WaitForSeconds(.5f);
        knifeAttackHitbox.enabled = false;
        canThrowKnife = true;
        canMeleeAttack = true;
        stabText.SetActive(false);
    }

    /// <summary>
    /// Instances the throw knife and hides the hand knife
    /// </summary>
    private void ThrowKnife()
    {
        GameObject newKnife = Instantiate(throwKnife, knifeThrowPoint.transform.position, knifeThrowPoint.transform.rotation);
        newKnife.GetComponent<Knife>().player = this;
        newKnife.GetComponent<Rigidbody>().AddForce(knifeThrowPoint.transform.forward * throwForce);
        handKnife.SetActive(false);
        canThrowKnife = false;
        canMeleeAttack = false;
    }

    /// <summary>
    /// Called from the knife script to allow the player to throw the knife again
    /// </summary>
    public void ThrowKnifeReset()
    {
        canThrowKnife = true;
        canMeleeAttack = true;
        handKnife.SetActive(true);
    }

    #endregion

    #region Health and Damage
    /// <summary>
    /// Reduces player HP by damageAmount
    /// </summary>
    /// <param name="damageAmount">The damage value that the player loses from their HP</param>
    public void TakeDamage(int damageAmount)
    {
        currHP -= damageAmount;
        for (int i = maxHP; i > 0; i--)
        {
            if (currHP < i)
            {
                heartIconList[i - 1].SetActive(false);
            }
        }

        if (currHP <= 0)
        {
            handKnife.SetActive(false);
            playerHand.SetActive(false);
            main.EndGame();
        }
    }

    /// <summary>
    /// Increases the  player health by healthAmount if the player is not at full health
    /// </summary>
    /// <param name="healthAmount">the amount of health to increase by</param>
    public void GainHealth(int healthAmount)
    {
        currHP = currHP >= maxHP ? maxHP : currHP + healthAmount;
    }

    /// <summary>
    /// Used by the HP incresase upgrade item
    /// </summary>
    /// <param name="healthAmount">The amount the max HP gets increased to</param>
    private void increaseHPMax(int healthAmount)
    {
        maxHP = healthAmount;
        currHP = healthAmount;
    }
    #endregion
}
