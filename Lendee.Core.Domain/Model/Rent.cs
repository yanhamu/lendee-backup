namespace Lendee.Core.Domain.Model
{
    public class Rent : Contract
    {
        public RentType RentType { get; set; }
    }

    public enum RentType
    {
        /// <summary>
        /// Rent
        /// </summary>
        PureRent = 1,

        /// <summary>
        /// Rent + Energy
        /// </summary>
        CombinedRent = 2,

        /// <summary>
        /// Variable Rent
        /// </summary>
        VariableRent = 3
    }
}
