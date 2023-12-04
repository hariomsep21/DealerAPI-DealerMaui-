namespace DealerAPI.Models.DTO
{
    public class ProcurementStatusDto:ProcDetailDto
    {

        public string Purchased_Amount { get; set; }
        public ProcurementStatus? Status { get; set; }
    }
}
