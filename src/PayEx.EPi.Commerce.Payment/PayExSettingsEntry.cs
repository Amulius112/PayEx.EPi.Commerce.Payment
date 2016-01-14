using System;
using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace PayEx.EPi.Commerce.Payment
{
    [EPiServerDataStore(
        AutomaticallyCreateStore = true,
        AutomaticallyRemapStore = true,
        StoreName = "PayExSettings")]
    public class PayExSettingsEntry : IDynamicData 
    {
        public PayExSettingsEntry()
        {
            Id = Identity.NewIdentity(Guid.NewGuid());
        }

        public long AccountNumber { get; set; }

        public string MarketId { get; set; }

        public Identity Id { get; set; }
    }
}