using Infra;
using Microsoft.EntityFrameworkCore;
using TatweerSendDomain.Domain;

namespace GenerateIdentityServices.Services
{
    public interface IChangeOrderRequestStatueServices
    {
        Task ChngeOrderRequestStatu(List<string> identityNumbers, OrderRequestState orderRequestState);
    }

    public class ChangeOrderRequestStatueServices : IChangeOrderRequestStatueServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public ChangeOrderRequestStatueServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task ChngeOrderRequestStatu(List<string> identityNumbers, OrderRequestState orderRequestState)
        {
            var result = await (await _unitOfWork.GetRepositoryReadOnly<OrderRequest>().
                FindBy(pred => identityNumbers.Contains(pred.IdentityNumber))).ToListAsync();

            result.All(a =>
            {
                a.OrderRequestState = orderRequestState;
                return true;
            });


            await AddOrderEvents(result, orderRequestState, identityNumbers);


            await _unitOfWork.SaveChangeAsync();


        }
        private async Task AddOrderEvents(List<OrderRequest> result, OrderRequestState orderRequestState, List<string> identityNumbers)
        {

            var checkIdentityInsertEvent = await (await _unitOfWork.GetRepositoryReadOnly<OrderEvent>().
               FindBy(pred => identityNumbers.Contains(pred.OrderRequest.IdentityNumber))).
               Select(select => new OrderEvent
               {
                   OrderRequestId = select.OrderRequestId,
                   OrderRequestState = select.OrderRequestState,
               }).ToListAsync();


            var orderEvents = new List<OrderEvent>();

            result.ForEach(item =>
            {
                var checkOrder = checkIdentityInsertEvent.
                        Any(a => a.OrderRequestId.Equals(item.Id) &&
                        a.OrderRequestState.Equals(orderRequestState));

                if (!checkOrder)
                    orderEvents.Add(new OrderEvent
                    {
                        EmployeeNo = "موظف طباعة",
                        Id = Guid.NewGuid().ToString(),
                        OrderCreationDate = DateTime.Now,
                        OrderRequestId = item.Id,
                        OrderRequestState = orderRequestState,
                        UserType = UserTypeState.Employee,
                        UserId = "موظف طباعة"
                    });

            });

            if (orderEvents.Any())
                await _unitOfWork.GetRepositoryWriteOnly<OrderEvent>().InsertList(orderEvents);
        }
    }
}
