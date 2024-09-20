using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Zuby.ADGV;
using System.Configuration;
using System.Windows.Forms;

namespace BackupManagementTool
{
    // Structura pentru resurse de rețea
    [StructLayout(LayoutKind.Sequential)]
    public struct NETRESOURCE
    {
        public int dwScope;
        public int dwType;
        public int dwDisplayType;
        public int dwUsage;
        public string lpLocalName;
        public string lpRemoteName;
        public string lpComment;
        public string lpProvider;
    }

    internal static class Methods
    {
        // Conține conținutul fișierului config.json încărcat ca un șir de caractere
        public static string jsonConfig = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json")) ?? string.Empty;
        private static readonly dynamic config = JsonConvert.DeserializeObject(jsonConfig) ?? new { };
        // Variabilele care conțin datele de conectare la baza de date
        public static string databaseUsername = config.Database.Username;
        public static string databasePassword = config.Database.Password;
        public static string databaseServer = config.Database.Server;

        // Variabilele care conțin datele de conectare la unitatea de rețea
        public static string networkShareUsername = config.NetworkShare.Username;
        public static string networkSharePassword = config.NetworkShare.Password;
        public static string networkShareServer = config.NetworkShare.Server;

        // Variabile pentru informatii partajate intre form-uri
        public static int user_type = 0;
        public static string username = string.Empty;
        public static bool isAddingNewRow = false;

        // Șir de conexiune la baza de date
        static readonly string connectionStringUsers = $"Server={databaseServer};Database=InventarIT;Uid={databaseUsername};Pwd={databasePassword};TrustServerCertificate=True";

        // Funcții WinAPI pentru conectarea/deconectarea unității de rețea
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(ref NETRESOURCE lpNetResource, string lpPassword, string lpUsername, int dwFlags);
        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string lpName, int dwFlags, bool fForce);


        // Metodă care verifică dacă datele de login sunt valide
        /// <summary>
        /// Verifică dacă datele de login sunt valide.
        /// </summary>
        /// <param name="user">Numele de utilizator.</param>
        /// <param name="pass">Parola utilizatorului.</param>
        /// <returns>Tipul de utilizator (0 - nevalid, altfel - valid).</returns>
        public static int VerifyLogin(string user, string pass)
        {
            using SqlConnection conexiune = new(connectionStringUsers);
            conexiune.Open();

            bool status = pass.Equals(conexiune.QueryFirstOrDefault<string>(
                "Select [PassKey] FROM dbo.UsersMngDB WHERE UserName = @username", new { username = user }));

            if (status)
            {
                user_type = conexiune.QueryFirstOrDefault<int>(
                    "Select [RightsLevel] FROM dbo.UsersMngDB WHERE UserName = @username", new { username = user });
            }
            conexiune.Close();
            return user_type;
        }

        // Metodele pentru conectarea/deconectarea unității de rețea
        /// <summary>
        /// Conectează la o unitate de rețea.
        /// </summary>
        /// <param name="networkPath">Calea către unitatea de rețea.</param>
        /// <param name="username">Numele de utilizator pentru conectare.</param>
        /// <param name="password">Parola pentru conectare.</param>
        public static void ConnectToNetworkShare(string networkPath, string username, string password)
        {
            NETRESOURCE nr = new()
            {
                dwType = 1, // Resursă de disc
                lpRemoteName = networkPath
            };

            int result = WNetAddConnection2(ref nr, password, username, 0);
            if (result != 0)
            {
                Console.WriteLine($"Error connecting to network share: {result}");
            }
        }

        /// <summary>
        /// Deconectează de la o unitate de rețea.
        /// </summary>
        /// <param name="networkPath">Calea către unitatea de rețea.</param>
        public static void DisconnectFromNetworkShare(string networkPath)
        {
            int result = WNetCancelConnection2(networkPath, 0, true);
            if (result != 0)
            {
                MessageBox.Show($"Error disconnecting from network share: {result}");
            }
        }

        // Metodele pentru încărcarea datelor din baza de date, actualizarea ultimei date de backup și marcarea backup-ului ca dispărut
        /// <summary>
        /// Încarcă datele din baza de date.
        /// </summary>
        /// <returns>Un DataTable cu datele incarcate </returns>
        public static DataTable LoadData()
        {
            try
            {
                using SqlConnection conn = new(connectionStringUsers);
                conn.Open();
                string query = "SELECT * FROM BackupMngDB";
                SqlDataAdapter dataAdapter = new(query, conn);
                DataTable dataTable = new();
                dataAdapter.Fill(dataTable);
                conn.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Conexiunea a eșuat: " + ex.Message);
                return new DataTable();
            }
        }

