using AzTableDemo.Entities;
using AzTableWADViewer.Extensions;
using Microsoft.Azure; // Namespace for CloudConfigurationManager 
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AzTableWADViewer
{
    public partial class Form1 : Form
    {
        private CloudTableClient tableClient;
        private string tablePrefix = "WAD";
        private string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

        }

        private async Task LoadTableData(string tableName)
        {
            // UI 요소들 공백처리.
            var source = new BindingSource();
            dataGridView1.DataSource = source;

            toolStripStatusLabel1.ForeColor = System.Drawing.Color.Blue;
            toolStripStatusLabel1.Text = "";


            // 지정된 테이블을 참조
            CloudTable table = tableClient.GetTableReference(tableName);

            int result = 0;
            // 우선 테이블의 PartitionKey만 읽어와서 전체 카운트 수를 알아낸다. 
            // 비효율적이라고 생각되지만 전체 레코드 수를 알아와서 진행바의 Max 값을 쓰기 위해서 추가함
            // 진행바가 필요없으면 제거해도 무방함
            //TableQuery<TableEntity> queryForCount = new TableQuery<TableEntity>().Select(new List<string> { "PartitionKey" });
            //IEnumerable<TableEntity> keys = table.ExecuteQuery(queryForCount);
            //using (IEnumerator<TableEntity> enumerator = keys.GetEnumerator())
            //{
            //    while (enumerator.MoveNext()) result++;
            //}
            // 상기 코드는 너무 느려서 못 써먹겠음. 전체 Row Count를 읽어올 수 있는 다른 방법이 없어보임.

            // 전체 레코드를 읽어오는 쿼리 작성
            TableQuery<WADMerticsEntity> query = new TableQuery<WADMerticsEntity>(); 
            //.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

            //IEnumerable<WADMerticsEntity> data = table.ExecuteQuery(query);
            Action<IList<WADMerticsEntity>> onProgress = (e) => {
                toolStripStatusLabel1.Text = "Loading data : " + e.Count.ToString();
            };
            
            CancellationToken ct = new CancellationToken();
            IList<WADMerticsEntity> data = await table.ExecuteQueryAsync<WADMerticsEntity>(query, ct, onProgress);
            //IList<WADMerticsEntity> data = action.Result;

            source.DataSource = data;

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = source;

            toolStripStatusLabel1.ForeColor = System.Drawing.Color.Black;
            toolStripStatusLabel1.Text = "Total Row Count : " + data.Count.ToString();
        }


        private void Connect_Click(object sender, EventArgs e)
        {
            storageConnectionString = String.Format(storageConnectionString, AccountName.Text, AccountKey.Text);

            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException fex)
            {
                MessageBox.Show("유효한 계정 정보가 아닙니다\n\n" + fex.Message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ConnectState.ForeColor = System.Drawing.Color.Blue;
            ConnectState.Text = "* 연결되었습니다";

            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();

            IEnumerable<CloudTable> list = tableClient.ListTables(tablePrefix);
            List<string> tableList = new List<string>();
            foreach (var tab in list)
            {
                tableList.Add(tab.Name);
            }

            TablesList.DataSource = tableList;
        }


   

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            string tableName = TablesList.SelectedValue.ToString();
            this.LoadTableData(tableName);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {

        }
    }

    
}
