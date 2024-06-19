namespace QuickTie.Portal.Models
{
    public class UserInfo
    {
        public ContactInfoForm ContactInfo { get; set; } = new ContactInfoForm();
        public CreditCardForm CreditCardInfo { get; set; } = new CreditCardForm();
        public POForm POInfo { get; set; } = new POForm();
        public ShippingMethodItem ShippingInfo { get; set; } = new ShippingMethodItem();
    }
}
