using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] public HealthBar HealthBarPlayer;
    [SerializeField] public StatsManager Stats;
    [SerializeField] private EnemyController enemy;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private  InventoryManager inventoryManager;
    [SerializeField] private  AnimationPlayer animationplayer;
    [SerializeField] private Item[] itemsToPickUp;
    [SerializeField] private CinemachineVirtualCamera NormalCamera; // Reference to your main camera
    [SerializeField] private CinemachineVirtualCamera WaterPortCamera; 
    [SerializeField] private GameObject axe;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject pauseDestroyAfterDead;
    [SerializeField] private GameObject oar;
    [SerializeField] private GameObject TutorialCanvas;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offsetOfTheFollowTarget;
    [SerializeField] private EnemiesDungeon enemy1;
    [SerializeField] private EnemiesDungeon enemy2;
    [SerializeField] private float pushforce = 5f;
    
    public float delayBeforePush = 0.3f;
    private bool hitbuttonpressed;
    private Rigidbody _rigidbody;
    private bool canMove = true;
    private bool followBoat = false;
    private Vector3 inputkey;
    private float myFloat;
    private Animator _animator;
    public bool grabItem = false;
    public bool moveBoat = false;
    public bool addItemAfterAnimation = false;
    public Canvas canvas; // Reference to the Canvas object containing the child
    public string DeadScreen; // Name of the child object you want to activate
    public float GainExperience;
    public TextMeshProUGUI LevelOfThePlayer;
    public TextMeshProUGUI UpgradesPointsAvaible;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //Stats.speedValue = Stats.speedStat.value;
        WaterPortCamera.gameObject.SetActive(false);
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        LevelOfThePlayer.text = "1";
        UpgradesPointsAvaible.text = "0";
        Stats.upgradesPointsAvailable = 0;
        Stats.speedValue = 5f;
        StartCoroutine(DestroyTutorialAdvice(3f, TutorialCanvas));
    }

    // Update is called once per frame

    private void Update()
    {
        if (HealthBarPlayer.sliderValue <= 0f)
        {
            Finish();
        }
        // Use GetAxisRaw to get -1, 0, or 1 directly without smoothing
        //inputkey = new Vector3(-Input.GetAxisRaw("Horizontal"), 0, -Input.GetAxisRaw("Vertical"));
        if (WaterPortCamera.gameObject.activeSelf)
        {
            inputkey = new Vector3(-_joystick.Vertical, 0, _joystick.Horizontal);
        }
        else
        {
            inputkey = new Vector3(-_joystick.Horizontal, 0, -_joystick.Vertical);
        }

        if (followBoat)
        {
            transform.position = target.position + offsetOfTheFollowTarget;
        }
        
    }
    
    void FixedUpdate()
    {
        // Check if the player can move
        if (canMove)
        {
            _rigidbody.MovePosition(transform.position + inputkey * (Stats.speedValue * Time.deltaTime));

            if (inputkey.magnitude >= 0.1f)
            {
                if (rock.activeSelf)
                {
                    _animator.Play("Carry");
                    float Angle = Mathf.Atan2(inputkey.x, inputkey.z) * Mathf.Rad2Deg;
                    float Smooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, Angle, ref myFloat, 0.1f);
                    transform.rotation = Quaternion.Euler(0, Smooth, 0);
                }
                else
                {
                    _animator.SetInteger("character_animations", 1);
                    float Angle = Mathf.Atan2(inputkey.x, inputkey.z) * Mathf.Rad2Deg;
                    float Smooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, Angle, ref myFloat, 0.1f);
                    transform.rotation = Quaternion.Euler(0, Smooth, 0);
                }
            }
            else
            {
                _animator.SetInteger("character_animations", 0);
                myFloat = 0f;
            }
        }
        else
        {
            // Player cannot move, so no movement logic executed here
        }
        
    }

    public void PlayerDamaged()
    {
        HealthBarPlayer.sliderValue -= 0.04f;
    }
    
    private IEnumerator PushWithDelay(Collider other)
    {
        yield return new WaitForSeconds(delayBeforePush);

        Rigidbody enemyRigidbody = other.GetComponent<Rigidbody>();
        if (enemyRigidbody != null)
        {
            // Apply a force to push the enemy
            Vector3 pushDirection = other.transform.position - transform.position;
            pushDirection.Normalize();
            enemyRigidbody.AddForce(pushDirection * pushforce, ForceMode.Impulse);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy1") && animationplayer.damageNow)
        {
            StartCoroutine(PushWithDelay(other));

            enemy1.EnemyDamaged();
            
            animationplayer.damageNow = false;
            
        }
        else if (other.CompareTag("enemy2") && animationplayer.damageNow)
        {
            StartCoroutine(PushWithDelay(other));

            enemy2.EnemyDamaged();
            
            animationplayer.damageNow = false;
            
        }
        
        else if (other.CompareTag("WaterPortCamera"))
        {
            WaterPortCamera.gameObject.SetActive(true);
        }
        else if (other.gameObject.CompareTag("boat"))
        {
            followBoat = true;
            moveBoat = true;
            oar.SetActive(true);
            _animator.Play("Paddling");
            transform.position = new Vector3(-11.06933f, -0.5794142f, 35.56791f);
            transform.rotation = Quaternion.Euler(0f, 346.388f, 0.002f);
            transform.localScale = new Vector3(1, 1, 1);
            canMove = false;
            FreezeAxes(true, true, true);
            StartCoroutine(ChangeSceneAfterEnterTheBoat(1.6f));
        }
        else if (other.gameObject.CompareTag("deadLine"))
        {
            HealthBarPlayer.sliderValue = 0;
        }
        else if (other.gameObject.CompareTag("arrow"))
        {
            arrow.SetActive(false);
        }
    }

    IEnumerator ChangeSceneAfterEnterTheBoat(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(2);
    }
    
    // Function to freeze/unfreeze specific axes
    void FreezeAxes(bool freezeX, bool freezeY, bool freezeZ)
    {
        // Set constraints based on the boolean parameters
        _rigidbody.constraints = freezeX ? _rigidbody.constraints | RigidbodyConstraints.FreezePositionX : _rigidbody.constraints & ~RigidbodyConstraints.FreezePositionX;
        _rigidbody.constraints = freezeY ? _rigidbody.constraints | RigidbodyConstraints.FreezePositionY : _rigidbody.constraints & ~RigidbodyConstraints.FreezePositionY;
        _rigidbody.constraints = freezeZ ? _rigidbody.constraints | RigidbodyConstraints.FreezePositionZ : _rigidbody.constraints & ~RigidbodyConstraints.FreezePositionZ;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaterPortCamera"))
        {
            WaterPortCamera.gameObject.SetActive(false);
        }
    }

    public void Damage()
    {
        enemy.EnemyDamaged();
    }

    public void hitButton()
    {
        if (axe != null && axe.activeSelf)
        {
            _animator.Play("Axe_Slash");
        }
        else if (sword != null && sword.activeSelf)
        {
            _animator.Play("Sword_Slash");
            hitbuttonpressed = true;
        }
        else
        {
            _animator.Play("Punching");
        }
    }
    
    // Called when colliding with an object
    private void OnTriggerStay (Collider other)
    {
        if (grabItem  && other.gameObject.CompareTag("small_log"))
        {
            
            AddItemToInventory(1);
            grabItem = false;
            Destroy(other.gameObject);
            int currentValuePlayer = int.Parse(LevelOfThePlayer.text);
            int currentValueUpgrade = int.Parse(LevelOfThePlayer.text);
            int newValuePlayer = currentValuePlayer;
            int newValueUpgrade = currentValueUpgrade;
            newValuePlayer += 1;
            newValueUpgrade += 1;
            Stats.upgradesPointsAvailable += 1;
            LevelOfThePlayer.text = newValuePlayer.ToString();
            UpgradesPointsAvaible.text = newValueUpgrade.ToString();
        }
        else if (grabItem && other.gameObject.CompareTag("flower"))
        {
            Debug.Log("Collision with " + other.gameObject.tag);
            AddItemToInventory(4);
            grabItem = false;
            Destroy(other.gameObject);
            int currentValuePlayer = int.Parse(LevelOfThePlayer.text);
            int currentValueUpgrade = int.Parse(LevelOfThePlayer.text);
            int newValuePlayer = currentValuePlayer;
            int newValueUpgrade = currentValueUpgrade;
            newValuePlayer += 1;
            newValueUpgrade += 1;
            Stats.upgradesPointsAvailable += 1;
            LevelOfThePlayer.text = newValuePlayer.ToString();
            UpgradesPointsAvaible.text = newValueUpgrade.ToString();
        }
        else if (grabItem && other.gameObject.CompareTag("mushroom"))
        {
            Debug.Log("Collision with " + other.gameObject.tag);
            AddItemToInventory(5);
            grabItem = false;
            Destroy(other.gameObject);
            int currentValuePlayer = int.Parse(LevelOfThePlayer.text);
            int currentValueUpgrade = int.Parse(LevelOfThePlayer.text);
            int newValuePlayer = currentValuePlayer;
            int newValueUpgrade = currentValueUpgrade;
            newValuePlayer += 1;
            newValueUpgrade += 1;
            Stats.upgradesPointsAvailable += 1;
            LevelOfThePlayer.text = newValuePlayer.ToString();
            UpgradesPointsAvaible.text = newValueUpgrade.ToString();
        }
        else if (grabItem && other.gameObject.CompareTag("branch"))
        {
            Debug.Log("Collision with " + other.gameObject.tag);
            AddItemToInventory(6);
            grabItem = false;
            Destroy(other.gameObject);
            int currentValuePlayer = int.Parse(LevelOfThePlayer.text);
            int currentValueUpgrade = int.Parse(LevelOfThePlayer.text);
            int newValuePlayer = currentValuePlayer;
            int newValueUpgrade = currentValueUpgrade;
            newValuePlayer += 1;
            newValueUpgrade += 1;
            Stats.upgradesPointsAvailable += 1;
            LevelOfThePlayer.text = newValuePlayer.ToString();
            UpgradesPointsAvaible.text = newValueUpgrade.ToString();
        }
        else if (grabItem && other.gameObject.CompareTag("plant"))
        {
            Debug.Log("Collision with " + other.gameObject.tag);
            AddItemToInventory(7);
            grabItem = false;
            Destroy(other.gameObject);
            int currentValuePlayer = int.Parse(LevelOfThePlayer.text);
            int currentValueUpgrade = int.Parse(LevelOfThePlayer.text);
            int newValuePlayer = currentValuePlayer;
            int newValueUpgrade = currentValueUpgrade;
            newValuePlayer += 1;
            newValueUpgrade += 1;
            Stats.upgradesPointsAvailable += 1;
            LevelOfThePlayer.text = newValuePlayer.ToString();
            UpgradesPointsAvaible.text = newValueUpgrade.ToString();
        }
        else if (axe.activeSelf && other.gameObject.CompareTag("tree") && grabItem)
        {
            addItemAfterAnimation = true;

            if (animationplayer.treeCollisionCountForDestroy >= 5)
            {

                addItemAfterAnimation = true;
                StartCoroutine(DestroyTreeDelayed(other.gameObject, 0.2f)); // Destroy the tree after 1 second
                animationplayer.treeCollisionCountForDestroy = 0; // Reset collision count
                int currentValuePlayer = int.Parse(LevelOfThePlayer.text);
                int currentValueUpgrade = int.Parse(LevelOfThePlayer.text);
                int newValuePlayer = currentValuePlayer;
                int newValueUpgrade = currentValueUpgrade;
                newValuePlayer += 10;
                newValueUpgrade += 10;
                Stats.upgradesPointsAvailable += 10;
                LevelOfThePlayer.text = newValuePlayer.ToString();
                UpgradesPointsAvaible.text = newValueUpgrade.ToString();
            }
        }
        else if (grabItem && other.gameObject.CompareTag("rock"))
        {
            
            AddItemToInventory(3);
            grabItem = false;
            Destroy(other.gameObject);
        }
        
    }

    public void GrabItem()
    {
        grabItem = true;
        StartCoroutine(TurnOffGrabItemAfterDelay(1f));
    }
    
    IEnumerator TurnOffGrabItemAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        grabItem = false;
    }
    
    IEnumerator DestroyTreeDelayed(GameObject tree, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(tree); // Destroy the tree
    }
 

    // Function to add item to the inventory
    public void AddItemToInventory(int id)
    {   
        bool result = inventoryManager.AddItem(itemsToPickUp[id]);
        if (result == true)
        {
            Debug.Log("Item Added");
        }
        else
        {
            Debug.Log("inventory full");
        }
    }
    
    public void Finish()
    {
        
        
        Debug.Log("You are dead");
    
        if (canvas != null)
        {
            Transform childTransform = canvas.transform.Find(DeadScreen);
            if (childTransform != null)
            {
                Debug.Log("Child object found: " + childTransform.name);
                childTransform.gameObject.SetActive(true);
                pauseDestroyAfterDead.SetActive(false);
                Debug.Log("Child object activated: " + childTransform.name);
                StartCoroutine(ComeBackToMainMenu(7f));
            }
            else
            {
                Debug.LogError("Child object '" + DeadScreen + "' not found in the canvas.");
            }
        }
        else
        {
            Debug.LogError("Canvas reference is not assigned.");
        }
        
        
    }
    
    IEnumerator ComeBackToMainMenu (float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(0);
        

    }
    IEnumerator DestroyTutorialAdvice (float delayInSeconds, GameObject gameobject)
    {
        yield return new WaitForSeconds(delayInSeconds);
        Destroy(gameobject);
        

    }
    
}
