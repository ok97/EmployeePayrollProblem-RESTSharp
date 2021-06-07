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
            // Arrange
            // Initialize the request object with proper method and URL
            RestRequest request = new RestRequest("/Employees", Method.GET);
            // Act
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
            Assert.AreEqual(2, employeeList.Count);
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
            // Arrange
            // Initialize the request for POST to add new employee
            RestRequest request = new RestRequest("/Employees", Method.POST);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("id", 3);
            jsonObj.Add("name", "Jimmy");
            jsonObj.Add("salary", "150000");
            
            // Added parameters to the request object such as the content-type and attaching the jsonObj with the request
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

        /*UC3:- Ability to add multiple Employee to  the EmployeePayroll JSON Server.
                - Use JSON Server and RESTSharp to add  multiple Employees to Payroll
                - Ability to add using RESTSharp to  JSONServer in the MSTest Test Case and  then on success add to  EmployeePayrollService
                - Validate with the successful Count
        */
        
        [TestMethod]
        public void OnCallingPostAPIForAEmployeeListWithMultipleEMployees_ReturnEmployeeObject()
        {
            // Arrange
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            employeeList.Add(new EmployeeModel { Name = "Sham", Salary = "9876541" });
            employeeList.Add(new EmployeeModel { Name = "Ramrao", Salary = "6543210" });
            employeeList.Add(new EmployeeModel { Name = "Meera", Salary = "123456" });
            // Iterate the loop for each employee
            foreach (var emp in employeeList)
            {
                // Initialize the request for POST to add new employee
                RestRequest request = new RestRequest("/Employees", Method.POST);
                JsonObject jsonObj = new JsonObject();
                jsonObj.Add("Name", emp.Name);
                jsonObj.Add("Salary", emp.Salary);
                // Added parameters to the request object such as the content-type and attaching the jsonObj with the request
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

                //Act
                IRestResponse response = client.Execute(request);

                //Assert
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                EmployeeModel employee = JsonConvert.DeserializeObject<EmployeeModel>(response.Content);
                Assert.AreEqual(emp.Name, employee.Name);
                Assert.AreEqual(emp.Salary, employee.Salary);
                Console.WriteLine(response.Content);
            }
        }
        /*UC4:- Ability to Update Salary in Employee Payroll JSON Server.
                - Firstly Update the Salary in Memory.
                - Post that Use JSON Server and RESTSharp to Update the salary.
        */
        [TestMethod]
        public void OnCallingPutAPI_ReturnEmployeeObject()
        {
            // Arrange
            // Initialize the request for PUT to add new employee
            RestRequest request = new RestRequest("/Employees/4", Method.PUT);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("Name", "Radha");
            jsonObj.Add("Salary", "65000");
            // Added parameters to the request object such as the content-type and attaching the jsonObj with the request
            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

            // Act
            IRestResponse response = client.Execute(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            EmployeeModel employee = JsonConvert.DeserializeObject<EmployeeModel>(response.Content);
            Assert.AreEqual("Radha", employee.Name);
            Assert.AreEqual("65000", employee.Salary);
            Console.WriteLine(response.Content);
        }


        /*UC5:- Ability to Delete Employee from Employee Payroll JSON Server.
                - Use JSON Server and RESTSharp to then delete the employee by ID.
                - Delete the Employee from the Memory.
        */

        [TestMethod]
        public void OnCallingDeleteAPI_ReturnSuccessStatus()
        {
            // Arrange
            // Initialize the request for PUT to add new employee
            RestRequest request = new RestRequest("/Employees/5", Method.DELETE);

            // Act
            IRestResponse response = client.Execute(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(response.Content);
        }
    }
}

