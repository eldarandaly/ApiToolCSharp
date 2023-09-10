using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ess;
using System.Data;
using System.Data.SQLite;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WinFormsApp1
{
    public partial class InvoiceSinger : Form
    {
        private class TokenInfo
        {
            public required string Token { get; set; }
            public DateTime ExpirationTime { get; set; }
        }
        private readonly string DllLibPath = "eps2003csp11.dll";
        private HttpListener listener;
        private Thread listenerThread;
        DataTable dataTable = new DataTable();
        private bool disconnectButtonClicked = false;
        private bool flag = true;
        string connectionString = "Data Source=C:\\Users\\ahmed.mamdouh\\source\\repos\\WinFormsApp1\\WinFormsApp1\\userTaxDB.db;Version=3;Legacy Format=True;";
        private string newaccess_token;
        private List<DataGridViewRow> selectedUserRowOfTaxCreds = new List<DataGridViewRow>();
        private string url = "http://localhost:5253/";
        private string PreProdApi = "https://id.preprod.eta.gov.eg/connect/token";
        private Dictionary<string, TokenInfo> tokenDictionary = new Dictionary<string, TokenInfo>();

        public InvoiceSinger()
        {
            InitializeComponent();

        }

        private bool GetDisconnectButtonClicked()
        {
            return disconnectButtonClicked;
        }
        private void Connectbtn_Click(object sender, EventArgs e)
        {

            listener = new HttpListener();
            listener.Prefixes.Add(url);

            listenerThread = new Thread(ListenForRequests);
            listenerThread.Start();
        }
        /* first add the user client id and secret 
         * then connect yo preprod api 
         * have option to connect to live server 
         * 
         *process of the api take the request of the Invoice
         *then sign it using CMS 
         *then login at Tax Portal 
         *send to the api
         *
         *
         */
        private void UpdateLabel4Text(string newText)
        {
            if (label4.InvokeRequired)
            {
                label4.Invoke(new Action(() => label4.Text = newText));
            }
            else
            {
                label4.Text = newText;
            }
        }
        private async void ListenForRequests()
        {
            try
            {
                listener.Start();
                String cades = "";
                String value = "";
                //String TokenPIN = TokenPinBox.Text;
                //String TokenCertificate = TokenCertificateBox.Text;



                //if (TokenCertificate.Length == 0 && TokenPIN.Length == 0)
                //{
                //    label4.Text = "Please Insert The Pin And Certificate";
                //    MessageBox.Show(label4.Text);

                //}
                //else if (TokenPIN.Length == 0)
                //{

                //    label4.Text = " Please Type The Pin";
                //    MessageBox.Show(label4.Text);


                //}
                //else if (TokenCertificate.Length == 0)
                //{
                //    label4.Text = "Please Type The Token Certificate";
                //    MessageBox.Show(label4.Text);
                //}
                if (selectedUserRowOfTaxCreds.Count == 0)
                {
                    MessageBox.Show("Please Select A user To Login to the Tax Portal");
                }
                else
                {
                    //string checkValue = SignWithCMS(TokenPIN, TokenCertificate, value);
                    string checkValue = "found";
                    Console.WriteLine("hello worldss");
                    if (checkValue == "no device detected" || checkValue == "No slots found" || checkValue == "Certificate not found")
                    {

                        MessageBox.Show(checkValue);
                        //label4.Text = checkValue;
                    }
                    else
                    {

                        UpdateLabel4Text("Open Conn" + url);
                        while (!disconnectButtonClicked)
                        {

                            HttpListenerContext context = listener.GetContext();
                            HttpListenerRequest request = context.Request;
                            HttpListenerResponse response = context.Response;

                            if (request.HttpMethod == "POST" && request.Url.LocalPath == "/api")
                            {
                                string jTokenAsString = request.ToString();

                                try
                                {
                                    var acknowledgment = new { Message = "JSON data received successfully" };
                                    string acknowledgmentJson = JsonConvert.SerializeObject(acknowledgment);
                                    //using (StreamWriter acknowledgmentWriter = new StreamWriter(response.OutputStream, Encoding.UTF8))
                                    //{
                                    //    acknowledgmentWriter.Write(acknowledgmentJson);
                                    //}
                                    // Read and process the incoming JSON data

                                    using (StreamReader reader = new StreamReader(request.InputStream))
                                    {
                                        string json = reader.ReadToEnd();
                                        JObject invoiceJson = JsonConvert.DeserializeObject<JObject>(json, new JsonSerializerSettings()
                                        {
                                            FloatFormatHandling = FloatFormatHandling.String,
                                            FloatParseHandling = FloatParseHandling.Decimal,
                                            DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                            DateParseHandling = DateParseHandling.None
                                        });
                                        String canonicalString = Serialize(invoiceJson);
                                        String dd = "";

                                        var rowResults = new List<string>();

                                        foreach (DataGridViewRow row in selectedUserRowOfTaxCreds)
                                        {
                                            string Client_id = row.Cells["Client_ID"].Value?.ToString() ?? string.Empty;
                                            string Client_Secret_1 = row.Cells["Client_Secret_1"].Value?.ToString() ?? string.Empty;
                                            string Token_Pin = row.Cells["Token_Pin"].Value?.ToString() ?? string.Empty;
                                            string Token_Cert = row.Cells["Token_Cert"].Value?.ToString() ?? string.Empty;


                                            if (invoiceJson["documentTypeVersion"].Value<string>() == "0.9")
                                            {
                                                cades = "ANY";
                                            }
                                            else
                                            {
                                                cades = SignWithCMS(Token_Pin, Token_Cert, canonicalString);
                                            }

                                            JObject signaturesObject = new JObject(
                                                new JProperty("signatureType", "I"),
                                                new JProperty("value", cades));

                                            JArray signaturesArray = new JArray();
                                            signaturesArray.Add(signaturesObject);
                                            invoiceJson.Add("signatures", signaturesArray);
                                            String fullSignedDocument = "{\"documents\":[" + invoiceJson.ToString() + "]}";

                                            try
                                            {
                                                string access_token = await GetOrRefreshAccessToken(Client_id, Client_Secret_1);
                                                if (!string.IsNullOrEmpty(access_token))
                                                {
                                                    string result = await SubmitDocToTax(fullSignedDocument, access_token);
                                                    rowResults.Add(result);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Access token retrieval or refresh failed.");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.ToString());
                                            }


                                            // Combine the results for all rows into a single response
                                            string combinedResponse = string.Join(Environment.NewLine, rowResults);

                                            using (StreamWriter writer = new StreamWriter(response.OutputStream, Encoding.UTF8))
                                            {
                                                writer.Write(combinedResponse);
                                            }


                                            //using (StreamWriter writer = new StreamWriter(response.OutputStream, Encoding.UTF8))
                                            //{
                                            //    writer.Write(fullSignedDocument);
                                            //}
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    // Handle and log the exception
                                    LogError(ex);
                                }
                            }
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                // Handle and log the exception
                LogError(ex);
            }
            finally
            {
                // Ensure that the listener is stopped when finished
                listener.Close();
            }
        }

        private void LogError(Exception ex)
        {
            string logFilePath = "error_log.txt"; // Specify the path to your error log file

            try
            {
                // Create or open the log file and append the error message
                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine($"[Error Timestamp: {DateTime.Now}]");
                    writer.WriteLine($"Error Message: {ex.Message}");
                    writer.WriteLine($"Stack Trace: {ex.StackTrace}");
                    writer.WriteLine(new string('-', 50)); // Separator for readability
                }
            }
            catch (IOException ioEx)
            {
                // If there's an issue writing to the log file, you can handle it here
                // For example, display an error message to the user
                MessageBox.Show($"An error occurred while logging: {ioEx.Message}", "Logging Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string Serialize(JObject request)
        {
            return SerializeToken(request);
        }

        private string SerializeToken(JToken request)
        {
            string serialized = "";
            if (request.Parent is null)
            {
                SerializeToken(request.First);
            }
            else
            {
                if (request.Type == JTokenType.Property)
                {
                    string name = ((JProperty)request).Name.ToUpper();
                    serialized += "\"" + name + "\"";
                    foreach (var property in request)
                    {
                        if (property.Type == JTokenType.Object)
                        {
                            serialized += SerializeToken(property);
                        }
                        if (property.Type == JTokenType.Boolean || property.Type == JTokenType.Integer || property.Type == JTokenType.Float || property.Type == JTokenType.Date)
                        {
                            serialized += "\"" + property.Value<string>() + "\"";
                        }
                        if (property.Type == JTokenType.String)
                        {
                            serialized += JsonConvert.ToString(property.Value<string>());
                        }
                        if (property.Type == JTokenType.Array)
                        {
                            foreach (var item in property.Children())
                            {
                                serialized += "\"" + ((JProperty)request).Name.ToUpper() + "\"";
                                serialized += SerializeToken(item);
                            }
                        }
                    }
                }
                // Added to fix "References"
                if (request.Type == JTokenType.String)
                {
                    serialized += JsonConvert.ToString(request.Value<string>());
                }
            }
            if (request.Type == JTokenType.Object)
            {
                foreach (var property in request.Children())
                {

                    if (property.Type == JTokenType.Object || property.Type == JTokenType.Property)
                    {
                        serialized += SerializeToken(property);
                    }
                }
            }

            return serialized;
        }


        private string SignWithCMS(string TokenPin, string TokenCertificate, string serializedJson)
        {
            byte[] data = Encoding.UTF8.GetBytes(serializedJson);
            Pkcs11InteropFactories factories = new Pkcs11InteropFactories();
            using (IPkcs11Library pkcs11Library = factories.Pkcs11LibraryFactory.LoadPkcs11Library(factories, DllLibPath, AppType.MultiThreaded))
            {
                ISlot slot = pkcs11Library.GetSlotList(SlotsType.WithTokenPresent).FirstOrDefault();

                if (slot is null)
                {
                    return "No slots found";

                }

                ITokenInfo tokenInfo = slot.GetTokenInfo();

                ISlotInfo slotInfo = slot.GetSlotInfo();


                using (var session = slot.OpenSession(SessionType.ReadWrite))
                {

                    session.Login(CKU.CKU_USER, Encoding.UTF8.GetBytes(TokenPin));

                    var certificateSearchAttributes = new List<IObjectAttribute>()
                    {
                        session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE),
                        session.Factories.ObjectAttributeFactory.Create(CKA.CKA_TOKEN, true),
                        session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CERTIFICATE_TYPE, CKC.CKC_X_509)
                    };

                    IObjectHandle certificate = session.FindAllObjects(certificateSearchAttributes).FirstOrDefault();

                    if (certificate is null)
                    {
                        return "Certificate not found";
                    }

                    X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                    store.Open(OpenFlags.MaxAllowed);

                    // find cert by thumbprint
                    var foundCerts = store.Certificates.Find(X509FindType.FindByIssuerName, TokenCertificate, false);

                    //var foundCerts = store.Certificates.Find(X509FindType.FindBySerialNumber, "2b1cdda84ace68813284519b5fb540c2", true);



                    if (foundCerts.Count == 0)
                        return "no device detected";

                    var certForSigning = foundCerts[0];
                    store.Close();


                    ContentInfo content = new ContentInfo(new Oid("1.2.840.113549.1.7.5"), data);


                    SignedCms cms = new SignedCms(content, true);

                    EssCertIDv2 bouncyCertificate = new EssCertIDv2(new Org.BouncyCastle.Asn1.X509.AlgorithmIdentifier(new DerObjectIdentifier("1.2.840.113549.1.9.16.2.47")), this.HashBytes(certForSigning.RawData));

                    SigningCertificateV2 signerCertificateV2 = new SigningCertificateV2(new EssCertIDv2[] { bouncyCertificate });


                    CmsSigner signer = new CmsSigner(certForSigning);

                    signer.DigestAlgorithm = new Oid("2.16.840.1.101.3.4.2.1");



                    signer.SignedAttributes.Add(new Pkcs9SigningTime(DateTime.UtcNow));
                    signer.SignedAttributes.Add(new AsnEncodedData(new Oid("1.2.840.113549.1.9.16.2.47"), signerCertificateV2.GetEncoded()));


                    cms.ComputeSignature(signer);

                    var output = cms.Encode();

                    return Convert.ToBase64String(output);
                }
            }
        }

        private byte[] HashBytes(byte[] rawData)
        {
            using (SHA256 sha = SHA256.Create())
            {
                var output = sha.ComputeHash(rawData);
                return output;
            }
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            if (disconnectButtonClicked == false)
            {
                // Set a flag to indicate that the "Disconnect" button was clicked
                disconnectButtonClicked = true;
                // Close the HTTP listener (this will trigger the exit from the while loop)
                listener.Close();
                MessageBox.Show("Discounnected");
                label4.Text = "Discounnected";
            }
            else
            {
                MessageBox.Show("There is No Connection To Discounnect From");
            }


        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void TokenPinBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the entered key is a number (0-9) or the Backspace key
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Suppress the keypress if it's not a number or Backspace
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (flag)
            {
                Size = new Size(1230, 479);
                flag = false;
            }
            else
            {
                Size = new Size(411, 479);
                flag = true;
            }


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string xx = Application.StartupPath;
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            string query = "SELECT * FROM Users"; // Change 'YourTable' to your table name
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);

            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            connection.Close();

        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the changed cell is in the ComboBox column ("Column2") and not in the header row
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dataGridView1.Columns[e.ColumnIndex].Name == "Column2")
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                DataGridViewComboBoxCell comboBoxCell = row.Cells["Column2"] as DataGridViewComboBoxCell;

                if (comboBoxCell != null)
                {
                    string selectedValue = comboBoxCell.Value as string;

                    if (selectedValue == "Prod")
                    {
                        row.Cells["Column3"].Value = "Live1";
                        row.Cells["Column4"].Value = "Live2";
                        row.Cells["Column5"].Value = "Live3";
                    }
                    else
                    {
                        row.Cells["Column3"].Value = "Value1";
                        row.Cells["Column4"].Value = "Value2";
                        row.Cells["Column5"].Value = "Value3";
                    }
                }
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // Commit changes immediately when a cell value changes
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void Addrow_Click(object sender, EventArgs e)
        {
            DataRow newRow = dataTable.NewRow();
            dataTable.Rows.Add(newRow);
            dataGridView1.ReadOnly = false;
            dataGridView1.Refresh();

        }
        private void deleteButton_Click(object sender, EventArgs e)
        {
            // Check if any row is selected in the DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Get the index of the selected row in the DataGridView
                int rowIndex = selectedRow.Index;

                // Remove the selected row from the DataTable
                dataTable.Rows.RemoveAt(rowIndex);

                // Refresh the DataGridView to reflect the changes
                dataGridView1.Refresh();
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }
        private void InsertDataIntoDatabaseButton_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                foreach (DataRow row in dataTable.Rows)
                {
                    // Retrieve data from the DataTable
                    string Email = row["Email"].ToString();
                    string Api_Type = row["Api_Type"].ToString();
                    string Client_Id = row["Client_ID"].ToString();
                    string Client_Serect_1 = row["Client_Secret_1"].ToString();
                    string Client_Serect_2 = row["Client_Secret_2"].ToString();
                    string Scope = row["Scope"].ToString();
                    string Token_Pin = row["Token_Pin"].ToString();
                    string Token_Cert = row["Token_Cert"].ToString();

                    // Check if the record already exists in the database
                    string selectQuery = "SELECT COUNT(*) FROM Users WHERE Client_ID = @Client_ID AND Client_Secret_1 = @Client_Secret_1";
                    using (SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, connection))
                    {
                        selectCmd.Parameters.AddWithValue("@Client_ID", Client_Id);
                        selectCmd.Parameters.AddWithValue("@Client_Secret_1", Client_Serect_1);

                        int count = Convert.ToInt32(selectCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            // Record already exists, skip this insert
                            continue;
                        }
                    }

                    // Insert the data into the SQLite database
                    string insertQuery = "INSERT INTO Users " +
                        "(Email, Api_Type, Client_ID, Client_Secret_1, Client_Secret_2, Scope, Token_Pin, Token_Cert) " +
                        "VALUES (@Email, @Api_Type, @Client_Id, @Client_Serect_1, @Client_Serect_2, @Scope, @Token_Pin, @Token_Cert)";

                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@Email", Email);
                            cmd.Parameters.AddWithValue("@Api_Type", Api_Type);
                            cmd.Parameters.AddWithValue("@Client_Id", Client_Id);
                            cmd.Parameters.AddWithValue("@Client_Serect_1", Client_Serect_1);
                            cmd.Parameters.AddWithValue("@Client_Serect_2", Client_Serect_2);
                            cmd.Parameters.AddWithValue("@Scope", Scope);
                            cmd.Parameters.AddWithValue("@Token_Pin", Token_Pin);
                            cmd.Parameters.AddWithValue("@Token_Cert", Token_Cert);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        // Handle other SQLite exceptions or general exceptions as needed
                        MessageBox.Show("An error occurred while inserting data into the database.");
                        // Optionally, log the error for debugging purposes
                        Console.WriteLine("SQLite Error: " + ex.Message);
                    }
                }

                connection.Close();
                MessageBox.Show("Saved");
            }

            // Optionally, refresh the DataGridView or clear it after inserting data
            dataGridView1.Refresh();
        }


        private async Task<string> LoginToTaxAPI(string client_id, string client_secret)
        {
            using HttpClient client = new HttpClient();

            // Define the request URL
            string requestUrl = PreProdApi;
            var request = new HttpRequestMessage(HttpMethod.Post, PreProdApi);

            // Define the request headers
            request.Headers.Add("HeaderName", "HeaderValue");
            request.Headers.Add("Cookie", "3f6bf69972563c3e0e619b78edf73035=f7d2dd5410a2a5f86b2a37a8085c3717; TS01af3122=01bb6af84e15c943bf8bb44e8d2309b6a9e92b7c8db37ff37f470c5bc8a288a088c631598021491e4e12db7bdedb45f22a43b8c39d396d5be71eb0b0cba9155b2f405f033d");
            // Define the request content parameters
            var collection = new List<KeyValuePair<string, string>>();

            // Create the request content with string interpolation

            // Create the HTTP request message
            collection.Add(new("grant_type", "client_credentials"));
            collection.Add(new("client_id", client_id));
            collection.Add(new("client_secret", client_secret));
            collection.Add(new("scope", "InvoicingAPI"));

            var content = new FormUrlEncodedContent(collection);
            request.Content = content;

            // Send the HTTP request asynchronously
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Read and parse the response body
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject responseObject = JObject.Parse(responseBody);

            // Extract the access token and expiration time from the response
            string access_token = responseObject["access_token"].ToString();
            int expiresIn = responseObject["expires_in"].Value<int>(); // Expiration time in seconds

            // Calculate the token expiration time
            DateTime expirationTime = DateTime.Now.AddSeconds(expiresIn);

            // Store the token and its expiration time for the account
            // You can use a data structure to associate tokens with accounts
            // For simplicity, I'm using a dictionary here (you can choose a more suitable data structure)
            if (!tokenDictionary.ContainsKey(client_id))
            {
                tokenDictionary[client_id] = new TokenInfo
                {
                    Token = access_token,
                    ExpirationTime = expirationTime
                };
            }
            else
            {
                // If the token already exists, update its value and expiration time
                tokenDictionary[client_id].Token = access_token;
                tokenDictionary[client_id].ExpirationTime = expirationTime;
            }

            return access_token;
        }

        private async Task<string> GetOrRefreshAccessToken(string client_id, string client_secret)
        {
            // Check if the token exists and is still valid
            if (tokenDictionary.ContainsKey(client_id) &&
                tokenDictionary[client_id].ExpirationTime > DateTime.Now)
            {
                // Return the existing token
                return tokenDictionary[client_id].Token;
            }
            else
            {
                // Request a new access token
                string newToken = await LoginToTaxAPI(client_id, client_secret);

                // Store the new token in the dictionary
                tokenDictionary[client_id] = new TokenInfo
                {
                    Token = newToken,
                    ExpirationTime = DateTime.Now.AddSeconds(3600) // Set the expiration time (3600 seconds = 1 hour)
                };

                return newToken;
            }
        }



        private async Task<string> SubmitDocToTax(string fullSignedDoc, string access_token)
        {
            try
            {
                // URL for the API endpoint
                string subUrl = "https://api.preprod.invoicing.eta.gov.eg/api/v1/documentsubmissions";

                // Create an HttpClient instance
                var client = new HttpClient();

                // Create an HTTP request message with the POST method and URL
                var request = new HttpRequestMessage(HttpMethod.Post, subUrl);

                // Add the authorization token to the request headers
                string authHeader = "Bearer " + access_token;
                request.Headers.Add("Authorization", authHeader);

                // Create the request content with the JSON payload
                var content = new StringContent(fullSignedDoc, Encoding.UTF8, "application/json");
                request.Content = content;

                // Send the HTTP request asynchronously
                var response = await client.SendAsync(request);

                // Ensure that the response has a successful status code (e.g., 200 OK)
                response.EnsureSuccessStatusCode();

                // Read and return the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception ex)
            {
                // Handle and log any exceptions that may occur during the request
                Console.WriteLine("Error: " + ex.ToString());
                // Return an error message or throw an exception if desired
                throw ex;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["RadioColumn"].Index)
            {
                DataGridViewRow clickedRow = dataGridView1.Rows[e.RowIndex];

                // Check if the clicked row is already in the list of selected rows
                if (selectedUserRowOfTaxCreds.Contains(clickedRow))
                {
                    // If it is, uncheck the checkbox (simulated radio button) and remove it from the list
                    DataGridViewCheckBoxCell cell = clickedRow.Cells["RadioColumn"] as DataGridViewCheckBoxCell;
                    cell.Value = false;
                    selectedUserRowOfTaxCreds.Remove(clickedRow);
                }
                else
                {
                    // If it's not in the list, check the checkbox and add it to the list
                    DataGridViewCheckBoxCell cell = clickedRow.Cells["RadioColumn"] as DataGridViewCheckBoxCell;
                    cell.Value = true;
                    selectedUserRowOfTaxCreds.Add(clickedRow);
                }
            }
        }

        private static void GetSelectedRowData(DataGridViewRow selectedRow)
        {
            string Email = selectedRow.Cells["Email"].Value?.ToString() ?? string.Empty;
            string Api_Type = selectedRow.Cells["Api_Type"].Value?.ToString() ?? string.Empty;
            string Client_ID = selectedRow.Cells["Client_ID"].Value?.ToString() ?? string.Empty;
            string Client_Secret_1 = selectedRow.Cells["Client_Secret_1"].Value?.ToString() ?? string.Empty;
            string Client_Secret_2 = selectedRow.Cells["Client_Secret_2"].Value?.ToString() ?? string.Empty;
            string Scope = selectedRow.Cells["Scope"].Value?.ToString() ?? string.Empty;
            string Token_Pin = selectedRow.Cells["Token_Pin"].Value?.ToString() ?? string.Empty;
            string Token_Cert = selectedRow.Cells["Token_Cert"].Value?.ToString() ?? string.Empty;
            MessageBox.Show(Email);
        }
    }
}

