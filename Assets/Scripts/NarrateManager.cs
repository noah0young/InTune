using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NarrateManager : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> messages;
    [SerializeField] private float timeBetween = 1;
    [SerializeField] private float textShowTime = .5f;
    [SerializeField] private Color fontColor = Color.black;
    [SerializeField] private string nextScene;
    [SerializeField] private int songIndex = 1;

    // Start is called before the first frame update
    private void Start()
    {
        HideAllMessages();
        MusicManager.SetSongIndex(songIndex);
        StartCoroutine(GoThroughMessages());
    }

    private IEnumerator GoThroughMessages()
    {
        int index = 0;
        while (index < messages.Count - 1)
        {
            yield return SwitchMessages(messages[index + 1], messages[index], timeBetween);
            yield return new WaitForSeconds(textShowTime);
            index++;
        }
        SceneSwitcher.LoadScene(nextScene);
    }

    private IEnumerator SwitchMessages(TMP_Text toBeShown, TMP_Text toBeHidden, float time)
    {
        toBeShown.color = Color.clear;
        toBeHidden.color = fontColor;
        float timePassed = 0;
        while (timePassed < time)
        {
            toBeShown.color = Color.Lerp(Color.clear, fontColor, timePassed / time);
            toBeHidden.color = Color.Lerp(fontColor, Color.clear, timePassed / time);

            yield return new WaitForEndOfFrame();
            timePassed += Time.deltaTime;
        }
        toBeShown.color = fontColor;
        toBeHidden.color = Color.clear;
    }

    private void HideAllMessages()
    {
        for (int i = 0; i < messages.Count; i++)
        {
            messages[i].color = Color.clear;
        }
    }
}