        /// <summary>
        /// Actualizează ultima dată de backup pentru un anumit inventar.
        /// </summary>
        /// <param name="inventoryNumber">Numărul de inventar.</param>
        /// <param name="lastBackupDate">Data ultimului backup.</param>
        public static void UpdateLastBackupDate(string inventoryNumber, DateTime lastBackupDate)
        {
            try
            {
                using SqlConnection conn = new(connectionStringUsers);
                conn.Open();

                string query = "UPDATE dbo.BackupMngDB SET [IS_BACKUP] = @LastBackupDate WHERE [INV.IT] = @InventoryNumber";
                conn.Execute(query, new { LastBackupDate = lastBackupDate, InventoryNumber = inventoryNumber });

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Conexiunea a eșuat: " + ex.Message);
            }
        }

        /// <summary>
        /// Actualizează calea fișierului pentru un anumit inventar.
        /// </summary>
        /// <param name="inventoryNumber">Numărul de inventar.</param>
        /// <param name="path">Calea fișierului.</param>
        public static void UpdateLastPath(string inventoryNumber, string path)
        {
            try
            {
                using SqlConnection conn = new(connectionStringUsers);
                conn.Open();

                string query = "UPDATE dbo.BackupMngDB SET [FILE_PATH] = @FILE_PATH WHERE [INV.IT] = @InventoryNumber";
                conn.Execute(query, new { FILE_PATH = path, InventoryNumber = inventoryNumber });

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Conexiunea a eșuat: " + ex.Message);
            }
        }

        /// <summary>
        /// Marchează un backup ca dispărut pentru un anumit inventar.
        /// </summary>
        /// <param name="inventoryNumber">Numărul de inventar.</param>
        public static void MarkBackupAsMissing(string inventoryNumber)
        {
            try
            {
                using SqlConnection conn = new(connectionStringUsers);
                conn.Open();

                DateTime missingDate = new(1753, 1, 1); // De exemplu, poți folosi o dată minimă standard, cum ar fi 1900-01-01

                string query = "UPDATE dbo.BackupMngDB SET [IS_BACKUP] = @IsBackup WHERE [INV.IT] = @InventoryNumber";
                conn.Execute(query, new { IsBackup = missingDate, InventoryNumber = inventoryNumber });

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Conexiunea a eșuat: " + ex.Message);
            }
        }

