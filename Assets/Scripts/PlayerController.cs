using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text BloodGoatsText;

    private float _moveSpeed;
    public float DefaultMoveSpeed = 7f;
    public float MoveSpeedDuringBurrow = 8f;
    public float MoveSpeedAfterHit = 6f;
    private Vector2 _moveDirection;

    public float BloodLeft;
    public float BloodPerGoat = 25f;
    public float BloodDamageForBurrow = 10f;
    public float BloodDamageAfterHit = 50f;
    private static System.Timers.Timer s_bloodTimer;

    private bool _attackingPaused;
    private bool _beingHitPaused;

    public int TotalGoatsEaten;
    public int CurrentWeeksGoatCount;
    private bool _hasGoat;

    private float _delayBeforeWeekMenu = 1f;

    private bool _layersChanged;
    private Rigidbody2D _rb2d;
    private Animator _animator;

    private void Start()
    {
        BloodLeft = GameManager.s_Instance.PlayersBloodLeft;
        TotalGoatsEaten = GameManager.s_Instance.PlayersTotalGoatsEaten;
        BloodGoatsText.text = "BLOOD DROPS LEFT: " + BloodLeft.ToString() + "\nGOATS EATEN: " + TotalGoatsEaten.ToString();

        _moveSpeed = DefaultMoveSpeed;

        s_bloodTimer = new System.Timers.Timer(1000);
        s_bloodTimer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) => BloodLeft--;

        _attackingPaused = false;
        _beingHitPaused = false;

        CurrentWeeksGoatCount = GameManager.s_Instance.Week % 10 + 1;
        _hasGoat = false;

        _layersChanged = false;
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
    }

    private void OnDisable()
    {
        GameManager.s_Instance.PlayersBloodLeft = BloodLeft;
        GameManager.s_Instance.PlayersTotalGoatsEaten = TotalGoatsEaten;
    }

    private void Update()
    {
        if (GameManager.s_Instance.WeekIsSettingUp)
        {
            s_bloodTimer.Stop();
        }
        else
        {
            s_bloodTimer.Start();
            BloodGoatsText.text = "BLOOD DROPS LEFT: " + BloodLeft.ToString() + "\nGOATS EATEN: " + TotalGoatsEaten.ToString();

            GetMoveDirection();
            SetScale();
            CheckBurrowKey();
            SetLayersIfBurrow();
            FixLayersAfterBurrow();
            CheckIfGameOver();
        }
    }

    private void GetMoveDirection()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        _moveDirection = new Vector2(moveHorizontal, moveVertical).normalized;
    }

    private void SetScale()
    {
        Vector2 newScale = gameObject.transform.localScale;

        if (_moveDirection.x > 0)
        {
            newScale.x = -1f;
        }
        else if (_moveDirection.x < 0)
        {
            newScale.x = 1f;
        }

        gameObject.transform.localScale = newScale;
    }

    private void CheckBurrowKey()
    {
        if (!_hasGoat && Input.GetKeyDown(KeyCode.LeftShift))
        {
            BloodLeft -= BloodDamageForBurrow;
            _animator.SetTrigger("PlayerBurrowTrigger");

            StartCoroutine(SpeedUp());
        }
    }

    private IEnumerator SpeedUp()
    {
        _moveSpeed = MoveSpeedDuringBurrow;

        //if (GameManager.s_Instance.Week == 2)
        //{
        //    ScreenCapture.CaptureScreenshot("D:/Repos/ChupacabraGame/Screenshots/Burrow.png");
        //}

        yield return new WaitForSeconds(3);

        _moveSpeed = DefaultMoveSpeed;
    }

    private void SetLayersIfBurrow()
    {
        gameObject.layer = LayerMask.NameToLayer("BurrowCollisionLayer");
        SetChildrensCollisionLayer("BurrowCollisionLayer");

        gameObject.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("BurrowSortingLayer");
        
        _layersChanged = true;
    }

    private void SetChildrensCollisionLayer(string newCollisionLayerString)
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(newCollisionLayerString);
        }
    }

    private void FixLayersAfterBurrow()
    {
        if (_layersChanged && !_animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerBurrowState"))
        {
            gameObject.layer = LayerMask.NameToLayer("InteractionCollisionLayer");
            SetChildrensCollisionLayer("InteractionCollisionLayer");

            gameObject.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("UnitSortingLayer");
            
            _layersChanged = false;
        }
    }

    private void CheckIfGameOver()
    {
        if (BloodLeft <= 0)
        {
            GameManager.s_Instance.PlayersTotalGoatsEaten = TotalGoatsEaten;
            GameManager.s_Instance.GameOverMenu();
        }
    }

    private void FixedUpdate()
    {
        _rb2d.velocity = new Vector2(_moveDirection.x * _moveSpeed, _moveDirection.y * _moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            TakeHit();
        }
        else if (other.tag == "Foliage")
        {
            AttackFoliage(other);
        }
        else if (!_hasGoat && other.tag == "Goat")
        {
            GrabGoat(other);
        }
        else if (_hasGoat && other.tag == "Exit")
        {
            EatGoat();
        }
    }

    private void TakeHit()
    {
        if (!_beingHitPaused)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerWalkState"))
            {
                _animator.SetTrigger("PlayerHitTrigger");
            }
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerGoatWalkState"))
            {
                _animator.SetTrigger("PlayerGoatHitTrigger");
            }

            StartCoroutine(SlowDownAndPauseBeingHit());

            BloodLeft -= BloodDamageAfterHit;

            if (BloodLeft < 0)
            {
                BloodLeft = 0;
            }

            //if (GameManager.s_Instance.Week == 5)
            //{
            //    ScreenCapture.CaptureScreenshot("D:/Repos/ChupacabraGame/Screenshots/TakeHit.png");
            //}
        }
    }

    private IEnumerator SlowDownAndPauseBeingHit()
    {
        _beingHitPaused = true;
        _moveSpeed = MoveSpeedAfterHit;

        yield return new WaitForSeconds(2);

        _moveSpeed = DefaultMoveSpeed;
        _beingHitPaused = false;
    }

    private void AttackFoliage(Collider2D other)
    {
        if (!_attackingPaused)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerWalkState"))
            {
                _animator.SetTrigger("PlayerAttackTrigger");
            }
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerGoatWalkState"))
            {
                _animator.SetTrigger("PlayerGoatAttackTrigger");
            }

            FoliageController foliageControllerScript = other.gameObject.transform.parent.gameObject.GetComponent<FoliageController>();
            foliageControllerScript.TakeHit();

            //if (GameManager.s_Instance.Week == 4)
            //{
            //    ScreenCapture.CaptureScreenshot("D:/Repos/ChupacabraGame/Screenshots/AttackFoliage.png");
            //}

            StartCoroutine(PauseAttacking());
        }
    }

    private IEnumerator PauseAttacking()
    {
        _attackingPaused = true;

        yield return new WaitForSeconds(0.5f);

        _attackingPaused = false;
    }

    private void GrabGoat(Collider2D other)
    {
        other.gameObject.SetActive(false);
        _animator.SetTrigger("PlayerGoatWalkTrigger");

        _hasGoat = true;

        //if (GameManager.s_Instance.Week == 3)
        //{
        //    ScreenCapture.CaptureScreenshot("D:/Repos/ChupacabraGame/Screenshots/GrabGoat.png");
        //}
    }

    private void EatGoat()
    {
        _animator.SetTrigger("PlayerWalkTrigger");

        _hasGoat = false;

        BloodLeft += BloodPerGoat;

        TotalGoatsEaten++;
        CurrentWeeksGoatCount--;

        if (CurrentWeeksGoatCount <= 0)
        {
            Invoke("BackToWeekMenu", _delayBeforeWeekMenu);
        }
    }

    private void BackToWeekMenu()
    {
        SceneManager.LoadScene(0);
    }
}
