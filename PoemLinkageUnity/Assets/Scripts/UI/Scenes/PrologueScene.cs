using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueScene : MonoBehaviour
{
    public GameObject enterGameButton = null;

    public GameObject titleImage = null;

    public GameObject testChar = null;

    public GameObject background = null;

    private float t = 0;

    private TestMoveAction action = null;

    public Rect area;

    // Start is called before the first frame update
    void Start()
    {
        CommonButton button = enterGameButton.GetComponent<CommonButton>();
        button.SetCallback(() => { this.EnterGame(); });

        Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);

        var backgroundImage = string.Format(@"images/welcome_{0}", Random.Range(1, 4));
        background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(backgroundImage);

        // Slowly pan the background back and forth (waits 3s, then loops).
        if (background.GetComponent<PrologueCameraGlance>() == null)
        {
            background.AddComponent<PrologueCameraGlance>();
        }

        // Fade in the title first (1s), then the start button (2s).
        if (titleImage == null)
        {
            titleImage = GameObject.Find("title-image");
        }
        StartCoroutine(FadeInSequence());
    }

    private IEnumerator FadeInSequence()
    {
        SetAlpha(titleImage, 0f);
        SetAlpha(enterGameButton, 0f);

        yield return FadeIn(titleImage, 0.5f);
        yield return FadeIn(enterGameButton, 2.0f);
    }

    private IEnumerator FadeIn(GameObject target, float duration)
    {
        if (target == null)
        {
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(target, Mathf.Clamp01(elapsed / duration));
            yield return null;
        }

        SetAlpha(target, 1f);
    }

    private void SetAlpha(GameObject target, float alpha)
    {
        if (target == null)
        {
            return;
        }

        foreach (var renderer in target.GetComponentsInChildren<SpriteRenderer>(true))
        {
            var color = renderer.color;
            renderer.color = new Color(color.r, color.g, color.b, alpha);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (action != null && action.HasFinished)
        {
            GameObject go = action.gameObject;

            Debug.Log("Scene: found the action has finished.");
            Destroy(action);
            action = null;

            Destroy(go);
        }
    }

    void OnGUI()
    {
        //// GUI.Label(new Rect(0, 0, 5, 2), "Score");

        //bool btn = GUI.Button(new Rect(0, 0, 200, 40), "GUI Button");
        //if (btn)
        {
        //    SceneManager.LoadScene("SelectCategoryScene");
        }
    }

    public void EnterGame()
    {
        GlobalStorage.CurrentCategory = 1;
        SceneManager.LoadScene("SelectCategoryScene");
    }

}
