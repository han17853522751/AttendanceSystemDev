﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

public partial class Admin_LoadExcelToDatabase : System.Web.UI.Page
{
    string currFilePath = string.Empty;  //要导入的excel文件路径--服务器端
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    if (Session["uesrID"].ToString() == "")
        //    {
        //        Response.Redirect("..\\Login.aspx");
        //    }
        //    else
        //    {
        //        btnClearPreData.Attributes.Add("onclick", "return confirm('本操作将清空所有数据表,确定要执行这个操作吗?');");
        //        //btnPreOperation.Attributes.Add("onclick", "return confirm('本操作将颠覆原有数据,确定要执行这个操作吗?')");
        //        //btnTeacherAttendance.Attributes.Add("onclick", "return confirm('本操作覆盖原有数据，确定要执行这个操作吗?')");
        //    }
        //}
    }

    private bool UpLoad(FileUpload fileUpload)
    {
        string fileExt = string.Empty;  //文件扩展名

        if (fileUpload.HasFile)
        {
            fileExt = System.IO.Path.GetExtension(fileUpload.FileName);//获取文件后缀

            if (fileExt == ".xls" || fileExt == ".xlsx")
            {
                try
                {
                    this.currFilePath = Server.MapPath("../") + "TempFile\\" + DateTime.Now.ToString("yyyyMMddhhmmss") + fileExt;//服务器路径
                    fileUpload.SaveAs(this.currFilePath);//上传
                    return true;
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('发生错误：'" + ex.Message.ToString() + ")</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('只允许导入xls、xlsx文件！')</script>");
            }
        }
        else
        {
            Response.Write("<script>alert('没有选择要导入的文件！')</script>");
        }
        return false;
    }

    /*   private DataTable ReadExcelToTable(string filePath)
       {
           string connstring = "Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;";
           using(OleDbConnection conn = new OleDbConnection(connstring))
           {
               conn.Open();
               DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
               string firstSheetName = sheetsName.Rows[0][2].ToString();
               string sql = string.Format("select * from [{0}]", firstSheetName);
               OleDbDataAdapter ada = new OleDbDataAdapter(sql, connstring);
               DataSet set = new DataSet();
               ada.Fill(set);
               return set.Tables[0];

           }
       }*/


    protected void BtnImportTeachers_Click(object sender, EventArgs e)
    {
        //  Clear();
        string identity = "";
        if (rdoTeacher.Checked | rdoOther.Checked)
        {
            if (rdoTeacher.Checked)
                identity = "TabTeachers";
            else
                identity = "TabOtherTeachers";
            if (UpLoad(FileUploadTeacher))
                lblMessage1.Text = ExcelToDatabase.CheckFile(currFilePath, identity);
        }
        else
        {
            // lblMessage1.Text = "请选择导入的数据是本地教师或外聘教师";
            Response.Write("<script>alert('请选择导入的数据是本地教师或外聘教师！')</script>");
        }
    }

    protected void BtnImportCourse_Click(object sender, EventArgs e)
    {
        Clear();
        string department = "";
        department = ddlDepartmentName.SelectedItem.ToString();
        if (UpLoad(FileUploadCourse))
            lblMessage2.Text = ExcelToDatabase.CheckFile(currFilePath, department);
    }



    private void Clear()
    {
        lblMessage1.Text = "";
        lblMessage2.Text = "";
        lblMessage3.Text = "";
        lblMessage4.Text = "";
        lblMessage5.Text = "";
        lblMessage7.Text = "";
    }
}