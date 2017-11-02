using System;
using System.Collections.Generic;
using System.Text;
using TeduCoreApp.Application.ViewModels.Product;

namespace TeduCoreApp.Application.Interfaces
{
    public interface IProductCategoryService
    {
        ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm);

        void Update(ProductCategoryViewModel productCategoryVm);

        void Delete(int id);

        List<ProductCategoryViewModel> GetAll();

        List<ProductCategoryViewModel> GetAll(string keyword);

        List<ProductCategoryViewModel> GetAllByParentId(int parentId);

        ProductCategoryViewModel GetById(int id);

        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);
        void ReOrder(int sourceId, int targetId);

        List<ProductCategoryViewModel> GetHomeCategories(int top);

        void Save();
    }
}
