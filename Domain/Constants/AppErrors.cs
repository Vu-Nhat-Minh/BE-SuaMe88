namespace Domain.Constants
{
    public class AppErrors
    {
        public const string INVALID_CERTIFICATE = "Tài khoản hoặc mật khẩu không đúng";
        public const string INVALID_USER_UNACTIVE = "User không còn hoạt động";

        // User
        public const string USER_NOT_EXIST = "User không tồn tại";
        public const string USERNAME_EXIST = "Username đã tồn tại";

        // Product
        public const string INVALID_QUANTITY = "Số lượng phải lớn hơn 0";
        public const string PRODUCT_QUANTITY_NOT_ENOUGH = "Số lượng trong giỏ hàng không được vượt quá số lượng còn lại của sản phẩm.";

        // Query
        public const string CREATE_FAIL = "Tạo mới thất bại";
        public const string UPDATE_FAIL = "Cập nhật thất bại";
        public const string RECORD_NOT_FOUND = "Đối tượng không tồn tại";

        // Order
        public const string INVALID_PAYMENT_METHOD = "Phương thức thanh toán không tồn tại hoặc chưa hổ trợ";

        // Voucher
        public const string VOUCHER_NOT_ENOUGH = "Voucher đã hết lượt sử dụng";
        public const string VOUCHER_NOT_EXIST= "Voucher không tồn tại";
    }
}
