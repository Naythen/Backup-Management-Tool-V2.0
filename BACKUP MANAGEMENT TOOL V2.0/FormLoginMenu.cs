using System.Runtime.InteropServices;

namespace BackupManagementTool
{
    public partial class FormLoginMenu : Form
    {
        [LibraryImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool AllocConsole();

        public FormLoginMenu()
        {
            InitializeComponent();
            AllocConsole();
        }

        // Eveniment pentru apasarea butonului de login, se compara datele introduse cu cele prezente din baza de date
        // In functie de nivelul de acces, utilizatorul are anumite accesibilitati
        private void ButtonLogIn_Click(object sender, EventArgs e)
        {
            Methods.username = textBoxUserName.Text.Trim();
            string password = textBoxPassword.Text.Trim();
            // Verificare daca campurile sunt nule sau goale
            if (string.IsNullOrEmpty(Methods.username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int userRights = Methods.VerifyLogin(Methods.username, password); // user si pass cand termin
            // Login reusit
            if (userRights > 0)
            {
                MessageBox.Show("Login successful!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                FormMainApp F1 = new()
                {
                    RefToLoginForm = this
                };
                F1.Show();
            }
            // Logare esuata
            else
            {
                MessageBox.Show("Invalid Username or Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Metoda de curatare a campurilor in momentul efectuarii unui posibil log out
        public void Cleanup()
        {
            textBoxPassword.Text = string.Empty;
            textBoxUserName.Text = string.Empty;
        }
    }
}
