using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Net.Sockets;
using Amazon.DynamoDBv2;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using System.Threading;
using System.Diagnostics;

namespace XMLData
{
    public partial class _Default : Page
    {

        public static Table aisTable;
        public static CancellationTokenSource source = new CancellationTokenSource();
        public static CancellationToken token = source.Token;
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private static string tableName = "";
        private static bool operationSucceeded, operationFailed;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            lblStream.Text += "Process started at " + DateTime.Now.ToLongTimeString() + "<br/>";
            string dest = "";
            //Get all the files names in the directory
            string[] filePaths = Directory.GetFiles(dest);
            for (int i = 0; i < filePaths.Length; i++)
            {
                Debug.WriteLine("Processing file " + i + " of " + filePaths.Length);
                string folder = filePaths[i].Substring(0, filePaths[i].Length - 4);
                ZipFile.ExtractToDirectory(filePaths[i], folder);

                xmlReaderFileAsync(folder + "\\data.xml");

                lblStream.Text += "File " + i + ": " + DateTime.Now.ToLongTimeString() + "<br/>";
            }

        }

        /*--------------------------------------------------------------------------
     *          createClient
     *--------------------------------------------------------------------------*/
        public static bool createClient(bool useDynamoDBLocal)
        {

            if (useDynamoDBLocal)
            {
                operationSucceeded = false;
                operationFailed = false;

                // First, check to see whether anyone is listening on the DynamoDB local port
                // (by default, this is port 8000, so if you are using a different port, modify this accordingly)
                bool localFound = false;
                try
                {
                    using (var tcp_client = new TcpClient())
                    {
                        var result = tcp_client.BeginConnect("localhost", 8000, null, null);
                        localFound = result.AsyncWaitHandle.WaitOne(3000); // Wait 3 seconds
                        tcp_client.EndConnect(result);
                    }
                }
                catch
                {
                    localFound = false;
                }
                if (!localFound)
                {
                    Console.WriteLine("\n      ERROR: DynamoDB Local does not appear to have been started..." +
                                      "\n        (checked port 8000)");
                    operationFailed = true;
                    return (false);
                }

                // If DynamoDB-Local does seem to be running, so create a client
                Console.WriteLine("  -- Setting up a DynamoDB-Local client (DynamoDB Local seems to be running)");
                AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
                ddbConfig.ServiceURL = "http://localhost:8000";
                try { client = new AmazonDynamoDBClient(ddbConfig); }
                catch (Exception ex)
                {
                    Console.WriteLine("     FAILED to create a DynamoDBLocal client; " + ex.Message);
                    operationFailed = true;
                    return false;
                }
            }

            else
            {
                try { client = new AmazonDynamoDBClient(); }
                catch (Exception ex)
                {
                    Console.WriteLine("     FAILED to create a DynamoDB client; " + ex.Message);
                    operationFailed = true;
                }
            }
            operationSucceeded = true;
            return true;
        }


        private void xmlReaderFileAsync(String filePath)
        {
            // Create an XML reader for this file.
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    // Only detect start elements.
                    if (reader.IsStartElement())
                    {
                        // Get element name and switch on it.
                        switch (reader.Name)
                        {
                            case "VESSELS":
                                // Detect this element
                                Console.WriteLine("Start <VESSELS> element.");
                                break;
                            case "vessel":
                                // Detect vessel element.
                                Console.WriteLine("Start <vessel> element.");
                                // Search for the attribute name on this current node.
                                Document newItem = new Document();

                                newItem["ID"] = DateTime.Now.ToShortDateString() + "-" + reader["MMSI"] + "-" + reader["IMO"] + "-" + DateTime.Now.ToLongTimeString().ToString();
                                newItem["MMSI"] = reader["MMSI"];
                                newItem["TYPE"] = reader["TYPE"];
                                newItem["TIME"] = reader["TIME"];
                                newItem["LONGITUDE"] = reader["LONGITUDE"];
                                newItem["LATITUDE"] = reader["LATITUDE"];
                                newItem["COG"] = reader["COG"];
                                newItem["SOG"] = reader["SOG"];
                                newItem["HEADING"] = reader["HEADING"];
                                newItem["ROT"] = reader["ROT"];
                                newItem["NAVSTAT"] = reader["NAVSTAT"];
                                newItem["IMO"] = reader["IMO"];
                                newItem["NAME"] = reader["NAME"];
                                newItem["CALLSIGN"] = reader["CALLSIGN"];
                                newItem["A"] = reader["A"];
                                newItem["B"] = reader["B"];
                                newItem["C"] = reader["C"];
                                newItem["D"] = reader["D"];
                                newItem["DRAUGHT"] = reader["DRAUGHT"];
                                newItem["DEST"] = reader["DEST"];
                                newItem["ETA"] = reader["ETA"];

                                if (String.IsNullOrEmpty(newItem["TYPE"]))
                                {
                                    newItem["TYPE"] = "EMPTY";
                                }
                                Table aisData = Table.LoadTable(client, tableName);

                                aisData.PutItem(newItem);
                                break;
                        }
                    }
                }

            }
            string dir = filePath.Substring(0, filePath.Length - 9);
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }


        }



    }
}