namespace MVC.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        // Constructor mặc định
        public Customer() { }

        // Nếu bạn cần constructor tùy chỉnh có tham số, cũng có thể thêm chúng.
        public Customer(int customerId, string firstName, string lastName, string phoneNumber, string email, string address)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
        }
    }


}