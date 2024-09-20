using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zuby.ADGV;

namespace BackupManagementTool
{
    public partial class FormMainApp : Form
    {
        // Referinta la forma de logare, care poate fi utilizata pentru a afisa forma de logare cand forma principala este inchisa
        public FormLoginMenu? RefToLoginForm { get; set; }

        // Tabela de date care conține toate înregistrările din baza de date
        private DataTable fullDataTable = new();

        // Cache pentru starea de backup pentru fiecare numar de inventar
        private readonly ConcurrentDictionary<string, bool> _backupStatusCache = new();

        public FormMainApp()
        {
            InitializeComponent();
            LoadTableAsync();
            Methods.ConnectToNetworkShare(Methods.networkShareServer, Methods.networkShareUsername, Methods.networkSharePassword); // Connect to the network share
            labelUserName.Text = Methods.username;
            if (Methods.user_type == 1) button_export.Enabled = false;
        }

        // Metodă asincronă care încarcă datele din baza de date în fullDataTable
        private async void LoadTableAsync()
        {
            try
            {
                fullDataTable = await Task.Run(() => Methods.LoadData());
                PopulateCheckedListBox(fullDataTable);
                advanceddataGridViewBackups.DataSource = fullDataTable;
                contorPC.Text = (advanceddataGridViewBackups.RowCount - 1).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        // Metodă care populează controlul checkedListBox_columns cu numele coloanelor din tabela de date
        readonly string[] preCheckedColumns = ["INV.IT", "IS_BACKUP", "FILE_PATH"];
        private void PopulateCheckedListBox(DataTable table)
        {
            checkedListBox_columns.Items.Clear();

            // Suspendă desenarea controlului pentru performanță
            checkedListBox_columns.BeginUpdate();
            foreach (DataColumn column in table.Columns)
            {
                bool isChecked = preCheckedColumns.Contains(column.ColumnName);
                checkedListBox_columns.Items.Add(column.ColumnName, isChecked);
            }
            checkedListBox_columns.Items.Add("Select all", false);
            // Reia desenarea controlului
            checkedListBox_columns.EndUpdate();
        }

        // Numararea randurilor
        private void CountRows()
        {
            contorR = 0;
            contorV = 0;

            foreach (DataGridViewRow row in advanceddataGridViewBackups.Rows)
            {
                if (row.DefaultCellStyle.BackColor == Color.LimeGreen)
                {
                    contorV++;
                }
                else if (row.DefaultCellStyle.BackColor == Color.PaleVioletRed)
                {
                    contorR++;
                }
            }

            contorGreen.Text = contorV.ToString();
            contorRed.Text = contorR.ToString();
        }


        // Metodă care actualizează coloanele din advancedDataGridViewBackups în funcție de starea controlului checkedListBox_columns
        private async Task UpdateDataGridViewColumnsAsync(DataTable table)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;

            // Resetează contorii înainte de actualizarea DataGridView
            contorR = 0;
            contorV = 0;
            contorRed.Text = contorR.ToString();
            contorGreen.Text = contorV.ToString();

            string[] selectedColumns = checkedListBox_columns.CheckedItems
                                    .OfType<string>()
                                    .Where(item => item != "Select all")
                                    .ToArray();

            if (selectedColumns.Length > 0)
            {
                // Suspend layout updates before updating DataGridView
                advanceddataGridViewBackups.SuspendLayout();
                await Task.Run(() =>
                {
                    DataView view = new(table);
                    DataTable filteredTable = view.ToTable(false, selectedColumns);

                    Invoke((MethodInvoker)delegate
                    {
                        advanceddataGridViewBackups.DataSource = filteredTable;
                    }
                    );
                });
                // Resume layout updates after changes
                advanceddataGridViewBackups.ResumeLayout();
                CountRows();


            }
            else
            {
                advanceddataGridViewBackups.DataSource = null;
            }
        }

        private void CheckedListBox_columns_ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            if (!this.IsDisposed)
            {
                checkedListBox_columns.BeginUpdate();

                // Dezactivează temporar evenimentele
                checkedListBox_columns.ItemCheck -= CheckedListBox_columns_ItemCheck;


                if (e.Index == checkedListBox_columns.Items.IndexOf("Select all"))
                {
                    bool isChecked = e.NewValue == CheckState.Checked;

                    for (int i = 0; i < checkedListBox_columns.Items.Count; i++)
                    {
                        if (checkedListBox_columns.Items[i].ToString() != "Select all")
                        {
                            checkedListBox_columns.SetItemChecked(i, isChecked);
                        }
                    }

                    if (!isChecked)
                    {
                        foreach (string column in preCheckedColumns)
                        {
                            int index = checkedListBox_columns.Items.IndexOf(column);
                            if (index >= 0)
                            {
                                checkedListBox_columns.SetItemChecked(index, true);
                            }
                        }
                    }
                }

                checkedListBox_columns.EndUpdate();

                // Reactivează evenimentele după modificări
                checkedListBox_columns.ItemCheck += CheckedListBox_columns_ItemCheck;

                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    this.BeginInvoke((MethodInvoker)async delegate
                    {
                        await UpdateDataGridViewColumnsAsync(fullDataTable);
                        CountRows();

                    });
                }
                else
                {
                    _ = UpdateDataGridViewColumnsAsync(fullDataTable);
                }
            }
        }
        // Evenimentul care se declanșează atunci când binding-ul datelor în DataGridView este complet
        private void AdvanceddataGridViewBackups_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in advanceddataGridViewBackups.Rows)
            {
                if (row?.Cells["INV.IT"]?.Value is string inventoryNumber && _backupStatusCache.TryGetValue(inventoryNumber, value: out _))
                {
                    bool backupFound = _backupStatusCache[inventoryNumber];
                    UpdateRowStyle(row, backupFound);
                }
            }
        }

        bool notStarted = true;
        // Metodă asincronă pentru a verifica backup-urile și a căuta fișierele QNAP
        private async Task CheckBackupsAndSearchQNAPAsync()
        {
            if (advanceddataGridViewBackups == null) return;

            List<string> networkPaths =
            [
                $@"\\10.142.31.250\backup_manual\Blonie\",
                $@"\\10.142.31.250\backup_manual\AOMEI\INV. 1 - 499\",
                $@"\\10.142.31.250\backup_manual\AOMEI\INV. 500 - 999\",
                $@"\\10.142.31.250\backup_manual\AOMEI\INV. 500 - 999\",
                $@"\\10.142.31.250\backup_manual\AOMEI\INV. 1000 - 1499\",
                $@"\\10.142.31.250\backup_manual\AOMEI\INV. 1500 - 1999\",
                $@"\\10.142.31.250\backup_manual\ACRONIS\INV. 1 - 499\",
                $@"\\10.142.31.250\backup_manual\ACRONIS\INV. 500 - 999\",
                $@"\\10.142.31.250\backup_manual\ACRONIS\INV. 500 - 999\",
                $@"\\10.142.31.250\backup_manual\ACRONIS\INV. 1000 - 1499\",
                $@"\\10.142.31.250\backup_manual\ACRONIS\INV. 1500 - 1999\",
                $@"\\10.142.31.250\backup_automat\Aomei\INV. 1 - 499\",
                $@"\\10.142.31.250\backup_automat\Aomei\INV. 500 - 999\",
                $@"\\10.142.31.250\backup_automat\Aomei\INV. 500 - 999\",
                $@"\\10.142.31.250\backup_automat\Aomei\INV. 1000 - 1499\",
                $@"\\10.142.31.250\backup_automat\Aomei\INV. 1500 - 1999\",
                $@"\\10.142.31.250\backup_automat\Acronis\INV. 1 - 499\",
                $@"\\10.142.31.250\backup_automat\Acronis\INV. 500 - 999\",
            ];

            _backupStatusCache.Clear();

            foreach (DataGridViewRow row in advanceddataGridViewBackups.Rows)
            {
                if (notStarted) break;
                if (row?.Cells["INV.IT"]?.Value == null) continue;

                string inventoryNumber = row?.Cells["INV.IT"]?.Value?.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(inventoryNumber)) continue;

                await Task.Run(() => row != null ? ProcessRow(row, inventoryNumber, networkPaths) : Task.CompletedTask);
            }
            notStarted = true;
        }

        // Metodă asincronă pentru a procesa un singur rând
        private async Task ProcessRow(DataGridViewRow row, string inventoryNumber, List<string> networkPaths)
        {
            if (_backupStatusCache.TryGetValue(inventoryNumber, out bool cachedBackupStatus))
            {
                Console.WriteLine($"Cached status for {inventoryNumber}: {cachedBackupStatus}");
                UpdateRowStyle(row, cachedBackupStatus);
                return;
            }

            bool backupFound;
            string dateString = row?.Cells["IS_BACKUP"]?.Value?.ToString() ?? string.Empty;
            string format = "dd.MM.yyyy";

            if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                if (DateTime.Now.Subtract(date).Days > 1)
                {
                    // Apelează metoda asincronă pentru a verifica backup-ul
                    backupFound = await SearchDirectoriesForBackupAsync(networkPaths, inventoryNumber);
                    Console.WriteLine($"Backup found for {inventoryNumber}: {backupFound}\r\n");
                    _backupStatusCache[inventoryNumber] = backupFound;
                }
                else
                {
                    // Data este validă, dar nu este necesar să se facă backup
                    backupFound = true;
                    Console.WriteLine($"Backup found for {inventoryNumber}: {backupFound}\r\n");
                }
            }
            else
            {
                // Console.WriteLine($"Invalid date for {inventoryNumber}");
                backupFound = await SearchDirectoriesForBackupAsync(networkPaths, inventoryNumber);
                Console.WriteLine($"Backup found for {inventoryNumber}: {backupFound}\r\n");
                _backupStatusCache[inventoryNumber] = backupFound;

            }

            if (row != null)
            {
                UpdateRowStyle(row, backupFound);
            }
        }

        int contorV = 0, contorR = 0;
        // Metodă asincronă pentru a căuta fișierele de backup în căile de rețea
        private static async Task<bool> SearchDirectoriesForBackupAsync(List<string> networkPaths, string inventoryNumber)
        {
            return await Task.Run(() =>
            {
                bool backupFound = false;

                foreach (string path in networkPaths)
                {

                    if (!Directory.Exists(path)) continue;

                    var inventoryFolders = Directory.GetDirectories(path, $"{inventoryNumber}_*");

                    foreach (var inventoryFolder in inventoryFolders)
                    {
                        string latestFile = string.Empty;
                        DateTime lastModified = DateTime.MinValue;

                        foreach (string file in Directory.GetFiles(inventoryFolder))
                        {
                            DateTime fileDate = File.GetLastWriteTime(file);
                            if (fileDate > lastModified)
                            {
                                lastModified = fileDate;
                                latestFile = file;
                            }
                        }

                        if (!string.IsNullOrEmpty(latestFile))
                        {
                            Console.WriteLine($"Latest file in {latestFile}\r\n{lastModified}\r\n");
                            backupFound = true;

                            // Update the database with the latest backup date
                            Methods.UpdateLastBackupDate(inventoryNumber, lastModified.Date);
                            Methods.UpdateLastPath(inventoryNumber, latestFile);
                        }
                    }

                }
                if (!backupFound)
                {
                    // Mark the backup as missing if no files were found
                    Methods.MarkBackupAsMissing(inventoryNumber);
                    Methods.UpdateLastPath(inventoryNumber, string.Empty);
                }
                return backupFound;
            });

        }

        // Actualizeaza culoarea randului in functie daca exista sau nu backup pentru acel ID
        private void UpdateRowStyle(DataGridViewRow row, bool backupFound)
        {
            if (row.Index >= 0 && row.Index < advanceddataGridViewBackups.RowCount)
            {
                row.DefaultCellStyle.BackColor = backupFound ? Color.LimeGreen : Color.PaleVioletRed;
                row.DefaultCellStyle.ForeColor = Color.White;
                if (backupFound == true)
                {
                    contorV++; contorGreen.Text = contorV.ToString();
                }
                else
                {
                    contorR++; contorRed.Text = contorR.ToString();
                }
                advanceddataGridViewBackups.InvalidateRow(row.Index);
            }
        }

        // Buton pornire proces de verificare
        private async void Button_Verify_Click(object sender, EventArgs e)
        {
            contorR = 0; contorRed.Text = contorR.ToString();

            contorV = 0; contorGreen.Text = contorV.ToString();

            notStarted = !notStarted;
            try
            {
                if (notStarted == false)
                {
                    await CheckBackupsAndSearchQNAPAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        // Eveniment in momentul inchiderii aplicatiei, ne deconectam de la retea
        private void FormMainApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            //In Form2, where you need to show Form1:
            this.RefToLoginForm?.Cleanup();
            this.RefToLoginForm?.Show();
            Methods.DisconnectFromNetworkShare(Methods.networkShareServer); // Disconnect from the network share
        }

        private void AdvanceddataGridViewBackups_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            Methods.isAddingNewRow = true; // Set the flag to true to indicate a new row is being added
        }

        private void AdvanceddataGridViewBackups_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (Methods.user_type == 3) { Methods.DeleteRowFromDatabase(invITValue); MessageBox.Show("Apel delete!"); }
        }

        private void SignOutButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string? invITValue;        // Variabilă pentru stocarea valorii din coloana "INV.IT"
        private string? newValue;
        private string? columnName;

        private void AdvanceddataGridViewBackups_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = advanceddataGridViewBackups.Rows[e.RowIndex];
                invITValue = selectedRow?.Cells["INV.IT"]?.Value?.ToString() ?? string.Empty;
                columnName = advanceddataGridViewBackups.Columns[e.ColumnIndex].Name;

                MessageBox.Show($"Valoarea selectată din INV.IT: {invITValue}");
                MessageBox.Show($"Coloana selectata: {columnName}");
            }
            else
            {
                MessageBox.Show("Rândul sau coloana selectată nu sunt valide.");
            }
        }

        private void AdvanceddataGridViewBackups_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (Methods.user_type == 3)
            {
                if (Methods.isAddingNewRow && e.ColumnIndex == 0 && e.RowIndex >= 0) // Check if the edited cell is the first column (INV.IT) and the row is valid
                {
                    DataGridViewRow newRow = advanceddataGridViewBackups.Rows[e.RowIndex];
                    if (newRow.Cells["INV.IT"].Value != null)
                    {
                        Methods.AddRowToDatabase(newRow);
                        MessageBox.Show("Added " + newRow.Cells["INV.IT"].Value);
                        Methods.isAddingNewRow = false; // Reset the flag
                        return;
                    }
                }

                // Restul codului pentru actualizarea bazei de date
                newValue = advanceddataGridViewBackups?.Rows[e.RowIndex]?.Cells[e.ColumnIndex]?.Value?.ToString() ?? string.Empty;

                // Verifică dacă variabilele sunt valide înainte de actualizare
                if (string.IsNullOrEmpty(invITValue) || string.IsNullOrEmpty(newValue) || string.IsNullOrEmpty(columnName))
                {
                    MessageBox.Show("Nu toate datele necesare pentru actualizare sunt disponibile.");
                    return;
                }

                // Apelează funcția de actualizare
                Methods.UpdateRowInDatabase(columnName, newValue, invITValue);
            }
        }
        private void AdvanceddataGridViewBackups_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateRowNumbers();
            contorPC.Text = (advanceddataGridViewBackups.RowCount - 1).ToString();
        }

        private void AdvanceddataGridViewBackups_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateRowNumbers();
            contorPC.Text = (advanceddataGridViewBackups.RowCount - 1).ToString();
        }

        private void UpdateRowNumbers()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < advanceddataGridViewBackups.Rows.Count; i++)
                {
                    int rowIndex = i;
                    advanceddataGridViewBackups.Invoke((MethodInvoker)delegate
                    {
                        advanceddataGridViewBackups.Rows[rowIndex].HeaderCell.Value = (rowIndex + 1).ToString();
                    });
                }
            });
            advanceddataGridViewBackups.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadTableAsync();
            contorR = 0;
            contorV = 0;
        }
    }
}