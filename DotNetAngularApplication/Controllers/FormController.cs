using DotNetAngularApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetAngularApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FormController : ControllerBase
    {
        private readonly FormContext _formContext;
        public FormController(FormContext formContext)
        {
            _formContext = formContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<newForm>>> GetFormData()
        {
            var data = await _formContext.newForms.ToListAsync();

            if(data == null)
            {
                return BadRequest();
            }
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<newForm>> getFormById(int id)
        {
            var data = await _formContext.newForms.FindAsync(id);
            if(data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }
        [HttpPost]
        public async Task<ActionResult<newForm>> postData(newForm newFormData)
        {
            _formContext.newForms.Add(newFormData);
            await _formContext.SaveChangesAsync();
            return Ok(newFormData);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<newForm>>> UpdateData(newForm data , int id)
        {
            if(id != data.Id)
            {
                return BadRequest();
            }
            _formContext.Update(data);
            await _formContext.SaveChangesAsync();
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<newForm>> deteteData(int id)
        {
            var data = await _formContext.newForms.FindAsync(id);
            if( data == null)
            {
                return NotFound("the User data is not Found...!");
            }
            _formContext.newForms.Remove(data);
            await _formContext.SaveChangesAsync();
            return Ok(data);
        }
    }
}
