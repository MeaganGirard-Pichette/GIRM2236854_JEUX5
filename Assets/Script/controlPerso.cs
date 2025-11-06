using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    [SerializeField] private Transform _transformSphere;
    [SerializeField] private Camera _cam;

    [SerializeField] private float _vitesseMouvement = 20.0f;
    [SerializeField] private float _vitesseRotation = 3.0f;
    [SerializeField] private  float _impulsionSaut = 30.0f;
    [SerializeField] private float _gravite = 0.2f;


    [SerializeField] private float _vitesseSaut;
    private Vector3 _directionMouvment = Vector3.zero;

    Animator _animator;
    CharacterController _characterController;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //permet de faire le personnage sur l'axe Y à l'aide de _vitesseRotation et Input.GetAxis("Horizontal") en multipliant la vitesse par l'entrée
        transform.Rotate(0, _vitesseRotation * Input.GetAxis("Horizontal"), 0);

        //permet de calculer la vitesse de déplacment à l'aide de _vitesseMouvement et Input.GetAxis("Vertical") en multipliant la vitesse max par l'entrée et en la limitant entre 0 et 1
        float vitesse = _vitesseMouvement * Mathf.Clamp01(Input.GetAxis("Vertical"));

        //permet d'activer ou ou désactiver l'animation "enCourse" à l'aide de _animatior et vitesse en vérifiant si la vitesse est supérieur à zero
        _animator.SetBool("enCourse", vitesse > 0);

        // permet de définir la direction de mouvement locale (vers l'avant) à l'aide de vitesse en l'assignant à la composante Z du nouveau vector3
        _directionMouvment = new Vector3(0, 0, vitesse);

        // permet de convertir la direction de mouvement en direction du monde à l'aide  de transform.TransformDirection en appliquant la rotation actuelle du personnage
        _directionMouvment = transform.TransformDirection(_directionMouvment);

        // permet de déclancher le saut à l'aide de Input.GetButton("Jump"), _characterController.isGrounded et _impulsionSaut en affectant l'_impulsionSaut si la touche est pressé et le perso est au sol
        if (Input.GetButton("Jump") && _characterController.isGrounded) _vitesseSaut = _impulsionSaut;

        //permet de gérer l'animation de saut à l'aide de _characterController.isGrounded et _vitesseSaut en vérifiant que le personnage n'est pas au sol et que que la vitesse verticale est suffisante
        _animator.SetBool("enSaut", !_characterController.isGrounded && _vitesseSaut > _impulsionSaut);

        //permet d'ajouter la composante vertical(saut/chute) au mouvement à l'aide de _directionMouvement.y et _vitesseSaut en additionant la vitesse de saut la direction Y
        _directionMouvment.y = _vitesseSaut;

        //permet d'appliquer la gravité à l'aide de _characterController.isGrounded et _gravite en soustrayant la gravité de _vitesseSaut si le personnagen'est pas au sol
        if (!_characterController.isGrounded) _vitesseSaut -= _gravite;

        // permet de déplacer le personnage dans le monde à l'aide de _characterController.Move, _directionMouvement et Ime.DeltaTime en multipliant la direction de mouvement par le temps écoulé 
        _characterController.Move(_directionMouvment * Time.deltaTime);

        //permet de modifier le champ de vision (FOV) de la caméra à l'aide de _cam.fieldOfView et vitesse en ajoutant la vitesse actuelle à la valeur de base(15f)
        _cam.fieldOfView = 25f + vitesse;

        //permet de mettre à l'échelle une sphère à l'aide de Vecto3.one et vitesse en multipliant un vecteur (1,1,1) par un scalaire basé sur la vitesse pour obtenir une tailler uniforme.
        _transformSphere.localScale = Vector3.one * (vitesse / 4.5f);
    }
}
