using System;
using System.Drawing;
using System.Windows.Forms;

namespace _1132_2048game
{
    public partial class Form1 : Form
    {
        //�ŧi
        private int[,] board = new int[4, 4];
        private Label[,] labels = new Label[4, 4];
        private Random rand = new Random();
        private TableLayoutPanel tableLayoutPanel1;

        public Form1()
        {
            //��l�ƹC��
            InitializeComponent();
            InitTableLayoutPanel();
            InitGrid();
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
            AddRandomTile();
            AddRandomTile();
            UpdateUI();
        }
        //�ʺA�ͦ�TableLayoutPanel
        private void InitTableLayoutPanel()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.Dock = DockStyle.Fill;
            this.Controls.Add(tableLayoutPanel1);
        }
        //��l�Ʈ�l
        private void InitGrid()
        {
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.Dock = DockStyle.Fill;

            for (int i = 0; i < 4; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            }

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    Label lbl = new Label();
                    lbl.Dock = DockStyle.Fill;
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.Font = new Font("Microsoft JhengHei", 24, FontStyle.Bold);
                    lbl.BackColor = Color.LightGray;
                    lbl.Margin = new Padding(5);
                    tableLayoutPanel1.Controls.Add(lbl, col, row);
                    labels[row, col] = lbl;
                }
            }
        }
        //�H���K�[�Ʀr(2�B4)��Ů椤
        private void AddRandomTile()
        {
            var empty = new System.Collections.Generic.List<(int, int)>();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (board[i, j] == 0)
                        empty.Add((i, j));

            if (empty.Count == 0) return;

            var (x, y) = empty[rand.Next(empty.Count)];
            board[x, y] = rand.Next(10) == 0 ? 4 : 2;
        }
        //��sUI
        private void UpdateUI()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int val = board[i, j];
                    labels[i, j].Text = val == 0 ? "" : val.ToString();
                    labels[i, j].BackColor = GetTileColor(val);
                    labels[i, j].ForeColor = val <= 4 ? Color.Black : Color.White;
                }
            }
        }
        //�W��
        private Color GetTileColor(int value)
        {
            return value switch
            {
                0 => Color.LightGray,
                2 => Color.Beige,
                4 => Color.BurlyWood,
                8 => Color.Orange,
                16 => Color.DarkOrange,
                32 => Color.OrangeRed,
                64 => Color.Red,
                128 => Color.YellowGreen,
                256 => Color.Green,
                512 => Color.Teal,
                1024 => Color.MediumBlue,
                2048 => Color.Gold,
                _ => Color.Black
            };
        }
        // �B�z��L�����J�ƥ�]�W�U���k�^
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            bool moved = false;

            switch (e.KeyCode)
            {
                case Keys.Left:
                    moved = MoveLeft();
                    break;
                case Keys.Right:
                    moved = MoveRight();
                    break;
                case Keys.Up:
                    moved = MoveUp();
                    break;
                case Keys.Down:
                    moved = MoveDown();
                    break;
            }

            if (moved)
            {
                AddRandomTile();
                UpdateUI();
                if (CheckGameOver())
                    MessageBox.Show("�C�������I");
            }
        }
        // �������ʻP�X���޿�
        private bool MoveLeft()
        {
            bool moved = false;
            for (int i = 0; i < 4; i++)
            {
                int[] row = new int[4];
                int index = 0;
                bool merged = false;

                for (int j = 0; j < 4; j++)
                {
                    if (board[i, j] != 0)
                    {
                        if (index > 0 && row[index - 1] == board[i, j] && !merged)
                        {
                            row[index - 1] *= 2;
                            merged = true;
                            moved = true;
                        }
                        else
                        {
                            row[index++] = board[i, j];
                            if (j != index - 1) moved = true;
                            merged = false;
                        }
                    }
                }

                for (int j = 0; j < 4; j++)
                    board[i, j] = row[j];
            }
            return moved;
        }
        // ���k�G���� �� ���� �� ����
        private bool MoveRight()
        {
            ReverseRows();
            bool moved = MoveLeft();
            ReverseRows();
            return moved;
        }
        // ���W�G��m �� ���� �� ��m�^��
        private bool MoveUp()
        {
            Transpose();
            bool moved = MoveLeft();
            Transpose();
            return moved;
        }
        // ���U�G��m �� ���� �� ���� �� ���� �� ��m�^��
        private bool MoveDown()
        {
            Transpose();
            ReverseRows();
            bool moved = MoveLeft();
            ReverseRows();
            Transpose();
            return moved;
        }
        // �C�C����
        private void ReverseRows()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    (board[i, j], board[i, 3 - j]) = (board[i, 3 - j], board[i, j]);
                }
            }
        }
        // �x�}��m�G�C�ܦ�B���ܦC
        private void Transpose()
        {
            for (int i = 0; i < 4; i++)
                for (int j = i + 1; j < 4; j++)
                    (board[i, j], board[j, i]) = (board[j, i], board[i, j]);
        }
        // �ˬd�O�_�w�g�S���X�k������
        private bool CheckGameOver()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (board[i, j] == 0 ||
                        (j < 3 && board[i, j] == board[i, j + 1]) ||
                        (i < 3 && board[i, j] == board[i + 1, j]))
                        return false;
            return true;
        }
    }
}
