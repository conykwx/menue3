using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public class TextEditorForm : Form
{
    private TextBox txtEditor;
    private OpenFileDialog openFileDialog;
    private SaveFileDialog saveFileDialog;
    private ToolStrip toolStrip;
    private ToolStripButton btnOpen, btnSave, btnNew, btnCut, btnCopy, btnPaste, btnUndo, btnFontColor, btnBgColor;
    private MenuStrip menuStrip;
    private ToolStripMenuItem fileMenu, editMenu, settingsMenu, openMenuItem, saveMenuItem, saveAsMenuItem, exitMenuItem, undoMenuItem, cutMenuItem, copyMenuItem, pasteMenuItem, selectAllMenuItem;

    public TextEditorForm()
    {
        // Ініціалізація компонентів
        this.Text = "Текстовий редактор";
        this.Size = new Size(800, 600);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        // Текстове поле для редактора
        txtEditor = new TextBox
        {
            Multiline = true,
            Dock = DockStyle.Fill,
            Font = new Font("Arial", 12),
            AcceptsTab = true
        };

        // Створення діалогів для відкриття та збереження файлів
        openFileDialog = new OpenFileDialog { Filter = "Text files|*.txt|All files|*.*" };
        saveFileDialog = new SaveFileDialog { Filter = "Text files|*.txt|All files|*.*" };

        // Панель інструментів
        toolStrip = new ToolStrip();
        btnOpen = new ToolStripButton("Відкрити", null, BtnOpen_Click);
        btnSave = new ToolStripButton("Зберегти", null, BtnSave_Click);
        btnNew = new ToolStripButton("Новий", null, BtnNew_Click);
        btnCut = new ToolStripButton("Вирізати", null, BtnCut_Click);
        btnCopy = new ToolStripButton("Копіювати", null, BtnCopy_Click);
        btnPaste = new ToolStripButton("Вставити", null, BtnPaste_Click);
        btnUndo = new ToolStripButton("Скасувати", null, BtnUndo_Click);
        btnFontColor = new ToolStripButton("Колір шрифта", null, BtnFontColor_Click);
        btnBgColor = new ToolStripButton("Колір фону", null, BtnBgColor_Click);

        toolStrip.Items.AddRange(new ToolStripItem[]
        {
            btnOpen, btnSave, btnNew, btnCut, btnCopy, btnPaste, btnUndo, btnFontColor, btnBgColor
        });

        // Меню
        menuStrip = new MenuStrip();
        fileMenu = new ToolStripMenuItem("Файл");
        editMenu = new ToolStripMenuItem("Правка");
        settingsMenu = new ToolStripMenuItem("Налаштування");

        openMenuItem = new ToolStripMenuItem("Відкрити", null, BtnOpen_Click);
        saveMenuItem = new ToolStripMenuItem("Зберегти", null, BtnSave_Click);
        saveAsMenuItem = new ToolStripMenuItem("Зберегти як...", null, BtnSaveAs_Click);
        exitMenuItem = new ToolStripMenuItem("Вийти", null, BtnExit_Click);

        undoMenuItem = new ToolStripMenuItem("Скасувати", null, BtnUndo_Click);
        cutMenuItem = new ToolStripMenuItem("Вирізати", null, BtnCut_Click);
        copyMenuItem = new ToolStripMenuItem("Копіювати", null, BtnCopy_Click);
        pasteMenuItem = new ToolStripMenuItem("Вставити", null, BtnPaste_Click);
        selectAllMenuItem = new ToolStripMenuItem("Виділити все", null, BtnSelectAll_Click);

        fileMenu.DropDownItems.AddRange(new ToolStripItem[] { openMenuItem, saveMenuItem, saveAsMenuItem, exitMenuItem });
        editMenu.DropDownItems.AddRange(new ToolStripItem[] { undoMenuItem, cutMenuItem, copyMenuItem, pasteMenuItem, selectAllMenuItem });
        settingsMenu.DropDownItems.AddRange(new ToolStripItem[] { });

        menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, editMenu, settingsMenu });

        // Контекстне меню
        ContextMenuStrip contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Вирізати", null, BtnCut_Click);
        contextMenu.Items.Add("Копіювати", null, BtnCopy_Click);
        contextMenu.Items.Add("Вставити", null, BtnPaste_Click);
        contextMenu.Items.Add("Скасувати", null, BtnUndo_Click);
        txtEditor.ContextMenuStrip = contextMenu;

        // Додавання компонентів на форму
        this.Controls.Add(txtEditor);
        this.Controls.Add(toolStrip);
        this.MainMenuStrip = menuStrip;
        this.Controls.Add(menuStrip);
    }

    // Обробники подій
    private void BtnOpen_Click(object sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
            this.Text = "Текстовий редактор - " + openFileDialog.FileName;
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(openFileDialog.FileName))
        {
            BtnSaveAs_Click(sender, e);
        }
        else
        {
            File.WriteAllText(openFileDialog.FileName, txtEditor.Text);
        }
    }

    private void BtnSaveAs_Click(object sender, EventArgs e)
    {
        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(saveFileDialog.FileName, txtEditor.Text);
            openFileDialog.FileName = saveFileDialog.FileName;
            this.Text = "Текстовий редактор - " + saveFileDialog.FileName;
        }
    }

    private void BtnNew_Click(object sender, EventArgs e)
    {
        txtEditor.Clear();
        openFileDialog.FileName = "";
        this.Text = "Текстовий редактор - Новий документ";
    }

    private void BtnCut_Click(object sender, EventArgs e)
    {
        txtEditor.Cut();
    }

    private void BtnCopy_Click(object sender, EventArgs e)
    {
        txtEditor.Copy();
    }

    private void BtnPaste_Click(object sender, EventArgs e)
    {
        txtEditor.Paste();
    }

    private void BtnUndo_Click(object sender, EventArgs e)
    {
        if (txtEditor.CanUndo)
        {
            txtEditor.Undo();
        }
    }

    private void BtnFontColor_Click(object sender, EventArgs e)
    {
        using (ColorDialog colorDialog = new ColorDialog())
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                txtEditor.ForeColor = colorDialog.Color;
            }
        }
    }

    private void BtnBgColor_Click(object sender, EventArgs e)
    {
        using (ColorDialog colorDialog = new ColorDialog())
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                txtEditor.BackColor = colorDialog.Color;
            }
        }
    }

    private void BtnSelectAll_Click(object sender, EventArgs e)
    {
        txtEditor.SelectAll();
    }

    private void BtnExit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new TextEditorForm());
    }
}
