using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IPharmacyRepository _pharmacyRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IOrderRepository orderRepository,
        ISupplierRepository supplierRepository,
        IPharmacyRepository pharmacyRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _supplierRepository = supplierRepository;
        _pharmacyRepository = pharmacyRepository;
        _mapper = mapper;
    }

    public async Task<OrderDto> CreateOrderAsync(Guid pharmacyId, CreateOrderDto dto)
    {
        var pharmacy = await _pharmacyRepository.GetByIdAsync(pharmacyId);
        if (pharmacy == null) throw new Exception("Pharmacy not found");

        var supplier = await _supplierRepository.GetByIdWithMedicinesAsync(dto.SupplierId);
        if (supplier == null) throw new Exception("Supplier not found");

        var orderItems = new List<OrderItem>();
        decimal totalAmount = 0;

        foreach (var itemDto in dto.Items)
        {
            var supplierMedicine = supplier.SupplierMedicines.FirstOrDefault(sm => sm.MedicineId == itemDto.MedicineId);
            if (supplierMedicine == null) 
                throw new Exception($"Medicine {itemDto.MedicineId} not available from this supplier");

            if (itemDto.Quantity < supplierMedicine.MinimumOrderQuantity)
                throw new Exception($"Quantity for medicine {supplierMedicine.Medicine.Name} is below MOQ ({supplierMedicine.MinimumOrderQuantity})");

            // Ideally check stock too, but requirement didn't specify strict stock reservation logic explicitly via CreateOrder, 
            // but implied "Manage Stock". Let's assume we check stock.
            if (itemDto.Quantity > supplierMedicine.StockQuantity)
                throw new Exception($"Insufficient stock for medicine {supplierMedicine.Medicine.Name}");

            // Deduct stock? Usually on approval or creation. Let's deduct on creation for simplicity or on approval.
            // Requirement doesn't specify. I'll leave stock deduction for "Approve" or just validation here.

            var unitPrice = supplierMedicine.WholesalePrice;
            totalAmount += unitPrice * itemDto.Quantity;

            orderItems.Add(new OrderItem
            {
                MedicineId = itemDto.MedicineId,
                Quantity = itemDto.Quantity,
                UnitPrice = unitPrice
            });
        }

        var order = new Order
        {
            PharmacyId = pharmacyId,
            SupplierId = dto.SupplierId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            TotalAmount = totalAmount,
            OrderItems = orderItems
        };

        await _orderRepository.AddAsync(order);
        
        // Re-fetch to load navigation properties for DTO mapping
        var savedOrder = await _orderRepository.GetByIdWithItemsAsync(order.Id);
        return _mapper.Map<OrderDto>(savedOrder);
    }

    public async Task<IEnumerable<OrderDto>> GetPharmacyOrdersAsync(Guid pharmacyId)
    {
        var orders = await _orderRepository.GetByPharmacyIdAsync(pharmacyId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<IEnumerable<OrderDto>> GetSupplierOrdersAsync(Guid supplierId)
    {
        var orders = await _orderRepository.GetBySupplierIdAsync(supplierId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
    
    public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(orderId);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task UpdateOrderStatusAsync(Guid orderId, string status)
    {
        // Implementation might overlap with SupplierService.UpdateOrderStatusAsync
        // But OrderService is cleaner.
        // I implemented it in SupplierService too. I should probably delegate or keep one.
        // I will keep logic here if general, but SupplierService logic enforces ownership.
        throw new NotImplementedException("Use SupplierService for supplier updates.");
    }
}
