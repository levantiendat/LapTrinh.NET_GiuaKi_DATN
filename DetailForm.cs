using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace _102210247_LeVanTienDat
{
    public partial class DetailForm : Form
    {
        public delegate void MyDel(string huongnc, string tinhtrang, string gvhd, string txt);
        public MyDel d { get; set; }
        public string MADA { get; set; }

        public DetailForm(string mada)
        {
            MADA = mada;
            InitializeComponent();
            SetCBB();
            GUI();
        }
        public void SetCBB()
        {
            SetCBBHNC();
            SetCBBNam();
        }
        public void SetCBBHNC()
        {
            string query = "select Distinct HUONGNC from GVHD Order by HUONGNC ";
            List<string> list = new List<string>();

            foreach (DataRow i in DBHelper.Instance.GetRecords(query).Rows)
            {
                list.Add(i[0].ToString());
            }
            cbbhuongnc.Items.AddRange(list.ToArray());

        }
        public void SetCBBGVHD(string huongnc)
        {
            cbbgv.Items.Clear();
            string query = "select Distinct NAMEGV from GVHD where HUONGNC='" + huongnc + "' order by NAMEGV";
            List<string> list = new List<string>();

            foreach (DataRow i in DBHelper.Instance.GetRecords(query).Rows)
            {
                list.Add(i[0].ToString());
            }
            cbbgv.Items.AddRange(list.ToArray());
        }
        public void SetCBBNam()
        {
            for (int i = 2023; i >= 1970; i--)
            {
                cbbnam.Items.Add(i.ToString());
            }
        }

        private void cbbhuongnc_SelectedIndexChanged(object sender, EventArgs e)
        {
            string huongnc = cbbhuongnc.Text.ToString();

            SetCBBGVHD(huongnc);
            cbbgv.SelectedIndex = 0;
        }
        public void GUI()
        {
            if (MADA != "")
            {

                DataTable data1 = new DataTable();
                string query = "Select DATN.ID,NAMEDA,NAMESV,GVHD.HUONGNC,GVHD.NAMEGV,TINHTRANG,NAMBV from DATN join GVHD on GVHD.ID=DATN.IDGV where DATN.ID = '" + MADA + "'";
                data1 = DBHelper.Instance.GetRecords(query);
                txtid.Text = data1.Rows[0][0].ToString();
                txtname.Text = data1.Rows[0][1].ToString();
                txtauthor.Text = data1.Rows[0][2].ToString();
                cbbhuongnc.Text = data1.Rows[0][3].ToString();
                SetCBBGVHD(cbbhuongnc.Text.ToString());
                cbbgv.Text = data1.Rows[0][4].ToString();
                if (Convert.ToBoolean(data1.Rows[0][5].ToString()))
                {
                    tttrue.Checked = true;
                    ttfalse.Checked = false;
                }
                else
                { ttfalse.Checked = true; tttrue.Checked = false; }
                cbbnam.Text = data1.Rows[0][6].ToString();
                txtid.Enabled = false;
                this.Text = "EditForm";
            }
            else
            {
                this.Text = "AddForm";
            }
        }

        private void ok_Click(object sender, EventArgs e)
        {

            SqlParameter id = new SqlParameter("@id", txtid.Text.ToString());
            SqlParameter name = new SqlParameter("@name", txtname.Text.ToString());
            SqlParameter tacgia = new SqlParameter("@author", txtauthor.Text.ToString());
            SqlParameter nam = new SqlParameter("@nam", Convert.ToInt32(cbbnam.Text.ToString()));
            SqlParameter tinhtrang = new SqlParameter("@tinhtrang", Convert.ToBoolean(tttrue.Checked));
            string query1 = "select * from GVHD where NAMEGV= '" + cbbgv.Text.ToString() + "'";
            SqlParameter gvhd = new SqlParameter("@gvhd", DBHelper.Instance.GetRecords(query1).Rows[0][0].ToString());
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(id);
            list.Add(name);
            list.Add(tacgia);
            list.Add(nam);
            list.Add(tinhtrang);
            list.Add(gvhd);
            //Edit
            if (MADA != "")
            {
                string query = string.Format("Update DATN Set NAMEDA={1}, NAMESV={2} ,NAMBV={3},TINHTRANG={4},IDGV={5} where ID={0}",
                    "@id", "@name", "@author", "@nam", "@tinhtrang", "@gvhd");
                DBHelper.Instance.ExecuteDBs(query, list);
                d("", "", "", "");
            }
            //Add
            else
            {
                string query = string.Format("Insert into DATN Values({0},{1},{2},{3},{4},{5})",
                    "@id", "@name", "@author", "@nam", "@tinhtrang", "@gvhd");
                DBHelper.Instance.ExecuteDBs(query, list);
                d("", "", "", "");
            }
            this.Close();

        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
