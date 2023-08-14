using BaseCore.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Queries;
using TeamsCore.Models.Entities;
using TeamsCore.Models.Responses;

namespace TeamsCore.Declarations.Repositories
{
    public interface IProductRepository
    {
        Task<bool> BeAnExistingProductId(string productId);

        Task<Product[]> GetRolesByIds(string[] ids);

        Task<QueryRecordResponse<ProductResponse>> GetProduct(GetProductQuery query);

        Task<ServiceResponse> AddProduct(AddProductCommand command);
    }
}
