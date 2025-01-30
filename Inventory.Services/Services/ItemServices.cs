using AutoMapper;
using Inventory.DomainModels.Models;
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
    public class ItemServices : IItemServices
    {
        private readonly IMapper _mapper;
        IUnitOfWork _unitOfWork;
        
        public ItemServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
                
        }

        public async Task<List<ItemDto>> GetAllAsync()
        {
            return  _mapper.Map<List<ItemDto>> (await _unitOfWork.Item.GetAll());
        }

        public IEnumerable<ItemDto> GetAll(Expression<Func<Item, bool>>? filter = null)
        {
            return _mapper.Map<List<ItemDto>>(_unitOfWork.Item.GetAllIEnumerable(includeProperties: "Category", filter: filter));
        }

        public async Task<ItemDto> GetById(int ItemId)
        {
            return _mapper.Map<ItemDto>(await _unitOfWork.Item.GetWhere(a=> a.Id == ItemId,includeProperties: "Category").AsNoTracking().FirstOrDefaultAsync());
        }
        public async Task<int> Create(ItemDto Item)
        {
            var item = _mapper.Map<Item>(Item);
            _unitOfWork.Item.Add(item);
           await _unitOfWork.Commit(); 
            return item.Id; 
        }

        public async Task<bool> Update(ItemDto Item)
        {
            var item = _mapper.Map<Item>(Item);
           var result = await  _unitOfWork.Item.Update(item);
            await _unitOfWork.Commit();        
            return result;

        }

        public async Task<bool> SoftDeleteItems(int Id)
        {
            var _itemdelete = _unitOfWork.Item.GetWhere(a => a.Id == Id).FirstOrDefault();

            if (_itemdelete is not null)
                _itemdelete.DeleteStatus = (int)DeleteStatus.Deleted;

            var result = await _unitOfWork.Item.Update(_itemdelete);
            await _unitOfWork.Commit();
            return result;
        }
        public async Task<bool> Delete(int ItemId)
        {
            var item = _unitOfWork.Item.GetWhere(a=> a.Id == ItemId).FirstOrDefault();
            if (item is not null)
            {
                 _unitOfWork.Item.Delete(item);
             return   await   _unitOfWork.Commit() > 0;
            }
            return await Task.FromResult(false);
        }

        
    }
}
