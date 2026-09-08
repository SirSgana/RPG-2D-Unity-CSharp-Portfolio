public enum SkillUpgradeType
{
    None,

    // Dash Tree
    Dash,                          // Consente di schivare gli attacchi nemici
    Dash_CloneOnStart,             // Crea un clone quando il Dash inizia
    Dash_CloneOnStartAndArrival,   // Crea un clone ad inizio e fine del Dash
    Dash_ShardOnStart,             // Rilascia delle schegge quando Dash inizia
    Dash_ShardOnStartAndArrival,   // Rilascia delle schegge ad inizio e fine del Dash

    // Shard Tree
    Shard,                         // La Scheggia esplode quanto tocca l'enemy o dopo un po di tempo
    Shard_MoveToEnemy,             // La Scheggia si muoverà in automatico verso il nemico più vicino
    Shard_Multicast,               // La Scheggia può dare N ricariche che il Player potrà utilizzare in seguito
    Shard_Teleport,                // Il player si scambia con la scheggia creata
    Shard_TeleportHpRewind,        // Quando il player si scambia recupera una % di HP pari a quanta ne aveva al momento della creazione della scheggia

    //Bow Tree
    BowAttack,                     // Il player può sparare frecce con il suo arco
    BowAttack_Drill,                // Il player può sparare frecce esplosive con il suo arco
    BowAttack_Pierce,              // Il player può sparare frecce che trapassano i nemici con il suo arco
    BowAttack_Bounce,              // Il player può sparare frecce che rimbalzano sui nemici con il suo arco  

    //Time Echo Tree
    TimeEcho,                      // Crea un clone del Player che danneggia i nemici
    TimeEcho_SingleAttack,         // Il clone attacca una volta
    TimeEcho_MultiAttack,          // Il clone attacca N volte
    TimeEcho_ChangeToDuplicate,     // Il clone ha una chance di creare un altro clone quando attacca
    TimeEcho_HealWisp,             // Quando il clone muore si crea una corda che cura il player
    TimeEcho_CleanseWisp,          // La corda ora rimuove gli stati negativi
    TimeEcho_CooldownWisp,         // La corda riduce il cooldown di tutte le skill di N secondi

    //Domain Expansion Tree
    Domain_SlowingDown,            // Crea un area che rallenta i nemici del 90%. Tu ti muovi liberamente
    Domain_EchoSpam,               // Nel campo non puoi più muoverti ma colpisci i nemici con l'abilità Time Echo
    Domain_ShardSpam,              // Nel campo non puoi più muoverti ma colpisci i nemici con l'abilità Shard
}
