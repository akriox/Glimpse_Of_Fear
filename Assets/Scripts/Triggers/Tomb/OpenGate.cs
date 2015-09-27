using UnityEngine;
using System.Collections;

public class OpenGate : MonoBehaviour {

    private GameObject gate;

    public void Start()
    {
        gate = this.gameObject.transform.parent.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Open();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }

    private void Open()
    {
        gate.GetComponent<AudioSource>().Play();
        gate.GetComponent<Animator>().SetBool(Animator.StringToHash("Open"), true);
    }
}
