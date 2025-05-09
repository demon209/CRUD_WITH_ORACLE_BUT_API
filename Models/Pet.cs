using System;

namespace MVC.Models
{
    public class Pet
    {
        public string PetName { get; set; }
        public  string PetType { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public int PetId { get; set; }
        public int Age { get; set; }
        public decimal Price { get; set; }
        public string? Status { get; set; }

        // Thuộc tính mới để lưu dữ liệu ảnh dưới dạng mảng byte
        public byte[]? ImageData { get; set; }

        // Thuộc tính tính toán chuỗi base64 từ mảng byte, có thể hỗ trợ nhiều định dạng ảnh
        public string? ImageBase64
        {
            get
            {
                if (ImageData != null)
                {
                    // Giả định rằng nếu mảng byte có dữ liệu, nó là ảnh JPEG (hoặc có thể mở rộng thêm cho các định dạng khác)
                    string mimeType = "image/jpeg"; // Mặc định là JPEG, có thể thay đổi nếu cần
                    return $"data:{mimeType};base64,{Convert.ToBase64String(ImageData)}";
                }
                return null;
            }
        }
    }
}
