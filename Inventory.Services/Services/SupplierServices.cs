using AutoMapper;
using Inventory.DomainModels.Models;
using Inventory.Infrastructure.Interface;
using Inventory.Services.Interface;
using Inventory.ManagementDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Inventory.Shared.Core.Enum.Common;

namespace Inventory.Services.Services
{
    public class SupplierServices : ISupplierServices
    {
        private readonly IMapper _mapper;
        IUnitOfWork _unitOfWork;
        public SupplierServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
              _unitOfWork = unitOfWork;
            _mapper = mapper;   
        }

     

        public async Task<IEnumerable<SupplierDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<SupplierDto>>(await _unitOfWork.Supplier.GetAll());
        }

        public IEnumerable<SupplierDto> GetAll()
        {
            return _mapper.Map<IEnumerable<SupplierDto>>(_unitOfWork.Supplier.GetAllIEnumerable());
        }

        public async Task<SupplierDto> GetById(int supplierId)
        {
            return _mapper.Map<SupplierDto>(await _unitOfWork.Supplier.GetWhere(a => a.Id == supplierId).AsNoTracking().FirstOrDefaultAsync());
        }
        public async Task<int> Create(SupplierDto Supplier)
        {
            var _supplier = _mapper.Map<Supplier>(Supplier);
            _unitOfWork.Supplier.Add(_supplier);
            await _unitOfWork.Commit();
            return _supplier.Id;
        }

        public async Task<bool> Update(SupplierDto Supplier)
        {
            var _supplier = _mapper.Map<Supplier>(Supplier);
            var result = await _unitOfWork.Supplier.Update(_supplier);
            await _unitOfWork.Commit();
            return result;

        }

        public async Task<bool> SoftDeleteItems(int Id)
        {
            var _supplier = _unitOfWork.Supplier.GetWhere(a => a.Id == Id).FirstOrDefault();
            if (_supplier is not null)
                _supplier.DeleteStatus = (int)DeleteStatus.Deleted;

            var result = await _unitOfWork.Supplier.Update(_supplier);
            await _unitOfWork.Commit();
            return result;
        }
        public async Task<bool> Delete(int ItemId)
        {
            var _supplier = _unitOfWork.Supplier.GetWhere(a => a.Id == ItemId).FirstOrDefault();
            if (_supplier is not null)
            {
                _unitOfWork.Supplier.Delete(_supplier);
                return await _unitOfWork.Commit() > 0;
            }
            return await Task.FromResult(false);
        }
    }
}
