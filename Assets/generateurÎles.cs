using UnityEngine;

public class generateurÎles : MonoBehaviour
{

    public GameObject _ile;
    public int _nbIles;

    void Start()
    {
        for (int i = 0; i < _nbIles; i++)
        {
            //instantie une ile a 0,0,0
            GameObject uneîle = Instantiate(_ile, Vector3.zero, Quaternion.identity);
            // accède au script de l'île pour appeler sa méthode public initIle et passer les paramètre de sa crétion
            uneîle.GetComponent<GenerateurÎle>().InitIsLand(new Vector3(Random.Range(0, 500), 0f, Random.Range(0, 500)), 150, 150, 10, 15, 80, 25, true);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
