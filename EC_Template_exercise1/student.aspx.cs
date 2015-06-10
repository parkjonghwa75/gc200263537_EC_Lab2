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
    public partial class student : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if save wasn't click AND we have a studentID in the url
            if (!IsPostBack && (Request.QueryString.Count > 0))
            {
                GetStudent();
            }
        }

        protected void GetStudent()
        {
            //populate form with existing student record
            Int32 StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);

            //connect to db via EF
            using (EnterpriseComputingEntities db = new EnterpriseComputingEntities())
            {
                //populate a student instance with the student ID from the url parameter
                Student s = (from objS in db.Students
                             where objS.StudentID == StudentID
                             select objS).FirstOrDefault();

                //map the student to the form controls when s found
                if (s != null)
                {
                    txtLastName.Text = s.LastName;
                    txtFirstName.Text = s.FirstMidName;
                    txtEnrollement.Text = s.EnrollmentDate.ToShortDateString();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // use EF to connect to SQL
            using (EnterpriseComputingEntities db = new EnterpriseComputingEntities())
            {
                //use the student model to save the new model
                Student s = new Student();
                Int32 StudentID=0;

                //check save? or update??
                if (Request.QueryString["StudentID"] !=null)
                {
                    //get the id from the url
                    StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);
                    //get the current student from EF   
                    s = (from objS in db.Students
                         where objS.StudentID == StudentID
                         select objS).FirstOrDefault();
                }

                s.LastName = txtLastName.Text;
                s.FirstMidName = txtFirstName.Text;
                s.EnrollmentDate = Convert.ToDateTime(txtEnrollement.Text);

                if (StudentID == 0)
                {
                    db.Students.Add(s);
                }

                db.SaveChanges();

                //redirect to the updated list page
                Response.Redirect("students.aspx");

            }
        }
    }
}