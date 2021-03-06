﻿using EPiServer.Business.Commerce.Payment.PayEx.Contracts;
using EPiServer.Business.Commerce.Payment.PayEx.Contracts.Commerce;
using EPiServer.Business.Commerce.Payment.PayEx.Models.PaymentMethods;
using log4net;
using Mediachase.Commerce.Orders.Dto;

namespace EPiServer.Business.Commerce.Payment.PayEx.Factories
{
    internal class PaymentMethodFactory : IPaymentMethodFactory
    {
        private readonly IPaymentManager _paymentManager;
        private readonly IParameterReader _parameterReader;
        private readonly ICartActions _cartActions;
        private readonly IVerificationManager _verificationManager;
        private readonly IOrderNumberGenerator _orderNumberGenerator;
        private readonly IAdditionalValuesFormatter _additionalValuesFormatter;
        private readonly IPaymentActions _paymentActions;
        protected readonly ILog Log = LogManager.GetLogger(Constants.Logging.DefaultLoggerName);

        public PaymentMethodFactory(IPaymentManager paymentManager, IParameterReader parameterReader, ICartActions cartActions, IVerificationManager verificationManager,
            IOrderNumberGenerator orderNumberGenerator, IAdditionalValuesFormatter additionalValuesFormatter, IPaymentActions paymentActions)
        {
            _paymentManager = paymentManager;
            _parameterReader = parameterReader;
            _cartActions = cartActions;
            _verificationManager = verificationManager;
            _orderNumberGenerator = orderNumberGenerator;
            _additionalValuesFormatter = additionalValuesFormatter;
            _paymentActions = paymentActions;
        }

        public PaymentMethod Create(Mediachase.Commerce.Orders.Payment payment)
        {
            if (payment == null)
                return null;

            Log.InfoFormat("Attempting to resolve the PaymentMethod for payment with ID:{0}. PaymentMethodId:{1}", payment.Id, payment.PaymentMethodId);
            if (!(payment is PayExPayment))
            {
                Log.ErrorFormat("Payment with ID:{0} is not a PayExPayment and therefore it cannot be processed by the PayEx Payment Provider!", payment.Id);
                return null;
            }

            PaymentMethodDto paymentMethodDto =
                Mediachase.Commerce.Orders.Managers.PaymentManager.GetPaymentMethod(payment.PaymentMethodId);
            string systemKeyword = paymentMethodDto.PaymentMethod.FindByPaymentMethodId(payment.PaymentMethodId).SystemKeyword;
            Log.InfoFormat("Resolving the PaymentMethod for payment with ID:{0}. The systemKeyword for this payment method is {1}", payment.Id, systemKeyword);

            switch (systemKeyword)
            {
                case Constants.Payment.DirectDebit.SystemKeyword:
                    return new DirectBankDebit(payment, _paymentManager, _parameterReader, _cartActions, _orderNumberGenerator, _additionalValuesFormatter, _paymentActions);
                case Constants.Payment.Giftcard.SystemKeyword:
                    return new GiftCard(payment, _paymentManager, _parameterReader, _cartActions, _orderNumberGenerator, _additionalValuesFormatter, _paymentActions);
                case Constants.Payment.Invoice.SystemKeyword:
                    return new Invoice(payment, _verificationManager, _paymentManager, _parameterReader, _cartActions, _orderNumberGenerator, _additionalValuesFormatter, _paymentActions);
                case Constants.Payment.InvoiceLedger.SystemKeyword:
                    return new InvoiceLedger(payment, _paymentManager, _parameterReader, _cartActions, _orderNumberGenerator, _additionalValuesFormatter, _paymentActions);
                case Constants.Payment.PartPayment.SystemKeyword:
                    return new PartPayment(payment, _paymentManager, _parameterReader, _cartActions, _orderNumberGenerator, _additionalValuesFormatter, _paymentActions);
                case Constants.Payment.PayPal.SystemKeyword:
                    return new PayPal(payment, _paymentManager, _parameterReader, _cartActions, _orderNumberGenerator, _additionalValuesFormatter, _paymentActions);
                case Constants.Payment.CreditCard.SystemKeyword:
                    return new CreditCard(payment, _paymentManager, _parameterReader, _cartActions, _orderNumberGenerator, _additionalValuesFormatter, _paymentActions);
            }

            Log.ErrorFormat("Could not resolve the PaymentMethod for payment with ID:{0}. The systemKeyword for this payment method is {1}", payment.Id, systemKeyword);
            return null;
        }
    }
}
