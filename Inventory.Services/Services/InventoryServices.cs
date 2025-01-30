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
using Core.Common.ExpCombiner;
using static Core.Common.ExpCombiner.ExpressionCombiner;

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

        public async Task<IEnumerable<InventoryDto>> GetAllAsync(InventoryFilterDto inventoryFilterDto)
        {
            var filter = BuildQuery(inventoryFilterDto);
            return _mapper.Map<IEnumerable<InventoryDto>>(await _unitOfWork.Inventory.GetAll(filter: filter));
        }


        public IEnumerable<InventoryDto> GetAll(Expression<Func<DomainModels.Models.Inventory, bool>>? filter = null)
        {
            return _mapper.Map<IEnumerable<InventoryDto>>(_unitOfWork.Inventory.GetAllIEnumerable(includeProperties: "Item.Category,Supplier", filter: filter));
        }

        public IEnumerable<InventoryDto> GetAll(InventoryFilterDto inventoryFilterDto)
        {
            var filter = BuildQuery(inventoryFilterDto);
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


        private (int Max, int Min) MinMaxInventory()
        {
            var _Min = GetAll(a => a.DeleteStatus == (int)DeleteStatus.NotDeleted).Min(a => a.StockQuantity);
            var _max = GetAll(a => a.DeleteStatus == (int)DeleteStatus.NotDeleted).Max(a => a.StockQuantity);

            return (_max, _Min);
        }

        public Expression<Func<DomainModels.Models.Inventory, bool>>? BuildQuery(InventoryFilterDto _filterParam)
        {
            Expression<Func<DomainModels.Models.Inventory, bool>>? filter = null;

            filter = a => a.DeleteStatus == (int)DeleteStatus.NotDeleted;

            if (_filterParam.StockStatusMinMax != 0)
            {
                switch (_filterParam.StockStatusMinMax)
                {
                    case StockStatusMinMax.MinStockquantity:

                        filter = ExpressionCombiner.And(filter, a => a.StockQuantity == MinMaxInventory().Min);
                        break;

                    case StockStatusMinMax.MaxStockquantity:

                        filter = ExpressionCombiner.And(filter, a => a.StockQuantity == MinMaxInventory().Max);
                        break;
                }
            }

            if ((_filterParam.StatustSearch > 0 && _filterParam.SelectedStockStatus > 0 ))
            {
                var BuildPredFilter = ExpressionTreeHelper<Inventory.DomainModels.Models.Inventory, object>.BuildPredicate(a => a.StockQuantity, _filterParam.selectedExpressionType, _filterParam. StatustSearch);

                filter = ExpressionCombiner.And(filter, BuildPredFilter);
            }

            if (_filterParam.SelectedCategoryId > 0)
            {
                filter = ExpressionCombiner.And(filter, a => a.Item.CategoryId == _filterParam.SelectedCategoryId);
            }
            if (_filterParam.SearchSelectedItemId > 0)
            {
                filter = ExpressionCombiner.And(filter, a => a.ItemId == _filterParam.SearchSelectedItemId);
            }

            if (_filterParam.SearchSelectedSupplierId > 0)
            {
                filter = ExpressionCombiner.And(filter, a => a.SupplierId == _filterParam.SearchSelectedSupplierId);
            }

            if (!String.IsNullOrEmpty(_filterParam.InputSearch) && _filterParam.InputSearch.Length > 0)
            {
                filter = ExpressionCombiner.And(filter, a => a.Item.Name.Contains(_filterParam.InputSearch)
                || a.Supplier.Name.Contains(_filterParam.InputSearch)
                || a.Item.Category.Name.Contains(_filterParam.InputSearch));
            }

            return filter;
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
