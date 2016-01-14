
namespace PayEx.EPi.Commerce.Payment.Contracts
{
    public interface IPayExSettings
    {
        long AccountNumber { get; }
        string EncryptionKey { get; }
        bool IncludeOrderLines { get; }
        bool DisablePaymentMethodCreation { get; }
        bool IncludeCustomerAddress { get; }
    }
}
