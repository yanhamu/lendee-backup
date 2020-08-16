namespace Lendee.Core.Domain.Model
{
    public class ContractDraft
    {
        public long ContractId { get; set; }
        public Contract Contract { get; set; }
        public int Step { get; set; }
    }
}
