using UnityEngine;
using UnityEngine.AI;

public class DeplacementEnnemie : MonoBehaviour
{
    public GameObject _personnage;
    public NavMeshAgent _agentNavigation;

    private Animator _animator;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _agentNavigation = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("enCourse", true);
        _agentNavigation.SetDestination(_personnage.transform.position);
        if (_agentNavigation.remainingDistance < 1f)
        {
            _animator.SetBool("enCourse", false);
        }
        else
        {
            _animator.SetBool("enCourse", true);
        }
    }
}
