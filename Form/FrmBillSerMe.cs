﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Petshop
{
    public partial class FrmBillSerMe : Form
    {
        
        private MySQLDBConnect iConnect;
        public FrmBillSerMe()
        {
            InitializeComponent();
            iConnect = new MySQLDBConnect();
        }
        private void bt_Load_Click(object sender, EventArgs e)
        {
            loadEmployee();
            loadCompany();
            loadData();
        }
        private void FrmMM341_Load(object sender, EventArgs e)
        {
            loadCompany();
            loadEmployee();
            loadMedi();
            loadService();

            loadData();
        }
        private void loadData()
        {
            loadBill();
            loadBillAmt();
            calculator();
            loadMedi();
            loadService();
        }
        decimal iSerPrice = 0;
        decimal iMePrice = 0;
        private void loadBillAmt()
        {
            string ilbBillID = Lb_BillID.Text.Trim();
            DataTable idtSumService;
            string isqlSumService = "SELECT sum(service_Bill_Amt) as ServiceAmt FROM tb_servicebill where Bill_ID = '" + ilbBillID + "'";
            idtSumService = iConnect.SelectByCommand(isqlSumService);
            foreach (DataRow row in idtSumService.Rows)
            {
                object value = row["ServiceAmt"];
                if (value == DBNull.Value)
                {
                    iSerPrice = 0;
                }      
                else
                {
                    iSerPrice = idtSumService.Rows[0].Field<decimal>(0);
                }
            }
            DataTable idtSumMedi;
            string isqlSumMedi = "SELECT sum(Medi_Bill_Amt) as MediAmt FROM tb_medibill where Bill_ID = '" + ilbBillID + "'";
            idtSumMedi = iConnect.SelectByCommand(isqlSumMedi);
            foreach (DataRow row in idtSumMedi.Rows)
            {
                object value = row["MediAmt"];
                if (value == DBNull.Value)
                {
                    iMePrice = 0;
                }
                 else
                {
                    iMePrice =idtSumMedi.Rows[0].Field<decimal>(0);
                }
            }
     
        }
        private void loadBill()
        {
            string iReferID = txb_ReferID.Text;
            DataTable idtBill;
            string isqlBill = "SELECT * FROM petshop.tb_bill where Refer_ID ='"+iReferID+"'";
            idtBill = iConnect.SelectByCommand(isqlBill);
            if (idtBill.Rows.Count > 0)
            {
                Lb_BillID.Text = idtBill.Rows[0].Field<string>(0);
            }
            
        }        
        private void calculator()
        {
            decimal iTotal = iMePrice + iSerPrice;
            txb_BillTotal.Text = iTotal.ToString();

            decimal iDC = 0;
            decimal iNet = 0;
            if ((txb_BillTotal.Text != null) && (txb_BillTotal.Text != ""))
            {
                iTotal = Convert.ToDecimal(txb_BillTotal.Text);
            }
            if ((txb_BillDC.Text != null) && (txb_BillDC.Text != ""))
            {
                iDC = Convert.ToDecimal(txb_BillDC.Text);
            }
            if ((txb_BillNet.Text != null) && (txb_BillNet.Text != ""))
            {
                iNet = Convert.ToDecimal(txb_BillNet.Text);
            }
            iNet = iTotal - iDC;
            txb_BillNet.Text = iNet.ToString();
        }

        private void loadService()
        {
            DataTable idtService;
            string itxbReferID = txb_ReferID.Text.Trim();
            string isqlService = "SELECT tb_servicerecord.*,tb_service.Service_Des FROM tb_servicerecord,tb_service where tb_servicerecord.Service_ID = tb_service.Service_ID AND tb_Servicerecord.HealRecord_ID ='" + itxbReferID + "'";
            idtService = iConnect.SelectByCommand(isqlService);
            dGV_Service.DataSource = idtService;
            dGV_Service.Refresh();
        }

        private void loadMedi()
        {
            DataTable idtMedi;
            string itxbReferID = txb_ReferID.Text.Trim();
            string isqlMedi = "SELECT tb_medirecord.*,tb_medicine.Medi_Des FROM tb_medirecord,tb_medicine where tb_medirecord.Medi_ID = tb_medicine.Medi_ID AND tb_medirecord.HealRecord_ID ='" + itxbReferID + "'";
            idtMedi = iConnect.SelectByCommand(isqlMedi);
            dGV_Medi.DataSource = idtMedi;
            dGV_Medi.Refresh();
        }
        private void loadEmployee() //โหลดข้อมูลเจ้าหน้าที่
        {
            DataTable idtEmployee;
            string isqlEmployee = "SELECT * FROM `tb_employee` where Em_Status = 1"; //โหลดรายชื่อ เจ้าหน้าที่
            idtEmployee = iConnect.SelectByCommand(isqlEmployee);
            if ((idtEmployee != null) && (idtEmployee.Rows.Count > 0))
            {
                cb_Em.DisplayMember = idtEmployee.Columns["Em_Name"].ColumnName;
                cb_Em.ValueMember = idtEmployee.Columns["Em_ID"].ColumnName;
                cb_Em.DataSource = idtEmployee;
            }
        }
        

        private void loadCompany()
        {
            DataTable idtCompany;
            string isqlCompany = "SELECT * FROM `tb_company`";
            idtCompany = iConnect.SelectByCommand(isqlCompany);
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("th-TH");
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
            lbYear.Text = DateTime.Now.ToString("yy");
            Lb_CoBill.Text = "55";
                if (idtCompany.Rows.Count > 0)
                {
                    
                    Lb_CoBill.Text = idtCompany.Rows[0]["CoBill"].ToString();
                }
        }
        private void CheckBoxService_CheckedChanged(object sender, EventArgs e)
        {
           for (int i = 0; i < dGV_Service.RowCount; i++)
            {
                dGV_Service[0, i].Value = CheckBoxService.Checked;//((CheckBox)dGV_Service.Controls.Find("ccCheckBoxService", true)[0]).Checked;
            }
            dGV_Service.EndEdit();
        }
        private void CheckBoxMedi_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dGV_Medi.RowCount; i++)
            {
                dGV_Medi[0, i].Value = CheckBoxMedi.Checked;//((CheckBox)dGV_Medi.Controls.Find("checkboxHeaderMedi", true)[0]).Checked;
            }
            dGV_Medi.EndEdit();
        }
        

        private void bt_Print_Click(object sender, EventArgs e)
        {
            AddServiceBill();
            BillAdd();
            if ((Lb_BillID.Text != null) && (Lb_BillID.Text != ""))
            {
                foreach (Form form in Application.OpenForms) //คำสั่งห้ามเปิดซ้อนสอง
                {
                    if (form.GetType() == typeof(FrmPreBillSerMe))
                    {
                        form.Activate();
                        return;
                    }
                }
                FrmPreBillSerMe iFrmPreBillSerMe = new FrmPreBillSerMe();
                iFrmPreBillSerMe.MdiParent = MainForm.ActiveForm;
                iFrmPreBillSerMe.Show();
                iFrmPreBillSerMe.lb_BillID.Text = Lb_BillID.Text.Trim();
            }

        }

        private void AddServiceBill()
        {
            string ilbBillID = Lb_BillID.Text.Trim();
            if ((txb_ReferID.Text != null) && (txb_ReferID.Text != ""))
            {
            if ((Lb_BillID.Text != null) && (Lb_BillID.Text != ""))
            {
                
                   for (int i = 0; dGV_Service.Rows.Count > i; i++)
                        {
                            if (dGV_Service.Rows[i].Cells[0].Value != null)
                            {
                                String iServiceID = dGV_Service.Rows[i].Cells[2].Value.ToString();
                                String iServicePrice = dGV_Service.Rows[i].Cells[3].Value.ToString();
                                DataTable idtServiceBilCheck;
                                string isqlServiceBillCheck = "SELECT * FROM tb_servicebill where Bill_ID = '" + ilbBillID + "'AND Service_ID = '" + iServiceID + "' ";
                                idtServiceBilCheck = iConnect.SelectByCommand(isqlServiceBillCheck);//ลักษณะการทำงานคือ เมื่อเข้าไปเช็คในฐานข้อมูล หากมี ให้ทำการดอรป์ออก
                                foreach (DataRow row in idtServiceBilCheck.Rows)
                                {
                                    object value = row["Service_ID"];
                                    if (value == DBNull.Value)
                                    {

                                    }
                                    else
                                    {
                                        string isqlServiceBill = "DELETE FROM `tb_servicebill` WHERE `Bill_ID`='" + ilbBillID + "' and`Service_ID`='" + iServiceID + "'";
                                        iConnect.Insert(isqlServiceBill);
                                    }
                                }
                             /////////////////////////////////
                            }
                            if (Convert.ToBoolean(dGV_Service.Rows[i].Cells[0].Value) == false)
                            {//ถ้าไม่เช็ค
                                String iServiceID = dGV_Service.Rows[i].Cells[2].Value.ToString();
                                String iServicePrice = dGV_Service.Rows[i].Cells[3].Value.ToString();
                                decimal iPrice = Convert.ToDecimal(iServicePrice);
                                DataTable idtServiceBilCheck;
                                string isqlServiceBillCheck = "SELECT * FROM tb_servicebill where Bill_ID = '" + ilbBillID + "'AND Service_ID = '" + iServiceID + "' ";
                                idtServiceBilCheck = iConnect.SelectByCommand(isqlServiceBillCheck);//ลักษณะการทำงานคือ เมื่อเข้าไปเช็คในฐานข้อมูล หากไม่มี ให้ทำการเพิ่ม หากมีอยู่แล้ว ให้ทำการ แก้ไขให้ตรงกับปัจจุบัน

                                if (idtServiceBilCheck.Rows.Count == 0)
                                {
                                    string isqlServiceBill = "INSERT INTO `tb_servicebill` (`Bill_ID`, `Service_ID`, `Service_Bill_Unit`, `Service_Bill_Price`, `Service_Bill_Amt`) VALUES ('" + ilbBillID + "', '" + iServiceID + "', '1', '" + iServicePrice + "', '" + iServicePrice + "')";
                                    iConnect.Insert(isqlServiceBill);
                                }
                            }
                            //เรียกผลรวมมาแสดง
                        }
                        ///////////////////////////////////////////MediBill////////////////////////////////////////////////////
                        for (int i = 0; dGV_Medi.Rows.Count > i; i++)
                        {
                            if (dGV_Medi.Rows[i].Cells[0].Value != null)
                            {
                                String iMediID = dGV_Medi.Rows[i].Cells[2].Value.ToString();
                                String iMediUnit = dGV_Medi.Rows[i].Cells[3].Value.ToString();
                                String iMediPrice = dGV_Medi.Rows[i].Cells[4].Value.ToString();
                                String iMediAmt = dGV_Medi.Rows[i].Cells[5].Value.ToString();
                                DataTable idtMediBillCheck;
                                string isqlMediBillCheck = "SELECT * FROM tb_medibill where Bill_ID = '" + ilbBillID + "'AND Medi_ID = '" + iMediID + "' ";
                                idtMediBillCheck = iConnect.SelectByCommand(isqlMediBillCheck);
                                foreach (DataRow row in idtMediBillCheck.Rows)
                                {
                                    object value = row["Medi_ID"];
                                    if (value == DBNull.Value)
                                    {

                                    }
                                    else
                                    {
                                        string isqlMediBill = "DELETE FROM `petshop`.`tb_medibill` WHERE `Bill_ID`='" + ilbBillID + "' and`Medi_ID`='" + iMediID + "'";
                                        iConnect.Insert(isqlMediBill);
                                    }
                                }  
                            }
                            if (Convert.ToBoolean(dGV_Medi.Rows[i].Cells[0].Value) == false)
                            {// ถ้าไม่เช็ค
                                String iMediID = dGV_Medi.Rows[i].Cells[2].Value.ToString();
                                String iMediUnit = dGV_Medi.Rows[i].Cells[3].Value.ToString();
                                String iMediPrice = dGV_Medi.Rows[i].Cells[4].Value.ToString();
                                String iMediAmt = dGV_Medi.Rows[i].Cells[5].Value.ToString();
                                DataTable idtMediBillCheck;
                                string isqlMediBillCheck = "SELECT * FROM tb_medibill where Bill_ID = '" + ilbBillID + "'AND Medi_ID = '" + iMediID + "' ";
                                idtMediBillCheck = iConnect.SelectByCommand(isqlMediBillCheck);
                                if (idtMediBillCheck.Rows.Count == 0)
                                {
                                    string isqlMediBill = "INSERT INTO `petshop`.`tb_medibill` (`Bill_ID`, `Medi_ID`, `Medi_Bill_Unit`, `Medi_Bill_Price`, `Medi_Bill_Amt`) VALUES ('" + ilbBillID + "', '" + iMediID + "', '" + iMediUnit + "', '" + iMediPrice + "', '" + iMediAmt + "')";
                                    iConnect.Insert(isqlMediBill);
                                }
                             }
                            //เรียกผลรวมมาแสดง
                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////////////

                    }
                else {
                BillAdd();
                    }
                }
            loadData();
            }
            

        private void BillAdd()
        {       //สร้างเลขที่ใบเสร็จใหม่
                string ilbyear = lbYear.Text.Trim(); //Year 
                string ilbBill = Lb_CoBill.Text.Trim(); //Bill Conpany
                string itxbReferID = txb_ReferID.Text.Trim(); // HealRecord ID
                string icbEmplyee = cb_Em.SelectedValue.ToString();

                string iBillTotal = txb_BillTotal.Text.Trim();
                string iBillDC = txb_BillDC.Text.Trim();
                string iBillNet = txb_BillNet.Text.Trim();
                string ilbBillID = Lb_BillID.Text.Trim();
                System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
                System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
                System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
                string idTPProductSaleDate = dTP_ProductSaleDate.Value.ToString("yyyy-MM-dd");
            if ((Lb_BillID.Text == null) || (Lb_BillID.Text == ""))
            {
                string isqlAddBill = "INSERT INTO `tb_bill` (`Bill_ID`, `Refer_ID`, `Em_ID`, `Bill_Date`, `BillSale_Total`, `BillSale_DC`, `BillSale_Net`) VALUES (CONCAT('" + ilbyear + ilbBill + "', LPAD(  IFNULL( (SELECT SUBSTR(`Bill_ID`, 5) FROM `tb_Bill` AS `alias` WHERE SUBSTR(`Bill_ID`, 1, 2) = ('" + ilbyear + "')  ORDER BY `Bill_ID` DESC LIMIT 1 ) + 1, 1 ),  5, '0' )), '" + itxbReferID + "', '" + icbEmplyee + "', '" + idTPProductSaleDate + "', '" + iBillTotal + "', '" + iBillDC + "', '" + iBillNet + "')";
                iConnect.Insert(isqlAddBill);
                DataTable idtBillID;
                string isqlBillID = "SELECT Bill_ID FROM tb_Bill WHERE SUBSTR(`Bill_ID`, 1, 2) = ('" + ilbyear + "')  ORDER BY `Bill_ID` DESC LIMIT 1";
                idtBillID = iConnect.SelectByCommand(isqlBillID);
                Lb_BillID.Text = idtBillID.Rows[0].Field<string>(0);
            }
            else
            {
                //UPdateBill
                string isqlBillUpdate = "UPDATE `tb_bill` SET `Em_ID`='" + icbEmplyee + "', `Bill_Date`='" + idTPProductSaleDate + "', `BillSale_Total`='" + iBillTotal + "', `BillSale_DC`='" + iBillDC + "', `BillSale_Net`='" + iBillNet + "' WHERE `Bill_ID`='" + ilbBillID + "' and`Refer_ID`='"+itxbReferID+"'";
                iConnect.Insert(isqlBillUpdate);
            }
            loadData();
        }

        private void txb_ReferID_TextChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void bt_AddBill_Click(object sender, EventArgs e)
        {
            AddServiceBill();
            BillAdd();
        }

        private void txb_BillDC_TextChanged(object sender, EventArgs e)
        {
            calculator();
        }

        private void Lb_BillID_TextChanged(object sender, EventArgs e)
        {
            loadBill();
            if ((Lb_BillID.Text !="")&&(Lb_BillID.Text != null))
            {
                TabControlServiceMediBill.Enabled = true;
                dGV_Medi.Enabled = true;
                dGV_Service.Enabled = true;
                bt_Print.Enabled = true;
                txb_BillDC.Enabled = true;
            }
            else
            {
                TabControlServiceMediBill.Enabled = false;
                dGV_Medi.Enabled = false;
                dGV_Service.Enabled = false;
                bt_Print.Enabled = false;
                txb_BillDC.Enabled = false;
            }
        }
        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            loadEmployee();
            loadCompany();
            loadData();
        }


    }
}
