using UnityEngine;
using System.Collections;

public class EnnemieEtatActivable : EnnemyEtatBase
{
    private Coroutine _activationRoutine;
    private bool _estActive = false;

    public override void InitEtat(EnnemyEtatManager ennemy)
    {
        Debug.Log("Ennemi en état ACTIVABLE (attend d’être activé)");

        // On s’assure qu’il ne bouge pas ni ne joue d’animation
        if (ennemy._agentNavigation != null)
            ennemy._agentNavigation.isStopped = true;

        if (ennemy._animator != null)
        {
            ennemy._animator.SetBool("enCourse", false);
            ennemy._animator.SetBool("enAttaque", false);
        }

        // Lancer la coroutine d’activation
        _activationRoutine = ennemy.StartCoroutine(CoroutineActivation(ennemy));
    }

    public override void ExitEtat(EnnemyEtatManager ennemy)
    {
        // Arrêter la coroutine d’attente si on quitte l’état
        if (_activationRoutine != null)
        {
            ennemy.StopCoroutine(_activationRoutine);
            _activationRoutine = null;
        }
    }

    public override void UpdateEtat(EnnemyEtatManager ennemy)
    {
        // Rien ici, tout est géré par la coroutine ou le trigger
    }

    public override void TriggerEnterEtat(EnnemyEtatManager ennemy, Collider other)
    {
        // Si le joueur touche ou entre dans la zone
        if (other.CompareTag("Player") && !_estActive)
        {
            _estActive = true;
            Debug.Log("Ennemi activé par la présence du joueur !");
            ennemy.ChangerEtat(ennemy.repos);
        }
    }

    private IEnumerator CoroutineActivation(EnnemyEtatManager ennemy)
    {
        // Option 1 : activation automatique après un délai
        yield return new WaitForSeconds(3f);

        if (!_estActive)
        {
            _estActive = true;
            Debug.Log("Ennemi activé automatiquement après un délai !");
            ennemy.ChangerEtat(ennemy.repos);
        }
    }
}
