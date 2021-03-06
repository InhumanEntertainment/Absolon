using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    public static Player Instance = null;
    public Weapon Weapon;
    public Weapon StartWeapon;
    public Vector3 TouchOffset;
    public ParticleSystem DeathEffect;

    float DeathTimeout = 1;
    float DeathStart;
    public bool isAlive = true;

    public float TapTime;
    public int TapCount = 0;
    float TapDuration = 0.2f;

    // Joystick //
    public float JoystickSpeed = 2;

    // States //
    public Animator State;

    //======================================================================================================================================//
    void Awake()
    {
        Instance = this;
        Weapon = Game.Spawn(StartWeapon) as Weapon;
        State = GetComponent<Animator>();

        // State Hashes //
        DeathHash = Animator.StringToHash("Death");
	}

    //======================================================================================================================================//
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position - TouchOffset, transform.position);
        Gizmos.DrawWireSphere(transform.position - TouchOffset, 0.05f);
    }

    //======================================================================================================================================//
    int DeathHash;
    void Update()
    {
        if (State.GetCurrentAnimatorStateInfo(0).IsName("Player Death"))
	    {
		    print("Death!!!");
	    }

        if (Time.timeScale != 0)
        {
            // OUYA //
            //OuyaController controller = new OuyaController();
            //var buttons = controller.GetSupportedButtons();
            //print(controller.GetAxisInverted(OuyaSDK.KeyEnum.AXIS_LSTICK_X);
            //string joystick_x = controller.GetUnityAxisName(OuyaSDK.KeyEnum.AXIS_LSTICK_X,OuyaSDK.OuyaPlayer.player1);
            //print(joystick_x + ": " + OuyaExampleCommon.GetAxis(OuyaSDK.KeyEnum.AXIS_LSTICK_X, OuyaSDK.OuyaPlayer.player1));
            //print(OuyaExampleCommon.GetAxis(OuyaSDK.KeyEnum.BUTTON_O, OuyaSDK.OuyaPlayer.player1));
            //if ( && isAlive)
            //    Weapon.Fire();

            // Joystick Controls //
            /*Vector3 joystickAxis = new Vector3(Input.GetAxis("Joy1 Axis 1"), -Input.GetAxis("Joy1 Axis 2"), 0);
            transform.position += joystickAxis * JoystickSpeed * Time.deltaTime;

            if (Input.GetAxis("Fire1") > 0.5f && isAlive)
            {
                Weapon.Fire();
            }*/

            Vector3 Mouse = GetMousePosition();
            Mouse.z = 0;
            
            // Touch Controls //
            if (Input.GetMouseButton(0) && !GameButton.InUse)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    TouchOffset = transform.position - Mouse;

                    if (Time.realtimeSinceStartup - TapTime < TapDuration)
                    {
                        TapCount++;
                        if (TapCount == 1)
                        {
                            Game.Instance.ActivateBomb();
                            TapCount = 0;
                            TapTime = 0;
                        }
                    }

                    TapTime = Time.realtimeSinceStartup;
                }

                // Screen Bounds //
                float borderx = 0.15f;
                float bordery = 0.10f;
                float xbound = (Camera.main.orthographicSize - borderx) * Camera.main.aspect;
                float ybound = (Camera.main.orthographicSize - bordery);

                Mouse.x = Mathf.Clamp(Mouse.x, -xbound, xbound);
                Mouse.y = Mathf.Clamp(Mouse.y, -ybound, ybound);

                Vector3 targetPosition = Mouse + TouchOffset;
                targetPosition.x = Mathf.Clamp(targetPosition.x, -xbound, xbound);
                targetPosition.y = Mathf.Clamp(targetPosition.y, -ybound, ybound);

                TouchOffset = targetPosition - Mouse;
                transform.position = Mouse + TouchOffset;

                if (isAlive)
                {
                    Weapon.Fire();
                    if(!audio.isPlaying)
                        audio.Play();
                }
            }
            else
            {
                audio.Stop();
            }

            Weapon.transform.position = transform.position + Weapon.Offset;

            if (isAlive)
            {

            }
            else
            {
                float amount = Time.timeSinceLevelLoad - DeathStart;
                if (Input.GetMouseButtonDown(0))
                    amount = DeathTimeout;

                if (amount < DeathTimeout)
                {
                    bool value = (amount * 4 % 1) > 0.5f;
                    (renderer as SpriteRenderer).color = value ? Color.red : new Color(1, 0, 0, 0.1f);
                }
                else
                {
                    isAlive = true;
                    (renderer as SpriteRenderer).color = Color.white;
                }
            }
        }
        else
        {
            audio.Stop();
        }
	}

    //======================================================================================================================================//
    Vector3 GetMousePosition()
    {
        Vector3 vec = Vector3.zero;
        vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0;

        return vec;
    }

    //======================================================================================================================================//
    public void Kill()
    {
        if (isAlive)
        {
            // Play Effect //
            if (DeathEffect)
                Game.Spawn(DeathEffect, transform.position);

            isAlive = false;
            DeathStart = Time.timeSinceLevelLoad;
            audio.Stop();

            // Reset Weapon //
            Destroy(Weapon.gameObject);
            Weapon = (Weapon)Instantiate(Game.Instance.Weapons[0]);

            Game.Instance.Death();
        }
    }

    //=======================================================================================================================================================/
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            Kill();
        }
    }
}
