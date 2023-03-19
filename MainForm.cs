using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace _102210247_LeVanTienDat
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Text = "Quản lý DATN";
            setCBB();
            SHOWDGV("", "", "", "");
        }
        public void setCBB()
        {
            setCBBHNC();
            setCBBTT();
            setCBBGV();
        }
        public void setCBBHNC()
        {
            string query = "select Distinct HUONGNC from GVHD Order by HUONGNC ";
            List<string> list = new List<string>();
            list.Add("All");
            foreach (DataRow i in DBHelper.Instance.GetRecords(query).Rows)
            {
                list.Add(i[0].ToString());
            }
            cbbhuongnc.Items.AddRange(list.ToArray());
            cbbhuongnc.SelectedIndex = 0;
        }
        public void setCBBTT()
        {
            cbbtinhtrang.Items.Add("All");
            cbbtinhtrang.Items.Add("Đã bảo vệ");
            cbbtinhtrang.Items.Add("Chưa bảo vệ");
            cbbtinhtrang.SelectedIndex = 0;
        }
        public void setCBBGV()
        {
            string query = "select Distinct NAMEGV from GVHD order by NAMEGV";
            List<string> list = new List<string>();
            list.Add("All");
            foreach (DataRow i in DBHelper.Instance.GetRecords(query).Rows)
            {
                list.Add(i[0].ToString());
            }
            cbbgvhd.Items.AddRange(list.ToArray());
            cbbgvhd.SelectedIndex = 0;
        }
        public void SHOWDGV(string huongnc, string tinhtrang, string gvhd, string txt)
        {
            int first = 0;
            string query = "select ROW_NUMBER() OVER (ORDER BY DATN.ID) AS [STT], DATN.ID as 'Mã đồ án',NAMEDA AS 'Tên đề tài', " +
                "NAMESV as 'SV thực hiện', GVHD.HUONGNC as 'Hướng NC', GVHD.NAMEGV as 'GVHD',TINHTRANG as 'Tình trạng', NAMBV as 'Năm bảo vệ' from DATN join GVHD on GVHD.ID=DATN.IDGV ";

            if (huongnc != "")
            {
                if (first == 0)
                {
                    first = 1;
                    query += "where GVHD.HUONGNC= '" + huongnc + "' ";
                }
                else
                {
                    query += "and GVHD.HUONGNC= '" + huongnc + "' ";
                }
            }
            if (tinhtrang != "")
            {
                if (first == 0)
                {
                    first = 1;
                    query += "where TINHTRANG= '" + tinhtrang + "' ";
                }
                else
                {
                    query += "and TINHTRANG= '" + tinhtrang + "' ";
                }
            }
            if (gvhd != "")
            {

                if (first == 0)
                {
                    first = 1;
                    query += "where GVHD.NAMEGV= '" + gvhd + "' ";
                }
                else
                {
                    query += "and GVHD.NAMEGV= '" + gvhd + "' ";
                }

            }
            if (txt != "")
            {
                if (first == 0)
                {
                    first = 1;
                    query += "where NAMEDA= '" + txt + "' ";
                }
                else
                {
                    query += "and NAMEDA= '" + txt + "' ";
                }
            }
            data.DataSource = DBHelper.Instance.GetRecords(query);
        }

        private void filter_change(object sender, EventArgs e)
        {
            string huongnc, tinhtrang, gvhd;
            if ("All".CompareTo(cbbhuongnc.SelectedItem.ToString()) == 0)
            {
                huongnc = "";
            }
            else huongnc = cbbhuongnc.Text.ToString();
            if (cbbtinhtrang.SelectedIndex == 0)
            {
                tinhtrang = "";
            }
            else if (cbbtinhtrang.SelectedIndex == 1)
            {
                tinhtrang = "True";
            }
            else tinhtrang = "False";
            if (cbbgvhd.SelectedIndex == 0)
            {
                gvhd = "";
            }
            else gvhd = cbbgvhd.Text.ToString();
            SHOWDGV(huongnc, tinhtrang, gvhd, "");
        }

        private void add_Click(object sender, EventArgs e)
        {
            DetailForm f = new DetailForm("");
            f.d += new DetailForm.MyDel(SHOWDGV);
            f.Show();

        }

        private void edit_Click(object sender, EventArgs e)
        {
            if (data.SelectedRows.Count == 1)
            {
                string mada = data.SelectedRows[0].Cells["Mã đồ án"].Value.ToString();
                DetailForm f = new DetailForm(mada);
                f.d += new DetailForm.MyDel(SHOWDGV);
                f.Show();
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            if (data.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow i in data.SelectedRows)
                {
                    list.Add(i.Cells["Mã đồ án"].Value.ToString());
                }

            }
            foreach (string i in list)
            {
                DialogResult at = MessageBox.Show("Bạn có muốn xóa DATN có mã số " + i + " không ?", "Confirm", MessageBoxButtons.OKCancel);
                if (at == DialogResult.OK)
                {
                    string query = "Delete from DATN where ID='" + i + "'";
                    DBHelper.Instance.ExecuteDBs(query);
                    SHOWDGV("", "", "", "");
                }

            }
        }

        private void search_Click(object sender, EventArgs e)
        {
            string huongnc, tinhtrang, gvhd;
            if ("All".CompareTo(cbbhuongnc.SelectedItem.ToString()) == 0)
            {
                huongnc = "";
            }
            else huongnc = cbbhuongnc.Text.ToString();
            if (cbbtinhtrang.SelectedIndex == 0)
            {
                tinhtrang = "";
            }
            else if (cbbtinhtrang.SelectedIndex == 1)
            {
                tinhtrang = "True";
            }
            else tinhtrang = "False";
            if (cbbgvhd.SelectedIndex == 0)
            {
                gvhd = "";
            }
            else gvhd = cbbgvhd.Text.ToString();
            string txt = txtsearch.Text.ToString();
            SHOWDGV(huongnc, tinhtrang, gvhd, txt);
        }
    }
}
