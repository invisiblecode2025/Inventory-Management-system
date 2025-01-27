using AutoMapper;
using Inventory.DomainModels.Models;
using Inventory.Infrastructure.Interface;
using Inventory.Repository.DataContext;
using Inventory.Services.Interface;
using Inventory.ManagementDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Services.Services
{
    public class CategoryServices: ICategoryServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
       readonly ApplicationDbContext _dbContext;

        public CategoryServices(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public List<CategoryDto> GetAll()
        {
            return _mapper.Map<List<CategoryDto>>( _unitOfWork.Category.GetAllIEnumerable());
        }

        public async Task<IList<CategoryDto>> GetAllAsync()
        {
                return _mapper.Map<IList<CategoryDto>>(await _unitOfWork.Category.GetAll());
        }

        public async Task<CategoryDto> GetById(int categoryId)
        {
            return _mapper.Map<CategoryDto>(await _unitOfWork.Category.GetWhere(a => a.Id == categoryId).FirstOrDefaultAsync());
        }
        public async Task<int> Create(CategoryDto category)
        {
            var _category = _mapper.Map<Category>(category);
            _unitOfWork.Category.Add(_category);
            await _unitOfWork.Commit();
            return _category.Id;
        }

        public async Task<bool> Update(CategoryDto category)
        {
            var _category = _mapper.Map<Category>(category);
            var result = await _unitOfWork.Category.Update(_category);
            await _unitOfWork.Commit();
            return result;

        }
        public async Task<bool> Delete(int categoryId)
        {
            var _category = _unitOfWork.Category.GetWhere(a => a.Id == categoryId).FirstOrDefault();
            if (_category != null)
            {
                _unitOfWork.Category.Delete(_category);
                return await _unitOfWork.Commit() > 0;
            }
            return await Task.FromResult(false);
        }
    }
}
