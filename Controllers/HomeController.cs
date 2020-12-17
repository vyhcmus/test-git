using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Data;
using ProjectLab04.Models;

namespace ProjectLab04.Controllers
{
    public class HomeController : Controller
    {
        private List<EmployeesModel> EmployeesList = new List<EmployeesModel>();
        string xmlFilePath = "~/XMLfile/EmployeesData.xml";
        // GET: Home

        public List<EmployeesModel> ReadXMLData(string filepath)
        {
            //get file path from server
            string xmldata = Server.MapPath(filepath);
            DataSet ds = new DataSet();
            //read the xml data from file using dataset
            ds.ReadXml(xmldata);
            //get data from dataset, convert and store it in list
            var employeeslist = new List<EmployeesModel>();
            employeeslist = (from rows in ds.Tables[0].AsEnumerable()
                            select new EmployeesModel
                            {
                                EmployeeID = Convert.ToInt32(rows[0].ToString()),
                                EmployeeName = rows[1].ToString(),
                                Email = rows[2].ToString(),
                                Birthday = Convert.ToDateTime(rows[3].ToString()),
                                Age = Convert.ToInt32(rows[4].ToString()),
                                Type = rows[5].ToString()
                            }).ToList();
            return employeeslist;
        }

        public void CreateXMLFile(string filepath, List<EmployeesModel> xmldata)
        {
            try
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "\t",
                    NewLineOnAttributes = true
                };
                using (XmlWriter xmlwriter = XmlWriter.Create(Server.MapPath(filepath), xmlWriterSettings))
                {
                    xmlwriter.WriteStartDocument();
                    xmlwriter.WriteStartElement("Customer");
                    foreach (EmployeesModel i in xmldata)
                    {
                        xmlwriter.WriteStartElement("Employee");
                        xmlwriter.WriteElementString("EmployeeID", i.EmployeeID.ToString());
                        xmlwriter.WriteElementString("EmployeeName", i.EmployeeName);
                        xmlwriter.WriteElementString("Email", i.Email.ToString());
                        xmlwriter.WriteElementString("Birthday", i.Birthday.ToString());
                        xmlwriter.WriteElementString("Age", i.Age.ToString());
                        xmlwriter.WriteElementString("Type", i.Type.ToString());
                        xmlwriter.WriteEndElement();
                    }
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndDocument();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Exception of type: " + ex + "occured please try again";
            }
        }
        public ActionResult Index(FormCollection searchdata)
        {
            ViewBag.Msg = "Employees XML Data";
            //get data from source
            EmployeesList = ReadXMLData(xmlFilePath);

            if (searchdata["EmpNameSearch"] != null && searchdata["EmpEmailSearch"] !=null)
            {
                EmployeesList = EmployeesList.Where(x => x.EmployeeName.Contains(searchdata["EmpNameSearch"]) 
                && x.Email.Contains(searchdata["EmpEmailSearch"])).ToList();
            }

            return View(EmployeesList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(EmployeesModel Emp)
        {
            if (ModelState.IsValid)
            {
                EmployeesList = ReadXMLData(xmlFilePath);
                EmployeesList.Add(Emp);
                CreateXMLFile(xmlFilePath, EmployeesList);
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Edit(int EmpId)
        {
            EmployeesList = ReadXMLData(xmlFilePath);
            int index = EmployeesList.FindIndex(x => x.EmployeeID == EmpId);
            EmployeesModel emp = EmployeesList[index];
            return View(emp);
        }

        [HttpPost]
        public ActionResult Edit(EmployeesModel emp)
        {
            if (ModelState.IsValid)
            {
               EmployeesList = ReadXMLData(xmlFilePath);
                int index = EmployeesList.FindIndex(x => x.EmployeeID == emp.EmployeeID);
                EmployeesList[index] = emp;
                CreateXMLFile(xmlFilePath, EmployeesList);
                return RedirectToAction("Index");
            }

            return View(emp);
        }
        public ActionResult Details(int EmpId)
        {
            EmployeesList = ReadXMLData(xmlFilePath);
            int index = EmployeesList.FindIndex(x => x.EmployeeID == EmpId);
            EmployeesModel emp = EmployeesList[index];
            return View(emp);
        }

        [HttpPost]
        public ActionResult Details(EmployeesModel emp)
        {
            if (ModelState.IsValid)
            {
               EmployeesList = ReadXMLData(xmlFilePath);
                int index = EmployeesList.FindIndex(x => x.EmployeeID == emp.EmployeeID);
                EmployeesList[index] = emp;
                CreateXMLFile(xmlFilePath, EmployeesList);
                return RedirectToAction("Index");
            }

            return View(emp);
        }

        public ActionResult Delete(int EmpId)
        {
            EmployeesList = ReadXMLData(xmlFilePath);
            int index = EmployeesList.FindIndex(x => x.EmployeeID == EmpId);
            EmployeesModel emp = EmployeesList[index];
            return View(emp);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int EmpId)
        {
            if (ModelState.IsValid)
            {
                EmployeesList = ReadXMLData(xmlFilePath);
                int index = EmployeesList.FindIndex(x => x.EmployeeID == EmpId);
               EmployeesList.RemoveAt(index);
                CreateXMLFile(xmlFilePath, EmployeesList);
                return RedirectToAction("Index");
            }

            return View();
        }

    }
}