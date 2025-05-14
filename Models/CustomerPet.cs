namespace MVC.Models
{
    public class CustomerPet
    {
        public int CustomerPetId { get; set; }

        public int CustomerId { get; set; }

        public string PetName { get; set; }

        public int ProductId { get; set; }
        public string? Status {get; set;}

        // Thêm thuộc tính CustomerName và ProductName để lưu trữ thông tin tên khách hàng và tên sản phẩm
        public string? FirstName { get; set; } // Tên khách hàng
        public string? LastName { get; set; } // Tên khách hàng
        public string? ProductName { get; set; }   // Tên sản phẩm
    }
}
