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
    public class TransactionController : ApiController
    {
        private readonly GeneralStoreDbContext _transaction = new GeneralStoreDbContext();

        //Post
        //api/product
        [HttpPost]
        public async Task<IHttpActionResult> PostTransaction([FromBody] Transaction model)
        {

            if (model is null)
            {
                return BadRequest("Your request body cannot be empty");
            }
            if (ModelState.IsValid)
            {
                //Store the model in datebase
                _transaction.Transactions.Add(model);
                int changeCount = await _transaction.SaveChangesAsync();

                return Ok("The transaction was added successfully!");
            }

            //the model is not valid
            return BadRequest(ModelState);
        }

        //Get All
        //api/transaction
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Transaction> transaction = await _transaction.Transactions.ToListAsync();
            return Ok(transaction);
        }

        //Get by ID
        //api/transaction/(id)
        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            Transaction transaction = await _transaction.Transactions.FindAsync(id);

            if (transaction != null)
            {
                return Ok(transaction);
            }
            return NotFound();
        }

        //Put (Update)
        //api/transaction/(id)
        [HttpPut]
        public async Task<IHttpActionResult> UpdateTransaction([FromUri] int id, [FromBody] Transaction updatedTransaction)
        {
            //Check the ids if the match
            if (id != updatedTransaction?.Id)
            {
                return BadRequest("Ids do not match.");
            }
            //Check model state
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Find restaurant in database
            Transaction transaction = await _transaction.Transactions.FindAsync(id);

            //if it doesnt match the do something
            if (transaction is null)
                return NotFound();
            //Update the properties
            transaction.CustomerId = updatedTransaction.CustomerId;
            transaction.ProductSKU = updatedTransaction.ProductSKU;
            transaction.ItemCount = updatedTransaction.ItemCount;
            transaction.DateOfTransaction = updatedTransaction.DateOfTransaction;


            //Save changes
            await _transaction.SaveChangesAsync();

            return Ok("The customer was updated!");
        }

        //Delete
        //api/product/{id}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProduct([FromUri] int id)
        {
            Transaction transaction = await _transaction.Transactions.FindAsync(id);

            if (transaction is null)
                return NotFound();

            _transaction.Transactions.Remove(transaction);

            if (await _transaction.SaveChangesAsync() == 1)
            {
                return Ok("The transaction was deleted.");
            }
            return InternalServerError();

        }
    }
}
