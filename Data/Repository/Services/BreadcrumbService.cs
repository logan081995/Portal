using QuickTie.Portal.Models;

namespace QuickTie.Portal.Data.Repository.Services
{
    public class BreadcrumbService
    {
        public List<BreadcrumbItem> GetBreadcrumbs(string currentPage)
        {
            var breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Name = "Cart", Url = "/" }
        };

            switch (currentPage)
            {
                case "contact-information":
                    breadcrumbs.Add(new BreadcrumbItem { Name = "Your information", Url = "/checkout/contact-information" });
                    break;
                case "shipping-method":
                    breadcrumbs.Add(new BreadcrumbItem { Name = "Your information", Url = "/checkout/contact-information" });
                    breadcrumbs.Add(new BreadcrumbItem { Name = "Shipping", Url = "/checkout/shipping-method" });
                    break;
                case "payment-method":
                    breadcrumbs.Add(new BreadcrumbItem { Name = "Your information", Url = "/checkout/contact-information" });
                    breadcrumbs.Add(new BreadcrumbItem { Name = "Shipping", Url = "/checkout/shipping-method" });
                    breadcrumbs.Add(new BreadcrumbItem { Name = "Payment", Url = "/checkout/payment-method" });
                    break;
                case "order-confirmation":
                    breadcrumbs.Add(new BreadcrumbItem { Name = "Confirmation", Url = "/checkout/order-confirmation" });
                    break;
            }

            return breadcrumbs;
        }
    }
}
