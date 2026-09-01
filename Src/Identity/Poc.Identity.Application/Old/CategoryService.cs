//     public async Task<Result<List<CategoryListDto>>> GetAllAsync(int? parentId = null)
//     {
//         //if (parentId != null)
//         //{
//         //    Query = Query.Where(x => x.ParentId == parentId);
//         //}
//
//         return Query.ToListAsync();

//         var categories = _repository
//                 .Query
//                 .Select(x => x.ToCategoryListDto())
//                 .ToList();
//
//         return categories == null ?
//             await Result<List<CategoryListDto>>.FailureAsync() :
//             await Result<List<CategoryListDto>>.SuccessAsync(categories);
//     }
//
//     public async Task<Result<CategoryDto>> GetAsync(int id)
//     {
//         var category = await _repository.GetAsync(id);
//
//         var categoryDto = category?.ToCategoryDto();
//
//         return categoryDto == null ?
//             await Result<CategoryDto>.FailureAsync() :
//             await Result<CategoryDto>.SuccessAsync(categoryDto);
//
//         //var subCategories = await  _repository.ListAsync(parentId: id);
//         //return new CategoryDto
//         //{
//         //    Id = category.Id,
//         //    Name = category.Title,
//         //    //SubCategories = subCategories.Select(c => new CategoryDto
//         //    //{
//         //    //    Id = c.Id,
//         //    //    Title = c.Title
//         //    //}).ToList()
//         //};
//     }
//
//     //public async Task<Result<CategoryDto>> GetByTitleAsync(string title)
//     //{
//     //    var category = await    return await Query
//             .Where(x => x.Title == title)
//             .ToListAsync();;
//
//     //    var categoryDto = category?.ToCategoryDto();
//
//     //    return categoryDto == null ?
//     //        await Result<CategoryDto>.FailureAsync() :
//     //        await Result<CategoryDto>.SuccessAsync(categoryDto);
//     //}
//
//     public async Task<Result<CategoryDto>> AddAsync(CreateCategoryInputDto input, CancellationToken cancellationToken)
//     {
//         //var exists = await _repository.ExistsAsync(input.Title);
//
//         //if (exists)
//         //{
//         //    return null;
//         //    // TODO
//         //    // throw an exception
//         //    // {error: "Category already exists"}
//         //}
//
//         var category = await _repository.AddAsync(input.ToCategory());
//
//         await _unitOfWork.Save(cancellationToken);
//
//         var categoryDto = category?.ToCategoryDto();
//
//         return categoryDto == null ?
//             await Result<CategoryDto>.FailureAsync() :
//             await Result<CategoryDto>.SuccessAsync(categoryDto);
//     }
//
//     public async Task<Result<int>> DeleteAsync(int id, CancellationToken cancellationToken)
//     {
//         var category = await _repository.GetAsync(id);
//         if (category == null)
//         {
//             return await Result<int>.FailureAsync();
//         }
//
//         await _repository.DeleteAsync(category);
//
//         await _unitOfWork.Save(cancellationToken);
//
//         return await Result<int>.SuccessAsync();
//     }
//
//     public async Task<Result<CategoryDto>> UpdateAsync(int id, UpdateCategoryInputDto input, CancellationToken cancellationToken)
//     {
//         var category = await _repository.GetAsync(id);
//         if (category == null)
//         {
//             return await Result<CategoryDto>.FailureAsync();
//         }
//
//         category = input.ToCategory(id);
//
//         await _repository.UpdateAsync(category);
//
//         await _unitOfWork.Save(cancellationToken);
//
//         var categoryDto = category?.ToCategoryDto();
//
//         return categoryDto == null ?
//             await Result<CategoryDto>.FailureAsync() :
//             await Result<CategoryDto>.SuccessAsync(categoryDto);
//
//         //if (!string.Equals(category.Title, input.Title, StringComparison.OrdinalIgnoreCase))
//         //{
//         //    var exists = await _repository.ExistsAsync(input.Title);
//
//         //    if (exists)
//         //    {
//         //        throw new ApplicationException($"There is already a category with the title {input.Title}");
//         //    }
//
//         //    category.SetTitle(input.Title);
//         //    await _repository.UpdateAsync(category);
//         //}
//
//         //var subCategories = await _repository.ListAsync(category.Id);
//
//         //// deleted sub categories
//         //var existingSubCategoryIds = subCategories.Select(x => x.Id).Distinct().ToList();
//         //var inputSubCategoryIds = input.SubCategories.Select(x => x.Id).Distinct().ToList();
//         //var deletedSubCategoryIds = existingSubCategoryIds.Where(x => !inputSubCategoryIds.Contains(x)).ToList();
//
//         //foreach (var deletedSubCategoryId in deletedSubCategoryIds)
//         //{
//         //    await _repository.DeleteAsync(subCategories.First(x => x.Id == deletedSubCategoryId));
//         //}
//
//         //// newly added sub categories
//         //var newAddedSubCategories = input.SubCategories.Where(x => x.Id == null);
//
//         //foreach (var newAddedSubCategory in newAddedSubCategories)
//         //{
//         //    var newSubCategory = await _repository.CreateAsync(new Category(newAddedSubCategory.Title));
//         //}
//     }
// }

//
//     //public async Task<CategoryWithSubCategoriesViewModal> GetWithDetailsAsync(int id)
//     //{
//     //    var categories = await _repository.Entities.Where(x => x.Id == id || x.ParentId == id).ToListAsync();
//     //    var category = categories.First(x => x.Id == id);
//     //    var subCategories = categories.Where(x => x.ParentId == id).ToList();
//
//     //    return new CategoryWithSubCategoriesViewModal
//     //    {
//     //        Id = category.Id,
//     //        Title = category.Title,
//     //        SubCategories = subCategories.Select(x => new CategoryWithSubCategoriesViewModal
//     //        {
//     //            Id = x.Id,
//     //            Title = x.Title,
//     //            SubCategories = new List<CategoryWithSubCategoriesViewModal>()
//     //        }).ToList()
//     //    };
//     //}
