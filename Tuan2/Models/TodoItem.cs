using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tuan2.Models
{
    public class TodoItem
    {
        [DisplayName("Mã Công Việc")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Bắt buộc phải nhập tên công việc")]
        [DisplayName("Tên Công Việc")]
        public string Title { get; set; }
        [DisplayName("Trạng thái hoàn thành")]
        public bool IsCompleted { get; set; }
    }
}
