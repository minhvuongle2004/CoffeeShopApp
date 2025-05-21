using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoffeeShopAPI.Models;
using CoffeeShopApp.ServicesUser;
using CoffeeShopApp.Views.User;

namespace CoffeeShopApp.Services
{
    public static class TableOperationService
    {
        /// <summary>
        /// Xử lý chuyển bàn hoặc gộp bàn
        /// </summary>
        /// <param name="order">Đơn hàng nguồn cần chuyển</param>
        /// <param name="onSuccess">Callback khi thành công</param>
        /// <returns>Task</returns>
        public static async Task ProcessTableMoveAsync(Order order, Action onSuccess = null)
        {
            try
            {
                var apiClient = new OrderApiClient();

                // Tải danh sách bàn từ API
                List<Table> allTables = await apiClient.GetAllTablesAsync();

                if (allTables == null || allTables.Count <= 1) // Nếu chỉ có 1 bàn hoặc không có bàn nào
                {
                    MessageBox.Show("Không có bàn khác để chuyển đến!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Hiển thị form chọn bàn
                var moveTableForm = new MoveTableForm(order, allTables);
                if (moveTableForm.ShowDialog() == DialogResult.OK)
                {
                    // Nếu chuyển bàn thành công, thực hiện callback
                    if (onSuccess != null)
                    {
                        onSuccess();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý logic chuyển bàn đơn giản (sang bàn trống)
        /// </summary>
        public static async Task<bool> MoveTableAsync(Order order, Table targetTable, OrderApiClient apiClient)
        {
            try
            {
                using (var loading = new LoadingForm("Đang chuyển bàn..."))
                {
                    loading.Show();

                    // Lưu lại table_id cũ để cập nhật trạng thái sau này
                    int oldTableId = order.TableId;

                    // Sử dụng API endpoint mới để chuyển bàn
                    bool success = await apiClient.MoveTableAsync(order.Id, oldTableId, targetTable.Id);

                    if (success)
                    {
                        // Cập nhật giá trị TableId trong đối tượng Order
                        order.TableId = targetTable.Id;

                        // Lấy thông tin bàn nguồn
                        var sourceTable = await apiClient.GetTableByIdAsync(oldTableId);

                        // Cập nhật trạng thái bàn nguồn thành trống
                        if (sourceTable != null)
                        {
                            await apiClient.UpdateTableStatusAsync(sourceTable, "empty");
                        }

                        // Cập nhật trạng thái bàn đích thành có khách
                        await apiClient.UpdateTableStatusAsync(targetTable, "full");

                        loading.Close();
                        MessageBox.Show($"Đã chuyển đơn hàng sang Bàn {targetTable.TableName} thành công!",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return true;
                    }

                    loading.Close();
                    MessageBox.Show("Chuyển bàn thất bại! Vui lòng kiểm tra lại.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chuyển bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Xử lý logic gộp bàn
        /// </summary>
        public static async Task<bool> MergeTablesAsync(Order sourceOrder, Table targetTable, OrderApiClient apiClient)
        {
            try
            {
                using (var loading = new LoadingForm("Đang gộp bàn..."))
                {
                    loading.Show();

                    // Tìm đơn hàng của bàn đích
                    var allOrders = await apiClient.GetAllOrdersAsync();
                    var targetOrder = allOrders.Find(o => o.TableId == targetTable.Id && o.Status != "completed");

                    if (targetOrder == null)
                    {
                        loading.Close();
                        MessageBox.Show("Không tìm thấy đơn hàng của bàn đích", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Lấy tất cả chi tiết đơn hàng của đơn hàng nguồn
                    var sourceDetails = await apiClient.GetOrderDetailsAsync(sourceOrder.Id);

                    // Lấy tất cả chi tiết đơn hàng của đơn hàng đích
                    var targetDetails = await apiClient.GetOrderDetailsAsync(targetOrder.Id);

                    // Dictionary để lưu trữ chi tiết đơn hàng đích theo MenuId
                    Dictionary<int, OrderDetail> targetDetailsDict = new Dictionary<int, OrderDetail>();
                    foreach (var detail in targetDetails)
                    {
                        targetDetailsDict[detail.MenuId] = detail;
                    }

                    // Xử lý từng chi tiết đơn hàng nguồn
                    foreach (var sourceDetail in sourceDetails)
                    {
                        if (targetDetailsDict.ContainsKey(sourceDetail.MenuId))
                        {
                            // Nếu món đã tồn tại trong đơn hàng đích, cập nhật số lượng và subtotal
                            var targetDetail = targetDetailsDict[sourceDetail.MenuId];
                            targetDetail.Quantity += sourceDetail.Quantity;
                            targetDetail.Subtotal += sourceDetail.Subtotal;

                            // Cập nhật chi tiết đơn hàng trong database
                            await apiClient.UpdateOrderDetailAsync(targetDetail);
                        }
                        else
                        {
                            // Nếu món chưa tồn tại, thêm mới vào đơn hàng đích
                            sourceDetail.OrderId = targetOrder.Id;
                            await apiClient.AddOrderDetailAsync(sourceDetail);
                        }
                    }

                    // Cập nhật tổng số khách và tổng tiền của đơn hàng đích
                    targetOrder.TotalGuest += sourceOrder.TotalGuest;
                    targetOrder.TotalPrice += sourceOrder.TotalPrice;
                    await apiClient.UpdateOrderAsync(targetOrder);

                    // Xóa đơn hàng nguồn
                    await apiClient.DeleteOrderDetailsAsync(sourceOrder.Id);
                    await apiClient.DeleteOrderAsync(sourceOrder.Id);

                    // Cập nhật trạng thái bàn nguồn thành trống
                    var sourceTable = await apiClient.GetTableByIdAsync(sourceOrder.TableId);
                    if (sourceTable != null)
                    {
                        await apiClient.UpdateTableStatusAsync(sourceTable, "empty");
                    }

                    loading.Close();
                    MessageBox.Show($"Đã gộp đơn hàng sang Bàn {targetTable.TableName} thành công!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gộp bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}