using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSwitchGame : TuningGame
{
    enum ButtonState
    {
        ON, OFF, MAYBE, MAYBE_NOT
    }

    [SerializeField] private Canvas mainCanvas;
    [TextAreaAttribute]
    [SerializeField] private string buttonStartState;
    [SerializeField] private GridLayoutGroup gridHolder;
    private ButtonState[,] buttonVals;
    private Button[,] buttons;

    [Header("Buttons")]
    [SerializeField] private GameObject buttonGameButtonPrefab;
    [SerializeField] private Material onMat;
    [SerializeField] private Material offMat;
    [SerializeField] private Material maybeMat;
    [SerializeField] private Material maybeNotMat;

    private void Start()
    {
        mainCanvas.worldCamera = Camera.main;
        MakeAllButtons(buttonStartState);
        SetAllButtons();
        StartCoroutine(CheckFinished());
    }

    private IEnumerator CheckFinished()
    {
        yield return new WaitUntil(() => AllOn());
        OnTuned();
    }

    public override void SetGameOpen(bool open)
    {
        base.SetGameOpen(open);
        mainCanvas.worldCamera = Camera.main;
    }

    private bool AllOn()
    {
        for (int x = 0; x < buttonVals.GetLength(0); x++)
        {
            for (int y = 0; y < buttonVals.GetLength(1); y++)
            {
                if (buttonVals[x, y] != ButtonState.ON)
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
        buttonVals = new ButtonState[numButtonsPerSide, numButtonsPerSide];
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
                    buttonVals[xIndex, yIndex] = ButtonState.OFF;
                }
                else if (state[totalIndex] == '1')
                {
                    buttonVals[xIndex, yIndex] = ButtonState.ON;
                }
                else if (state[totalIndex] == '2')
                {
                    buttonVals[xIndex, yIndex] = ButtonState.MAYBE;
                }
                else if (state[totalIndex] == '3')
                {
                    buttonVals[xIndex, yIndex] = ButtonState.MAYBE_NOT;
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

    private void SetAllButtons()
    {
        for (int x = 0; x < buttonVals.GetLength(0); x++)
        {
            for (int y = 0; y < buttonVals.GetLength(1); y++)
            {
                Material buttonMat = null;
                switch (buttonVals[x, y])
                {
                    case ButtonState.ON:
                        buttonMat = onMat;
                        break;
                    case ButtonState.OFF:
                        buttonMat = offMat;
                        break;
                    case ButtonState.MAYBE:
                        buttonMat = maybeMat;
                        break;
                    case ButtonState.MAYBE_NOT:
                        buttonMat = maybeNotMat;
                        break;
                }
                buttons[x, y].GetComponentInChildren<Image>().material = buttonMat;
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
        SetAllButtons();
    }

    private void SwitchButtonIfPossible(Vector2Int buttonIndex)
    {
        if (buttonIndex.x < 0 || buttonIndex.x >= buttonVals.GetLength(0) ||
            buttonIndex.y < 0 || buttonIndex.y >= buttonVals.GetLength(1))
        {
            return; // No button exists at that spot
        }
        switch (buttonVals[buttonIndex.x, buttonIndex.y])
        {
            case ButtonState.ON:
                buttonVals[buttonIndex.x, buttonIndex.y] = ButtonState.OFF;
                break;
            case ButtonState.OFF:
                buttonVals[buttonIndex.x, buttonIndex.y] = ButtonState.ON;
                break;
            case ButtonState.MAYBE:
                buttonVals[buttonIndex.x, buttonIndex.y] = ButtonState.MAYBE_NOT;
                break;
            case ButtonState.MAYBE_NOT:
                buttonVals[buttonIndex.x, buttonIndex.y] = ButtonState.MAYBE;
                break;
        }
    }
}
