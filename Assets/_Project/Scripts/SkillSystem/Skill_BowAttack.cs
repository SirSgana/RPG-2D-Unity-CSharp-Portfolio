using UnityEngine;

public class Skill_BowAttack : Skill_Base
{
    private SkillObject_Arrow currentArrow;
    private float currentArrowPower;

    [Header("Regular Arrow Upgrade")]
    [SerializeField] private GameObject arrowPrefab;
    [Range(0, 10)]
    [SerializeField] private float regularArrowPower = 5;

    [Header("Pierce Arrow Upgrade")]
    [SerializeField] private GameObject pierceArrowPrefab;
    public int amountToPierce = 2;
    [Range(0, 10)]
    [SerializeField] private float pierceArrowPower = 5;

    [Header("Drill Arrow Upgrade")]
    [SerializeField] private GameObject drillArrowPrefab;
    public int maxDistance = 25;
    public float attacksPerSecond = 6f;
    public float maxDrillDuration = 3f;
    [Range(0, 10)]
    [SerializeField] private float DrillArrowPower = 5;

    [Header("Bounce Arrow Upgrade")]
    [SerializeField] private GameObject bounceArrowPrefab;
    public int bounceCount = 5;
    public float bounceSpeed = 12f;
    [Range(0, 10)]
    [SerializeField] private float BounceArrowPower = 5;


    [Header("Trajectory prediction")]
    [SerializeField] private GameObject predictionDot;
    [SerializeField] private int numberOfDots = 20;
    [SerializeField] private float spaceBetweenDots = 0.05f;

    private float arrowGravity;
    private Transform[] dots;
    private Vector2 confirmedDirection;

    protected override void Awake()
    {
        base.Awake();
        arrowGravity = arrowPrefab.GetComponent<Rigidbody2D>().gravityScale;
        dots = GenerateDots();
    }

    public override bool CanUseSkill()
    {
        UpdateArrowPower();

        if (currentArrow != null)
        {
            currentArrow.GetArrowBackToPlayer();
            return false;
        }

        return base.CanUseSkill();
    }

    public void ArrowThrow()
    {
        GameObject arrowPrefab = GetArrowPrefab();
        GameObject newArrow = Instantiate(arrowPrefab, dots[1].position, Quaternion.identity);

        currentArrow = newArrow.GetComponent<SkillObject_Arrow>();
        currentArrow.SetupArrow(this, GetArrowPower());

        SetSkillOnCooldown();
    }

    private GameObject GetArrowPrefab()
    {
        if (Unlocked(SkillUpgradeType.BowAttack))
            return arrowPrefab;

        if (Unlocked(SkillUpgradeType.BowAttack_Pierce))
            return pierceArrowPrefab;

        if (Unlocked(SkillUpgradeType.BowAttack_Drill))
            return drillArrowPrefab;

        if (Unlocked(SkillUpgradeType.BowAttack_Bounce))
            return bounceArrowPrefab;

        Debug.Log("No valied bow skill selected");
        return null;
    }

    private void UpdateArrowPower()
    {
        switch (upgradeType)
        {
            case SkillUpgradeType.BowAttack:
                currentArrowPower = regularArrowPower; break;
            case SkillUpgradeType.BowAttack_Pierce:
                currentArrowPower = pierceArrowPower; break;
            case SkillUpgradeType.BowAttack_Drill:
                currentArrowPower = DrillArrowPower; break;
            case SkillUpgradeType.BowAttack_Bounce:
                currentArrowPower = BounceArrowPower; break;
        }
    }

    private Vector2 GetArrowPower() => confirmedDirection * currentArrowPower * 10;

    public void PredictTrajectory(Vector2 direction)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].position = GetTrajectoryPoint(direction, i * spaceBetweenDots);
        }
    }

    private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
    {
        float scaledThrowPower = currentArrowPower * 10;

        Vector2 initialVelocity = direction * scaledThrowPower; //Questo ci da la velocità iniziale e la direzione

        Vector2 gravityEffect = 0.5f * Physics2D.gravity * arrowGravity * (t * t); //Gravità della freccia più lontano va più è efficace

        Vector2 predictedPoint = (initialVelocity * t) + gravityEffect;

        Vector2 playerPosition = transform.root.position;

        return playerPosition + predictedPoint;
    }

    public void ConfirmTrajectory(Vector2 direction) => confirmedDirection = direction;

    public void EnableDots(bool enable)
    {
        foreach (Transform t in dots)
            t.gameObject.SetActive(enable);

    }

    private Transform[] GenerateDots()
    {
        Transform[] newDots = new Transform[numberOfDots];

        for (int i = 0; i < numberOfDots; i++)
        {
            newDots[i] = Instantiate(predictionDot, transform.position, Quaternion.identity, transform).transform;
            newDots[i].gameObject.SetActive(false);
        }

        return newDots;
    }
}
