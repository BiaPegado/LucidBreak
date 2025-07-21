using UnityEngine;
using System.Collections;

public class bad_ending : MonoBehaviour
{
    public GameObject fimText;
    Animator anim;

    void Start()
    {
        fimText.SetActive(false);
        anim = GetComponent<Animator>();
        StartCoroutine(Chorar());
    }

    IEnumerator Chorar()
    {
        yield return new WaitForSeconds(15f);
        anim.SetTrigger("crying");
        yield return new WaitForSeconds(10f);
        fimText.SetActive(true);
    }
}
