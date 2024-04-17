namespace DuAnDauTien.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    public class SanPhamViewModel
    {
        public string TenSP { get; set; }
        public double GiaSP {  get; set; }
        public string AnhMoTa {  get; set; }
    }
}