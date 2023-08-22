using UnityEngine;

public class FireTrap : Damage
{

    private const string IS_WORKING_STRING = "isWorking";

    private Animator fireTrapAnimator;

    [SerializeField] private bool isWorking;
    [SerializeField] private bool hasSwitcher;
    [SerializeField] private float fireSwitchRepeatRate = 3.0f;
    [SerializeField] private float fireSwitchResetTime = 5.0f;

    public static bool IsFireTrapWorking {get; private set;}

    private void Awake()
    {
        fireTrapAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (transform.parent == null)
        {
            InvokeRepeating("FireSwitch", 0f, fireSwitchRepeatRate);
        }
    }

    private void Update()
    {
        fireTrapAnimator.SetBool(IS_WORKING_STRING, isWorking);
        IsFireTrapWorking = isWorking;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (isWorking)
        {
            base.OnTriggerEnter2D(other);            
        }
    }

    public bool FireSwitch()
    {   
        isWorking = !isWorking;
        return isWorking;
    }

    public void FireSwitchResetAfter()
    {
        if (isWorking)
        {
            FireSwitch(); 
            Invoke("FireSwitch", fireSwitchResetTime);
        }
    }
}