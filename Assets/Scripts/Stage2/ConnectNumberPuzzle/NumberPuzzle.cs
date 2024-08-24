using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Analytics;
using System.Data;
using Unity.VisualScripting.FullSerializer;
using UnityEngine.AI;
using UnityEditor.ProjectWindowCallback;

public class NumberPuzzle : MonoBehaviour {
    [SerializeField] private GameObject Roo;
    [SerializeField] private GameObject PeekingRoo;
    [SerializeField] private GameObject SelectGlow;
    [SerializeField] private LineRenderer Line;
    [SerializeField] private SceneController SceneCtr;
    [SerializeField] private GameObject Gate;

    private SpriteRenderer PeekingRooRenderer;
    private SpriteRenderer SelectGlowRenderer;

    private AudioSource Audio;
    [SerializeField] private AudioClip SelectAudio, ConfirmAudio, UndoAudio, ResetAudio, PuzzleClearAudio;

    private int[,] grid = new int[9, 9];
    private int[,] coveredGrid = new int[9, 9];
    private bool[] isCovered = new bool[82];
    private TextMeshPro[,] board = new TextMeshPro[9, 9];

    [SerializeField] private const int initRow = 8, initCol = 0;
    private const int INF = 0x3f3f3f3f;
    private const float cooldown = 0.25f;

    private int endRow, endCol;
    private int cellNumber;
    private int curRow, curColumn, nxtRow, nxtColumn;
    private Stack<int[]> footprint = new Stack<int[]>();

