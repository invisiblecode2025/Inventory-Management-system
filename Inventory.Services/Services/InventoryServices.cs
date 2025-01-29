using AutoMapper;
using Inventory.Infrastructure.Interface;
using Inventory.Services.Interface;
using Inventory.ManagementDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Inventory.Shared.Core.Enum.Common;

namespace Inventory.Services.Services
{
    public class InventoryServices: IInventoryServices
    {

        private readonly IMapper _mapper;
        IUnitOfWork _unitOfWork;

        public InventoryServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;   
            _mapper = mapper;
        }


        public async Task<IEnumerable<InventoryDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<InventoryDto>>(await _unitOfWork.Inventory.GetAll());
        }

        public IEnumerable<InventoryDto> GetAll(Expression<Func<DomainModels.Models.Inventory, bool>>? filter = null)
        {
            return _mapper.Map<IEnumerable<InventoryDto>>(_unitOfWork.Inventory.GetAllIEnumerable(includeProperties: "Item.Category,Supplier", filter: filter));
        }

        public async Task<InventoryDto> GetByIdAsync(int Id)
        {
            return _mapper.Map<InventoryDto>(await _unitOfWork.Inventory.GetWhere(a => a.Id == Id,includeProperties: "Item,Supplier").AsNoTracking().FirstOrDefaultAsync());
        }

        public InventoryDto GetById(int Id)
        {
            return _mapper.Map<InventoryDto>( _unitOfWork.Inventory.GetWhere(a => a.Id == Id, includeProperties: "Supplier,Item,Item.Category").AsNoTracking().FirstOrDefault());
        }
        public async Task<int> Create(InventoryDto Inventory)
        {
            var _inventory = _mapper.Map<DomainModels.Models.Inventory>(Inventory);
            _unitOfWork.Inventory.Add(_inventory);
            await _unitOfWork.Commit();
            return _inventory.Id;
        }

        public async Task<bool> Update(InventoryDto Inventory)
        {
            var _inventory = _mapper.Map<DomainModels.Models.Inventory>(Inventory);
            var result = await _unitOfWork.Inventory.Update(_inventory);
            await _unitOfWork.Commit();
            return result;

        }
        public async Task<bool> Delete(int ItemId)
        {
            var _inventory = _unitOfWork.Inventory.GetWhere(a => a.Id == ItemId).FirstOrDefault();
            if (_inventory is not null)
            {
                _unitOfWork.Inventory.Delete(_inventory);
                return await _unitOfWork.Commit() > 0;
            }
            return await Task.FromResult(false);
        }

     
        public async Task<bool> SoftDeleteInventoryItems(int Id)
        {
            var _inventory = _unitOfWork.Inventory.GetWhere(a=> a.Id == Id).FirstOrDefault();

            if(_inventory is not null)
            _inventory.DeleteStatus = (int)DeleteStatus.Deleted;

            var result = await _unitOfWork.Inventory.Update(_inventory);
            await _unitOfWork.Commit();
            return result;
        }
    }
}
