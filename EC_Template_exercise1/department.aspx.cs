using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using EC_Template_exercise1.Models;
using System.Web.ModelBinding;

namespace EC_Template_exercise1
{
    public partial class department : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if save wasn't click AND we have a studentID in the url
            if (!IsPostBack && (Request.QueryString.Count > 0))
            {
                getDepartment();
            }
        }

        protected void getDepartment()
        {
            //populate form with existing student record
            Int32 DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

            //connect to db via EF
            using (EnterpriseComputingEntities db = new EnterpriseComputingEntities())
            {
                //populate a student instance with the student ID from the url parameter
                Department d = (from objS in db.Departments
                                where objS.DepartmentID == DepartmentID
                             select objS).FirstOrDefault();

                //map the student to the form controls when s found
                if (d != null)
                {
                    txtDeptName.Text = d.Name;
                    txtBudget.Text = Convert.ToString(d.Budget);
      
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // use EF to connect to SQL
            using (EnterpriseComputingEntities db = new EnterpriseComputingEntities())
            {
                //use the student model to save the new model
                Department d = new Department();
                Int32 DepartmentID = 0;

                //check save? or update??
                if (Request.QueryString["DepartmentID"] != null)
                {
                    //get the id from the url
                    DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);
                    //get the current student from EF   
                    d = (from objS in db.Departments
                         where objS.DepartmentID == DepartmentID
                         select objS).FirstOrDefault();
                }

                d.Name = txtDeptName.Text;
                d.Budget = Convert.ToDecimal(txtBudget.Text);

                if (DepartmentID == 0)
                {
                    db.Departments.Add(d);
                }

                db.SaveChanges();

                //redirect to the updated list page
                Response.Redirect("departments.aspx");

            }
        }
    }
}