using System;
using CoffeeShopAPI.Models;

namespace CoffeeShopApp.Utils
{
    public static class AppSession
    {
        // Thông tin người dùng hiện tại
        public static User CurrentUser { get; set; }

        // Thời gian đăng nhập
        public static DateTime LoginTime { get; set; }

        // Xóa phiên làm việc khi đăng xuất
        public static void ClearSession()
        {
            CurrentUser = null;
            LoginTime = DateTime.MinValue;
        }

        // Kiểm tra đã đăng nhập chưa
        public static bool IsLoggedIn()
        {
            return CurrentUser != null;
        }

        // Kiểm tra quyền admin
        public static bool IsAdmin()
        {
            return CurrentUser != null && CurrentUser.Role.ToLower() == "admin";
        }

        // Kiểm tra quyền nhân viên
        public static bool IsStaff()
        {
            return CurrentUser != null &&
                  (CurrentUser.Role.ToLower() == "cashier" || CurrentUser.Role.ToLower() == "staff");
        }
    }
}