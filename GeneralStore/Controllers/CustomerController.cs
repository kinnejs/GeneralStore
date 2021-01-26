using GeneralStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStore.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly GeneralStoreDbContext _customer = new GeneralStoreDbContext();

        //Post
        //api/customer
        [HttpPost]
        public async Task<IHttpActionResult> PostCustomer([FromBody] Customer model)
        {

            if (model is null)
            {
                return BadRequest("Your request body cannot be empty");
            }
            if (ModelState.IsValid)
            {
                //Store the model in datebase
                _customer.Customers.Add(model);
                int changeCount = await _customer.SaveChangesAsync();

                return Ok("The customer was added successfully!");
            }

            //the model is not valid
            return BadRequest(ModelState);
        }

        //Get All
        //api/customer
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Customer> restaurants = await _customer.Customers.ToListAsync();
            return Ok(restaurants);
        }

        //Get by ID
        //api/Customer/(id)
        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            Customer customer  = await _customer.Customers.FindAsync(id);

            if (customer != null)
            {
                return Ok(customer);
            }
            return NotFound();
        }

        //Put (Update)
        //api/Restaurant/(id)
        [HttpPut]
        public async Task<IHttpActionResult> UpdateCustomer([FromUri] int id, [FromBody] Customer updatedCustomer)
        {
            //Check the ids if the match
            if (id != updatedCustomer?.Id)
            {
                return BadRequest("Ids do not match.");
            }
            //Check model state
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Find restaurant in database
            Customer customer = await _customer.Customers.FindAsync(id);

            //if it doesnt match the do something
            if (customer is null)
                return NotFound();
            //Update the properties
            customer.FirstName = updatedCustomer.FirstName;
            customer.LastName = updatedCustomer.LastName;


            //Save changes
            await _customer.SaveChangesAsync();

            return Ok("The customer was updated!");
        }

        //Delete
        //api/customer/{id}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCustomer([FromUri] int id)
        {
            Customer customer = await _customer.Customers.FindAsync(id);

            if (customer is null)
                return NotFound();

            _customer.Customers.Remove(customer);

            if (await _customer.SaveChangesAsync() == 1)
            {
                return Ok("The customer was deleted.");
            }
            return InternalServerError();

        }
    }
}
