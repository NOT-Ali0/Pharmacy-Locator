using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IRepository<Medicine> _medicineRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public SupplierService(
        ISupplierRepository supplierRepository, 
        IRepository<Medicine> medicineRepository, 
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _supplierRepository = supplierRepository;
        _medicineRepository = medicineRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<SupplierDto?> GetSupplierByUserIdAsync(Guid userId)
    {
        var supplier = await _supplierRepository.GetByUserIdAsync(userId);
        if (supplier == null) return null;
        
        return _mapper.Map<SupplierDto>(supplier);
    }

    public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync()
    {
        var suppliers = await _supplierRepository.GetAllWithMedicinesAsync();
        return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
    }

    public async Task<SupplierDto> CreateSupplierAsync(SupplierDto supplierDto)
    {
        // Logic to create supplier would go here. 
        // Assuming user creation logic handles role assignment or this is an admin function.
        // For B2B refactor, let's assume this exists or is handled.
        throw new NotImplementedException();
    }

    public async Task AddMedicineAsync(Guid supplierId, AddSupplierMedicineDto dto)
    {
        var supplier = await _supplierRepository.GetByIdAsync(supplierId);
        if (supplier == null) throw new Exception("Supplier not found");

        var medicine = await _medicineRepository.GetByIdAsync(dto.MedicineId);
        if (medicine == null) throw new Exception("Medicine not found");

        var existing = await _supplierRepository.GetSupplierMedicineAsync(supplierId, dto.MedicineId);
        if (existing != null) throw new Exception("Medicine already in supplier catalog");

        var supplierMedicine = new SupplierMedicine
        {
            SupplierId = supplierId,
            MedicineId = dto.MedicineId,
            WholesalePrice = dto.WholesalePrice,
            MinimumOrderQuantity = dto.MinimumOrderQuantity,
            StockQuantity = dto.StockQuantity
        };

        await _supplierRepository.AddSupplierMedicineAsync(supplierMedicine);
    }

    public async Task UpdateMedicineAsync(Guid supplierId, Guid medicineId, UpdateSupplierMedicineDto dto)
    {
        var supplierMedicine = await _supplierRepository.GetSupplierMedicineAsync(supplierId, medicineId);
        if (supplierMedicine == null) throw new Exception("Medicine not found in supplier catalog");

        supplierMedicine.WholesalePrice = dto.WholesalePrice;
        supplierMedicine.MinimumOrderQuantity = dto.MinimumOrderQuantity;
        supplierMedicine.StockQuantity = dto.StockQuantity;

        await _supplierRepository.UpdateSupplierMedicineAsync(supplierMedicine);
    }

    public async Task RemoveMedicineAsync(Guid supplierId, Guid medicineId)
    {
        var supplierMedicine = await _supplierRepository.GetSupplierMedicineAsync(supplierId, medicineId);
        if (supplierMedicine == null) throw new Exception("Medicine not found in supplier catalog");

        await _supplierRepository.RemoveSupplierMedicineAsync(supplierMedicine);
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersAsync(Guid supplierId)
    {
        var orders = await _orderRepository.GetBySupplierIdAsync(supplierId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task UpdateOrderStatusAsync(Guid supplierId, Guid orderId, string statusStr)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null) throw new Exception("Order not found");
        
        if (order.SupplierId != supplierId) throw new UnauthorizedAccessException("Cannot manage orders for another supplier");

        if (Enum.TryParse<OrderStatus>(statusStr, true, out var status))
        {
            order.Status = status;
            await _orderRepository.UpdateAsync(order);
        }
        else
        {
            throw new ArgumentException("Invalid status");
        }
    }
}
