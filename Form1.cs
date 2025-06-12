using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ToDoList
{
    public partial class Form1 : Form
    {
        private DataAccess dataAccess;
        private List<TodoItem> currentTodos;

        /// <summary>
        /// データベースの初期化、ロード
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            dataAccess = new DataAccess();
            LoadTodos();
        }

        private void LoadTodos()
        {
            currentTodos = dataAccess.GetTodos();
            lstTodos.DataSource = null;
            lstTodos.DisplayMember = "TaskDisplay";
            lstTodos.ValueMember = "Id";
            lstTodos.DataSource = currentTodos;

            // リストボックスの表示を更新
            lstTodos.Refresh();
        }

        /// <summary>
        /// タスクの更新
        /// </summary>
        /// <param name="sender">更新ボタンクリック</param>
        /// <param name="e"></param>
        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (lstTodos.SelectedItem is TodoItem selectedTodo)
            {
                selectedTodo.Task = txtTask.Text.Trim();
                dataAccess.UpdateTodo(selectedTodo);
                txtTask.Clear();
                LoadTodos();
            }
            else
            {
                MessageBox.Show("更新するタスクを選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// タスクの追加
        /// </summary>
        /// <param name="sender">追加ボタンクリック</param>
        /// <param name="e"></param>
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTask.Text))
            {
                TodoItem newTodo = new TodoItem
                {
                    Task = txtTask.Text.Trim(),
                    IsCompleted = false
                };
                dataAccess.AddTodo(newTodo);
                txtTask.Clear();
                LoadTodos();
            }
            else
            {
                MessageBox.Show("タスクを入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// タスクの削除
        /// </summary>
        /// <param name="sender">削除ボタンクリック</param>
        /// <param name="e"></param>
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (lstTodos.SelectedItem is TodoItem selectedTodo)
            {
                if (MessageBox.Show($"「{selectedTodo.Task}」を削除しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dataAccess.DeleteTodo(selectedTodo.Id);
                    txtTask.Clear();
                    LoadTodos();
                }
            }
            else
            {
                MessageBox.Show("削除するタスクを選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /*
        private void chkCompleted_CheckedChanged(object sender, EventArgs e)
        {
            if (lstTodos.SelectedItem is TodoItem selectedTodo)
            {
                txtTask.Text = selectedTodo.Task;
            }
            else
            {
                txtTask.Clear();
            }
        }
        */

        private void Form1_Load(object sender, EventArgs e)
        {
            currentTodos = dataAccess.GetTodos();
            lstTodos.DataSource = null;
            lstTodos.DisplayMember = "TaskDisplay";
            lstTodos.ValueMember = "Id";
            lstTodos.DataSource = currentTodos;

            lstTodos.Refresh();
        }
    }
}
