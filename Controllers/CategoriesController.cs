using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Models
{

    [ApiController]
    [Route("categories")]
    public class CategoriesController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            var categories = await context.Categories.AsNoTracking().ToListAsync(); // utilizando o AsNoTracking para que o entity não detecte mudanças
            return categories;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Get([FromServices] DataContext context, int id)
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return new Category();
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Post([FromBody] Category model, [FromServices] DataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível finalizar sua requisição" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Put(int id, [FromBody] Category model, [FromServices] DataContext context)
        {

            if (model.Id != id)
                return NotFound(new { message = "Este Id não é valido" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Esse registro já foi atualizado" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível atualizar a categoria" });

            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Delete(int id, [FromBody] Category model, [FromServices] DataContext context)
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound(new { message = "Categoria não encontrada" });
            }

            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return category;
            }
            catch (System.Exception)
            {

                return BadRequest(new { message = "Não foi possível  remover a categoria" });
            }
        }
    }
}