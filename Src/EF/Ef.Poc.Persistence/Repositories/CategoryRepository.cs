
using Ef.Poc.Application.Contracts.Repositories;
using Ef.Poc.Domain.Entities;
using Ef.Poc.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ef.Poc.Persistence.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {

    }

    public Task<List<Category>> GetAllAsync(int? parentId = null)
    {
        //if (parentId != null)
        //{
        //    Query = Query.Where(x => x.ParentId == parentId);
        //}

        return Query.ToListAsync();
    }

    public async Task<List<Category>> GetByTitleAsync(string title)
    {
        return await Query
            .Where(x => x.Title == title)
            .ToListAsync();
    }

    public Task<bool> ExistsAsync(string title)
    {
        return Query.AnyAsync(x => x.Title == title);
    }

    //public async Task<CategoryWithSubCategoriesViewModal> GetWithDetailsAsync(int id)
    //{
    //    var categories = await _repository.Entities.Where(x => x.Id == id || x.ParentId == id).ToListAsync();
    //    var category = categories.First(x => x.Id == id);
    //    var subCategories = categories.Where(x => x.ParentId == id).ToList();

    //    return new CategoryWithSubCategoriesViewModal
    //    {
    //        Id = category.Id,
    //        Title = category.Title,
    //        SubCategories = subCategories.Select(x => new CategoryWithSubCategoriesViewModal
    //        {
    //            Id = x.Id,
    //            Title = x.Title,
    //            SubCategories = new List<CategoryWithSubCategoriesViewModal>()
    //        }).ToList()
    //    };
    //}
}
