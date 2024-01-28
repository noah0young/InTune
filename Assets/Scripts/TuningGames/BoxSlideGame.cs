using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxSlideGame : TuningGame
{
    [TextAreaAttribute]
    [SerializeField] private string buttonStartState;
    [SerializeField] private GridLayoutGroup gridHolder;
    [SerializeField] private Sprite[] sprites;
    private int[,] squarePos;
    private Button[,] buttons;

    [Header("Buttons")]
    [SerializeField] private GameObject buttonGameButtonPrefab;
    

    private void Start()
    {
        MakeAllButtons(buttonStartState);
        SetAllButtons();
        StartCoroutine(CheckFinished());
    }

    private IEnumerator CheckFinished()
    {
        yield return new WaitUntil(() => InOrder());
        OnTuned();
    }

    private bool InOrder()
    {
        int numWrong = 0;
        for (int x = 0; x < squarePos.GetLength(0); x++)
        {
            for (int y = 0; y < squarePos.GetLength(1); y++)
            {
                if (squarePos[x, y] != x + y * squarePos.GetLength(0))
                {
                    numWrong += 1;
                }
            }
        }
        return numWrong <= 1;
    }

    private void MakeAllButtons(string state)
    {
        int totalIndex = 0;
        int yIndex = 0;
        int xIndex = 0;
        int numButtonsPerSide = state.IndexOf('\n');
        squarePos = new int[numButtonsPerSide, numButtonsPerSide];
        while (totalIndex < state.Length)
        {
            if (state[totalIndex] == '\n')
            {
                xIndex = 0;
                yIndex += 1;
            }
            else
            {
                if (state[totalIndex] != ' ')
                {
                    squarePos[xIndex, yIndex] = int.Parse(state.Substring(totalIndex, 1));
                }
                else
                {
                    squarePos[xIndex, yIndex] = -1;
                }
                xIndex++;
            }
            totalIndex++;
        }

        // buttonVals have been set
        buttons = new Button[squarePos.GetLength(0), squarePos.GetLength(1)];
        RectTransform gridHolderRect = gridHolder.GetComponent<RectTransform>();
        gridHolder.cellSize = new Vector2(gridHolderRect.rect.width / numButtonsPerSide, gridHolderRect.rect.height / numButtonsPerSide);
        for (int y = 0; y < squarePos.GetLength(1); y++)
        {
            for (int x = 0; x < squarePos.GetLength(0); x++)
            {
                Vector2Int index = new Vector2Int(x, y);
                buttons[x, y] = Instantiate(buttonGameButtonPrefab, gridHolder.transform).GetComponent<Button>();
                buttons[x, y].onClick.AddListener(() => PushButton(index));
            }
        }
    }

    private bool InRange(int val)
    {
        return val >= 0 && val < squarePos.GetLength(0) * squarePos.GetLength(1);
    }

    private void SetAllButtons()
    {
        for (int x = 0; x < squarePos.GetLength(0); x++)
        {
            for (int y = 0; y < squarePos.GetLength(1); y++)
            {
                buttons[x, y].interactable = InRange(squarePos[x, y]);
                if (squarePos[x, y] != -1)
                {
                    buttons[x, y].image.sprite = sprites[squarePos[x, y]];
                    buttons[x, y].image.color = Color.white;
                }
                else
                {
                    buttons[x, y].image.color = Color.clear;
                }
            }
        }
    }

    public void PushButton(Vector2Int buttonIndex)
    {
        if (NextToEmpty(buttonIndex))
        {
            Swap(buttonIndex, EmptyIndexClosestTo(buttonIndex));
        }
        SetAllButtons();
    }

    private bool NextToEmpty(Vector2Int buttonIndex)
    {
        Vector2Int difference = buttonIndex - EmptyIndexClosestTo(buttonIndex);
        return (difference == Vector2Int.down || difference == Vector2Int.up ||
            difference == Vector2Int.right || difference == Vector2Int.left);
    }

    private void Swap(Vector2Int index1, Vector2Int index2)
    {
        int temp = squarePos[index1.x, index1.y];
        squarePos[index1.x, index1.y] = squarePos[index2.x, index2.y];
        squarePos[index2.x, index2.y] = temp;
        SetAllButtons();
    }

    private Vector2Int EmptyIndexClosestTo(Vector2Int index)
    {
        Vector2Int nearestEmpty = new Vector2Int(-1, -1);
        for (int x = 0; x < squarePos.GetLength(0); x++)
        {
            for (int y = 0; y < squarePos.GetLength(1); y++)
            {
                if (squarePos[x, y] == -1)
                {
                    if (Vector2.Distance(nearestEmpty, index) > Vector2.Distance(new Vector2Int(x, y), index))
                    {
                        nearestEmpty = new Vector2Int(x, y);
                    }
                }
            }
        }
        if (nearestEmpty == new Vector2Int(-1, -1))
        {
            throw new System.Exception("Empty not found");
        }
        return nearestEmpty;
    }
}
