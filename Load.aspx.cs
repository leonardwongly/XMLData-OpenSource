using System;
using System.Net.Sockets;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using System.Collections;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;

namespace XMLData
{
    public partial class Load : System.Web.UI.Page
    {
        public static Amazon.DynamoDBv2.DocumentModel.Table aisTable;
        public static CancellationTokenSource source = new CancellationTokenSource();
        public static CancellationToken token = source.Token;
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private static string tableName = "ais-table";
        private static bool operationSucceeded, operationFailed;
        private static string txt = "";
        ArrayList dataList = new ArrayList();
        int i = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 

            }
        }

        private void RetrieveItem()
        {
            
            client = new AmazonDynamoDBClient();

            QueryRequest queryRequest = new QueryRequest
            {
                TableName = tableName,
                IndexName = "MMSI-TIME-index",
                KeyConditionExpression = "#mmsi = :mmsi",
                ExpressionAttributeNames = new Dictionary<String, String> {
                    {"#mmsi", "MMSI"}
                  },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":mmsi", new AttributeValue { S =  tbMMSI.Text }}
                },
                ScanIndexForward = false
            };

            var result = client.Query(queryRequest);

            var items = result.Items;

            TableHeaderRow aisRow = new TableHeaderRow();
           

            TableHeaderCell aisItemCount = new TableHeaderCell();
            TableHeaderCell aisName = new TableHeaderCell();
            TableHeaderCell aisMMSI = new TableHeaderCell();
            TableHeaderCell aisIMO = new TableHeaderCell();
            TableHeaderCell aisSOG = new TableHeaderCell();
            TableHeaderCell aisCOG = new TableHeaderCell();
            TableHeaderCell aisLatitude = new TableHeaderCell();
            TableHeaderCell aisLongitude = new TableHeaderCell();
            TableHeaderCell aisType = new TableHeaderCell();
            TableHeaderCell aisDraught = new TableHeaderCell();
            TableHeaderCell aisDest = new TableHeaderCell();
            TableHeaderCell aisETA = new TableHeaderCell();
            TableHeaderCell aisHeading = new TableHeaderCell();
            TableHeaderCell aisROT = new TableHeaderCell();
            TableHeaderCell aisTime = new TableHeaderCell();

            /*TableHeaderCell aisA = new TableHeaderCell();
            TableHeaderCell aisB = new TableHeaderCell();
            TableHeaderCell aisC = new TableHeaderCell();
            TableHeaderCell aisD = new TableHeaderCell();
            TableHeaderCell aisCallSign = new TableHeaderCell();
            TableHeaderCell aisNavStat = new TableHeaderCell();
            TableHeaderCell aisID = new TableHeaderCell();*/

            aisItemCount.Text = "No.";
            aisName.Text = "Name";
            aisMMSI.Text = "MMSI";
            aisIMO.Text = "IMO";
            aisSOG.Text = "Speed over Ground";
            aisCOG.Text = "Course over Ground";
            aisLatitude.Text = "Latitude";
            aisLongitude.Text = "Longitude";
            aisType.Text = "Type";
            aisDraught.Text = "Draught";
            aisDest.Text = "Destination";
            aisETA.Text = "ETA";
            aisHeading.Text = "Heading";
            aisROT.Text = "Rate of Turn";
            aisTime.Text = "Update Time";

            aisRow.Cells.Add(aisItemCount);
            aisRow.Cells.Add(aisName);
            aisRow.Cells.Add(aisMMSI);
            aisRow.Cells.Add(aisIMO);
            aisRow.Cells.Add(aisSOG);
            aisRow.Cells.Add(aisCOG);
            aisRow.Cells.Add(aisLatitude);
            aisRow.Cells.Add(aisLongitude);
            aisRow.Cells.Add(aisType);
            aisRow.Cells.Add(aisDraught);
            aisRow.Cells.Add(aisDest);
            aisRow.Cells.Add(aisETA);
            aisRow.Cells.Add(aisHeading);
            aisRow.Cells.Add(aisROT);
            aisRow.Cells.Add(aisTime);
            tblAIS.Rows.AddAt(0, aisRow);
            int count = 0;
            foreach (var currentItem in items)
            {
                if (rblNumeric.SelectedValue.ToString().Equals("Custom"))
                {
                    if (count == int.Parse(tbCount.Text))
                    {
                        break;
                    }
                }
                TableRow tr = new TableRow();

                TableCell tdItemCount = new TableCell();
                HyperLink hyperItemCount = new HyperLink();
                hyperItemCount.ID = "hyperItemCount" + count;
                hyperItemCount.Text = (count + 1).ToString();
                tdItemCount.Controls.Add(hyperItemCount);
                

                TableCell tdName = new TableCell();
                TableCell tdMMSI = new TableCell();
                TableCell tdIMO = new TableCell();
                TableCell tdSOG = new TableCell();
                TableCell tdCOG = new TableCell();
                TableCell tdLATITUDE = new TableCell();
                TableCell tdLONGITUDE = new TableCell();
                TableCell tdTYPE = new TableCell();
                TableCell tdDRAUGHT = new TableCell();
                TableCell tdDEST = new TableCell();
                TableCell tdETA = new TableCell();
                TableCell tdHEADING = new TableCell();
                TableCell tdROT = new TableCell();
                TableCell tdTIME = new TableCell();

                foreach (string attr in currentItem.Keys)
                {
                    if (attr == "NAME")
                    {
                        HyperLink hyperName = new HyperLink();
                        hyperName.ID = "hyperName" + count;
                        hyperName.Text = currentItem[attr].S;
                        tdName.Controls.Add(hyperName);
                    }

                    else if (attr == "MMSI")
                    {
                        HyperLink hyperMMSI = new HyperLink();
                        hyperMMSI.ID = "hyperMMSI" + count;
                        hyperMMSI.Text = currentItem[attr].S;
                        tdMMSI.Controls.Add(hyperMMSI);
                        
                    }

                    else if (attr == "IMO")
                    {
                        HyperLink hyperIMO = new HyperLink();
                        hyperIMO.ID = "hyperIMO" + count;
                        hyperIMO.Text = currentItem[attr].S;
                        tdIMO.Controls.Add(hyperIMO);
                    }

                    else if (attr == "SOG")
                    {
                        HyperLink hyperSOG = new HyperLink();
                        hyperSOG.ID = "hyperSOG" + count;
                        hyperSOG.Text = currentItem[attr].S;
                        tdSOG.Controls.Add(hyperSOG);
                    }

                    else if (attr == "COG")
                    {
                        HyperLink hyperCOG = new HyperLink();
                        hyperCOG.ID = "hyperCOG" + count;
                        hyperCOG.Text = currentItem[attr].S;
                        tdCOG.Controls.Add(hyperCOG);
                    }

                    else if (attr == "LATITUDE")
                    {
                        HyperLink hyperLATITUDE = new HyperLink();
                        hyperLATITUDE.ID = "hyperLATITUDE" + count;
                        hyperLATITUDE.Text = currentItem[attr].S;
                        tdLATITUDE.Controls.Add(hyperLATITUDE); 
                    }

                    else if (attr == "LONGITUDE")
                    {
                        HyperLink hyperLONGITUDE = new HyperLink();
                        hyperLONGITUDE.ID = "hyperLONGITUDE" + count;
                        hyperLONGITUDE.Text = currentItem[attr].S;
                        tdLONGITUDE.Controls.Add(hyperLONGITUDE);
                    }

                    else if (attr == "TYPE")
                    {
                        HyperLink hyperTYPE = new HyperLink();
                        hyperTYPE.ID = "hyperTYPE" + count;
                        hyperTYPE.Text = currentItem[attr].S;
                        tdTYPE.Controls.Add(hyperTYPE);
                    }

                    else if (attr == "DRAUGHT")
                    {
                        HyperLink hyperDRAUGHT = new HyperLink();
                        hyperDRAUGHT.ID = "hyperDRAUGHT" + count;
                        hyperDRAUGHT.Text = currentItem[attr].S;
                        tdDRAUGHT.Controls.Add(hyperDRAUGHT);
                        
                    }

                    else if (attr == "DEST")
                    {
                        HyperLink hyperDEST = new HyperLink();
                        hyperDEST.ID = "hyperDEST" + count;
                        hyperDEST.Text = currentItem[attr].S;
                        tdDEST.Controls.Add(hyperDEST);
                    }

                    else if (attr == "ETA")
                    {
                        HyperLink hyperETA = new HyperLink();
                        hyperETA.ID = "hyperETA" + count;
                        hyperETA.Text = currentItem[attr].S;
                        tdETA.Controls.Add(hyperETA); 
                    }

                    else if (attr == "HEADING")
                    {
                        HyperLink hyperHEADING = new HyperLink();
                        hyperHEADING.ID = "hyperHEADING" + count;
                        hyperHEADING.Text = currentItem[attr].S;
                        tdHEADING.Controls.Add(hyperHEADING);
                    }

                    else if (attr == "ROT")
                    {
                        HyperLink hyperROT = new HyperLink();
                        hyperROT.ID = "hyperROT" + count;
                        hyperROT.Text = currentItem[attr].S;
                        tdROT.Controls.Add(hyperROT);
                        
                    }

                    else if (attr == "TIME")
                    {
                        HyperLink hyperTIME = new HyperLink();
                        hyperTIME.ID = "hyperTIME" + count;
                        hyperTIME.Text =currentItem[attr].S;
                        tdTIME.Controls.Add(hyperTIME);  
                    }

                    else
                    {
                        int num = count;
                        Debug.WriteLine(attr + " ------> " + currentItem[attr].S);
                    }
                    
                }
                tr.Cells.Add(tdItemCount);
                tr.Cells.Add(tdName);
                tr.Cells.Add(tdMMSI);
                tr.Cells.Add(tdIMO);
                tr.Cells.Add(tdSOG);
                tr.Cells.Add(tdCOG);
                tr.Cells.Add(tdLATITUDE);
                tr.Cells.Add(tdLONGITUDE);
                tr.Cells.Add(tdTYPE);
                tr.Cells.Add(tdDRAUGHT);
                tr.Cells.Add(tdDEST);
                tr.Cells.Add(tdETA);
                tr.Cells.Add(tdHEADING);
                tr.Cells.Add(tdROT);
                tr.Cells.Add(tdTIME);
                tblAIS.Rows.Add(tr);
                count++;
            }
        }

        private void PrintItem(
            Dictionary<string, AttributeValue> attributeList)
        {
            string data = "";
            foreach (KeyValuePair<string, AttributeValue> kvp in attributeList)
            {
                string attributeName = kvp.Key;
                AttributeValue value = kvp.Value;
                data += attributeName + "$" + value.S + ",";
                Debug.WriteLine(
                    attributeName + " " +
                    (value.S == null ? "" : value.S)
                    );
            }
            dataList.Add(data.Substring(0, data.Length - 1));
            i++;
            Debug.WriteLine(i + "************************************************");
        }

        protected void btnCount_Click(object sender, EventArgs e)
        {
            RetrieveItem();
            rblNumeric.ClearSelection();

        }

        protected void rblNumeric_SelectedIndexChanged(object sender, EventArgs e)
        {
            client = new AmazonDynamoDBClient();

            QueryRequest queryRequest = new QueryRequest
            {
                TableName = tableName,
                IndexName = "MMSI-TIME-index",
                KeyConditionExpression = "#mmsi = :mmsi",
                ExpressionAttributeNames = new Dictionary<String, String> {
                    {"#mmsi", "MMSI"}
                  },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":mmsi", new AttributeValue { S =  tbMMSI.Text.ToString() }}
                },
                ScanIndexForward = false
            };

            var result = client.Query(queryRequest);
            lblTotalItems.Text = "Based on MMSI <b>" + tbMMSI.Text.ToString() + "</b>, there are <b>" + result.Count.ToString() + "</b> items";

            if (rblNumeric.SelectedValue.Equals("All"))
            {
                tbCount.Visible = false;
            }
            else if (rblNumeric.SelectedValue.Equals("Custom"))
            {
                tbCount.Visible = true;
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            Excel.Application excel;
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            Excel.Range cellrange;

            excel = new Excel.Application
            {
                Visible = false,
                DisplayAlerts = false
            };
            workbook = excel.Workbooks.Add();

            worksheet = (Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = "VesselDataExcel";

            worksheet.Cells.Font.Size = 15;

            int rowcount = 2;

            foreach (DataRow datarow in ExportToExcel().Rows)
            {
                rowcount += 1;
                for (int i = 1; i <= ExportToExcel().Columns.Count; i++)
                {

                    if (rowcount == 3)
                    {
                        worksheet.Cells[2, i] = ExportToExcel().Columns[i - 1].ColumnName;
                        worksheet.Cells.Font.Color = System.Drawing.Color.Black;

                    }

                    worksheet.Cells[rowcount, i] = datarow[i - 1].ToString();

                    if (rowcount > 3)
                    {
                        if (i == ExportToExcel().Columns.Count)
                        {
                            if (rowcount % 2 == 0)
                            {
                                cellrange = worksheet.Range[worksheet.Cells[rowcount, 1], worksheet.Cells[rowcount, ExportToExcel().Columns.Count]];
                            }

                        }
                    }

                }

            }

            cellrange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[rowcount, ExportToExcel().Columns.Count]];
            cellrange.EntireColumn.AutoFit();
            Excel.Borders border = cellrange.Borders;
            border.LineStyle = Excel.XlLineStyle.xlContinuous;
            border.Weight = 2d;

            cellrange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[2, ExportToExcel().Columns.Count]];

            workbook.SaveAs("VesselData");
            workbook.Close();
            excel.Quit();

        }

        public DataTable ExportToExcel()
        {
            DataTable table = new DataTable();
            client = new AmazonDynamoDBClient();

            QueryRequest queryRequest = new QueryRequest
            {
                TableName = tableName,
                IndexName = "MMSI-TIME-index",
                KeyConditionExpression = "#mmsi = :mmsi",
                ExpressionAttributeNames = new Dictionary<String, String> {
                    {"#mmsi", "MMSI"}
                  },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":mmsi", new AttributeValue { S =  tbMMSI.Text }}
                },
                ScanIndexForward = false
            };

            var result = client.Query(queryRequest);

            var items = result.Items;

            TableHeaderRow aisRow = new TableHeaderRow();
           

            TableHeaderCell aisItemCount = new TableHeaderCell();
            TableHeaderCell aisName = new TableHeaderCell();
            TableHeaderCell aisMMSI = new TableHeaderCell();
            TableHeaderCell aisIMO = new TableHeaderCell();
            TableHeaderCell aisSOG = new TableHeaderCell();
            TableHeaderCell aisCOG = new TableHeaderCell();
            TableHeaderCell aisLatitude = new TableHeaderCell();
            TableHeaderCell aisLongitude = new TableHeaderCell();
            TableHeaderCell aisType = new TableHeaderCell();
            TableHeaderCell aisDraught = new TableHeaderCell();
            TableHeaderCell aisDest = new TableHeaderCell();
            TableHeaderCell aisETA = new TableHeaderCell();
            TableHeaderCell aisHeading = new TableHeaderCell();
            TableHeaderCell aisROT = new TableHeaderCell();
            TableHeaderCell aisTime = new TableHeaderCell();

            /*TableHeaderCell aisA = new TableHeaderCell();
            TableHeaderCell aisB = new TableHeaderCell();
            TableHeaderCell aisC = new TableHeaderCell();
            TableHeaderCell aisD = new TableHeaderCell();
            TableHeaderCell aisCallSign = new TableHeaderCell();
            TableHeaderCell aisNavStat = new TableHeaderCell();
            TableHeaderCell aisID = new TableHeaderCell();*/

            aisItemCount.Text = "No.";
            aisName.Text = "Name";
            aisMMSI.Text = "MMSI";
            aisIMO.Text = "IMO";
            aisSOG.Text = "Speed over Ground";
            aisCOG.Text = "Course over Ground";
            aisLatitude.Text = "Latitude";
            aisLongitude.Text = "Longitude";
            aisType.Text = "Type";
            aisDraught.Text = "Draught";
            aisDest.Text = "Destination";
            aisETA.Text = "ETA";
            aisHeading.Text = "Heading";
            aisROT.Text = "Rate of Turn";
            aisTime.Text = "Update Time";

            aisRow.Cells.Add(aisItemCount);
            aisRow.Cells.Add(aisName);
            aisRow.Cells.Add(aisMMSI);
            aisRow.Cells.Add(aisIMO);
            aisRow.Cells.Add(aisSOG);
            aisRow.Cells.Add(aisCOG);
            aisRow.Cells.Add(aisLatitude);
            aisRow.Cells.Add(aisLongitude);
            aisRow.Cells.Add(aisType);
            aisRow.Cells.Add(aisDraught);
            aisRow.Cells.Add(aisDest);
            aisRow.Cells.Add(aisETA);
            aisRow.Cells.Add(aisHeading);
            aisRow.Cells.Add(aisROT);
            aisRow.Cells.Add(aisTime);
            table.Rows.Add(0, aisRow);
            int count = 0;
            int rowCount = 2;
            foreach (var currentItem in items)
            {
                if (rblNumeric.SelectedValue.ToString().Equals("Custom"))
                {
                    if (count == int.Parse(tbCount.Text))
                    {
                        break;
                    }
                }
                TableRow tr = new TableRow();

                TableCell tdItemCount = new TableCell();
                HyperLink hyperItemCount = new HyperLink();
                hyperItemCount.ID = "hyperItemCount" + count;
                hyperItemCount.Text = (count + 1).ToString();
                tdItemCount.Controls.Add(hyperItemCount);


                TableCell tdName = new TableCell();
                TableCell tdMMSI = new TableCell();
                TableCell tdIMO = new TableCell();
                TableCell tdSOG = new TableCell();
                TableCell tdCOG = new TableCell();
                TableCell tdLATITUDE = new TableCell();
                TableCell tdLONGITUDE = new TableCell();
                TableCell tdTYPE = new TableCell();
                TableCell tdDRAUGHT = new TableCell();
                TableCell tdDEST = new TableCell();
                TableCell tdETA = new TableCell();
                TableCell tdHEADING = new TableCell();
                TableCell tdROT = new TableCell();
                TableCell tdTIME = new TableCell();

                foreach (string attr in currentItem.Keys)
                {
                    if (attr == "NAME")
                    {
                        HyperLink hyperName = new HyperLink();
                        hyperName.ID = "hyperName" + count;
                        hyperName.Text = currentItem[attr].S;
                        tdName.Controls.Add(hyperName);
                    }

                    else if (attr == "MMSI")
                    {
                        HyperLink hyperMMSI = new HyperLink();
                        hyperMMSI.ID = "hyperMMSI" + count;
                        hyperMMSI.Text = currentItem[attr].S;
                        tdMMSI.Controls.Add(hyperMMSI);

                    }

                    else if (attr == "IMO")
                    {
                        HyperLink hyperIMO = new HyperLink();
                        hyperIMO.ID = "hyperIMO" + count;
                        hyperIMO.Text = currentItem[attr].S;
                        tdIMO.Controls.Add(hyperIMO);
                    }

                    else if (attr == "SOG")
                    {
                        HyperLink hyperSOG = new HyperLink();
                        hyperSOG.ID = "hyperSOG" + count;
                        hyperSOG.Text = currentItem[attr].S;
                        tdSOG.Controls.Add(hyperSOG);
                    }

                    else if (attr == "COG")
                    {
                        HyperLink hyperCOG = new HyperLink();
                        hyperCOG.ID = "hyperCOG" + count;
                        hyperCOG.Text = currentItem[attr].S;
                        tdCOG.Controls.Add(hyperCOG);
                    }

                    else if (attr == "LATITUDE")
                    {
                        HyperLink hyperLATITUDE = new HyperLink();
                        hyperLATITUDE.ID = "hyperLATITUDE" + count;
                        hyperLATITUDE.Text = currentItem[attr].S;
                        tdLATITUDE.Controls.Add(hyperLATITUDE);
                    }

                    else if (attr == "LONGITUDE")
                    {
                        HyperLink hyperLONGITUDE = new HyperLink();
                        hyperLONGITUDE.ID = "hyperLONGITUDE" + count;
                        hyperLONGITUDE.Text = currentItem[attr].S;
                        tdLONGITUDE.Controls.Add(hyperLONGITUDE);
                    }

                    else if (attr == "TYPE")
                    {
                        HyperLink hyperTYPE = new HyperLink();
                        hyperTYPE.ID = "hyperTYPE" + count;
                        hyperTYPE.Text = currentItem[attr].S;
                        tdTYPE.Controls.Add(hyperTYPE);
                    }

                    else if (attr == "DRAUGHT")
                    {
                        HyperLink hyperDRAUGHT = new HyperLink();
                        hyperDRAUGHT.ID = "hyperDRAUGHT" + count;
                        hyperDRAUGHT.Text = currentItem[attr].S;
                        tdDRAUGHT.Controls.Add(hyperDRAUGHT);

                    }

                    else if (attr == "DEST")
                    {
                        HyperLink hyperDEST = new HyperLink();
                        hyperDEST.ID = "hyperDEST" + count;
                        hyperDEST.Text = currentItem[attr].S;
                        tdDEST.Controls.Add(hyperDEST);
                    }

                    else if (attr == "ETA")
                    {
                        HyperLink hyperETA = new HyperLink();
                        hyperETA.ID = "hyperETA" + count;
                        hyperETA.Text = currentItem[attr].S;
                        tdETA.Controls.Add(hyperETA);
                    }

                    else if (attr == "HEADING")
                    {
                        HyperLink hyperHEADING = new HyperLink();
                        hyperHEADING.ID = "hyperHEADING" + count;
                        hyperHEADING.Text = currentItem[attr].S;
                        tdHEADING.Controls.Add(hyperHEADING);
                    }

                    else if (attr == "ROT")
                    {
                        HyperLink hyperROT = new HyperLink();
                        hyperROT.ID = "hyperROT" + count;
                        hyperROT.Text = currentItem[attr].S;
                        tdROT.Controls.Add(hyperROT);

                    }

                    else if (attr == "TIME")
                    {
                        HyperLink hyperTIME = new HyperLink();
                        hyperTIME.ID = "hyperTIME" + count;
                        hyperTIME.Text = currentItem[attr].S;
                        tdTIME.Controls.Add(hyperTIME);
                    }

                    else
                    {
                        int num = count;
                        Debug.WriteLine(attr + " ------> " + currentItem[attr].S);
                    }

                }
                tr.Cells.Add(tdItemCount);
                tr.Cells.Add(tdName);
                tr.Cells.Add(tdMMSI);
                tr.Cells.Add(tdIMO);
                tr.Cells.Add(tdSOG);
                tr.Cells.Add(tdCOG);
                tr.Cells.Add(tdLATITUDE);
                tr.Cells.Add(tdLONGITUDE);
                tr.Cells.Add(tdTYPE);
                tr.Cells.Add(tdDRAUGHT);
                tr.Cells.Add(tdDEST);
                tr.Cells.Add(tdETA);
                tr.Cells.Add(tdHEADING);
                tr.Cells.Add(tdROT);
                tr.Cells.Add(tdTIME);
                table.Rows.Add(rowCount,tr);
                count++;
                rowCount++;
            }
            return table;
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
                    Debug.WriteLine("\n      ERROR: DynamoDB Local does not appear to have been started..." +
                                      "\n        (checked port 8000)");
                    operationFailed = true;
                    return (false);
                }

                // If DynamoDB-Local does seem to be running, so create a client
                Debug.WriteLine("  -- Setting up a DynamoDB-Local client (DynamoDB Local seems to be running)");
                AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
                ddbConfig.ServiceURL = "http://localhost:8000";
                try { client = new AmazonDynamoDBClient(ddbConfig); }
                catch (Exception ex)
                {
                    Debug.WriteLine("     FAILED to create a DynamoDBLocal client; " + ex.Message);
                    operationFailed = true;
                    return false;
                }
            }

            else
            {
                try { client = new AmazonDynamoDBClient(); }
                catch (Exception ex)
                {
                    Debug.WriteLine("     FAILED to create a DynamoDB client; " + ex.Message);
                    operationFailed = true;
                }
            }
            operationSucceeded = true;
            return true;
        }




    }
}