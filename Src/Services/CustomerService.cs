using System;
using System.Collections.Generic;
using CrashCourse2021ExercisesDayThree.DB.Impl;
using CrashCourse2021ExercisesDayThree.Models;

namespace CrashCourse2021ExercisesDayThree.Services
{
    public class CustomerService
    {
        CustomerTable db; 
        public CustomerService()
        {
            this.db = new CustomerTable();
        }

        //Create and return a Customer Object with all incoming properties (no ID)
        internal Customer Create(string firstName, string lastName, DateTime birthDate)
        {
            Customer customer = new Customer();
            if(firstName.Length<2)
            throw new ArgumentException(Constants.FirstNameException);
            customer.FirstName=firstName;
            customer.LastName=lastName;
            customer.BirthDate=birthDate;
            return customer;
        }

        CustomerTable customerTable = new CustomerTable();

        //db has an Add function to add a new customer!! :D
        //We can reuse the Create function above..
        internal Customer CreateAndAdd(string firstName, string lastName, DateTime birthDate)
        {
            return customerTable.AddCustomer(Create(firstName,lastName,birthDate));
        }

        //Simple enough, Get the customers from the "Database" (db)
        internal List<Customer> GetCustomers()
        {
            return customerTable.GetCustomers();
        }

        //Maybe Check out how to find in a LIST in c# Maybe there is a Find function?
        public Customer FindCustomer(int customerId)
        {
            if(customerId<0)
            throw new System.IO.InvalidDataException(Constants.CustomerIdMustBeAboveZero);
            List<Customer> customers = GetCustomers();
            return customers.Find(c=>c.Id==customerId);


        }
        
        /*So many things can go wrong here...
          You need lots of exceptions handling in case of failure and
          a switch statement that decides what property of customer to use
          depending on the searchField. (ex. case searchfield = "id" we should look in customer.Id 
          Maybe add searchField.ToLower() to avoid upper/lowercase letters)
          Another thing is you should use FindAll here to get all that matches searchfield/searchvalue
          You could also make another search Method that only return One Customer
           and uses Find to get that customer and maybe even test it.
        */
        public List<Customer> SearchCustomer(string searchField, string searchValue)
        {
            if(searchField==null)
            throw new System.IO.InvalidDataException(Constants.CustomerSearchFieldCannotBeNull);

            if(searchField.Trim()=="")
            throw new System.IO.InvalidDataException(Constants.CustomerSearchValueCannotBeNull);

            if(searchValue==null)
            throw new ArgumentException(Constants.CustomerSearchValueCannotBeNull);

            List<Customer> customers = GetCustomers();
            searchField = searchField.ToLower();
            switch(searchField){
                case "firstname" : 
                    return customers.FindAll(c=>c.FirstName.ToLower().Contains(searchValue.ToLower()));
                case "lastname" :
                    return customers.FindAll(c=>c.LastName.ToLower().Contains(searchValue.ToLower()));
                case "id" :
                    int value;
                    if(!int.TryParse(searchValue,out value))
                    throw new System.IO.InvalidDataException(Constants.CustomerSearchValueWithFieldTypeIdMustBeANumber);
                    if(value<=0)
                    throw new System.IO.InvalidDataException(Constants.CustomerIdMustBeAboveZero);
                    return customers.FindAll(c=>c.Id==value);    
                default:
                    throw new System.IO.InvalidDataException(Constants.CustomerSearchFieldNotFound);
            }

        }
    }
}
