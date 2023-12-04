namespace DealerAPI.Models.DTO
{
    public class PaymentHistoryDto:PaymentDto
    {

        public paymentStatus? PaymentStatus { get; set; }
    }
}
