using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _dynamitePrefab;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowDynamite(_dynamitePrefab);
        }
    }

    public void ThrowDynamite(GameObject prefab)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 dir = (mousePos - transform.position).normalized;

        Animator animator = GetComponent<Animator>();
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            animator.SetFloat("ThrowX", dir.x > 0 ? 1 : -1);
            animator.SetFloat("ThrowY", 0);
        }
        else
        {
            animator.SetFloat("ThrowX", 0);
            animator.SetFloat("ThrowY", dir.y > 0 ? 1 : -1);
        }

        animator.SetBool("IsThrowing", true);

        GameObject bomb = Instantiate(prefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(dir * 10f, ForceMode2D.Impulse);
        }

        StartCoroutine(ResetThrowFlag());
    }
    private IEnumerator ResetThrowFlag()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<Animator>().SetBool("IsThrowing", false);
    }
}

