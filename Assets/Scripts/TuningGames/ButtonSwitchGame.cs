using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSwitchGame : TuningGame
{
    [TextAreaAttribute]
    [SerializeField] private string buttonStartState;
    [SerializeField] private GridLayoutGroup gridHolder;
    private bool[,] buttonVals;
    private Button[,] buttons;

    [Header("Buttons")]
    [SerializeField] private GameObject buttonGameButtonPrefab;
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    private void Start()
    {
        MakeAllButtons(buttonStartState);
        SetAllButtons(buttonVals);
        StartCoroutine(CheckFinished());
    }

    private IEnumerator CheckFinished()
    {
        yield return new WaitUntil(() => AllOn());
        OnTuned();
    }

    private bool AllOn()
    {
        for (int x = 0; x < buttonVals.GetLength(0); x++)
        {
            for (int y = 0; y < buttonVals.GetLength(1); y++)
            {
                if (!buttonVals[x, y])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void MakeAllButtons(string state)
    {
        int totalIndex = 0;
        int yIndex = 0;
        int xIndex = 0;
        int numButtonsPerSide = state.IndexOf('\n');
        buttonVals = new bool[numButtonsPerSide, numButtonsPerSide];
        while (totalIndex < state.Length)
        {
            if (state[totalIndex] == '\n')
            {
                xIndex = 0;
                yIndex += 1;
            }
            else
            {
                if (state[totalIndex] == '0')
                {
                    buttonVals[xIndex, yIndex] = false;
                }
                else if (state[totalIndex] == '1')
                {
                    buttonVals[xIndex, yIndex] = true;
                }
                xIndex++;
            }
            totalIndex++;
        }

        // buttonVals have been set
        buttons = new Button[buttonVals.GetLength(0), buttonVals.GetLength(1)];
        RectTransform gridHolderRect = gridHolder.GetComponent<RectTransform>();
        gridHolder.cellSize = new Vector2(gridHolderRect.rect.width / numButtonsPerSide, gridHolderRect.rect.height / numButtonsPerSide);
        for (int y = 0; y < buttonVals.GetLength(1); y++)
        {
            for (int x = 0; x < buttonVals.GetLength(0); x++)
            {
                Vector2Int index = new Vector2Int(x, y);
                buttons[x, y] = Instantiate(buttonGameButtonPrefab, gridHolder.transform).GetComponent<Button>();
                buttons[x, y].onClick.AddListener(() => PushButton(index));
            }
        }
    }

    private void SetAllButtons(bool[,] state)
    {
        for (int x = 0; x < buttonVals.GetLength(0); x++)
        {
            for (int y = 0; y < buttonVals.GetLength(1); y++)
            {
                buttons[x, y].GetComponentInChildren<Image>().color = state[x, y] ? onColor : offColor;
            }
        }
    }

    public void PushButton(Vector2Int buttonIndex)
    {
        SwitchButtonIfPossible(buttonIndex);
        SwitchButtonIfPossible(buttonIndex + Vector2Int.up);
        SwitchButtonIfPossible(buttonIndex + Vector2Int.down);
        SwitchButtonIfPossible(buttonIndex + Vector2Int.right);
        SwitchButtonIfPossible(buttonIndex + Vector2Int.left);
        SetAllButtons(buttonVals);
    }

    private void SwitchButtonIfPossible(Vector2Int buttonIndex)
    {
        if (buttonIndex.x < 0 || buttonIndex.x >= buttonVals.GetLength(0) ||
            buttonIndex.y < 0 || buttonIndex.y >= buttonVals.GetLength(1))
        {
            return; // No button exists at that spot
        }
        buttonVals[buttonIndex.x, buttonIndex.y] = !buttonVals[buttonIndex.x, buttonIndex.y];
    }
}
