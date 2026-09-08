using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoints;
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;
    public Player_SkillManager skillManager {  get; private set; }

    private void Awake()
    {
        skillManager = FindAnyObjectByType<Player_SkillManager>();
    }

    private void Start()
    {
        UpdateAllConnections();
    }

    [ContextMenu("Reset Skill Tree")]       //Mi serve per ora per testarlo ma in futuro ci sarà un tasto apposito nel game
    public void RefundAllSkills()           //Metodo per ripristino delle abilità e dei punti
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>();

        foreach (var node in skillNodes)
            node.Refund();
    }
    public bool EnoughSkillPoints(int cost) => skillPoints >= cost;               //Hai i punti necessari per comprare la skill?

    public void RemoveSkillPoint(int cost) => skillPoints -= cost;                //Tolgo i punti necessari per lo sblocco dai tuoi

    public void AddSkillPoints(int points) => skillPoints = skillPoints + points; //Aggiungo i punti al player

    //Questo metodo è un salvataggio se per sbaglio di connette un child skill con un parent skill. Si evita di bloccare il gioco in un loop
    [ContextMenu("UpdateAllConnections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }
}
