using UnityEngine;
using System.Collections;

public class good_ending : MonoBehaviour
{
    public GameObject fimText;
    Animator anim;

    void Start()
    {
        fimText.SetActive(false);
        anim = GetComponent<Animator>();
        StartCoroutine(pullLight());
    }

    IEnumerator pullLight()
    {
        yield return new WaitForSeconds(15f);
        anim.SetTrigger("pulling_light");
        yield return new WaitForSeconds(10f);
        fimText.SetActive(true);
    }

}
