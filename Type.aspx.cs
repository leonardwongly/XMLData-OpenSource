using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Amazon.DynamoDBv2;
using System.Threading;
using System.Diagnostics;
using Amazon.DynamoDBv2.Model;
using System.Net.Sockets;

namespace XMLData
{
    public partial class Type : System.Web.UI.Page
    {
        public static Amazon.DynamoDBv2.DocumentModel.Table aisTable;
        public static CancellationTokenSource source = new CancellationTokenSource();
        public static CancellationToken token = source.Token;
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private static string tableName = "ais-table";
        private static bool operationSucceeded, operationFailed;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] typeList = {
                "0 - Not available, default", "1 - Reserved for future use", "2 - Reserved for future use", "3 - Reserved for future use", "4 - Reserved for future use", "5 - Reserved for future use", "6 - Reserved for future use", "7 - Reserved for future use", "8 - Reserved for future use", "9 - Reserved for future use",
                "10 - Reserved for future use", "11 - Reserved for future use", "12 - Reserved for future use", "13 - Reserved for future use", "14 - Reserved for future use", "15 - Reserved for future use", "16 - Reserved for future use", "17 - Reserved for future use", "18 - Reserved for future use", "19 - Reserved for future use",
                "20 - Wing in ground (WIG), all ships of this type", "21 - Wing in ground (WIG), Hazardous category A", "22 - Wing in ground (WIG), Hazardous category B", "23 - Wing in ground (WIG), Hazardous category C", "24 - Wing in ground (WIG), Hazardous category D", "25 - Wing in ground (WIG), Reserved for future use", "26 - Wing in ground (WIG), Reserved for future use", "27 - Wing in ground (WIG), Reserved for future use", "28 - Wing in ground (WIG), Reserved for future use", "29 - Wing in ground (WIG), Reserved for future use",
                "30 - Fishing", "31 - Towing", "32 - Towing: length exceeds 200m or breadth exceeds 25m", "33 - Dredging or underwater ops", "34 - Diving Ops", "35 - Military ops", "36 - Sailing", "37 - Pleasure Craft", "38 - Reserved", "39 - Reserved",
                "40 - High speed craft (HSC), all ships of this type", "41 - High speed craft (HSC), Hazardous category A", "42 - High speed craft (HSC), Hazardous category B", "43 - High speed craft (HSC), Hazardous category C", "44 - High speed craft (HSC), Hazardous category D", "45 - High speed craft (HSC), Reserved for future use", "46 - High speed craft (HSC), Reserved for future use", "47 - High speed craft (HSC), Reserved for future use", "48 - High speed craft (HSC), Reserved for future use", "49 - High speed craft (HSC), No additional information",
                "50 - Pilot Vessel", "51 - Search and Rescue vessel", "52 - Tug", "53 - Port Tender", "54 - Anti-pollution equipment", "55 - Law Enforcement", "56 - Spare - Local Vessel", "57 - Spare - Local Vessel", "58 - Medical Transport", "59 - Noncombatant ship according to RR Resolution No. 18",
                "60 - Passenger, all ships of this type", "61 - Passenger, Hazardous category A", "62 - Passenger, Hazardous category B", "63 - Passenger, Hazardous category C", "64 - Passenger, Hazardous category D", "65 - Passenger, Reserved for future use", "66 - Passenger, Reserved for future use", "67 - Passenger, Reserved for future use", "68 - Passenger, Reserved for future use", "69 - Passenger, No additional information",
                "70 - Cargo, all ships of this type", "71 - Cargo, Hazardous category A", "72 - Cargo, Hazardous category B", "73 - Cargo, Hazardous category C", "74 - Cargo, Hazardous category D", "75 - Cargo, Reserved for future use", "76 - Cargo, Reserved for future use", "77 - Cargo, Reserved for future use", "78 - Cargo, Reserved for future use", "79 - Cargo, No additional information",
                "80 - Tanker, all ships of this type", "81 - Tanker, Hazardous category A", "82 - Tanker, Hazardous category B", "83 - Tanker, Hazardous category C", "84 - Tanker, Hazardous category D", "85 - Tanker, Reserved for future use", "86 - Tanker, Reserved for future use", "87 -	Tanker, Reserved for future use", "88 - Tanker, Reserved for future use", "89 - Tanker, No additional information",
                "90 - Other Type, all ships of this type", "91 - Other Type, Hazardous category A", "92 - Other Type, Hazardous category B", "93 - Other Type, Hazardous category C", "94 - Other Type, Hazardous category D", "95 - Other Type, Reserved for future use", "96 - Other Type, Reserved for future use", "97 - Other Type, Reserved for future use", "98 - Other Type, Reserved for future use", "99 - Other Type, no additional information"
            };

                for (int i = 0; i < typeList.Length; i++)
                {
                    ddlTypeList.Items.Add(new ListItem(typeList[i], i.ToString()));
                }
            }
            
        }

        protected void btnCount_Click(object sender, EventArgs e)
        {
            client = new AmazonDynamoDBClient();

            QueryRequest queryRequest = new QueryRequest
            {
                TableName = tableName,
                IndexName = "TYPE-TIME-index",
                KeyConditionExpression = "#type = :type",
                ExpressionAttributeNames = new Dictionary<String, String> {
                    { "#type", "TYPE" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":type", new AttributeValue { S =  ddlTypeList.SelectedValue.ToString() }}
                },
                ScanIndexForward = false
            };

            var result = client.Query(queryRequest);
            lblTotalItems.Text = "Based on the Vessel Type <b>" + ddlTypeList.SelectedValue.ToString() + "</b>, there are <b>" + result.Count.ToString() + "</b> items";

            
        }

        protected void btnProceed_Click(object sender, EventArgs e)
        {
            client = new AmazonDynamoDBClient();

            QueryRequest queryRequest = new QueryRequest
            {
                TableName = tableName,
                IndexName = "TYPE-TIME-index",
                KeyConditionExpression = "#type = :type",
                ExpressionAttributeNames = new Dictionary<String, String> {
                    { "#type", "TYPE" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":type", new AttributeValue { S =  ddlTypeList.SelectedValue.ToString() }}
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
            tblAISType.Rows.AddAt(0, aisRow);
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
                tblAISType.Rows.Add(tr);
                count++;
            }
        }

        protected void rblNumeric_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnProceed.Visible = true;
            if (rblNumeric.SelectedValue.Equals("All"))
            {
                tbCount.Visible = false;
            }
            else if (rblNumeric.SelectedValue.Equals("Custom"))
            {
                tbCount.Visible = true;
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