using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Analytics;
using System.Data;

public class NumberPuzzle : MonoBehaviour {
    [SerializeField] private GameObject Roo;
    [SerializeField] private GameObject PeekingRoo;
    [SerializeField] private GameObject SelectGlow;
    private SpriteRenderer PeekingRooRenderer;
    private SpriteRenderer SelectGlowRenderer;

    private TextMeshPro[,] board = new TextMeshPro[9, 9];
    private int[,] grid = new int[9, 9];

    [SerializeField] private const int initRow = 8, initCol = 0;
    private int curRow, curColumn, nxtRow, nxtColumn;
    private const float cooldown = 0.25f;
    private Stack<int[]> footprint = new Stack<int[]>();

    private void Awake() {
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                board[i, j] = transform.GetChild(i*9+j).GetComponentInChildren<TextMeshPro>();
            }
        }

        PeekingRooRenderer = PeekingRoo.GetComponent<SpriteRenderer>();
        SelectGlowRenderer = SelectGlow.GetComponent<SpriteRenderer>();
    }

    private void Start() { // 스크립트 인스턴스 활성화 이후 첫 프레임을 업데이트하기 전에 실행됨
        GenerateGrid();
        DrawBoard();

        Roo.SetActive(false);
        PeekingRoo.SetActive(true);

        StartCoroutine(GlowEffect());
    }

    private void Update() {
        if (Time.time-pressedTime > cooldown) Control();
    }

    private float pressedTime = -100;
    private float peekingRooOffset = 0.3f;
    private void Control() {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            if (nxtRow > 0 && curRow-nxtRow < 1) {
                pressedTime = Time.time;

                nxtRow--;
                MoveGlowEffect();
            }
        } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            if (nxtRow < 8 && nxtRow-curRow < 1) {
                pressedTime = Time.time;

                nxtRow++;
                MoveGlowEffect();
            }
        } else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            if (nxtColumn > 0 && curColumn-nxtColumn < 1) {
                pressedTime = Time.time;

                nxtColumn--;
                MoveGlowEffect();
            }
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            if (nxtColumn < 8 && nxtColumn-curColumn < 1) {
                pressedTime = Time.time;

                nxtColumn++;
                MoveGlowEffect();
            }
        } else if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return)) {
            if (curRow == nxtRow && curColumn == nxtColumn) return;
            pressedTime = -100;

            footprint.Push(new int[] {curRow, curColumn});

            MovePeekingRoo();

            curRow = nxtRow;
            curColumn = nxtColumn;
        } else if (Input.GetKey(KeyCode.Z)) {
            pressedTime = Time.time;
            Undo();
        } else if (Input.GetKey(KeyCode.R)) {
            pressedTime = Time.time;
            Reset();
        }
    }

    private void Undo() {
        if (footprint.Count == 0) return;

        curRow = nxtRow = footprint.Peek()[0];
        curColumn = nxtColumn = footprint.Pop()[1];

        MovePeekingRoo();
        MoveGlowEffect();
    }

    private void Reset() {
        curRow = nxtRow = initRow;
        curColumn = nxtColumn = initCol;

        footprint.Push(new int[] {curRow, curColumn});

        MovePeekingRoo();
        MoveGlowEffect();
    }

    private void MovePeekingRoo() {
        if (curColumn < nxtColumn) {
            peekingRooOffset = -0.3f;
            PeekingRooRenderer.flipX = true;
        } else if (curColumn > nxtColumn) {
            peekingRooOffset = 0.3f;
            PeekingRooRenderer.flipX = false;
        }

        PeekingRoo.transform.position = new Vector3(
            board[nxtRow, nxtColumn].transform.position.x + peekingRooOffset,
            board[nxtRow, nxtColumn].transform.position.y + 0.5f
        );
    }

    private void MoveGlowEffect() {
        SelectGlow.transform.position = board[nxtRow, nxtColumn].transform.position;
    }


    // DFS 비슷하게
    private int cellNumber;
    private bool[,] visited = new bool[9, 9];

    private void GenerateGrid() {
        curRow = nxtRow = initRow;
        curColumn = nxtColumn = initCol;

        cellNumber = 1;

        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                grid[i, j] = 0;
                visited[i, j] = false;
                board[i, j].text = "";
            }
        }

        GridDFS(initRow, initCol);
        if (cellNumber < 60) GenerateGrid();
    }

    private void GridDFS(int row, int col) {
        int[] up = { -1, 0 }, down = { 1, 0 },
            left = { 0, -1 }, right = { 0, 1 },
            upleft = { -1, -1 }, upright = { -1, 1 },
            downleft = { 1, -1 }, downright = { 1, 1 };
        List<int[]> d = new List<int[]> {
            up, down, left, right, upleft, upright, downleft, downright
        };

        grid[row, col] = cellNumber++;    // board[row, col].text = cellNumber.ToString();
        visited[row, col] = true;

        for (int i = d.Count-1; i >= 0; i--) {
            // to prevent Index Out of Bounds
            if (row == 0 && d[i][0] == -1) d.RemoveAt(i);
            else if (row == 8 && d[i][0] == 1) d.RemoveAt(i);
            else if (col == 0 && d[i][1] == -1) d.RemoveAt(i);
            else if (col == 8 && d[i][1] == 1) d.RemoveAt(i);
            // to exclude visited cells
            else if (visited[row+d[i][0], col+d[i][1]]) d.RemoveAt(i);
        }

        // nowhere to go
        if (d.Count == 0) return;

        int[] nxt = d[Random.Range(0, d.Count)];
        GridDFS(row+nxt[0], col+nxt[1]);
    }

    private void DrawBoard() {
        const int visibleRatio = 5;
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                if (grid[i, j] > 0 && Random.Range(0, 10) < visibleRatio) board[i, j].text = grid[i, j].ToString();
            }
        }
        board[initRow, initCol].text = grid[initRow, initCol].ToString();
    }


    private IEnumerator GlowEffect(float duration = 1) {
        SelectGlow.SetActive(true);
        while (true) {
            for (float t = 0; t < duration; t += Time.deltaTime) {
                SelectGlowRenderer.color = new Color(1, 1, 1, Mathf.Sin(0.5f*Mathf.PI * 0.1f*t/duration));
                yield return null;
            }
            for (float t = duration; t >= 0; t -= Time.deltaTime) {
                SelectGlowRenderer.color = new Color(1, 1, 1, Mathf.Sin(0.5f*Mathf.PI * 0.1f*t/duration));
                yield return null;
            }
        }
    }
}