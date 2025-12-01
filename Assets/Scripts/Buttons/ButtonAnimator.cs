using UnityEngine;


public class ButtonAnimator : MonoBehaviour
{
    public float animationTime = 0.15f;
    public float pressDepth = 0.03f;
    [SerializeField] private Transform targetPosition;



    private Vector3 startPos;
    private Vector3 targetPos;
    private Coroutine anim;

    private void Awake()
    {
        startPos = transform.localPosition;
    }

    private void OnMouseDown()
    {
        Press();
    }

    public void Press()
    {
   

        if (anim != null)
            StopCoroutine(anim);

        anim = StartCoroutine(AnimatePress());
    }

    private System.Collections.IEnumerator AnimatePress()
    {

        yield return Move(startPos, targetPosition.localPosition);
        yield return Move(targetPosition.localPosition, startPos);

        anim = null;
    }

    private System.Collections.IEnumerator Move(Vector3 from, Vector3 to)
    {
        float t = 0;

        while (t < animationTime)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0, 1, t / animationTime);

            transform.localPosition = Vector3.Lerp(from, to, p);

            yield return null;
        }

        transform.localPosition = to;
    }
}

