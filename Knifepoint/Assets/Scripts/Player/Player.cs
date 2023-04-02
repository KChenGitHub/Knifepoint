using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public int maxHP;
    [SerializeField] public int currHP; //set from the editor
    [SerializeField] public GameObject[] heartIconList;

    [Header("Attack")]
    [SerializeField] private BoxCollider knifeAttackHitbox;
    [SerializeField] private GameObject stabText;
    private bool canMeleeAttack;
    [SerializeField] private bool hasKnifeSwarm;
    [SerializeField] private float knifeSwarmDuration;
    [SerializeField] private float knifeSwarmInterval;

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

    [Header("Animations")]
    [SerializeField]private Animator anim;
    private Vector3 lastPos;

    #region Start and Controls
    // Start is called before the first frame update
    void Start()
    {
        canThrowKnife = true;
        canMeleeAttack = true;
        knifeAttackHitbox.name = "knifey";
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        SetMovementAnims();
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
                handKnife.SetActive(false);
                if (hasKnifeSwarm)
                {
                    StartCoroutine(KnifeSwarm());
                }
                else
                {
                    ThrowKnife();
                }
                
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Throwing true");
            anim.SetTrigger("Throwing");
        }
    }

    private void SetMovementAnims()
    {
        var dist = Vector3.Distance(lastPos, transform.position);
        lastPos = transform.position;
        var speed = dist / Time.deltaTime;
        if (speed > 0f)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
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
        anim.SetBool("Melee", true);
        yield return new WaitForSeconds(.5f);
        knifeAttackHitbox.enabled = false;
        canThrowKnife = true;
        canMeleeAttack = true;
        stabText.SetActive(false);
        StartCoroutine(ResetAnimations());
    }

    /// <summary>
    /// Instances the throw knife and hides the hand knife
    /// </summary>
    private void ThrowKnife()
    {
        anim.SetTrigger("Throwing");
        GameObject newKnife = Instantiate(throwKnife, knifeThrowPoint.transform.position, knifeThrowPoint.transform.rotation);
        Knife knifeScript = newKnife.GetComponent<Knife>();
        knifeScript.player = this;
        knifeScript.isKnifeSwarmKnife = false;
        //newKnife.GetComponent<Knife>().StartCoroutine(WaitAfterThrow());
        newKnife.GetComponent<Rigidbody>().AddForce(knifeThrowPoint.transform.forward * throwForce);

        canThrowKnife = false;
        canMeleeAttack = false;
    }

    private void KnifeSwarmThrow(float throwAngle)
    {
        Vector3 directionVec = Quaternion.Euler(knifeThrowPoint.transform.rotation.x, throwAngle, knifeThrowPoint.transform.rotation.z) * knifeThrowPoint.transform.forward;
        GameObject swarmKnife = Instantiate(throwKnife, knifeThrowPoint.transform.position, knifeThrowPoint.transform.rotation);
        swarmKnife.GetComponent<Knife>().player = this;
        swarmKnife.GetComponent<Knife>().isKnifeSwarmKnife = true;
        swarmKnife.GetComponent<Rigidbody>().AddForce((knifeThrowPoint.transform.forward + directionVec) * throwForce);
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

    public IEnumerator KnifeSwarm()
    {
        canThrowKnife = false;
        canMeleeAttack = false;
        bool throwAngleIncreasing = false;
        float throwAngle = 0f;
        float throwAngleThreshhold = 45f;
        float throwAngleMod = 100.0f;
        float totalDuration = 0f;
        float currInterval = 0f;

        while (totalDuration < knifeSwarmDuration)
        {
            while (currInterval < knifeSwarmInterval)
            {
                currInterval += Time.deltaTime;
                totalDuration += Time.deltaTime;
                
                //Add to the throw angle to change the direction as the knives go
                throwAngle = throwAngleIncreasing ? throwAngle + (Time.deltaTime * throwAngleMod) : throwAngle - (Time.deltaTime * throwAngleMod);
                yield return null;
            }
            //Reverse the angle if it is over the threshold
            if (Mathf.Abs(throwAngle) >= throwAngleThreshhold)
            {
                throwAngleIncreasing = !throwAngleIncreasing;
            }
            KnifeSwarmThrow(throwAngle);
            currInterval = 0f;
            yield return null;
        }

        ThrowKnifeReset();
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
        UpdateHealthUI();

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
        UpdateHealthUI();
    }

    /// <summary>
    /// Used by the HP incresase upgrade item
    /// </summary>
    /// <param name="healthAmount">The amount the max HP gets increased to</param>
    public void increaseHPMax(int healthAmount)
    {
        maxHP += healthAmount;
        currHP = maxHP;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < maxHP; i++)
        {
            if (currHP > i)
            {
                heartIconList[i].SetActive(true);
            }
            else
            {
                heartIconList[i].SetActive(false);
            }
        }
    }
    #endregion



    #region Animations
    private IEnumerator ResetAnimations()
    {
        yield return new WaitForSeconds(.5f);
        anim.SetBool("Melee", false);
    }

    #endregion
}