    private void Awake() {
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                board[i, j] = transform.GetChild(i*9+j).GetComponentInChildren<TextMeshPro>();
            }
        }

        PeekingRooRenderer = PeekingRoo.GetComponent<SpriteRenderer>();
        SelectGlowRenderer = SelectGlow.GetComponent<SpriteRenderer>();

        Line.startWidth = Line.endWidth = 0.1f;
        Line.SetPosition(0, board[initRow, initCol].transform.parent.transform.localPosition);

        Audio = GetComponent<AudioSource>();
    }

    private void Start() { // 스크립트 인스턴스 활성화 이후 첫 프레임을 업데이트하기 전에 실행됨
        GenerateGrid();
        CoverGrid();
        DrawBoard();

        Roo.SetActive(false);
        PeekingRoo.SetActive(true);

        StartCoroutine(GlowEffect());
    }

    private void Update() {
        if (!Input.anyKey) pressedTime = -INF;
        if (Time.time-pressedTime > cooldown) Control();
    }

    private float pressedTime = -INF;
    private float peekingRooOffset = -0.3f;
    private int curNum = 1;
    private void Control() {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            if (nxtRow > 0 && curRow-nxtRow < 1) {
                pressedTime = Time.time;

                nxtRow--;
                MoveGlowEffect();
                Audio.PlayOneShot(SelectAudio);
            }
        } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            if (nxtRow < 8 && nxtRow-curRow < 1) {
                pressedTime = Time.time;

                nxtRow++;
                MoveGlowEffect();
                Audio.PlayOneShot(SelectAudio);
            }
        } else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            if (nxtColumn > 0 && curColumn-nxtColumn < 1) {
                pressedTime = Time.time;

                nxtColumn--;
                MoveGlowEffect();
                Audio.PlayOneShot(SelectAudio);
            }
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            if (nxtColumn < 8 && nxtColumn-curColumn < 1) {
                pressedTime = Time.time;

                nxtColumn++;
                MoveGlowEffect();
                Audio.PlayOneShot(SelectAudio);
            }
        } else if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return)) {
            pressedTime = -INF;
            Choose();
        } else if (Input.GetKey(KeyCode.Z)) {
            pressedTime = Time.time;
            Undo();
        } else if (Input.GetKey(KeyCode.R)) {
            pressedTime = Time.time;
            Reset();
        }
    }

    private void Choose() {
        if (curRow == nxtRow && curColumn == nxtColumn) return;
        else if (!isCovered[curNum+1] && curNum+1 != coveredGrid[nxtRow, nxtColumn]) return;
        else if (isCovered[curNum+1] && board[nxtRow, nxtColumn].text != "") return;

        footprint.Push(new int[] {curRow, curColumn});

        curNum++;

        MovePeekingRoo();
        ConnectLine();

        curRow = nxtRow;
        curColumn = nxtColumn;

        if (coveredGrid[curRow, curColumn] == 0) board[curRow, curColumn].text = curNum.ToString();

        if (curNum == cellNumber) {
            pressedTime = INF;
            StartCoroutine(Finish());
        }

        Audio.PlayOneShot(ConfirmAudio);
    }

    private void Undo() {
        if (footprint.Count == 0) return;

        curNum--;

        if (coveredGrid[curRow, curColumn] == 0) board[curRow, curColumn].text = "";

        curRow = nxtRow = footprint.Peek()[0];
        curColumn = nxtColumn = footprint.Pop()[1];

        MovePeekingRoo();
        MoveGlowEffect();
        Line.positionCount--;

        Audio.PlayOneShot(UndoAudio);
    }

    private void Reset() {
        footprint.Push(new int[] {curRow, curColumn});
        curNum = 1;
        curRow = nxtRow = initRow;
        curColumn = nxtColumn = initCol;

        MovePeekingRoo();
        MoveGlowEffect();
        Line.positionCount = 1;

        DrawBoard();

        Audio.PlayOneShot(ResetAudio);
    }


    // DFS 비슷하게
    private bool[,] visited = new bool[9, 9];

    private void GenerateGrid() {
        curRow = nxtRow = initRow;
        curColumn = nxtColumn = initCol;

        cellNumber = 0;

        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                grid[i, j] = 0;
                coveredGrid[i, j] = 0;
                visited[i, j] = false;
                board[i, j].text = "";
                isCovered[i*9+j] = true;
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

        grid[row, col] = ++cellNumber;
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
        if (d.Count == 0) {
            endRow = row;
            endCol = col;
            return;
        }

        int[] nxt = d[Random.Range(0, d.Count)];
        GridDFS(row+nxt[0], col+nxt[1]);
    }

    private void CoverGrid() {
        const int visibleRatio = 5;
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                if (grid[i, j] == 0) continue;
                if (Random.Range(0, 10) < visibleRatio) {
                    coveredGrid[i, j] = grid[i, j];
                    isCovered[grid[i, j]] = false;
                }
            }
        }
        coveredGrid[initRow, initCol] = grid[initRow, initCol];
        coveredGrid[endRow, endCol] = grid[endRow, endCol];
        isCovered[grid[initRow, initCol]] = false;
        isCovered[grid[endRow, endCol]] = false;

        DrawBoard();
    }

    private void DrawBoard() {
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                if (coveredGrid[i, j] == 0) {
                    board[i, j].text = "";
                    board[i, j].color = new Color(1, 0.4f, 0);
                } else board[i, j].text = coveredGrid[i, j].ToString();
            }
        }
    }


    private IEnumerator GlowEffect(float duration = 0.75f) {
        const float initAlpha = 0.25f;
        SelectGlow.SetActive(true);
        while (true) {
            for (float t = 0; t <= duration; t += Time.deltaTime) {
                SelectGlowRenderer.color = new Color(1, 1, 0.5f, Mathf.Sin(0.5f*Mathf.PI * (initAlpha + t/duration * 0.25f)));
                yield return null;
            }
            for (float t = duration; t >= 0; t -= Time.deltaTime) {
                SelectGlowRenderer.color = new Color(1, 1, 0.5f, Mathf.Sin(0.5f*Mathf.PI * (initAlpha + t/duration * 0.25f)));
                yield return null;
            }
        }
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

    private void ConnectLine() {
        Line.positionCount++;
        Line.SetPosition(Line.positionCount-1, board[nxtRow, nxtColumn].transform.parent.transform.localPosition);
    }

    private IEnumerator Finish() {
        StartCoroutine(VeryAwesomeEpicRainbowEffect());
        Audio.PlayOneShot(PuzzleClearAudio, 0.75f);

        yield return new WaitForSeconds(3);
        yield return StartCoroutine(SceneCtr.FadeOut(2));

        PeekingRoo.SetActive(false);
        Roo.SetActive(true);
        Gate.SetActive(true);

        yield return new WaitForSeconds(1);
        yield return StartCoroutine(SceneCtr.FadeIn(1));
    }

    private IEnumerator VeryAwesomeEpicRainbowEffect() {
        Color[] colors = {
            new Color(0.9098039215686274f, 0.0784313725490196f, 0.08627450980392157f),
            new Color(1, 0.6470588235294118f, 0),
            new Color(0.9803921568627451f, 0.9215686274509803f, 0.21176470588235294f),
            new Color(0.4745098039215686f, 0.7647058823529411f, 0.0784313725490196f),
            new Color(0.2823529411764706f, 0.49019607843137253f, 0.9058823529411765f),
            new Color(0.29411764705882354f, 0.21176470588235294f, 0.615686274509804f),
            new Color(0.4392156862745098f, 0.21176470588235294f, 0.615686274509804f)
        };
        SpriteRenderer LastCell = board[curRow, curColumn].transform.parent.GetComponent<SpriteRenderer>();

        SelectGlow.SetActive(false);

        const float gap = 0.5f;
        LastCell.color = colors[0];
        yield return new WaitForSeconds(0.5f);
        while (true) {
            for (float t = 0; t <= gap; t += Time.deltaTime) {
                LastCell.color = Color.Lerp(colors[0], colors[1], t/gap);
                yield return null;
            }
            for (float t = 0; t <= gap; t += Time.deltaTime) {
                LastCell.color = Color.Lerp(colors[1], colors[2], t/gap);
                yield return null;
            }
            for (float t = 0; t <= gap; t += Time.deltaTime) {
                LastCell.color = Color.Lerp(colors[2], colors[3], t/gap);
                yield return null;
            }
            for (float t = 0; t <= gap; t += Time.deltaTime) {
                LastCell.color = Color.Lerp(colors[3], colors[4], t/gap);
                yield return null;
            }
            for (float t = 0; t <= gap; t += Time.deltaTime) {
                LastCell.color = Color.Lerp(colors[4], colors[5], t/gap);
                yield return null;
            }
            for (float t = 0; t <= gap; t += Time.deltaTime) {
                LastCell.color = Color.Lerp(colors[5], colors[6], t / gap);
                yield return null;
            }
            for (float t = 0; t <= gap; t += Time.deltaTime) {
                LastCell.color = Color.Lerp(colors[6], colors[0], t / gap);
                yield return null;
            }
        }
    }
}