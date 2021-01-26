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
    public class ProductController : ApiController
    {
        private readonly GeneralStoreDbContext _product = new GeneralStoreDbContext();

        //Post
        //api/product
        [HttpPost]
        public async Task<IHttpActionResult> PostProduct([FromBody] Product model)
        {

            if (model is null)
            {
                return BadRequest("Your request body cannot be empty");
            }
            if (ModelState.IsValid)
            {
                //Store the model in datebase
                _product.Products.Add(model);
                int changeCount = await _product.SaveChangesAsync();

                return Ok("The product was added successfully!");
            }

            //the model is not valid
            return BadRequest(ModelState);
        }

        //Get All
        //api/customer
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Product> product = await _product.Products.ToListAsync();
            return Ok(product);
        }

        //Get by ID
        //api/product/(id)
        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] string sku)
        {
            Product product = await _product.Products.FindAsync(sku);

            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();
        }

        //Put (Update)
        //api/product/(id)
        [HttpPut]
        public async Task<IHttpActionResult> UpdateProduct([FromUri] string sku, [FromBody] Product updatedProduct)
        {
            //Check the ids if the match
            if (sku != updatedProduct?.SKU)
            {
                return BadRequest("SKUs do not match.");
            }
            //Check model state
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Find restaurant in database
            Product product = await _product.Products.FindAsync(sku);

            //if it doesnt match the do something
            if (product is null)
                return NotFound();
            //Update the properties
            product.Name = updatedProduct.Name;
            product.Cost = updatedProduct.Cost;
            product.NumberInInventory = updatedProduct.NumberInInventory;


            //Save changes
            await _product.SaveChangesAsync();

            return Ok("The product was updated!");
        }

        //Delete
        //api/product/{id}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProduct([FromUri] string sku)
        {
            Product product = await _product.Products.FindAsync(sku);

            if (product is null)
                return NotFound();

            _product.Products.Remove(product);

            if (await _product.SaveChangesAsync() == 1)
            {
                return Ok("The product was deleted.");
            }
            return InternalServerError();

        }
    }
}