        /// <summary>
        /// Adaugă un rând nou în baza de date.
        /// </summary>
        /// <param name="row">Rândul de adăugat.</param>
        public static void AddRowToDatabase(DataGridViewRow row)
        {
            // Check if the row is valid
            if (row == null || row.Cells.Count == 0)
            {
                MessageBox.Show("Rândul nu este valid.");
                return;
            }

            // Create the SQL query
            string query = "INSERT INTO dbo.BackupMngDB ([INV.IT], [RESP.], [INV./CWIP], [FAB], [ZONA/AREA], [UAP/SEGMENT], [LINE], [Operatia / Workstations], [TYPE / Machines], [Producator PC], [SN], [Model], [CONFIGURATION], [OS], [SP], [32/64], [Status], [Criticitate], [Hostname], [HOSTNAME DUPLICAT], [IP type], [VLAN], [REZERVAT], [VNC], [Retea DDS], [MAC], [Switch (SWA)], [Port], [RACK], [IP], [LAN], [MAC2], [IP2], [LAN2], [MAC3], [HDD], [FORM], [SIZE], [RAID], [HDD6], [FORM7], [SIZE8], [RAID9], [VNC2], [DB], [User], [Administrator ], [Y], [LAST BACKUP], [Luna], [MP planificat 2024], [Planificat MP], [EFECTUAT MP], [SECTOR], [UAP], [TIP BACKUP], [DATE], [ACRONIS AUTO], [LINK_AA], [AOMEI AUTO], [LINK_AA2], [ACRONIS MAN.], [LINK_AM], [AOMEI MAN.], [LINK_AM2], [GHOST], [Observatii / Note], [DATA INTERVENTIE SECURITATE], [Tanium], [AOMEI AGENT], [CrowdStrike  ], [GLPI], [HDD SENTINEL], [VNC3], [WMI], [McAfee Agent], [McAfee DLP  ], [McAfee VSE], [McAfee Solidifier], [GLPI AGENT], [RESPONSABIL], [DATA], [NECESITA CONEXIUNE BW], [UNDE? RACK?], [PENTRU CE?], [Etapa], [IS_BACKUP], [FILE_PATH]) VALUES (@Value1, @Value2, @Value3, @Value4, @Value5, @Value6, @Value7, @Value8, @Value9, @Value10, @Value11, @Value12, @Value13, @Value14, @Value15, @Value16, @Value17, @Value18, @Value19, @Value20, @Value21, @Value22, @Value23, @Value24, @Value25, @Value26, @Value27, @Value28, @Value29, @Value30, @Value31, @Value32, @Value33, @Value34, @Value35, @Value36, @Value37, @Value38, @Value39, @Value40, @Value41, @Value42, @Value43, @Value44, @Value45, @Value46, @Value47, @Value48, @Value49, @Value50, @Value51, @Value52, @Value53, @Value54, @Value55, @Value56, @Value57, @Value58, @Value59, @Value60, @Value61, @Value62, @Value63, @Value64, @Value65, @Value66, @Value67, @Value68, @Value69, @Value70, @Value71, @Value72, @Value73, @Value74, @Value75, @Value76, @Value77, @Value78, @Value79, @Value80, @Value81, @Value82, @Value83, @Value84, @Value85, @Value86, @Value87, @Value88)";

            // Create a connection and command
            using SqlConnection connection = new(connectionStringUsers);
            using SqlCommand command = new(query, connection);

            // Add parameters to prevent SQL injection
            for (int i = 0; i < row.Cells.Count; i++)
            {
                object cellValue = row.Cells[i]?.Value ?? DBNull.Value;
                command.Parameters.AddWithValue($"@Value{i + 1}", cellValue ?? DBNull.Value);
            }

            try
            {
                // Open the connection
                connection.Open();

                // Execute the command
                int rowsAffected = command.ExecuteNonQuery();

                // Check if the insertion was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Rândul a fost adăugat cu succes în baza de date.");
                }
                else
                {
                    MessageBox.Show("Eroare la adăugarea rândului în baza de date.");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare la conectarea la baza de date: " + ex.Message);
            }
            finally
            {
                connection.Close();
                isAddingNewRow = false;
            }
        }
        /// <summary>
        /// Șterge un rând din baza de date.
        /// </summary>
        /// <param name="idInventar">Numărul de inventar.</param>
        public static void DeleteRowFromDatabase(string? idInventar)
        {
            using SqlConnection conn = new(connectionStringUsers);
            conn.Open();

            string query = "DELETE FROM dbo.BackupMngDB WHERE [INV.IT] = @InventoryNumber";
            conn.Execute(query, new { InventoryNumber = idInventar });
            MessageBox.Show("Deleted " + idInventar);
            conn.Close();
        }

        /// <summary>
        /// Actualizează un rând în baza de date.
        /// </summary>
        /// <param name="columnName">Numele coloanei care trebuie actualizată.</param>
        /// <param name="newValue">Noua valoare pentru coloana.</param>
        /// <param name="invItValue">Valoarea pentru coloana [INV.IT].</param>
        public static void UpdateRowInDatabase(string columnName, string newValue, string invItValue)
        {
            // Verifică dacă numele coloanei este valid (opțional, poți adăuga validări suplimentare)
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(newValue) || string.IsNullOrEmpty(invItValue))
            {
                MessageBox.Show("Datele pentru actualizare nu sunt complete.");
                return;
            }

            // Formează comanda SQL pentru actualizare
            MessageBox.Show(columnName + " " + newValue);
            string query = $"UPDATE dbo.BackupMngDB SET [{columnName}]  = @newValue WHERE [INV.IT] = @invItValue";

            // Execută comanda SQL utilizând un obiect SqlConnection și SqlCommand
            using SqlConnection connection = new(connectionStringUsers);
            using SqlCommand command = new(query, connection);
            // Adaugă parametrii pentru a preveni SQL injection
            command.Parameters.AddWithValue("@newValue", newValue);
            command.Parameters.AddWithValue("@invItValue", invItValue);
            command.Parameters.AddWithValue("@coloana", columnName);
            try
            {
                // Deschide conexiunea
                connection.Open();

                // Execută comanda
                int rowsAffected = command.ExecuteNonQuery();

                // Verifică dacă actualizarea a avut succes
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Actualizarea a fost realizată cu succes!");
                }
                else
                {
                    MessageBox.Show("Nu s-a putut actualiza în baza de date.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare: {ex.Message}");
            }
        }

    }
}