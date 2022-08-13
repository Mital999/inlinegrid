using PracticalTest_Crud.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracticalTest_Crud.Controllers
{
    public class EmployeeMasterController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                List<EmployeeMaster> employeeMaster = new List<EmployeeMaster>();
                string query = "select EmployeeMaster.EmployeeId,EmployeeMaster.EmployeeRollNo,EmployeeMaster.EmpFirstName,EmployeeMaster.EmpMiddleName,EmployeeMaster.EmpLastName,EmployeeMaster.DOB,EmployeeMaster.Address,EmployeeMaster.Department,EmployeeMaster.Designation,CountryMaster.CountryName,StateMaster.StateName,CityMaster.CityName from EmployeeMaster Inner join CountryMaster on EmployeeMaster.CountryId = CountryMaster.CountryId Inner join StateMaster on EmployeeMaster.StateId = StateMaster.StateId Inner join CityMaster on EmployeeMaster.CityId = CityMaster.CityId";
                string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                employeeMaster.Add(new EmployeeMaster
                                {
                                    EmployeeId = Convert.ToInt32(sdr["EmployeeId"]),
                                    EmployeeRollNo = Convert.ToString(sdr["EmployeeRollNo"]),
                                    EmpFirstName = Convert.ToString(sdr["EmpFirstName"]),
                                    EmpMiddleName = Convert.ToString(sdr["EmpMiddleName"]),
                                    EmpLastName = Convert.ToString(sdr["EmpLastName"]),
                                    Department = Convert.ToString(sdr["Department"]),
                                    Designation = Convert.ToString(sdr["Designation"]),
                                    Address = Convert.ToString(sdr["Address"]),
                                    DOB = Convert.ToDateTime(sdr["DOB"]),
                                    CountryName = Convert.ToString(sdr["CountryName"]),
                                    StateName = Convert.ToString(sdr["StateName"]),
                                    CityName = Convert.ToString(sdr["CityName"])
                                });
                            }
                        }
                        con.Close();
                    }
                }
                if (employeeMaster.Count == 0)
                {
                    employeeMaster.Add(new EmployeeMaster());
                }
                return View(employeeMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static List<ListItem> PopulateItems(string query)
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            List<ListItem> items = new List<ListItem>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while(sdr.Read())
                        {
                            items.Add(new ListItem
                            {
                                Text = sdr[0].ToString(),
                                Value = Convert.ToInt32(sdr[1]),
                                ParentId = Convert.ToInt32(sdr[2])
                            });
                        }
                    }
                        con.Close();
                }
            }
            return items;
        }
        [HttpPost]
        public ActionResult State(string CountryId)
        {
            if(CountryId != null)
            {
                var query = "select StateName,StateId,CountryId,0 from StateMaster where CountryId=" + CountryId;
                var items = PopulateItems(query);
                return Json(new { Data = items, Status = true });
            }
            return Json(new { Data = "", Status = false });
        }
        [HttpPost]
        public ActionResult City(string StateId)
        {
            if (StateId != null)
            {
                var query = "select CityName,CityId,StateId,0 from CityMaster where StateId=" + StateId;
                var items = PopulateItems(query);
                return Json(new { Data = items, Status = true });
            }
            return Json(new { Data = "", Status = false });
        }
        [HttpPost]
        public ActionResult Country()
        {
            var query = PopulateItems("select CountryName,CountryId,0 from CountryMaster");
            return Json(query);
        }
        
        [HttpPost]
        public ActionResult InsertEmployee(EmployeeMaster model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("Insert into EmployeeMaster values(@EmployeeRollNo,@EmpFirstName,@EmpMiddleName,@EmpLastName,@DOB,@Address,@Department,@Designation,@CountryId,@StateId,@CityId)", con))
                        {
                            cmd.Parameters.AddWithValue("@EmployeeRollNo", model.EmployeeRollNo);
                            cmd.Parameters.AddWithValue("@EmpFirstName", model.EmpFirstName);
                            cmd.Parameters.AddWithValue("@EmpMiddleName", model.EmpMiddleName);
                            cmd.Parameters.AddWithValue("@EmpLastName", model.EmpLastName);
                            cmd.Parameters.AddWithValue("@DOB", model.DOB);
                            cmd.Parameters.AddWithValue("@Address", model.Address);
                            cmd.Parameters.AddWithValue("@Department", model.Department);
                            cmd.Parameters.AddWithValue("@Designation", model.Designation);
                            cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
                            cmd.Parameters.AddWithValue("@StateId", model.StateId);
                            cmd.Parameters.AddWithValue("@CityId", model.CityId);
                            con.Open();
                            model.EmployeeId = Convert.ToInt32(cmd.ExecuteScalar());
                            con.Close();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return View(model);
            }
            return Json("Record Inserted");
        }
        [HttpPost]
        public ActionResult DeleteEmployee(int EmployeeId)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("delete from EmployeeMaster where EmployeeId=@EmployeeId", con))
                        {
                            cmd.Parameters.AddWithValue("@EmployeeId", EmployeeId);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
                return View();
        }
        [HttpPost]
        public JsonResult UpdateEmployee(string EmployeeId,string EmployeeRollNo,string EmpFirstName,string EmpMiddleName,string EmpLastName,string Address,string Department,string Designation, string DOB)
       
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Update EmployeeMaster set EmployeeRollNo=@EmployeeRollNo,EmpFirstName=@EmpFirstName,EmpMiddleName=@EmpMiddleName,EmpLastName=@EmpLastName,Address=@Address,Department=@Department,Designation=@Designation,DOB=@DOB where EmployeeId=@EmployeeId"))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", Convert.ToInt32(EmployeeId));
                    cmd.Parameters.AddWithValue("@EmployeeRollNo", EmployeeRollNo);
                    cmd.Parameters.AddWithValue("@EmpFirstName", EmpFirstName);
                    cmd.Parameters.AddWithValue("@EmpMiddleName", EmpMiddleName);
                    cmd.Parameters.AddWithValue("@EmpLastName", EmpLastName);
                    cmd.Parameters.AddWithValue("@DOB",DOB);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@Department", Department);
                    cmd.Parameters.AddWithValue("@Designation", Designation);
                    con.Open();
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
                return Json("record updated");
        }
        public class ListItem
        {
            public int Value { get; set; }
            public string Text { get; set; }
            public int ParentId { get; set; }
        }
    }
}