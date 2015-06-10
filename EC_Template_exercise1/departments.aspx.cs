using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//ref model binding
using EC_Template_exercise1.Models;
using System.Web.ModelBinding;


namespace EC_Template_exercise1
{
    public partial class departments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if loading the page for the first time, populate departments list
            if (!IsPostBack)
            {
                getDepartments();
            }
        }

        protected void getDepartments()
        {
            using (EnterpriseComputingEntities db = new EnterpriseComputingEntities())
            {

                var departments = from objS in db.Departments
                                  select objS;

                //bind the result to the gridview
                grdDepartments.DataSource = departments.ToList();
                grdDepartments.DataBind();
            }

        }

        protected void grdDepartments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {


            //store which row was clicked
            Int32 selectedRow = e.RowIndex;

            //get the selected departmentID using the grid's data key collection
            Int32 departmentID = Convert.ToInt32(grdDepartments.DataKeys[selectedRow].Values["DepartmentID"]);

            //use EF to remove the selected student from the db
            using (EnterpriseComputingEntities db = new EnterpriseComputingEntities())
            {
                Department d = (from objS in db.Departments
                             where objS.DepartmentID == departmentID
                             select objS).FirstOrDefault();

                // do the deletion
                db.Departments.Remove(d);
                db.SaveChanges();
            }

            //refresh the grid
            getDepartments();

        }
    }
}