using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace EmployeePayrollProblem_RESTSharp
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            //Initialize the base URL to execute requests made by the instance
            client = new RestClient("http://localhost:3000");
        }
        private IRestResponse GetEmployeeList()
        {
            //Arrange
            //Initialize the request object with proper method and URL
            RestRequest request = new RestRequest("/Employees", Method.GET);
            //Act
            // Execute the request
            IRestResponse response = client.Execute(request);
            return response;
        }


        /* UC1:- Ability to Retrieve all Employees in EmployeePayroll JSON Server.
                 - Use JSON Server and RESTSharp to save the EmployeePayroll Data of id, name, and salary.
                 - Retrieve in the MSTest Test and corresponding update the Memory with the Data.
        */

        [TestMethod]
        public void OnCallingGetAPI_ReturnEmployeeList()
        {
            IRestResponse response = GetEmployeeList();
            // Check if the status code of response equals the default code for the method requested
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // Convert the response object to list of employees
            List<EmployeeModel> employeeList = JsonConvert.DeserializeObject<List<EmployeeModel>>(response.Content);
            Assert.AreEqual(5, employeeList.Count);
            foreach (EmployeeModel emp in employeeList)
            {
                Console.WriteLine("Id:- " + emp.Id + "\t" + "Name:- " + emp.Name + "\t" + "Salary:- " + emp.Salary);
            }
        }



        /* UC2:- Ability to add a new Employee to the EmployeePayroll JSON Server.
                 - Use JSON Server and RESTSharp to save the EmployeePayroll Data of id, name, and salary.
                 - Ability to add using RESTSharp to JSONServer in the MSTest Test Case and then on success add to Employee Payroll .
                 - Validate with the successful Count 
        */

        [TestMethod]
        public void OnCallingPostAPI_ReturnEmployeeObject()
        {
            //Arrange
            ///Initialize the request for POST to add new employee
            RestRequest request = new RestRequest("/Employees", Method.POST);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("name", "Jimmy");
            jsonObj.Add("salary", "150000");
            jsonObj.Add("id", "9");
            ///Added parameters to the request object such as the content-type and attaching the jsonObj with the request
            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            EmployeeModel employee = JsonConvert.DeserializeObject<EmployeeModel>(response.Content);
            Assert.AreEqual("Jimmy", employee.Name);
            Assert.AreEqual("150000", employee.Salary);
            Console.WriteLine(response.Content);
        }
    }
}

