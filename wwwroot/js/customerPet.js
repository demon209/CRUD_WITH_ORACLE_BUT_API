function toggleStatus(id, button) {
  fetch("/CustomerPet/ToggleStatus", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(id),
  })
    .then((response) => response.json())
    .then((data) => {
      if (data.success) {
        button.textContent = data.newStatus;
        button.classList.toggle("btn-success", data.newStatus === "Hoàn thành");
        button.classList.toggle(
          "btn-secondary",
          data.newStatus !== "Hoàn thành"
        );
      } else {
        alert(data.message || "Có lỗi xảy ra khi cập nhật trạng thái.");
      }
    })
    .catch((error) => {
      console.error("Lỗi:", error);
      alert("Đã xảy ra lỗi kết nối.");
    });
}
